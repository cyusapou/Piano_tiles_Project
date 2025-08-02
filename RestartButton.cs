using Godot;

using System;



public partial class RestartButton : Button

{

public override void _Ready()

{

Pressed += OnRestartPressed;

GD.Print("RestartButton ready and connected.");

}



private void OnRestartPressed()

{

GD.Print("Restart button pressed!");

GetTree().ChangeSceneToFile("res://game.tscn");

}

}
