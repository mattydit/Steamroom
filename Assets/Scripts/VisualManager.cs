using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualManager : MonoBehaviour
{
    public GameObject audioPeer;
    public GameObject flowField;
    public GameObject speaker;
    public GameObject NoiseFlowField;
    public bool usingSpeaker;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindSpeakers();

        if (audioPeer.GetComponent<AudioSource>().isPlaying)
        {
            flowField.SetActive(true);
            usingSpeaker = false;
        }
        else if (audioPeer.GetComponent<AudioSource>().isPlaying == false && speaker != null)
        {
            if (speaker.GetComponent<AudioSource>().isPlaying)
            {
                usingSpeaker = true;
                flowField.SetActive(true);
            }
        }
        else
        {
            flowField.SetActive(false);
        }



    }

    private void FindSpeakers()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("speaker"))
        {
            if (go.GetComponent<AudioSource>().isPlaying)
            {
                speaker = go;
            }
        }
    }


}
