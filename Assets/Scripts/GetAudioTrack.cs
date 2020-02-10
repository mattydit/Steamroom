using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetAudioTrack : MonoBehaviour
{
    string file = "file://D:/Music/Madvillain - Raid feat. MED.wav";
    public GameObject fileBrowser;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetAudioClip());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator GetAudioClip()
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(file, AudioType.WAV))
        {
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
            }
            else
            {
                AudioClip myclip = DownloadHandlerAudioClip.GetContent(www);
            }
        }

    }

    //private void OpenFileBrowser(FileBrowserMode fileBrowserMode)

}


