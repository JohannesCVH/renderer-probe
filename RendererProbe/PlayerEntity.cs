using System.Numerics;
using Raylib_cs;

namespace RendererProbe;

public class PlayerEntity : Entity
{
	public PlayerEntity(Vector3 pos, float scaleF, float angle, Triangle[] triangles) : base(pos, scaleF, angle, triangles)
	{
		
	}
	
	new public void Draw()
	{
		HandleInput();
		base.Draw();
	}

	public void HandleInput()
	{
		float movement = 4.0f;
		
		if (Raylib.IsKeyDown(KeyboardKey.W))
		{
			if (PositionY <= Globals.WORLD_SIZE)
				PositionY += movement;
		}
		if (Raylib.IsKeyDown(KeyboardKey.S))
		{
			if (PositionY >= -Globals.WORLD_SIZE)
				PositionY -= movement;
		}
		if (Raylib.IsKeyDown(KeyboardKey.D))
		{
			if (PositionX <= Globals.WORLD_SIZE)
				PositionX += movement;
		}
		if (Raylib.IsKeyDown(KeyboardKey.A))
		{
			if (PositionX >= -Globals.WORLD_SIZE)
				PositionX -= movement;
		}
		
		Camera.CAMERA_X = PositionX;
		Camera.CAMERA_Y = PositionY;
	}
}