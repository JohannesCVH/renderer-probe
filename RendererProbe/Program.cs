using Raylib_cs;
using System.Numerics;
using static RendererProbe.Globals;
using static RendererProbe.Graphics;

namespace RendererProbe;

internal class Program
{
	static void Main(string[] args)
	{
		Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Hello Renderer");
		Raylib.SetTargetFPS(30);

		string filePath = Path.Combine(
			Environment.CurrentDirectory,
			"Assets/teapot.obj"
		);

		ObjReader objReader = new ObjReader(filePath);
		Triangle[] teapotMesh = objReader.Triangles.ToArray();

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

		PlayerEntity player = new PlayerEntity(new Vector3(0.0f, 0.0f, 0.0f), 10.0f, 0.0f, []);

		Entity box = new Entity(
			new Vector3(0.0f, 0.0f, 0.0f),
			30,
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
			Raylib.DrawFPS(ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.99f));
			Raylib.DrawText($"Camera Zoom: {Camera.CAMERA_ZOOM:0.00}", ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.90f), 16, Color.White);
			// Raylib.DrawText($"FOV: {WINDOW_FOV}", ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.90f), 16, Color.White);

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

			player.Draw();

			//Rotate
			// player.Rotate(-2.0f);

			box.Draw();
			if (ENABLE_ROTATION)
			{
				box.RotateRoll();
				box.RotateYaw();
				box.RotatePitch();
			}

			Raylib.EndDrawing();
		}

		Raylib.CloseWindow();
	}

	static void DrawGrid()
	{
		Vector3 cameraPos = new Vector3(Camera.CAMERA_X, Camera.CAMERA_Y, 0.0f);
		cameraPos = Vector3Normalize(cameraPos);
		
		for (int i = -9; i < 10; i++)
		{
			Raylib.DrawLineEx(
				ToScreenSpaceVec2(new Vector2(-1.0f, 0.0f)),
				ToScreenSpaceVec2(new Vector2(1.0f, 0.0f)),
				3.0f, 
				Color.White
			);

			Raylib.DrawLineEx(
				ToScreenSpaceVec2(new Vector2(0.0f, -1.0f)),
				ToScreenSpaceVec2(new Vector2(0.0f, 1.0f)),
				3.0f,
				Color.White
			);

			float startX  = ToScreenSpaceX(i / 10.0f);
			float startY  = ToScreenSpaceY(1.0f);
			float endX    = ToScreenSpaceX(i / 10.0f);
			float endY    = ToScreenSpaceY(-1.0f);

			Raylib.DrawLine((int)startX, (int)startY, (int)endX, (int)endY, Color.White);
			Raylib.DrawText($"{(i + cameraPos.X) / 10.0f * WORLD_SIZE * Camera.CAMERA_ZOOM:0}", (int)startX + 4, ToScreenSpaceY(0.0f) + 4, 10, Color.White);

			startX = ToScreenSpaceX(1.0f);
			startY = ToScreenSpaceY(i / 10.0f);
			endX = ToScreenSpaceX(-1.0f);
			endY = ToScreenSpaceY(i / 10.0f);

			Raylib.DrawLine((int)startX, (int)startY, (int)endX, (int)endY, Color.White);
			if (i != 0)
				Raylib.DrawText($"{(i + cameraPos.Y) / 10.0f * WORLD_SIZE * Camera.CAMERA_ZOOM:0}", ToScreenSpaceX(0.0f) + 4, (int)startY + 4, 10, Color.White);
		}
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
		if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.Up))
		{
			if (Camera.CAMERA_ZOOM > 0.1f)
				Camera.CAMERA_ZOOM -= 0.05f;
		}

		if (Raylib.IsKeyDown(KeyboardKey.LeftControl) && Raylib.IsKeyDown(KeyboardKey.Down))
		{
			if (Camera.CAMERA_ZOOM < 1.0f)
				Camera.CAMERA_ZOOM += 0.05f;
		}

		//Rotate
		if (Raylib.IsKeyDown(KeyboardKey.R) && DateTime.Now.Subtract(SETTING_CHANGE_LAST_UPDATED).Milliseconds > 300)
		{
			ENABLE_ROTATION = ENABLE_ROTATION ? false : true;
			SETTING_CHANGE_LAST_UPDATED = DateTime.Now;
		}
	}
}