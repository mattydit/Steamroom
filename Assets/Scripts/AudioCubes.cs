using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioCubes : MonoBehaviour
{
    public AudioPeer audioPeer;
    public VisualManager visualManager;
    public SpeakerAudioAnalyzer speakerAA;
    AudioSource audioSrc;

    public GameObject cube;
    public int poolSize = 100;
    public float spawnRate;
    public GameObject terrain;
    public bool spawnCubes;

    public List<GameObject> cubes;

    public Vector3 min;
    public Vector3 max;

    public Vector2 scaleMinMax;
    public int band;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = audioPeer.GetComponent<AudioSource>();
        cubes = new List<GameObject>();
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
        if (audioPeer.audioSwitchedOn == true && audioSrc.isPlaying)
        {
            foreach (GameObject go in cubes)
            {
                float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, audioPeer.audioBandBuffer[band]);
                go.transform.localScale = new Vector3(scale, scale, scale);
            }
        }
        else if (visualManager.usingSpeaker == true)
        {
            if (speakerAA == null)
            {
                speakerAA = visualManager.speaker.GetComponent<SpeakerAudioAnalyzer>();
            }
            else
            {
                foreach (GameObject go in cubes)
                {
                    float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, speakerAA.audioBandBuffer[band]);
                    go.transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }

    IEnumerator SpawnCubes()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Vector3 position = new Vector3(Random.Range(min.x, max.x), 1, Random.Range(min.z, max.z));
            //Debug.Log(position);
            Quaternion spawnRotation = Quaternion.identity;
            GameObject instance = Instantiate(cube, position, spawnRotation, gameObject.transform);
            cubes.Add(instance);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
