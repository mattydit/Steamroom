using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Oculus.Platform;
using Oculus.Platform.Models;

public class NetworkManager : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, IOnEventCallback
{
    public const byte InstantiateVrAvatarEventCode = 123;

    string gameVersion = "1";

    bool isConnecting;

    [SerializeField]
    private string roomName = "steamroomOculus";

    private string oculusId;
    private string username;

    public GameObject terrainObjects;

    private void Awake()
    {
        //myAvatar = GetComponent<OvrAvatar>();
        //myAvatar.gameObject.SetActive(false);
        Core.AsyncInitialize().OnComplete(OnInitializationCallback);
    }

    private void OnInitializationCallback(Message<PlatformInitialize> msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error during initialization. Error Message: {0}",
                msg.GetError().Message);
        }
        else
        {
            Entitlements.IsUserEntitledToApplication().OnComplete(OnIsEntitledCallback);
        }
    }

    private void OnIsEntitledCallback(Message msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error verifying the user is entitled to the application. Error Message: {0}",
                msg.GetError().Message);
        }
        else
        {
            GetLoggedInUser();
        }
    }

    private void GetLoggedInUser()
    {
        Users.GetLoggedInUser().OnComplete(OnLoggedInUserCallback);
    }

    private void OnLoggedInUserCallback(Message<User> msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error getting logged in user. Error Message: {0}",
                msg.GetError().Message);
        }
        else
        {
            oculusId = msg.Data.ID.ToString(); // do not use msg.Data.OculusID;
            Debug.Log(oculusId);
            Debug.Log(msg.Data.OculusID);
            username = msg.Data.OculusID;
            GetUserProof();
        }
    }

    private void GetUserProof()
    {
        Users.GetUserProof().OnComplete(OnUserProofCallback);
    }

    private void OnUserProofCallback(Message<UserProof> msg)
    {
        if (msg.IsError)
        {
            Debug.LogErrorFormat("Oculus: Error getting user proof. Error Message: {0}",
                msg.GetError().Message);
        }
        else
        {
            string oculusNonce = msg.Data.Value;
            // Photon Authentication can be done here

            PhotonNetwork.AuthValues = new AuthenticationValues();
            PhotonNetwork.AuthValues.UserId = oculusId;
            PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Oculus;
            PhotonNetwork.AuthValues.AddAuthParameter("userid", oculusId);
            PhotonNetwork.AuthValues.AddAuthParameter("nonce", oculusNonce);
            PhotonNetwork.NickName = username;

            Connect();
            
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        //Connect();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConnected()
    {
        
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Yurt");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        
    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        
    }

    public void OnDisconnected(DisconnectCause cause)
    {
        
    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        
    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        
    }

    public void OnCreatedRoom()
    {
        
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        
    }

    public void OnJoinedRoom()
    {
        GameObject localAvatar = Instantiate(Resources.Load("LocalAvatar")) as GameObject;
        PhotonView photonView = localAvatar.GetComponent<PhotonView>();
        OvrAvatar ovrAvatarlocal = localAvatar.GetComponent<OvrAvatar>();

        ovrAvatarlocal.oculusUserID = oculusId;
        Debug.Log(Users.GetLoggedInUser().ToString());

        if (PhotonNetwork.AllocateViewID(photonView))
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions
            {
                CachingOption = EventCaching.AddToRoomCache,
                Receivers = ReceiverGroup.Others
            };

            PhotonNetwork.RaiseEvent(InstantiateVrAvatarEventCode, photonView.ViewID, raiseEventOptions, SendOptions.SendReliable);
        }
        else
        {
            Debug.LogError("Failed to allocate a ViewID");

            Destroy(localAvatar);
        }
        //terrainObjects.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            terrainObjects.GetComponent<PhotonView>().RPC("RPC_SpawnObjects", RpcTarget.AllBuffered);
        }
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        Photon.Realtime.RoomOptions roomOptions = new Photon.Realtime.RoomOptions();
        roomOptions.PublishUserId = true;
        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    public void OnLeftRoom()
    {
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == InstantiateVrAvatarEventCode)
        {
            GameObject remoteAvatar = Instantiate(Resources.Load("RemoteAvatar")) as GameObject;
            PhotonView photonView = remoteAvatar.GetComponent<PhotonView>();
            OvrAvatar ovrAvatarRemote = remoteAvatar.GetComponent<OvrAvatar>();
            TMPro.TextMeshPro playername = remoteAvatar.GetComponentInChildren<TMPro.TextMeshPro>();
            photonView.ViewID = (int)photonEvent.CustomData;
            ovrAvatarRemote.oculusUserID = photonView.Owner.UserId;
            playername.text = photonView.Owner.NickName;
        }
    }

    public void Connect()
    {
        isConnecting = true;

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

}
