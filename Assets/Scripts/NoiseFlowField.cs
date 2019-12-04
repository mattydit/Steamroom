using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFlowField : MonoBehaviour
{
    FastNoise fn;
    public Vector3Int gridSize;
    public float increment;
    public Vector3 offset, offsetSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnDrawGizmos()
    {
        fn = new FastNoise();

        float xOff = 0f;
        for(int x = 0; x < gridSize.x; x++)
        {
            float yOff = 0f;
            for(int y = 0; y < gridSize.y; y++)
            {
                float zOff = 0f;
                for(int z = 0; z < gridSize.z; z++)
                {
                    float noise = fn.GetSimplex(xOff + offset.x, yOff + offset.y, zOff + offset.z) + 1;
                    Vector3 noiseDirection = new Vector3(Mathf.Cos(noise * Mathf.PI), Mathf.Sin(noise * Mathf.PI), Mathf.Cos(noise * Mathf.PI));

                    Gizmos.color = new Color(noiseDirection.normalized.x, noiseDirection.normalized.y, noiseDirection.normalized.z, 1f);
                    Vector3 pos = new Vector3(x, y, z) + transform.position;
                    Vector3 endpos = pos + Vector3.Normalize(noiseDirection);
                    Gizmos.DrawLine(pos, endpos);
                    zOff += increment;
                }
                yOff += increment;
            }
            xOff += increment;
        }
    }
}
