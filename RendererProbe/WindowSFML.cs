using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static RendererProbe.Globals;

namespace RendererProbe;

public class WindowSFML
{
    public Entity MainEntity { get; set; }
    
    public void Run()
	{
		var videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
		var window = new RenderWindow(videoMode, "Hello MetaBalls");
		window.SetFramerateLimit(30);
		window.SetVerticalSyncEnabled(true);
		window.KeyPressed += Window_KeyPressed;

		string fontPath = Path.Combine(
			AppDomain.CurrentDomain.BaseDirectory,
			"Fonts/open-sans/OpenSans-Regular.ttf"
		);
		Font font = new Font(fontPath);
		
		//FPS
		var clock = new Clock();
		float fps = 0.0f;
		Text fpsText = new Text()
		{
			Font = font,
            CharacterSize = 18
		};

        //Various texts
        Text fovTxt = new Text()
        {
            Font = font,
            CharacterSize = 18,
            Position = new Vector2f(fpsText.Position.X, fpsText.Position.Y + 16)
        };
        Text perspectiveTxt = new Text()
        {
            Font = font,
            CharacterSize = 18,
            Position = new Vector2f(fovTxt.Position.X, fovTxt.Position.Y + 16)
        };
        Text rotateTxt = new Text()
        {
            Font = font,
            CharacterSize = 18,
            Position = new Vector2f(perspectiveTxt.Position.X, perspectiveTxt.Position.Y + 16)
        };

        //Load Meshes
        string filePathTeapot = Path.Combine(
			Environment.CurrentDirectory,
			"Assets/teapot.obj"
		);
		
		string filePathShip = Path.Combine(
			Environment.CurrentDirectory,
			"Assets/ship.obj"
		);

		string filePathCube = Path.Combine(
			Environment.CurrentDirectory,
			"Assets/cube.obj"
		);

		ObjReader teapotReader = new ObjReader(filePathTeapot);
		Triangle[] teapotMesh = teapotReader.Triangles.ToArray();
		ObjReader shipReader = new ObjReader(filePathShip);
		Triangle[] shipMesh = shipReader.Triangles.ToArray();
		ObjReader cubeReader = new ObjReader(filePathCube);
		Triangle[] cubeMesh = cubeReader.Triangles.ToArray();

        //Set camera position
        Camera.CAMERA_X = 0.0f;
		Camera.CAMERA_Y = 0.0f;
		Camera.CAMERA_Z = 0.0f;

        //Create entity
        MainEntity = new Entity(
			new Vector4(0.0f, 0.0f, 8.0f, 1.0f),
			1.0f,
			0.0f,
			teapotMesh.Select(x => new Triangle(x)).ToArray()
		);
		MainEntity.Rotation = 0.5f;

		while (window.IsOpen)
		{
			//Setup
			window.DispatchEvents();
			clock.Restart();
			window.Clear();
			
			//Draw
			window.Draw(fpsText);
            window.Draw(fovTxt);
            window.Draw(perspectiveTxt);
            window.Draw(rotateTxt);
            
            MainEntity.Draw(window);
			if (ENABLE_ROTATION)
			{
				MainEntity.Rotate();
			}
            
			window.Display();
            
			//Update texts
			fps = 1.0f / clock.ElapsedTime.AsSeconds();
			fpsText.DisplayedString = $"FPS: {fps:0.00}";

            fovTxt.DisplayedString = $"FOV: {WINDOW_FOV}";
            perspectiveTxt.DisplayedString = $"Perspective: {PERSPECTIVE}";
            rotateTxt.DisplayedString = $"Rotation: {ENABLE_ROTATION}";
		}
	}

    private void Window_KeyPressed(object sender, KeyEventArgs eventArgs)
	{
		var window = (Window)sender;
		if (eventArgs.Code == Keyboard.Key.Escape)
			window.Close();

        if (eventArgs.Code == Keyboard.Key.Add)
            WINDOW_FOV += 2;
        if (eventArgs.Code == Keyboard.Key.Subtract)
            WINDOW_FOV -= 2;

        if (eventArgs.Code == Keyboard.Key.P)
            PERSPECTIVE = PERSPECTIVE ? false : true;

        if (eventArgs.Code == Keyboard.Key.Up)
            MainEntity.PositionZ += 0.5f;
        if (eventArgs.Code == Keyboard.Key.Down)
            MainEntity.PositionZ -= 0.5f;

        if (eventArgs.Code == Keyboard.Key.R && DateTime.Now.Subtract(SETTING_CHANGE_LAST_UPDATED).Milliseconds > 100)
		{
			ENABLE_ROTATION = ENABLE_ROTATION ? false : true;
			SETTING_CHANGE_LAST_UPDATED = DateTime.Now;
		}
	}
}
