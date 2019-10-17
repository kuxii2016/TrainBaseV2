/*
 * 
 *   TrainBase Backup Manager Version 1 from 28.05.2019 written by Michael Kux
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

public class BackUpManager : MonoBehaviour
{
    [Header("NEEDS")]
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public LogWriterManager Logger;
    public BackUpManager IM;
    public Text BackupFolder;
    public int FoundRows = 0;
    public int selectedID;
    public int unselectedID;
    public Button[] slots;
    public Image ReadOn;
    public string Reader = "";
    public GameObject RestoreWindow;
    public Text RestoreText;
    public string Handler;
    
    void Start ()
    {
        GetRows();
        selectedID = -1;
        BackupFolder.text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups: ";
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE BackupManager Manager -> Message is Normal.");
        }
    }

    public void GetRows()
    {
        ReadOn.color = Color.yellow;
        string[] imports = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/");
        for (int i = 0; i < imports.Length; i++)
        {
            IM.slots[i].GetComponentInChildren<Text>().text = Path.GetFileName(imports[i]);
            FoundRows = (i + 1);
            slots[i].gameObject.SetActive(true);
        }
    }

    public void SelectedID(int id)
    {
        for (int u = 0; u < 53; u++)
        {
            IM.slots[u].GetComponentInChildren<Text>().color = Color.black;
            GetRows();
            //unselectedID = u;
        }
        IM.slots[id].GetComponentInChildren<Text>().color = Color.blue;
        selectedID = id;
        RestoreWindow.gameObject.SetActive(true);
        RestoreText.text = "Dadenbank auf das Backup von:  " + IM.slots[id].GetComponentInChildren<Text>().text + "  wieder zurück setzten?";
        Handler = IM.slots[id].GetComponentInChildren<Text>().text;
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL BackupManager :: Import from  " + IM.slots[id].GetComponentInChildren<Text>().text);
        }
        Reader = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Backups/" + IM.slots[id].GetComponentInChildren<Text>().text);
    }

    public void NoFix()
    {
        IM.slots[selectedID].GetComponentInChildren<Text>().color = Color.black;
        selectedID = -1;
    }

    public void DeleteDB()
    {
        try
        {
            ReadOn.color = Color.yellow;
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + Handler.ToString());
            for(int i = 0; i <53; i++)
            {
                slots[i].gameObject.SetActive(false);
            }
            GetRows();
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL BackupManager :: try Delete Current Database. \n");
                sw.Close();
            }
        }
        catch (Exception ex)
        {
            ReadOn.color = Color.red;
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL BackupManager :: Error beim Löschen der Datenbank." + ex + "\n \n");
                sw.Close();
            }
        }
        finally
        {
            ReadOn.color = Color.green;
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL BackupManager :: Datenbank Gelöscht \n");
                sw.Close();
            }
        }
        StartManager.SystemMeldung.GetComponent<Text>().color = Color.red;
        StartManager.SystemMeldung.GetComponent<Text>().text = ("Backup vom: " + Handler + " wurde Gelöscht.!");
    }

}