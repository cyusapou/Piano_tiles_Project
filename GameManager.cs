using Godot;
using System;

public partial class GameManager : Node
{
	// === CORE CONFIG ===
	[Export] public Vector2[] SpawnPoints = {
		new(52.75f, -100f), // Lane 1
		new(155.5f, -100f), // Lane 2
		new(258.25f, -100f), // Lane 3
		new(361f, -100f)  // Lane 4                
	};
	
	[Export] public PackedScene TileScene;
	[Export] public PackedScene YouWonScene;
	[Export] public PackedScene YouLostScene;
	[Export] public float SpawnInterval = 0.8f;
	[Export] public int WinScore = 20;
	[Export] public int MaxActiveTiles = 5;

	// === RUNTIME ===
	private float _spawnTimer;
	private int _score;
	private int _activeTiles;
	private bool _gameEnded;
	private Random _random = new();
	private Node _endScreen; // Reference to the end screen for cleanup

	// === MAIN LOOP ===
	public override void _Ready()
	{
		ValidateResources();
		InitializeScoreLabel();
		GetTree().Paused = false;
	}

	public override void _Process(double delta)
	{
		if (_gameEnded || _activeTiles >= MaxActiveTiles) return;
		
		_spawnTimer += (float)delta;
		if (_spawnTimer >= SpawnInterval)
		{
			SpawnTile();
			_spawnTimer = 0;
		}
	}

	// === SPAWNER SYSTEM ===
	private void SpawnTile()
	{
		try
		{
			var tile = TileScene.Instantiate<Tile>();
			AddChild(tile);
			
			tile.Position = SpawnPoints[_random.Next(0, SpawnPoints.Length)];
			
			tile.TileClicked += OnTileClicked;
			tile.TileMissed += OnTileMissed;
			
			_activeTiles++;
		}
		catch (Exception e)
		{
			GD.PrintErr($"Spawn failed: {e.Message}");
		}
	}

	// === GAME FLOW ===
	private void EndGame(bool won)
	{
		if (_gameEnded) return;
		
		_gameEnded = true;
		_endScreen = won ? YouWonScene.Instantiate() : YouLostScene.Instantiate();
		
		GetTree().Root.AddChild(_endScreen);
		GetTree().Paused = true;
	}

	// === BUTTON ACTIONS ===
	private void CleanUpAndResume()
	{
		// Safely remove the end screen if it exists
		if (_endScreen != null && IsInstanceValid(_endScreen))
		{
			_endScreen.QueueFree();
			_endScreen = null;
		}
		GetTree().Paused = false;
	}

	public void RestartGame()
	{
		CleanUpAndResume();
		GetTree().ReloadCurrentScene();
	}

	public void GoToMainMenu()
	{
		CleanUpAndResume();
		GetTree().ChangeSceneToFile("res://MainMenu.tscn");
	}

	// === HELPERS ===
	private void ValidateResources()
	{
		TileScene ??= GD.Load<PackedScene>("res://Tile.tscn");
		YouWonScene ??= GD.Load<PackedScene>("res://YouWon.tscn");
		YouLostScene ??= GD.Load<PackedScene>("res://YouLost.tscn");
	}

	private void InitializeScoreLabel()
	{
		if (!HasNode("ScoreLabel"))
		{
			var label = new Label {
				Name = "ScoreLabel",
				Text = "Score: 0",
				Position = new Vector2(20, 20),
			};
			label.AddThemeColorOverride("font_color", new Color(1, 0, 0)); // Red
			AddChild(label);
		}
	}

	private void OnTileClicked()
	{
		if (_gameEnded) return;

		_score++;
		GetNode<Label>("ScoreLabel").Text = $"Score: {_score}";
		_activeTiles--;
		if (_score >= WinScore) EndGame(true);
	}

	private void OnTileMissed()
	{
		if (_gameEnded) return;

		_activeTiles--;
		EndGame(false);
	}
}
