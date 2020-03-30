using UnityEngine;
using Oculus.Platform;
using Oculus.Platform.Models;
using Photon.Realtime;

public class OculusAuth : MonoBehaviour
{
    public OvrAvatar myAvatar;

    private string oculusId;

    private void Awake()
    {
        myAvatar = GetComponent<OvrAvatar>();
        myAvatar.gameObject.SetActive(false);
        Core.AsyncInitialize().OnComplete(OnInitializationCallback);
    }

    private void Start()
    {
        
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
            myAvatar.oculusUserID = oculusId;
            myAvatar.gameObject.SetActive(true);
            GetUserProof();
            Debug.Log("ovrAvatar initialized = " + myAvatar.Initialized);
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
           
        }
    }
}