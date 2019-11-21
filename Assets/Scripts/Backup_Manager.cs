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

public class Backup_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    [Header("Elements")]
    public GameObject[] Slots;
    public Text Page;
    public int PageOffset = 0;
    public int PageOffset2 = 12;
    public int CurrentPage = 1;
    public int SelectedID = -1;

    void Start()
    {
        startManager.Log("Lade Backup_Manager -> Nachricht ist Normal.", "Load Backup_Manager -> message is normal");
        ClearScreen();
        FindBackups();
    }

    void Update()
    {

    }

    public void FindBackups()
    {
        string[] imports = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/");
        for (int i = PageOffset; i < imports.Length && i < PageOffset2; i++)
        {
            Slots[i - PageOffset].GetComponentInChildren<Text>().text = Path.GetFileName(imports[i]);
            Slots[i - PageOffset].gameObject.SetActive(true);
        }
    }

    void ClearScreen()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].SetActive(false);
        }
    }

    public void PageVorward()
    {
        string[] imports = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/");
        if (imports.Length >= PageOffset)
        {
            ClearScreen();
            PageOffset2 = PageOffset2 + 12;
            PageOffset = PageOffset + 12;
            CurrentPage = CurrentPage + 1;
            Page.text = CurrentPage.ToString();
            FindBackups();
        }
        else
        {
            ClearScreen();
            PageOffset2 = 12;
            PageOffset = 0;
            CurrentPage = 1;
            Page.text = CurrentPage.ToString();
            FindBackups();
        }
    }

    public void PageBack()
    {
        if (PageOffset == 0)
        {
        }
        else
        {
            ClearScreen();
            PageOffset2 = PageOffset2 - 12;
            PageOffset = PageOffset - 12;
            CurrentPage = CurrentPage - 1;
            Page.text = CurrentPage.ToString();
            FindBackups();
        }
    }

    public void CreateBackup()
    {
        try
        {
            if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year))
            {
            }
            else
            {
                File.Copy(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year);
            }
        }
        catch (Exception ex)
        {
            startManager.Notify("Backup wurde nicht Erstellt", "Backup not Creadet", "red", "red");
            startManager.LogError("Backup wurde nicht Erstellt.", "Backup not Creadet", " Backup_Manager :: CreateBackup(); Error: " + ex);
            startManager.Error("CreateBackup(Backup);",  "" + ex);
        }
        finally
        {
            startManager.Notify("Backup wurde Erstellt", "Backup Creadet", "cyan", "cyan");
        }
        FindBackups();
    }

    public void Selected(int id)
    {
        SelectedID = (id + PageOffset);
    }

    public void UnSelected()
    {
        SelectedID = -1;
    }

    public void Delete()
    {
        File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + Slots[SelectedID].GetComponentInChildren<Text>().text);
        SelectedID = -1;
        ClearScreen();
        startManager.Notify("Backup wurde Gelöscht", "Backup Deleted", "red", "red");
    }

    public void ReCreate()
    {
        try
        {
            string connection = "URI=file:" + System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db";
            IDbConnection dbcon = new SqliteConnection(connection);
            dbcon.Close();
            dbcon = null;
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db");
            Debug.Log("Step 1");
            File.Copy(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + Slots[SelectedID].GetComponentInChildren<Text>().text, System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db");
        }
        catch (Exception ex)
        {
            startManager.Notify("Kann Datenbank nicht Überschreiben", "Can not Replace the Database", "red", "red");
            startManager.LogError("Backup wurde nicht Wieder Hergestellt.", "Backup not ReCreadet", " Backup_Manager :: ReCreate(); Error: " + ex);
            startManager.Error("ReCreate(Backup);", "" + ex);
        }
        finally
        {

        }
    }
}