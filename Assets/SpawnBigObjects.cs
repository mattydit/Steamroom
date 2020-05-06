using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBigObjects : MonoBehaviour
{
    private PhotonView photonView;

    public GameObject[] terrainObjects;
    public int spawnSize = 20;
    public MeshFilter meshFilter;
    public MeshCollider meshCollider;

    private Vector3 min;
    private Vector3 max;
    private Mesh mesh;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        meshFilter = GetComponentInParent<MeshFilter>();
        meshCollider = GetComponentInParent<MeshCollider>();
        mesh = meshFilter.mesh;
        min = mesh.bounds.min;
        max = mesh.bounds.max;

        photonView.RPC("RPC_SpawnObjects", RpcTarget.AllBuffered);
        
        //RPC_SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    void RPC_SpawnObjects()
    {
        for (int i = 0; i < spawnSize; i++)
        {
            int vertIndex = Random.Range(0, mesh.vertices.Length);

            Vector3 randomVertPos = mesh.vertices[vertIndex];
            Vector3 instancePos = transform.TransformPoint(randomVertPos);
            
            Vector3 normal = mesh.normals[vertIndex];
            Vector3 yOffsetPos = new Vector3(instancePos.x, instancePos.y - 0.6f, instancePos.z);
            //normalPos = meshCollider.ClosestPoint(normalPos);

            //Quaternion rotation = Quaternion.Euler(normal);

            GameObject go = terrainObjects[(int)Random.Range(0, terrainObjects.Length)];
            GameObject instance = Instantiate(go, yOffsetPos, Quaternion.identity, gameObject.transform);
            instance.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        }
    }
}
