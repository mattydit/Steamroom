using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStreamAudioClip : MonoBehaviour
{
    public AudioSource streamAudioSrc;
    public GameObject speaker;
    public AudioPeer audioPeer;
    public AudioSource localAudioSrc;
    public StreamAudio streamAudio;

    // Start is called before the first frame update
    void Start()
    {
        audioPeer = GetComponent<AudioPeer>();
        localAudioSrc = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (speaker == null)
        {
            speaker = GameObject.FindGameObjectWithTag("speaker");
            if (speaker != null)
            {
                streamAudioSrc = speaker.GetComponent<AudioSource>();
            }
        }

        if (speaker != null)
        {
            if (streamAudioSrc.isPlaying == true && localAudioSrc.clip == null)
            {
                localAudioSrc.clip = streamAudioSrc.clip;
                //localAudioSrc.mute = true;
                audioPeer.audioSwitchedOn = true;

                if (streamAudio.isStreaming == false)
                {
                    localAudioSrc.mute = true;
                }
            }
        }
       
    }
}
