/*
 * 
 *   TrainBase Update Manager Version 1 from 31.08.2018 written by Michael Kux
 * 
*/
using System;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;

public class UpdateManager : MonoBehaviour {

    public SettingsManager settings;
    public StartUpManager StartManager;
    public LogWriterManager Logger;
    public ProgrammSettings PM;
    public string Version = "";
    public string OnlineVersion = "";
    public string UpdateURL = "";
    public string CurrDBVer = "";
    public string Patcher = "";
    public string NewsText = "";
    public string OnlineSystemBuild = "";
    public Image ReadOn;
    public bool Update;
    public GameObject UpdateElement;
    public Text NewsInhalt;
    public Text VersionText;
    public Text CurrVersion;

    void Start ()
    {
        Version = PM.Version;
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Update Manager -> Message is Normal.");
        }

        if (settings.AutoUpdateBool == true)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Update :: Update Ceck is Enabled ");
            }
            StartCoroutine(CheckVersion());
            StartCoroutine(CheckNews());
            RefreschScreen();
        }
        else
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Update :: Update Ceck is Disabled ");
            }
            StartManager.SystemMeldung.color = Color.yellow;
            StartManager.SystemMeldung.text = ("Update Prüfung ist Deaktiviert.");
        }
    }

    public void RefreschScreen()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Update :: Set Current Screen ");
        }
        //StartManager.OnlineSystemBuild.GetComponent<Text>().text = StartManager.UpdateText.ToString();
        //StartManager.NewsInhalt.GetComponent<Text>().text = StartManager.NewsText.ToString();
        //StartManager.AktuellesSystemBuild.GetComponent<Text>().text = Version;
    }

    private IEnumerator CheckNews()
    {
        {
            WWW www = new WWW("https://" + UpdateURL + "/news.txt");
            yield return www;
            ReadOn.color = Color.blue;
            if (www.error != null)
            {
                ReadOn.color = Color.red;
                StartManager.SystemMeldung.color = Color.red;
                StartManager.SystemMeldung.text = ("Keine Aktive Internet Verbindung: Update Check Fehlgeschlagen.");
                //StartManager.UpdateElement.gameObject.SetActive(false);
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Update Manager :: ERROR by Get Update Data");
                    Logger.PrintLog("MODUL Update Manager :: No Internect Connection or Server is current Down.!");
                }
            }
            else
            {
                //ReadOn.color = Color.green;
                NewsText = www.text;
                NewsInhalt.GetComponent<Text>().text = www.text;
            }
        }
    }

    private IEnumerator CheckVersion()
    {
        {
            WWW www = new WWW("https://pastebin.com/raw/FsaKrV07");
            yield return www;
            ReadOn.color = Color.blue;
            if (www.error != null)
            {
                ReadOn.color = Color.red;
                StartManager.SystemMeldung.color = Color.red;
                StartManager.SystemMeldung.text = ("Keine Aktive Internet Verbindung: Update Check Fehlgeschlagen.");
                //StartManager.UpdateElement.gameObject.SetActive(false);
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Update Manager :: ERROR by Get Update Data");
                    Logger.PrintLog("MODUL Update Manager :: No Internect Connection or Server is current Down.!");
                }
            }
            else
            {
                //ReadOn.color = Color.green;
                //StartManager.UpdateText = www.text;
                VersionText.GetComponent<Text>().text = www.text;
                //StartManager.SystemMeldung.color = Color.yellow;
                OnlineVersion = www.text;
                //StartManager.SystemMeldung.text = ("Neue Online Version:  " + OnlineVersion);
                Update = true;
            }
        }
        EnableUpdateWindows();
    }

    public void EnableUpdateWindows()
    {
        if (OnlineVersion == Version)
        {
            //StartManager.SystemMeldung.color = Color.white;
            //StartManager.SystemMeldung.text = ("Aktuellste Version.");
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Update Manager :: TrainBaseV2 Version is Up to Date.!!");
            }
        }
        else
        {
            CurrVersion.text = PM.Version.ToString();
            UpdateElement.gameObject.SetActive(true);
            StartManager.SystemMeldung.text = ("Neue Version Gefunden: Build " + OnlineVersion);
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Update Manager :: You use ah Old Version please make ah Update");
            }
        }
    }

    public void CheckNewUpdate()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Update Manager :: Manuell Update Check");
        }
        StartCoroutine(SettingsCheckVersion());
    }

    private IEnumerator SettingsCheckVersion()
    {
        {
            WWW www = new WWW("https://pastebin.com/raw/FsaKrV07");
            yield return www;
            ReadOn.color = Color.blue;
            if (www.error != null)
            {
                ReadOn.color = Color.red;
                StartManager.SystemMeldung.color = Color.red;
                StartManager.SystemMeldung.text = ("Keine Aktive Internet Verbindung: Update Check Fehlgeschlagen.");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Update Manager :: ERROR by Get Update Data");
                    Logger.PrintLog("MODUL Update Manager :: No Internect Connection or Server is current Down.!");
                }
            }
            else
            {
                StartManager.SystemMeldung.color = Color.yellow;
                StartManager.SystemMeldung.text = ("Letzte Online Version:  " + OnlineVersion);
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Update Manager :: Letzte Online Version: " + OnlineVersion);
                }
            }
        }
    }
}