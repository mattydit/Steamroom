using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class PlayerNameInput : MonoBehaviour
{
    const string playerNamePrefKey = "PlayerName";
    

    // Start is called before the first frame update
    void Start()
    {
        string defaultName = "default";
        InputField _inputField = this.GetComponent<InputField>();

        if (_inputField != null)
        {
            if(PlayerPrefs.HasKey(playerNamePrefKey))
            {
                Debug.Log("Key FOUND");
                defaultName = PlayerPrefs.GetString(playerNamePrefKey);
                _inputField.text = defaultName;
            }
            else
            {
               Debug.Log("Key NOT FOUND");
                //PlayerPrefs.SetString(playerNamePrefKey, "");
            }

            
        }

        PhotonNetwork.NickName = defaultName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerName(string value)
    {
        
        if(string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player name is empty");
            return;
        }
        
        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString(playerNamePrefKey, value);

        Debug.Log(playerNamePrefKey);
    }
}
