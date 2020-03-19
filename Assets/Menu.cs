using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject pointer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OpenMenu();   
    }

    public void OpenMenu()
    {
        if (OVRInput.GetDown(OVRInput.RawButton.B) || Input.GetKeyDown("escape"))
        {
            if (menuCanvas.activeSelf == false)
            {
                menuCanvas.SetActive(true);
                pointer.SetActive(true);
            }
            else if (menuCanvas.activeSelf == true)
            {
                menuCanvas.SetActive(false);
                pointer.SetActive(false);
            }


        }
    }
}
