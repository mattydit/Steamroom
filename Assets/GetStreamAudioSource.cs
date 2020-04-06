using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.PUN;

public class GetStreamAudioSource : MonoBehaviour
{
    public AudioSource streamAudioSrc;
    public StreamAudio audioStream;
    public GameObject speaker;
    public AudioPeer audioPeer;

    // Start is called before the first frame update
    void Start()
    {
        speaker = GameObject.FindGameObjectWithTag("speaker");
    }

    // Update is called once per frame
    void Update()
    {
        if (audioStream.isStreaming == true)
        {
            streamAudioSrc = speaker.GetComponent<AudioSource>();
            audioPeer.audioSrc = streamAudioSrc;
        }
    }
}
