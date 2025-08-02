using Godot;
using System;

public partial class YouWon : CanvasLayer
{
	private GameManager _gameManager;

	public override void _Ready()
	{
		GD.Print("YouWon scene ready");

		ProcessMode = ProcessModeEnum.Always;

		Node mainNode = GetTree().Root.GetNodeOrNull("Main");
		if (mainNode == null)
		{
			GD.PrintErr("Main node not found in scene tree.");
			return;
		}

		_gameManager = mainNode.GetNodeOrNull<GameManager>("GameManager");
		if (_gameManager == null)
		{
			GD.PrintErr("GameManager not found inside Main node.");
			return;
		}
		
		// Get the buttons with the correct names
		var restartButton = GetNode<Button>("RestartGame");
		var mainMenu = GetNode<Button>("MainMenu");

		if (restartButton == null || mainMenu == null)
		{
			GD.PrintErr("Error: One or more buttons were not found. Check the node names.");
			return;
		}

		// Connect signals to the GameManager's methods
		restartButton.Pressed += () => {
			GD.Print("RestartGame pressed");
			_gameManager.RestartGame();
		};

		mainMenu.Pressed += () => {
			GD.Print("MainMenu pressed");
			_gameManager.GoToMainMenu();
		};
	}
}
