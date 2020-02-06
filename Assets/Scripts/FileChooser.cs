using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GracesGames.SimpleFileBrowser.Scripts
{
    public class FileChooser : MonoBehaviour
    {
        public GameObject fileBrowserPrefab;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OpenFileBrowser(FileBrowserMode fileBrowserMode)
        {
            GameObject fileBrowserObject = Instantiate(fileBrowserPrefab, transform);
            fileBrowserObject.name = "FileBrowser";

            //set the mode
            FileBrowser fileBrowserscript = fileBrowserObject.GetComponent<FileBrowser>();

            fileBrowserscript.SetupFileBrowser(ViewMode.Landscape);

            fileBrowserscript.OpenFilePanel(FileExtensions);
        }
    }
}
