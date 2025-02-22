using Raylib_cs;
using System.Numerics;
using static RendererProbe.Globals;

namespace RendererProbe;

internal class Program
{
	static void Main(string[] args)
	{
		Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Renderer-Probe");
		Raylib.SetTargetFPS(30);

		string filePathTeapot = Path.Combine(
			Environment.CurrentDirectory,
			"Assets/teapot.obj"
		);
		
		string filePathShip = Path.Combine(
			Environment.CurrentDirectory,
			"Assets/ship.obj"
		);

		ObjReader teapotReader = new ObjReader(filePathTeapot);
		Triangle[] teapotMesh = teapotReader.Triangles.ToArray();
		ObjReader shipReader = new ObjReader(filePathShip);
		Triangle[] shipMesh = shipReader.Triangles.ToArray();

		Triangle[] cubeTris =
		[
			// SOUTH
			new Triangle( 0.0f, 0.0f, 0.0f,    0.0f, 1.0f, 0.0f,    1.0f, 1.0f, 0.0f ),
			new Triangle( 0.0f, 0.0f, 0.0f,    1.0f, 1.0f, 0.0f,    1.0f, 0.0f, 0.0f ),

			// EAST                                                      
			new Triangle( 1.0f, 0.0f, 0.0f,    1.0f, 1.0f, 0.0f,    1.0f, 1.0f, 1.0f ),
			new Triangle( 1.0f, 0.0f, 0.0f,    1.0f, 1.0f, 1.0f,    1.0f, 0.0f, 1.0f ),

			// NORTH                                                     
			new Triangle( 1.0f, 0.0f, 1.0f,    1.0f, 1.0f, 1.0f,    0.0f, 1.0f, 1.0f ),
			new Triangle( 1.0f, 0.0f, 1.0f,    0.0f, 1.0f, 1.0f,    0.0f, 0.0f, 1.0f ),

			// WEST                                                      
			new Triangle( 0.0f, 0.0f, 1.0f,    0.0f, 1.0f, 1.0f,    0.0f, 1.0f, 0.0f ),
			new Triangle( 0.0f, 0.0f, 1.0f,    0.0f, 1.0f, 0.0f,    0.0f, 0.0f, 0.0f ),

			// TOP                                                       
			new Triangle( 0.0f, 1.0f, 0.0f,    0.0f, 1.0f, 1.0f,    1.0f, 1.0f, 1.0f ),
			new Triangle( 0.0f, 1.0f, 0.0f,    1.0f, 1.0f, 1.0f,    1.0f, 1.0f, 0.0f ),

			// BOTTOM                                                    
			new Triangle( 1.0f, 0.0f, 1.0f,    0.0f, 0.0f, 1.0f,    0.0f, 0.0f, 0.0f ),
			new Triangle( 1.0f, 0.0f, 1.0f,    0.0f, 0.0f, 0.0f,    1.0f, 0.0f, 0.0f )
		];

		Camera.CAMERA_X = 0.0f;
		Camera.CAMERA_Y = 0.0f;
		Camera.CAMERA_Z = 0.0f;

		Entity box = new Entity(
			new Vector4(0.0f, 0.0f, 8.0f, 1.0f),
			1.0f,
			0.0f,
			teapotMesh.Select(x => new Triangle(x)).ToArray()
		);
		box.Rotation = 1.0f;
		
		
		while (!Raylib.WindowShouldClose())
		{
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.Black);
			// DrawGrid();
			HandleInput();
			Camera.HandleInput();
			Raylib.DrawFPS(Graphics.ToScreenSpaceX(-0.99f), Graphics.ToScreenSpaceY(0.99f));
			// Raylib.DrawText($"Camera Zoom: {Camera.CAMERA_ZOOM:0.00}", ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.90f), 16, Color.White);
			Raylib.DrawText($"FOV: {WINDOW_FOV}", Graphics.ToScreenSpaceX(-0.99f), Graphics.ToScreenSpaceY(0.90f), 16, Color.White);
			Raylib.DrawText($"Perspective: {PERSPECTIVE}", Graphics.ToScreenSpaceX(-0.99f), Graphics.ToScreenSpaceY(0.86f), 16, Color.White);
			Raylib.DrawText($"Camera Yaw: {Camera.CAMERA_YAW}", Graphics.ToScreenSpaceX(-0.99f), Graphics.ToScreenSpaceY(0.82f), 16, Color.White);

			// float fovRad = AngleToRad(WINDOW_FOV);
			// Z_NEAR = 1 / (float)Math.Tan(fovRad / 2);
			// Z_FAR = Z_NEAR * 10;

			//Update Coordinates
			//cube.PositionX += 0.02f * cubeXDir;
			//if (cube.PositionX >= 1.0f && cubeXDir == 1.0f) cubeXDir = -1.0f;
			//else if (cube.PositionX <= -1.0f && cubeXDir == -1.0f) cubeXDir = 1.0f;

			//cube.PositionY += 0.01f * cubeYDir;
			//if (cube.PositionY >= 1.0f && cubeYDir == 1.0f) cubeYDir = -1.0f;
			//else if (cube.PositionY <= -1.0f && cubeYDir == -1.0f) cubeYDir = 1.0f;

			// camera.Draw();

			//Rotate
			// player.Rotate(-2.0f);

			box.Draw();
			if (ENABLE_ROTATION)
			{
				box.Rotate();
				// box.RotateRoll();
				// box.RotateYaw();
				// box.RotatePitch();
			}

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}

	//Handle Screen Input
	private static void HandleInput()
	{
		if (Raylib.IsKeyDown(KeyboardKey.KpAdd))
			WINDOW_FOV += 2;
		else if (Raylib.IsKeyDown(KeyboardKey.KpSubtract))
			WINDOW_FOV -= 2;

		if (Raylib.IsKeyPressed(KeyboardKey.P))
			PERSPECTIVE = PERSPECTIVE ? false : true;


		//Camera Zoom
		// if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.Up))
		// {
		// 	if (Camera.CAMERA_ZOOM > 0.1f)
		// 		Camera.CAMERA_ZOOM -= 0.05f;
		// }

		// if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.Down))
		// {
		// 	if (Camera.CAMERA_ZOOM < 1.0f)
		// 		Camera.CAMERA_ZOOM += 0.05f;
		// }

		//Rotate
		if (Raylib.IsKeyDown(KeyboardKey.R) && DateTime.Now.Subtract(SETTING_CHANGE_LAST_UPDATED).Milliseconds > 100)
		{
			ENABLE_ROTATION = ENABLE_ROTATION ? false : true;
			SETTING_CHANGE_LAST_UPDATED = DateTime.Now;
		}
	}
}