using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerID : MonoBehaviour
{
    public string oculusID;
    public OvrAvatar remoteAvatar;
    public PhotonView photonView;

    // Start is called before the first frame update
    void Awake()
    {
        remoteAvatar = GetComponent<OvrAvatar>();
        photonView = GetComponent<PhotonView>();

        oculusID = photonView.Owner.UserId;
        //Debug.Log("RemoteAvatar owner UserID = " + oculusID);
        remoteAvatar.oculusUserID = oculusID;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
