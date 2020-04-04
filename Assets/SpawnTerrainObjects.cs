using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTerrainObjects : MonoBehaviour
{
    public GameObject[] terrainObjects;
    public int spawnSize = 20;

    private Vector3 min;
    private Vector3 max;

    // Start is called before the first frame update
    void Start()
    {
        min = GetComponent<MeshFilter>().mesh.bounds.min;
        max = GetComponent<MeshFilter>().mesh.bounds.max;

        for (int i = 0; i < spawnSize; i++)
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
