using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;

public class GetStreamAudioSource : MonoBehaviour
{
    public AudioSource streamAudioSrc;
    public GameObject speaker;
    public AudioPeer audioPeer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speaker == null)
        {
            speaker = GameObject.FindGameObjectWithTag("speaker");
        }

        
        if (speaker.GetComponent<AudioSource>().isPlaying == true && streamAudioSrc == null)
        {
            streamAudioSrc = speaker.GetComponent<AudioSource>();
            audioPeer.audioSrc = streamAudioSrc;
        }
        
        
    }
}
