using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int terrainSize = 10;

        Vector3[] vertices = this.transform.GetComponent<MeshFilter>().mesh.vertices;


        //Generate some random terrain
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices[i].x, Mathf.PerlinNoise(vertices[i].x / 5f, vertices[i].z / 5f) * 5f, vertices[i].z);
        }
        //you could start work with tiles while terrain generation and edit 4 vertices

        int tileCount = 5;
        float tileHeight;

        //for better results we ceep track of already placed tiles
        List<int> usedVerts = new List<int>();

        for (int i = 0; i < tileCount; i++)
        {
            //get a start point
            int vertIndex = Random.Range(0, vertices.Length);

            // array out of range ?
            if (vertIndex - (tileCount + 2) < 0)
            {
                Debug.Log("Out of Range");
            }
            

            //set new high
            tileHeight = vertices[vertIndex].y * 3;
            //if not already in use by a other tile
            for (int v = 0; v < usedVerts.Count; v++)
            {
                if (vertIndex == usedVerts[v])
                {
                    tileHeight = vertices[usedVerts[v]].y;
                    break;
                }
            }

            //add used verts to the list and update the mesh
            usedVerts.Add(vertIndex);
            vertices[vertIndex] = new Vector3(vertices[vertIndex].x, tileHeight, vertices[vertIndex].z);
            vertIndex -= 1;
            usedVerts.Add(vertIndex);
            vertices[vertIndex] = new Vector3(vertices[vertIndex].x, tileHeight, vertices[vertIndex].z);
            vertIndex -= terrainSize;
            usedVerts.Add(vertIndex);
            vertices[vertIndex] = new Vector3(vertices[vertIndex].x, tileHeight, vertices[vertIndex].z);
            vertIndex -= 1;
            usedVerts.Add(vertIndex);
            vertices[vertIndex] = new Vector3(vertices[vertIndex].x, tileHeight, vertices[vertIndex].z);
        }
        //bake
        this.transform.GetComponent<MeshFilter>().mesh.vertices = vertices;
        this.transform.GetComponent<MeshFilter>().mesh.RecalculateNormals();
        DestroyImmediate(this.GetComponent<MeshCollider>());
        this.gameObject.AddComponent<MeshCollider>();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
