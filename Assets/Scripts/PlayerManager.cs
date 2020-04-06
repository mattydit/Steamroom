using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;
    //public GameObject avatar;

    public Transform playerGlobal;
    public Transform playerLocal;
    public Transform trackingSpace;

    void Start()
    {
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            PlayerManager.LocalPlayerInstance = this.gameObject;

            playerGlobal = GameObject.Find("OVRPlayerController").transform;
            playerLocal = playerGlobal.Find("OVRCameraRig/TrackingSpace/CenterEyeAnchor");
            trackingSpace = playerGlobal.Find("OVRCameraRig/TrackingSpace");
            this.transform.SetParent(trackingSpace);
            this.transform.localPosition = Vector3.zero;
        }

        //DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
