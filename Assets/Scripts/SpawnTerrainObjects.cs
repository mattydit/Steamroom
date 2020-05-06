using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTerrainObjects : MonoBehaviour
{
    public GameObject[] terrainObjects;
    public int spawnSize = 500;

    private Vector3 min;
    private Vector3 max;
    private Mesh mesh;
    

    // Start is called before the first frame update
    void Start()
    {
        mesh = GetComponentInParent<MeshFilter>().mesh;
        min = mesh.bounds.min;
        max = mesh.bounds.max;
        Random.InitState(125);
       
        for (int i = 0; i < spawnSize; i++)
        {
            int vertIndex = Random.Range(0, mesh.vertices.Length);

            Vector3 randomVertPos = mesh.vertices[vertIndex];
            Vector3 instancePos = transform.TransformPoint(randomVertPos);
            Vector3 normal = mesh.normals[vertIndex];
            Quaternion rotation = Quaternion.Euler(normal);

            //Vector3 offsetInstancePos = new Vector3(instancePos.x, instancePos.y - 0.05f, instancePos.z);

            GameObject go = terrainObjects[(int)Random.Range(0, terrainObjects.Length)];
            GameObject instance = Instantiate(go, instancePos, Quaternion.identity, gameObject.transform);
            instance.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            instance.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
}
