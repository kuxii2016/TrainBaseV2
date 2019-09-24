/*
 * 
 *   TrainBase Inventory Manager Version 1 from 18.07.2018 written by Michael Kux
 *    *    *   Last Edit 31.08.2018
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
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using DiscordPresence;

[System.Serializable]
public class InventoryData : System.Object
{
    public string dbDatum;
    public string dbStueck;
    public string dbBeschreibung;
    public string dbArtkNR;
    public int dbPreis;
}

public class InventoryManager : MonoBehaviour
{

    public LogWriterManager Logger;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public SettingsManager USettings;
    public Network_Handler NH;
    public SettingsManager SM;
    public Discord_Menue_Update DMU;
    [Header("Inventory List Items")]
    public List<InventoryData> InVD;
    public GameObject[] Slots;
    public GameObject[] Datum;
    public GameObject[] Stuekzahl;
    public GameObject[] Beschreibung;
    public GameObject[] ArtkNR;
    public GameObject[] Preis;
    public Toggle[] DeleteEditToggle;
    public bool[] Select;
    public int SelectedID = 0;
    public int PageID = 0;
    public int Items;
    public int RcpItems;
    public Button ADD;
    public Button Remove;
    public Button Edit;
    [Header("Inventory add Items")]
    public InputField ARTIKELNUMMER;
    public InputField STUECKZAHL;
    public InputField BESCHREIBUNG;
    public InputField PREIS;
    public int CurrentLimit;
    [Header("Inventory Edit Items")]
    public InputField EARTIKELNUMMER;
    public InputField ESTUECKZAHL;
    public InputField EBESCHREIBUNG;
    public InputField EPREIS;
    [Header("Seiten View Elements")]
    public Button Page1Back;
    public Button Page1Vorwd;
    public Button Page2Back;
    public Button Page2Vorwd;
    public Button Page3Back;
    public Button Page3Vorwd;
    public Button Page4Back;
    public Button Page4Vorwd;
    public Button Page5Back;
    public Button Page5Vorwd;
    public Button Page6Back;
    public Button Page6Vorwd;
    public Button Page7Back;
    public Button Page7Vorwd;
    public Button Page8Back;
    public Button Page8Vorwd;
    public Button Page9Back;
    public Button Page9Vorwd;
    public Button Page10Back;
    public Button Page10Vorwd;
    public Text PageNumber;
    public Text SeitenSumme;
    public int SiteSumme;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Inventory_Manager List -> Message is Normal.");
            Logger.PrintLogEnde();
        }
        ReadAllItems();
        readIntervall();
        CleanScreen();
        RefreschSumme();
        RcpItems = SM.ImportRPC;
    }

    void readIntervall()
    {
        for (int i = 0; i < InVD.Count; i++)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ITEM ID: " + (i + 1) + " | " + InVD[i].dbBeschreibung + " | " + InVD[i].dbArtkNR + " | " + InVD[i].dbStueck);
            }
            Items = i + 1;
        }
        Logger.PrintLogEnde();
    }

    public void RefreschSumme()
    {
        for (int s = 0; s < InVD.Count; s++)
        {
            if (USettings.ErstelleInvenoryPriceBool == true)
            {
                SiteSumme += (Convert.ToInt16(InVD[s].dbStueck) * InVD[s].dbPreis);
                SeitenSumme.GetComponent<Text>().text = SiteSumme.ToString() + " €";
            }
        }
    }

    void Update()
    {
        if (PageID == 1)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 2)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 24;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 3)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 48;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 4)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 72;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 5)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 96;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 6)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 120;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 7)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 144;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 8)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 168;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 9)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 192;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }

        if (PageID == 10)
        {
            for (int d = 0; d < Select.Length; d++)
            {
                Select[d] = DeleteEditToggle[d].isOn;
                if (DeleteEditToggle[d].isOn)
                {
                    SelectedID = d + 216;
                    StartManager.SystemMeldung.color = Color.magenta;
                }
            }
        }
    }

    public void ReadAllItems()
    {
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    Items = InVD.Count;
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read all Items:  " + ex + "\n");
            }
        }
        dbConnection.Close();
        dbConnection = null;
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Items Loadet");
        }
    }

    public void ReadItemList1()
    {
        PageID = 1;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 0; i < InVD.Count && i < 24; i++)
                    {
                        Slots[i].gameObject.SetActive(true);
                        Datum[i].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 0)
                        {
                            Page1Vorwd.gameObject.SetActive(false);
                            PageNumber.gameObject.SetActive(false);
                            Page1Back.gameObject.SetActive(false);
                        }
                        if (InVD.Count >= 24)
                        {
                            Page1Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList2()
    {
        PageID = 2;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 24; i < InVD.Count && i < 48; i++)
                    {
                        Slots[i - 24].gameObject.SetActive(true);
                        Datum[i - 24].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 24].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 24].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 24].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 24].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 24)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page2Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 48)
                        {
                            Page2Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList3()
    {
        PageID = 3;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 48; i < InVD.Count && i < 72; i++)
                    {
                        Slots[i - 48].gameObject.SetActive(true);
                        Datum[i - 48].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 48].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 48].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 48].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 48].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 48)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page3Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 72)
                        {
                            Page3Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList4()
    {
        PageID = 4;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 72; i < InVD.Count && i < 96; i++)
                    {
                        Slots[i - 72].gameObject.SetActive(true);
                        Datum[i - 72].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 72].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 72].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 72].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 72].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 72)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page4Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 96)
                        {
                            Page4Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList5()
    {
        PageID = 5;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 96; i < InVD.Count && i < 120; i++)
                    {
                        Slots[i - 96].gameObject.SetActive(true);
                        Datum[i - 96].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 96].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 96].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 96].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 96].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 96)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page5Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 120)
                        {
                            Page5Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList6()
    {
        PageID = 6;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 120; i < InVD.Count && i < 144; i++)
                    {
                        Slots[i - 120].gameObject.SetActive(true);
                        Datum[i - 120].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 120].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 120].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 120].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 120].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 120)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page6Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 144)
                        {
                            Page6Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList7()
    {
        PageID = 7;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 144; i < InVD.Count && i < 168; i++)
                    {
                        Slots[i - 144].gameObject.SetActive(true);
                        Datum[i - 144].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 144].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 144].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 144].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 144].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 144)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page7Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 168)
                        {
                            Page7Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList8()
    {
        PageID = 8;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 168; i < InVD.Count && i < 192; i++)
                    {
                        Slots[i - 168].gameObject.SetActive(true);
                        Datum[i - 168].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 168].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 168].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 168].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 168].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 168)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page8Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 192)
                        {
                            Page8Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList9()
    {
        PageID = 9;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 192; i < InVD.Count && i < 216; i++)
                    {
                        Slots[i - 192].gameObject.SetActive(true);
                        Datum[i - 192].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 192].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 192].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 192].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 192].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 192)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page9Back.gameObject.SetActive(true);
                        }
                        if (InVD.Count >= 216)
                        {
                            Page9Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }

    public void ReadItemList10()
    {
        PageID = 10;
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        InVD = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    InVD.Add(inVD);
                    for (int i = 216; i < InVD.Count && i < 240; i++)
                    {
                        Slots[i - 216].gameObject.SetActive(true);
                        Datum[i - 216].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + InVD[i].dbDatum + "  ";
                        Stuekzahl[i - 216].GetComponent<Text>().text = " " + InVD[i].dbStueck + "  ";
                        Beschreibung[i - 216].GetComponent<Text>().text = " " + InVD[i].dbBeschreibung + "  ";
                        ArtkNR[i - 216].GetComponent<Text>().text = " " + InVD[i].dbArtkNR + "  ";
                        Preis[i - 216].GetComponent<Text>().text = " " + InVD[i].dbPreis + "  €";
                        if (InVD.Count >= 216)
                        {
                            Page1Vorwd.gameObject.SetActive(false);
                            PageNumber.gameObject.SetActive(false);
                            Page1Back.gameObject.SetActive(false);
                        }
                        if (InVD.Count >= 240)
                        {
                            Page1Vorwd.gameObject.SetActive(true);
                            PageNumber.gameObject.SetActive(true);
                        }
                    }
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Inventory Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: Page " + PageID + " Found " + InVD.Count + " Rows");
        }
    }


    //Ident Fix **
    public void DeleteItem()
    {
        SiteSumme = SiteSumme - InVD[SelectedID].dbPreis;
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        string sql = "DELETE FROM Inventory WHERE ARTKNR='" + InVD[SelectedID].dbArtkNR + "' AND STUECK='" + InVD[SelectedID].dbStueck + "' AND  BESCHREIBUNG='" + InVD[SelectedID].dbBeschreibung + "' AND PREIS='" + InVD[SelectedID].dbPreis + "'  ";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        Logger.Message("Eintrag Erfogreich Gelöscht", "ROT");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager ::  Item: " + InVD[SelectedID].dbBeschreibung + "wurde Gelöscht.!");
        }
        SelectedID = -1;
    }

    public void DeleteSchutz()
    {
        SelectedID = -1;
    }

    public void SaveItem()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Inventory (ARTKNR , STUECK , BESCHREIBUNG , PREIS) VALUES" + " (@ARTKNR , @STUECK , @BESCHREIBUNG ,  @PREIS)";
            command.Parameters.AddWithValue("@ARTKNR", ARTIKELNUMMER.text);
            command.Parameters.AddWithValue("@STUECK", STUECKZAHL.text);
            command.Parameters.AddWithValue("@BESCHREIBUNG", BESCHREIBUNG.text);
            command.Parameters.AddWithValue("@PREIS", PREIS.text);
            if (Settings.InventoryLimit >= CurrentLimit)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    StartManager.SystemMeldung.color = Color.red;
                    StartManager.SystemMeldung.text = ("Error: 12 Write to Bank");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Inventory_Manager :: ItemADD: ERROR by Save new Item:" + ex + "\n");
                    }
                }
                finally
                {
                    dbConnection.Close();
                    StartManager.SystemMeldung.color = Color.green;
                    StartManager.SystemMeldung.text = ("ITEM: " + ARTIKELNUMMER.text + " | " + BESCHREIBUNG.text + " Stückzahl: " + STUECKZAHL.text + "  Gespeichert.!");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Inventory_Manager :: ItemADD: " + ARTIKELNUMMER.text + " | " + BESCHREIBUNG.text + " Stückzahl: " + STUECKZAHL.text + "  Gespeichert.!");
                    }
                }
            }
            else
            {
                StartManager.SystemMeldung.color = Color.red;
                StartManager.SystemMeldung.text = ("Fehler beim Speichern des Items Aktuelles limit ist bei 240 Items.!");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Inventory_Manager :: ItemADD: Current Item Limit is to Low for your Entry ");
                }
            }
        }
        SiteSumme = SiteSumme + int.Parse(PREIS.text);

        Logger.Message("Eintrag Erfolgreich Gespeichert", "CYAN");
    }

    public void AddTEXT()
    {
        Logger.Message("Neuen Eintrag Anlegen", "GELB");
    }

    public void RemoveTEXT()
    {
        StartManager.SystemMeldung.text = "Lösche ausgewählten Eintrag.!";
    }

    public void EditTEXT()
    {
        StartManager.SystemMeldung.text = "Bearbeite Ausgewählten Eintrag.!";
    }

    public void UpdateItem()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Inventory  SET ARTKNR = @ARTKNR , STUECK = @STUECK , BESCHREIBUNG = @BESCHREIBUNG , PREIS = @PREIS  WHERE ARTKNR='" + InVD[SelectedID].dbArtkNR + "' AND STUECK='" + InVD[SelectedID].dbStueck + "' AND  BESCHREIBUNG='" + InVD[SelectedID].dbBeschreibung + "' AND PREIS='" + InVD[SelectedID].dbPreis + "'  ";
            command.Parameters.AddWithValue("@ARTKNR", EARTIKELNUMMER.text);
            command.Parameters.AddWithValue("@STUECK", ESTUECKZAHL.text);
            command.Parameters.AddWithValue("@BESCHREIBUNG", EBESCHREIBUNG.text);
            command.Parameters.AddWithValue("@PREIS", EPREIS.text);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                StartManager.SystemMeldung.color = Color.red;
                StartManager.SystemMeldung.text = ("Fehler beim Datenlesen CODE:12");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Inventory_Manager ::  ERROR by Edit the Item: " + ex + "\n");
                }
            }
            finally
            {
                dbConnection.Close();
                StartManager.SystemMeldung.color = Color.green;
                StartManager.SystemMeldung.text = (InVD[SelectedID].dbBeschreibung + "  wurde Bearbeitet.!");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Inventory_Manager ::  " + InVD[SelectedID].dbBeschreibung + "  Edited.!");
                }
            }
        }
        SelectedID = -1;
        Logger.Message("Eintrag Geändert", "GRUEN");
    }

    public void GetItemData()
    {
        EARTIKELNUMMER.text = InVD[SelectedID].dbArtkNR;
        ESTUECKZAHL.text = InVD[SelectedID].dbStueck;
        EBESCHREIBUNG.text = InVD[SelectedID].dbBeschreibung;
        EPREIS.text = InVD[SelectedID].dbPreis.ToString();
        PresenceManager.UpdatePresence("Edit Item", InVD[SelectedID].dbBeschreibung , DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void CleanScreen()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].gameObject.SetActive(false);
            DeleteEditToggle[i].isOn = false;
        }
        SelectedID = -1;
        PageNumber.gameObject.SetActive(false);
        Page1Back.gameObject.SetActive(false);
        Page1Vorwd.gameObject.SetActive(false);
        Page2Back.gameObject.SetActive(false);
        Page2Vorwd.gameObject.SetActive(false);
        Page3Back.gameObject.SetActive(false);
        Page3Vorwd.gameObject.SetActive(false);
        Page4Back.gameObject.SetActive(false);
        Page4Vorwd.gameObject.SetActive(false);
        Page5Back.gameObject.SetActive(false);
        Page5Vorwd.gameObject.SetActive(false);
        Page6Back.gameObject.SetActive(false);
        Page6Vorwd.gameObject.SetActive(false);
        Page7Back.gameObject.SetActive(false);
        Page7Vorwd.gameObject.SetActive(false);
        Page8Back.gameObject.SetActive(false);
        Page8Vorwd.gameObject.SetActive(false);
        Page9Back.gameObject.SetActive(false);
        Page9Vorwd.gameObject.SetActive(false);
        Page10Back.gameObject.SetActive(false);
        Page10Vorwd.gameObject.SetActive(false);
    }

    public void SendTrainToClient()
    {
        NH.TrySendTrainData("ITEM" + "," + InVD[SelectedID].dbArtkNR.ToString() + "," + InVD[SelectedID].dbStueck.ToString() + "," + InVD[SelectedID].dbBeschreibung.ToString() + "," + InVD[SelectedID].dbPreis.ToString() + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + ", " + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Inventory_Manager :: RPC-Send Lok");
        }
    }

    public void VerifyTrainData()
    {
        ADDRPCTRAIN();
    }
    
    public void ADDRPCTRAIN()
    {
        Logger.Message("Item Über RPC Emfangen", "LILA");
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Inventory(ARTKNR, STUECK, BESCHREIBUNG, PREIS ) VALUES " + "(@ARTKNR,  @STUECK,  @BESCHREIBUNG, @PREIS )";
            command.Parameters.AddWithValue("@ARTKNR", NH.CacheData1);
            command.Parameters.AddWithValue("@STUECK", NH.CacheData2);
            command.Parameters.AddWithValue("@BESCHREIBUNG", NH.CacheData3);
            command.Parameters.AddWithValue("@PREIS", Convert.ToInt32(NH.CacheData4));
            if (InVD.Count <= 240)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Logger.Message("Item Erfolgreich Empfangen, Fehler beim Update", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Inventory_Manager ::  RPC-Update Fehler beim Update " + ex + "\n");
                    }
                }
                finally
                {
                    RcpItems = +1;
                    dbConnection.Close();
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Inventory_Manager :: RPC-Update" + NH.CacheData0 + "  " + NH.CacheData3 + "  Saved.!");
                    }
                }
            }
            SelectedID = -1;
            ReadAllItems();
            Logger.Message("Item Erfolgreich Empfangen, und Gespeichert", "GRUEN");
            NH.CreateNewConnection();
            SaveSettings();
        }
    }

    public void SaveSettings()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Settings  SET IMPORTRPC= @IMPORTRPC";
            command.Parameters.AddWithValue("@IMPORTRPC", RcpItems);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Inventory_Manager :: ERROR by Save Settings: " + ex + "\n");
                    Debug.Log(ex);
                }
            }
            finally
            {
                dbConnection.Close();
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Inventory_Manager :: Save Finsch ");
                }
            }
        }
        SM.ReadSettings();
        RcpItems = SM.ImportRPC;
    }
}