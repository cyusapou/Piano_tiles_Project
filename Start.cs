using Godot;
using System;

public partial class Start : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Make the button gold
		var goldStyle = new StyleBoxFlat();
		goldStyle.BgColor = new Color(1.0f, 0.84f, 0.0f); // Gold
		AddThemeStyleboxOverride("normal", goldStyle);
		AddThemeStyleboxOverride("hover", goldStyle);
		AddThemeStyleboxOverride("pressed", goldStyle);
		AddThemeStyleboxOverride("focus", goldStyle);

		// Connect the "Pressed" signal to the "_on_Pressed" method.
		Pressed += _on_Pressed;
	}

	// This method is called when the "Start" button is clicked.
	private void _on_Pressed()
	{
		GD.Print("Switching to Main.tscn");
		// Change the scene to Main.tscn
		GetTree().ChangeSceneToFile("res://Main.tscn");
	}
}
