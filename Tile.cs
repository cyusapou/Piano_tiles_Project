using Godot;

using System;



public partial class Tile : Area2D

{

[Signal] public delegate void TileClickedEventHandler();

[Signal] public delegate void TileMissedEventHandler();


[Export] public Texture2D TileTexture;

[Export] public float MoveSpeed = 300f;

[Export] public float DestroyY = 800f;


private Sprite2D _sprite;

private CollisionShape2D _collision;



public override void _Ready()

{

try

{

// Create sprite with texture or fallback

_sprite = new Sprite2D {

Texture = TileTexture != null ? TileTexture : CreateFallbackTexture(),

Centered = true

};

AddChild(_sprite);



// Create collision shape

_collision = new CollisionShape2D {

Shape = new RectangleShape2D {

Size = _sprite.Texture.GetSize()

}

};

AddChild(_collision);



// Enable input

Monitorable = true;

Monitoring = true;

InputEvent += HandleInputEvent; // Fixed method name

}

catch (System.Exception e)

{

GD.PrintErr($"Tile setup failed: {e.Message}");

QueueFree();

}

}



private void HandleInputEvent(Node viewport, InputEvent @event, long shapeIdx)

{

if (@event is InputEventMouseButton mouseEvent &&

mouseEvent.Pressed &&

mouseEvent.ButtonIndex == MouseButton.Left)

{

EmitSignal(SignalName.TileClicked);

QueueFree();

}

}



private Texture2D CreateFallbackTexture()

{

var image = new Image();

// Correct static method call:

image = Image.Create(64, 64, false, Image.Format.Rgba8);

image.Fill(Colors.White);

return ImageTexture.CreateFromImage(image);

}



public override void _Process(double delta)

{

Position += new Vector2(0, MoveSpeed * (float)delta);

if (Position.Y > DestroyY && !IsQueuedForDeletion())

{

EmitSignal(SignalName.TileMissed);

QueueFree();

}

}

}
