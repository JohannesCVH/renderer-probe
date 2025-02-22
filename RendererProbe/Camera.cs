using System.Numerics;
using Raylib_cs;

namespace RendererProbe;

internal static class Camera
{
	public static float CAMERA_X = 0.0f;
	public static float CAMERA_Y = 0.0f;
	public static float CAMERA_Z = 0.0f;
	public static Vector3 CAMERA_DIRECTION = new Vector3(0.0f, 0.0f, 0.0f);
	public static float CAMERA_YAW = 0.0f;
	public static DateTime LAST_UPDATED = DateTime.Now;
	
	public static void HandleInput()
	{
		float movement = 4.0f;
		float rotationSpeed = 2.0f;
		
		if ((DateTime.Now - LAST_UPDATED).Milliseconds < 100)
			return;
		
		if (Raylib.IsKeyDown(KeyboardKey.W))
		{
			if (CAMERA_Y <= Globals.WORLD_SIZE)
				CAMERA_Y += movement;
		}
		if (Raylib.IsKeyDown(KeyboardKey.S))
		{
			if (CAMERA_Y >= -Globals.WORLD_SIZE)
				CAMERA_Y -= movement;
		}
		if (Raylib.IsKeyDown(KeyboardKey.D))
		{
			if (CAMERA_YAW + rotationSpeed >= 360) 
				CAMERA_YAW = 0 + rotationSpeed;
			else
				CAMERA_YAW += rotationSpeed;
			// else if (CAMERA_YAW + rotationSpeed < 0) 
			// 	CAMERA_YAW = 360 + rotationSpeed;
			
			LAST_UPDATED = DateTime.Now;
		}
		if (Raylib.IsKeyDown(KeyboardKey.A))
		{
			if (CAMERA_YAW - rotationSpeed < 0) 
				CAMERA_YAW = 360 - rotationSpeed;
			else
				CAMERA_YAW -= rotationSpeed;
			// else if (CAMERA_YAW - rotationSpeed < 0) 
			// 	CAMERA_YAW = 360 - rotationSpeed;
			// else CAMERA_YAW = rotationSpeed;
			
			LAST_UPDATED = DateTime.Now;
		}
	}
}