using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NoiseFlowField))]
public class AudioFlowField : MonoBehaviour
{
    NoiseFlowField noiseFlowfield;
    public AudioPeer audioPeer;
    public VisualManager visualManager;
    public SpeakerAudioAnalyzer speakerAA;

    AudioSource audioSrc;

    //Speed
    public bool useSpeed;
    public Vector2 moveSpeedMinMax, rotateSpeedMinMax;

    //Scale
    public bool useScale;
    public Vector2 scaleMinMax;

    //Material
    public Material mat;
    private Material[] audioMat;
    public bool useColour1;
    public bool useColour2;
    public Gradient gradient1;
    public Gradient gradient2;
    private Color[] colour1;
    private Color[] colour2;
    [Range(0f, 1f)]
    public float colourThreshold1;
    public float colourMultiplier1;
    [Range(0f, 1f)]
    public float colourThreshold2;
    public float colourMultiplier2;
    public string colourName1;
    public string colourName2;

    // Start is called before the first frame update
    void Start()
    {
        audioSrc = audioPeer.GetComponent<AudioSource>();
        noiseFlowfield = GetComponent<NoiseFlowField>();

        int countBand = 0;

        for (int i = 0; i < noiseFlowfield.amountOfParticles; i++)
        {
            int band = countBand % 8;
            //noiseFlowfield.particleMeshRenderer[i].material = audioMat[band];
            noiseFlowfield.particles[i].audioBand = band;
            countBand++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (audioPeer.audioSwitchedOn == true && audioSrc.isPlaying)
        {
            if (useSpeed)
            {
                noiseFlowfield.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, audioPeer.amplitudeBuffer) / 2;
                noiseFlowfield.particleRotSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, audioPeer.amplitudeBuffer) / 2;
            }

            for (int i = 0; i < noiseFlowfield.amountOfParticles; i++)
            {
                if (useScale)
                {
                    float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, audioPeer.audioBandBuffer[noiseFlowfield.particles[i].audioBand]) / 2;
                    noiseFlowfield.particles[i].transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
        else if (visualManager.usingSpeaker == true)
        {
            if (useSpeed)
            {
                noiseFlowfield.particleMoveSpeed = Mathf.Lerp(moveSpeedMinMax.x, moveSpeedMinMax.y, speakerAA.amplitudeBuffer) / 2;
                noiseFlowfield.particleRotSpeed = Mathf.Lerp(rotateSpeedMinMax.x, rotateSpeedMinMax.y, speakerAA.amplitudeBuffer) / 2;
            }

            for (int i = 0; i < noiseFlowfield.amountOfParticles; i++)
            {
                if (useScale)
                {
                    float scale = Mathf.Lerp(scaleMinMax.x, scaleMinMax.y, speakerAA.audioBandBuffer[noiseFlowfield.particles[i].audioBand]) / 2;
                    noiseFlowfield.particles[i].transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        }
    }

       
}
