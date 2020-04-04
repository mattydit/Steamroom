using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SurfaceCreator : MonoBehaviour
{
    private Mesh mesh;

    [Range(0f, 1f)]
    public float strength = 1f;

    [Range(1, 200)]
    public int resolution = 10;

    public float frequency = 1f;

    [Range(1, 8)]
    public int octaves = 1;

    [Range(1f, 4f)]
    public float lacunarity = 2f;

    [Range(0f, 1f)]
    public float persistence = 0.5f;

    [Range(1, 3)]
    public int dimensions = 3;

    public NoiseMethodType type;
    public Gradient colouring;
    public bool colouringforStrength;
    public bool damping;
    public Vector3 offset;
    public Vector3 rotation;

    private int current_Res;
    private Vector3[] vertices;
    private Vector3[] normals;
    private Color[] colours;

    private void OnEnable()
    {
        if (mesh == null)
        {
            mesh = new Mesh();
            mesh.name = "Surface Mesh";
            GetComponent<MeshFilter>().mesh = mesh;
        }
        Refresh();
    }

    public void Refresh()
    {
        if (resolution != current_Res)
        {
            CreateGrid();
        }

        Quaternion q = Quaternion.Euler(rotation);
        Vector3 point00 = q * new Vector3(-0.5f, -0.5f) + offset;
        Vector3 point10 = q * new Vector3(0.5f, -0.5f) + offset;
        Vector3 point01 = q * new Vector3(-0.5f, 0.5f) + offset;
        Vector3 point11 = q * new Vector3(0.5f, 0.5f) + offset;

        NoiseMethod method = Noise.methods[(int)type][dimensions - 1];
        float stepSize = 1f / resolution;

        float amplitude = damping ? strength / frequency : strength;

        for (int v = 0, y = 0; y <= resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, y * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, y * stepSize);

            for (int x = 0; x <= resolution; x++, v++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, x * stepSize);
                float sample = Noise.Sum(method, point, frequency, octaves, lacunarity, persistence);
                sample = type == NoiseMethodType.Value ? (sample - 0.5f) : (sample * 0.5f);

                if (colouringforStrength)
                {
                    colours[v] = colouring.Evaluate(sample + 0.5f);
                    sample *= amplitude;
                }
                else
                {
                    //scale sample after -1/2 - 1/2 range
                    sample *= amplitude;
                    colours[v] = colouring.Evaluate(sample + 0.5f);
                }

                if (type != NoiseMethodType.Value)
                {
                    sample = sample * 0.5f + 0.5f;
                }
                vertices[v].y = sample;
               
            }
        }
        mesh.vertices = vertices;
        mesh.colors = colours;
        mesh.RecalculateNormals();
        DestroyImmediate(this.GetComponent<MeshCollider>());
        this.gameObject.AddComponent<MeshCollider>();
    }

    private void CreateGrid()
    {
        current_Res = resolution;
        mesh.Clear();

        vertices = new Vector3[(resolution + 1) * (resolution + 1)];
        normals = new Vector3[vertices.Length];
        Vector2[] uvs = new Vector2[vertices.Length];
        colours = new Color[vertices.Length];
        float stepSize = 1f / resolution;

        for (int v = 0, z = 0; z <= resolution; z++)
        {
            for (int x = 0; x <= resolution; x++, v++)
            {
                vertices[v] = new Vector3(x * stepSize - 0.5f, 0f, z * stepSize - 0.5f);
                normals[v] = Vector3.up;
                uvs[v] = new Vector2(x * stepSize, z * stepSize);
                colours[v] = Color.black;
            }
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.colors = colours;

        int[] triangles = new int[resolution * resolution * 6];

        for (int t = 0, v = 0, y = 0; y < resolution; y++, v++)
        {
            for (int x = 0; x < resolution; x++, v++, t += 6)
            {
                triangles[t] = v;
                triangles[t + 1] = v + resolution + 1;
                triangles[t + 2] = v + 1;
                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + resolution + 1;
                triangles[t + 5] = v + resolution + 2;
            }
        }

        mesh.triangles = triangles;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
