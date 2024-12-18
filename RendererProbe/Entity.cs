using System.Numerics;

namespace RendererProbe;

public class Entity
{
    public float PositionX { get; set; }
    public float PositionY { get; set; }
    public float PositionZ { get; set; }

    public float ScaleFactor { get; set; }
    private float _angle;
    public float Angle { 
        get { return _angle; } 
        set {
            if (_angle + value >= 360) 
                _angle = 0 + value;
            else if (_angle + value < 0) 
                _angle = 360 + value;
            else _angle = value;
        }
    }
    private Mesh Mesh { get; set; }
    public Entity(float posX, float posY, float posZ, float scaleF, float angle, Triangle[] triangles)
    {
        PositionX = posX;
        PositionY = posY;
        PositionZ = posZ;
        ScaleFactor = scaleF;
        Angle = angle;
        Mesh = new Mesh { Triangles = triangles };
        Scale();
        Rotate(Angle);
    }

    public void Draw()
    {
        Mesh.DrawMesh(PositionX, PositionY, PositionZ);
    }

    public void NormalizeCoordinates()
    {
        for (int i = 0; i < Mesh.Triangles.Length; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 vec3 = Mesh.Triangles[i].Vertices[j];
            }
        }
    }

    public void UpdateCoordinates(float posX, float posY, float posZ)
    {
        PositionX = posX;
        PositionY = posY;
        PositionZ = posZ;
    }

    public void Rotate(float angle)
    {   
        Angle += angle;

        float angleRad = Graphics.AngleToRad(angle);

        for (int i = 0; i < Mesh.Triangles.Length; i++)
        {
            for (int j = 0; j < Mesh.Triangles[i].Vertices.Length; j++)
            {
                float x = Mesh.Triangles[i].Vertices[j].X;
                float y = Mesh.Triangles[i].Vertices[j].Y;
                float z = Mesh.Triangles[i].Vertices[j].Z;

                float x2 = x * (float)Math.Cos(angleRad) - (y * (float)Math.Sin(angleRad));
                float y2 = x * (float)Math.Sin(angleRad) + (y * (float)Math.Cos(angleRad));

                Mesh.Triangles[i].Vertices[j].X = x2;
                Mesh.Triangles[i].Vertices[j].Y = y2;
                Mesh.Triangles[i].Vertices[j].Z = z;
            }
        }
    }

    public void Scale()
    {
        for (int i = 0; i < Mesh.Triangles.Length; i++)
        {
            for (int j = 0; j < Mesh.Triangles[i].Vertices.Length; j++)
            {
                Mesh.Triangles[i].Vertices[j].X *= ScaleFactor / Constants.WORLD_SIZE;
                Mesh.Triangles[i].Vertices[j].Y *= ScaleFactor / Constants.WORLD_SIZE;
                Mesh.Triangles[i].Vertices[j].Z *= ScaleFactor / Constants.WORLD_SIZE;
            }
        }
    }
}