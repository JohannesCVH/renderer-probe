using Raylib_cs;
using System.Numerics;
using static RendererProbe.Constants;
using static RendererProbe.Graphics;

namespace RendererProbe;

internal class Program
{
    static void Main(string[] args)
    {
        Raylib.InitWindow(WINDOW_WIDTH, WINDOW_HEIGHT, "Hello Renderer");
        Raylib.SetTargetFPS(30);

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

        //Triangle[] tris = [
        //    new Triangle(
        //        0.0f,   1.0f,   0.0f,
        //        -1.0f,  -1.0f,  0.0f,
        //        1.0f,   -1.0f,  0.0f
        //    )
        //];

        Triangle[] tris = [
            new Triangle(
                0.5f,   1.0f,   0.0f,
                0.0f,   0.0f,   0.0f,
                1.0f,   0.0f,  0.0f
            )
        ];

        //float cubeXDir = 1.0f;
        //float cubeYDir = 1.0f;
        Entity cube = new Entity(0.0f, 0.0f, 0.0f, 10.0f, 0.0f, tris);

        float rotationAngle = 5.0f;

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
            DrawGrid();
            HandleInput();
            Raylib.DrawFPS(ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.99f));
            Raylib.DrawText($"FOV: {WINDOW_FOV}", ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.90f), 16, Color.White);
            Raylib.DrawText($"Camera Zoom: {Camera.CAMERA_ZOOM}", ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.78f), 16, Color.White);

            float fovRad = AngleToRad(WINDOW_FOV);
            Z_NEAR = 1 / (float)Math.Tan(fovRad / 2);
            Z_FAR = Z_NEAR * 10;

            //Update Coordinates
            //cube.PositionX += 0.02f * cubeXDir;
            //if (cube.PositionX >= 1.0f && cubeXDir == 1.0f) cubeXDir = -1.0f;
            //else if (cube.PositionX <= -1.0f && cubeXDir == -1.0f) cubeXDir = 1.0f;

            //cube.PositionY += 0.01f * cubeYDir;
            //if (cube.PositionY >= 1.0f && cubeYDir == 1.0f) cubeYDir = -1.0f;
            //else if (cube.PositionY <= -1.0f && cubeYDir == -1.0f) cubeYDir = 1.0f;

            cube.Draw();

            //Rotate Cube
            //cube.Rotate(-1.0f);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }

    static void DrawGrid()
    {
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
            Raylib.DrawText($"{i / 10.0f * WORLD_SIZE * Camera.CAMERA_ZOOM:0}", (int)startX + 4, ToScreenSpaceY(0.0f) + 4, 10, Color.White);

            startX = ToScreenSpaceX(1.0f);
            startY = ToScreenSpaceY(i / 10.0f);
            endX = ToScreenSpaceX(-1.0f);
            endY = ToScreenSpaceY(i / 10.0f);

            Raylib.DrawLine((int)startX, (int)startY, (int)endX, (int)endY, Color.White);
            if (i != 0)
                Raylib.DrawText($"{i / 10.0f * WORLD_SIZE * Camera.CAMERA_ZOOM:0}", ToScreenSpaceX(0.0f) + 4, (int)startY + 4, 10, Color.White);
        }
    }

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
    }
}



//Raylib.DrawLineEx(ToScreenSpaceVec2(line1S), ToScreenSpaceVec2(line1E), 3.0f, Color.White);
//float line1AR = (float)Math.Atan((double)(line1E.Y / line1E.X));
//float line1AD = line1AR * 180.0f / (float)Math.PI;
//Raylib.DrawText($"Line 1 Angle: {line1AD}", ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.84f), 16, Color.White);
//
//rotationAngle += 5;
//if (rotationAngle >= 360) rotationAngle = 0;
//float line2AR = AngleToRad(rotationAngle);
//float newX = line2E.X * (float)Math.Cos(line2AR) - line2E.Y * (float)Math.Sin(line2AR);
//float newY = line2E.X * (float)Math.Sin(line2AR) + line2E.Y * (float)Math.Cos(line2AR);
//Raylib.DrawLineEx(ToScreenSpaceVec2(line1S), ToScreenSpaceVec2(new Vector2(newX, newY)), 3.0f, Color.Red);
////Raylib.DrawText($"Line 2 Angle: {line1AD}", NormalizeCoordX(-0.99f), NormalizeCoordY(0.78f), 16, Color.White);
//Raylib.DrawText($"Line 2 Hypo: {Math.Sqrt(Math.Pow(line2E.X, 2) + Math.Pow(line2E.Y, 2))}", ToScreenSpaceX(-0.99f), ToScreenSpaceY(0.78f), 16, Color.White);