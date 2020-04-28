using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCubes : MonoBehaviour
{
    public GameObject cube;
    //public int poolSize = 100;
    public float spawnRate;
    public GameObject terrain;
    public bool spawnCubes;

    private Vector3 min;
    private Vector3 max;

    // Start is called before the first frame update
    void Start()
    {
        min = terrain.GetComponent<MeshFilter>().mesh.bounds.min;
        max = terrain.GetComponent<MeshFilter>().mesh.bounds.max;
        min = min * 100;
        max = max * 100;
        Debug.Log(min);
        Debug.Log(max);
        StartCoroutine(SpawnCubes());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnCubes()
    {
        while (spawnCubes == true)
        {
            Vector3 position = new Vector3(Random.Range(min.x, max.x), 1, Random.Range(min.z, max.z));
            //Debug.Log(position);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(cube, position, spawnRotation, gameObject.transform);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
