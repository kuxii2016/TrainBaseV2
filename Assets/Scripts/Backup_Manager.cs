﻿using System;
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
    public string[] Backup;
    public string BackupS;
    public string NameOfPath;

    void Start()
    {
        startManager.Log("Lade Backup_Manager -> Nachricht ist Normal.", "Load Backup_Manager -> message is normal");
        NameOfPath = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db").Replace("\\", "/");
        ClearScreen();
        FindBackups();
        Debug.Log(SystemInfo.operatingSystemFamily);
    }

    void Update()
    {

    }

    public void FindBackups()
    {
        string[] imports = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/");
        for (int i = PageOffset; i < imports.Length && i < PageOffset2; i++)
        {
            Backup[i - PageOffset] = Path.GetFileName(imports[i]).ToString();
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
        SelectedID = (id);
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
        if (SystemInfo.operatingSystemFamily.ToString() == "Windows")
        {
            try
            {
                File.Copy(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + Slots[SelectedID].GetComponentInChildren<Text>().text, System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db-rename");
            }
            catch (Exception ex)
            {
                startManager.LogError("Backup wurde nicht Wieder Hergestellt.", "Backup not ReCreadet", " Backup_Manager :: ReCreate(); Error: " + ex);
                startManager.Error("ReCreate(Backup);", "" + ex);
            }
            finally
            {

                startManager.Notify("Datenbank muss unter Win, Manuell umbenannt Werden.!", "Can not Replace the Database", "red", "red");
                startManager.Log("Sorry Windows nutzer, Leider muss das Backup Manuell wieder Hergestellt werden, \nDazu einfach in den Datenbank Ordner von TrainBaseV2 die Alte datenbank Loeschen \n(Programm muss Geschlossen sein, und von der anderen das -rename Loeschen.", "");
            }
        }
        else // Unix Have ah other Funtion under Unix will work This :D
        {
            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(100);
                File.Delete(NameOfPath);
            }
            catch (Exception ex)
            {
                startManager.Notify("Kann Datenbank nicht Überschreiben", "Can not Replace the Database", "red", "red");
                startManager.LogError("Backup wurde nicht Wieder Hergestellt.", "Backup not ReCreadet", " Backup_Manager :: ReCreate(); Error: " + ex);
                startManager.Error("ReCreate(Backup);", "" + ex);
            }
            finally
            {
                File.Copy(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + Slots[SelectedID].GetComponentInChildren<Text>().text, System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db");
                startManager.Notify("Datenbank zurück Gesetzt", "Database Replaced.", "green", "green");
                startManager.Log("Modul Backup_Manager :: Datenbank Erfolgreich zurück Gesetzt", "Modul Backup_Manager ::  Replaced");
            }
        }
    }
}