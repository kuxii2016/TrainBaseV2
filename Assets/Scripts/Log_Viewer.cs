using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Log_Viewer : MonoBehaviour {

    public GameObject LogWindow;
    public Text InputText;
    public Start_Manager startManager;

	void Update ()
    {
        if (startManager.WriteLog == true && Input.GetKeyDown(KeyCode.F12))
        {
            ReadInput();
            LogWindow.gameObject.SetActive(true);
        }
    }

    public void ReadInput()
    {
        InputText.text = File.ReadAllText(startManager.LogPath + "last.log");
    }
}
