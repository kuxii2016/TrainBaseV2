using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using System.Threading;

public class Update_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;

    [Header("Elements")]
    public GameObject Updatewindows;
    public Text ThisVersion;
    public Text OnlineVersion;
    public Text NewsText;

    [Header("Workflow")]
    public string UpdateURL = "https://pastebin.com/raw/FsaKrV07";
    public bool Checked = false;
    public string OVersion;
    public string TVersion;
    public string ParsedOVersion;
    public string ParsedTVersion;


    void Start ()
    {
    }

    public IEnumerator CheckVersion()
    {
        {
            WWW www = new WWW(UpdateURL);
            yield return www;
            if (www.error != null)
            {
                startManager.Log("MODUL  Update_Manager :: Keine Verbindung zum Server", "MODUL Update_Manager :: No Connection to the Server");
                startManager.Error("CheckVersion(Update)", www.error.ToString());
            }
            else
            {
                OVersion = www.text;
                OnlineVersion.text = www.text;
                ParsedOVersion = OVersion.Replace(".", "");
            }
        }
    }

    private void Update()
    {
        if(Checked == true)
        {
            if (UserSettings.AutoUpdateCheck.isOn == true)
            {
                //EnableUpdateWindows();
            }
            else
            {
                startManager.Log("MODUL  Update_Manager :: Versions Check nicht Aktiv", "MODUL Update_Manager :: Version check not active");
                Checked = false;
            }
        }
    }

    public void ManuellCheck()
    {
        startManager.Notify("Check version", "Checked version", "cyan", "cyan");
        EnableUpdateWindows();
    }

    public void DisableWin()
    {
        Checked = false;
    }

    public void EnableUpdateWindows()
    {
    }

    public IEnumerator CheckNews()
    {
        {
            WWW www = new WWW("https://" + startManager.UpdateUrl + "/" + startManager.ProgrammVersion + "/news.txt");
            yield return www;
            if (www.error != null)
            {
                startManager.Error("CheckNews(Update)", www.error.ToString());
            }
            else
            {
                NewsText.text = www.text;
            }
        }
        Checked = true;
    }
}