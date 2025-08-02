using Godot;
using System;

public partial class YouLost : CanvasLayer
{
	private GameManager _gameManager;

	public override void _Ready()
	{
		GD.Print("YouLost scene ready");

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
		
		// Corrected paths for buttons based on your scene tree
		var playAgain = GetNode<Button>("PlayAgain");
		var mainMenu = GetNode<Button>("MainMenu");

		if (playAgain == null || mainMenu == null)
		{
			GD.PrintErr("Error: One or more buttons were not found. Check the node names.");
			return;
		}

		playAgain.Pressed += () => {
			GD.Print("PlayAgain pressed");
			_gameManager.RestartGame();
		};

		mainMenu.Pressed += () => {
			GD.Print("MainMenu pressed");
			_gameManager.GoToMainMenu();
		};
	}
}
