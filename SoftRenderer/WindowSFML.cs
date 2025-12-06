using System.Numerics;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using static SoftRenderer.Globals;
using static SoftRenderer.Constants;

namespace SoftRenderer;

public class WindowSFML
{
    private Entity MainEntity { get; set; }
	private RenderWindow Window { get; set; }
	private DateTime PrevTime = DateTime.Now;


	private Font _font = new Font(FONT_PATH);
	private WindowText _fpsTxt { get; set; }
	private WindowText _fovTxt { get; set; }
	private WindowText _perspectiveTxt { get; set; }
	private WindowText _rotateTxt { get; set; }
    
    public void Run()
	{
		var videoMode = new VideoMode(WINDOW_WIDTH, WINDOW_HEIGHT);
		Window = new RenderWindow(videoMode, "Hello Render Probe");
		Window.SetFramerateLimit(20);
		Window.SetVerticalSyncEnabled(true);
		Window.KeyPressed += Window_KeyPressed;
		
		_fpsTxt = new WindowText(_font, new Vector2f(0, 0));
		_fovTxt = new WindowText(_font, new Vector2f(0, 16));
        _perspectiveTxt = new WindowText(_font, new Vector2f(0, 32));
        _rotateTxt = new WindowText(_font, new Vector2f(0, 48));

        //Load Meshes
        string filePathTeapot = Path.Combine(
			Directory.GetCurrentDirectory(),
			"./Assets/teapot.obj"
		);
		
		string filePathShip = Path.Combine(
			Directory.GetCurrentDirectory(),
			"./Assets/ship.obj"
		);

		string filePathCube = Path.Combine(
			Directory.GetCurrentDirectory(),
			"./Assets/cube.obj"
		);

		ObjReader teapotReader = new ObjReader(filePathTeapot);
		Triangle[] teapotMesh = teapotReader.Triangles.ToArray();
		// ObjReader shipReader = new ObjReader(filePathShip);
		// Triangle[] shipMesh = shipReader.Triangles.ToArray();
		// ObjReader cubeReader = new ObjReader(filePathCube);
		// Triangle[] cubeMesh = cubeReader.Triangles.ToArray();

        //Set camera position
        Camera.CAMERA_X = 0.0f;
		Camera.CAMERA_Y = 0.0f;
		Camera.CAMERA_Z = 0.0f;

        //Create entity
        MainEntity = new Entity(
			new Vector4(0.0f, 0.0f, 12.0f, 1.0f),
			1.4f,
			0.0f,
			teapotMesh.Select(x => new Triangle(x)).ToArray()
		);
		MainEntity.Rotation = 0.25f;


		UPDATE_TIMER.Start();
		Task.Run(() =>
        {
			while (IS_RUNNING)
            {
                Update();

				if (UPDATE_TIMER.ElapsedMilliseconds >= UPDATE_INTERVAL)
					UPDATE_TIMER.Restart();
            }
        });

		while (Window.IsOpen)
		{
			var curTime = DateTime.Now;
			var deltaTime = curTime - PrevTime;
			PrevTime = curTime;
			
			Window.DispatchEvents();
			Window.Clear();

			Draw(deltaTime.Milliseconds);
		}
	}

	private void Update()
    {	
        MainEntity.Update();

		if (ENABLE_ROTATION)
		{
			MainEntity.Rotate();
		}
    }

	private void Draw(int dt)
    {
		MainEntity.Draw(Window);
		
        Window.Draw(_fpsTxt.Text);
		Window.Draw(_fovTxt.Text);
		Window.Draw(_perspectiveTxt.Text);
		Window.Draw(_rotateTxt.Text);

		float fps = 1000.0f / dt;
		_fpsTxt.DisplayedString = $"FPS: {fps:0}";

		_fovTxt.DisplayedString = $"FOV: {WINDOW_FOV}";
		_perspectiveTxt.DisplayedString = $"Perspective: {PERSPECTIVE}";
		_rotateTxt.DisplayedString = $"Rotation: {ENABLE_ROTATION}";

		Window.Display();
    }

    private void Window_KeyPressed(object sender, KeyEventArgs eventArgs)
	{
		var window = (Window)sender;
		if (eventArgs.Code == Keyboard.Key.Escape)
        {
			IS_RUNNING = false;
            window.Close();
        }

        if (eventArgs.Code == Keyboard.Key.Add)
            WINDOW_FOV += 2;
        if (eventArgs.Code == Keyboard.Key.Subtract)
            WINDOW_FOV -= 2;

        if (eventArgs.Code == Keyboard.Key.P)
            PERSPECTIVE = PERSPECTIVE ? false : true;

		if (eventArgs.Code == Keyboard.Key.L)
            DRAW_LINES = DRAW_LINES ? false : true;

        if (eventArgs.Code == Keyboard.Key.Up)
            MainEntity.Position = new Vector4(MainEntity.Position.X, MainEntity.Position.Y, MainEntity.Position.Z + 0.25f, MainEntity.Position.W);
        if (eventArgs.Code == Keyboard.Key.Down)
            MainEntity.Position = new Vector4(MainEntity.Position.X, MainEntity.Position.Y, MainEntity.Position.Z - 0.25f, MainEntity.Position.W);

        if (eventArgs.Code == Keyboard.Key.R && DateTime.Now.Subtract(SETTING_CHANGE_LAST_UPDATED).Milliseconds > 100)
		{
			ENABLE_ROTATION = ENABLE_ROTATION ? false : true;
			SETTING_CHANGE_LAST_UPDATED = DateTime.Now;
		}
	}
}
