using System.Numerics;

namespace RendererProbe;

public class ObjReader
{
    private string[] Lines { get; set; }
    public List<Triangle> Triangles { get; set; } = new List<Triangle>();
    
    public ObjReader(string filePath)
    {
        Lines = File.ReadAllLines(filePath);
        Process();
    }

    private void Process()
    {
        List<Vector3> vertices = new List<Vector3>();
        
        for (int i = 0; i < Lines.Length; i++)
        {
            string[] tokens = Lines[i].Split(' ');
            if (tokens[0].Equals("v"))
            {
                Vector3 vec = new Vector3(
                    float.Parse(tokens[1]),
                    float.Parse(tokens[2]),
                    float.Parse(tokens[3])
                );

                vertices.Add(vec);
            }
        }

        for (int i = 0; i < Lines.Length; i++)
        {
            string[] tokens = Lines[i].Split(' ');
            if (tokens[0].Equals("f"))
            {
                int vecLoc1 = int.Parse(tokens[1]) - 1;
                int vecLoc2 = int.Parse(tokens[2]) - 1;
                int vecLoc3 = int.Parse(tokens[3]) - 1;
                
                Triangle triangle = new Triangle(
                    vertices[vecLoc1].X,
                    vertices[vecLoc1].Y,
                    vertices[vecLoc1].Z,

                    vertices[vecLoc2].X,
                    vertices[vecLoc2].Y,
                    vertices[vecLoc2].Z,

                    vertices[vecLoc3].X,
                    vertices[vecLoc3].Y,
                    vertices[vecLoc3].Z
                );

                Triangles.Add(triangle);
            }
        }
    }
}
