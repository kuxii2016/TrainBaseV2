/*
 * 
 *   TrainBase WagonView Manager Version 1 from 29.03.2018 written by Michael Kux
 *    *   Last Edit 31.08.2018
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
using DiscordPresence;

[System.Serializable]
public class WagonData : System.Object
{
    public int DBTyp;
    public string DBFarbe;
    public int DBHersteller;
    public string DBErstellt;
    public string DBKatalognummer;
    public string DBSeriennummer;
    public int DBKaufTag;
    public int DBKaufMonat;
    public int DBKaufJahr;
    public int DBPreis;
    public int DBKupplung;
    public int DBLicht;
    public int DBPreiser;
    public int DBSpurweite;
    public string DBIdentifyer;
    public int DBLagerort;
}

public class WagonView : MonoBehaviour
{
    public LogWriterManager Logger;
    public GameObject TrainList;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public SettingsManager Usettings;
    public Network_Handler NH;
    public Discord_Menue_Update DMU;
    [Header("Lok View - Elemente")]
    public List<WagonData> Trains;
    [Tooltip("SlotElemente zum Einschalten")]
    public GameObject[] Slot;
    [Tooltip("SlotElemente TextReihe 1")]
    public GameObject[] Slot1;
    [Tooltip("SlotElemente TextReihe 2")]
    public GameObject[] Slot2;
    [Tooltip("SlotElemente TextReihe 3")]
    public GameObject[] Slot3;
    [Tooltip("SelectedButtons")]
    public Toggle[] DeleteEditToggle;
    [Tooltip("Selected ID")]
    public int SelectedID = -1;
    [Tooltip("SlotElemente Picture")]
    public RawImage[] vBild;
    [Tooltip("Edit Picture")]
    public RawImage trainBild;
    [Tooltip("Edit Picture Text")]
    public Text trainText;
    [Tooltip("PageID")]
    public int PageID = 0;
    public bool[] Select;
    public int RowLoks = 0;
    public string[] vTyp;
    public string[] vHersteller;
    public string[] vTag;
    public string[] vMonat;
    public string[] vJahr;
    [Tooltip("Spurweite")]
    public string[] vSpur;
    public InputField Farbe;
    public Dropdown WagonTyp;
    public Dropdown Hersteller;
    public Dropdown Spurweite;
    public InputField Katalognummer;
    public InputField Seriennummer;
    public Dropdown KaufdatumTag;
    public Dropdown KaufdatumMonat;
    public Dropdown KaufdatumJahr;
    public InputField Preis;
    public Toggle Kupplung;
    public Toggle Licht;
    public Toggle Preiser;
    public Dropdown Lager;
    public Text GUUID;
    public Text TopWindow;
    public string EditFarbe;
    public int EditWagonTyp;
    public int EditHersteller;
    public int EditSpurweite;
    public string EditKatalognummer;
    public string EditSeriennummer;
    public int EditKaufdatumTag;
    public int EditKaufdatumMonat;
    public int EditKaufdatumJahr;
    public string EditPreis;
    public int EditKupplung;
    public int EditLicht;
    public int EditPreiser;
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
    private DatenExporter dataexporter;
    public Image ReadOn;
    public bool ShowImage = false;
    private string filePath1 = "";
    public string[] tmpGuid;
    public int counter = 0;
    public int RcpItems;
    [Header("WWW-Send")]
    public GameObject Win;
    public InputField SendOK;
    public Texture2D[] TrainPic;
    public string uniqueID = "";

    void Start()
    {
        ShowImage = Usettings.ZeigeLokbilderBool;
        dataexporter = new DatenExporter();
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Train List -> Message is Normal.");
        }
        ReadAllTrainsNew();

        if (Usettings.ImagesAutoReadBool == true)
        {
            GetTrainPictures();
        }
        readIntervall();
        CleanScreen();
        RowLoks = Trains.Count;
        RcpItems = Usettings.ImportRPC;
    }

    public void ReadAllTrainsNew()
    {
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            ReadOn.color = Color.blue;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.Error("MODUL Wagon_View :: ReadAllWagons(): " + ex + "\n");
            }
            Logger.Message("Fehler beim Laden der Wagonliste", "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        ReadCustomPic();
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
                }
            }
        }
    }

    void readIntervall()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Wagon_View :: Wagon ID: " + (i + 1) + " | " + vHersteller[Trains[i].DBHersteller] + " | " + (vSpur[Trains[i].DBSpurweite]) + " | " + Trains[i].DBKatalognummer);
            }
        }
        Logger.PrintLogEnde();
    }

    public void ReadCustomPic()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            StartCoroutine(setImage((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + Usettings.ImageType), i));
        }
        Logger.PrintLogEnde();

        Logger.PrintLog("MODUL Wagon_View :: Loadet " + (Trains.Count + 1) + " Train Pictures.");
    }

    public void GetTrainData1()
    {
        PageID = 1;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "1";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 0; i < Trains.Count && i < 24; i++)
                    {
                        Slot[i].gameObject.SetActive(true);
                        Slot1[i].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + (i + 1) + "." + Usettings.ImageType);
                            vBild[i].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count <= 24)
                        {
                            Page1Vorwd.gameObject.SetActive(false);
                            PageNumber.gameObject.SetActive(false);
                            Page1Back.gameObject.SetActive(false);
                        }
                        if (Trains.Count >= 24)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData2()
    {
        PageID = 2;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "2";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 24; i < Trains.Count && i < 48; i++)
                    {
                        Slot[i - 24].gameObject.SetActive(true);
                        Slot1[i - 24].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 24].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 24].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 24].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 24].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 24)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page2Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 48)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData3()
    {
        PageID = 3;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "3";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 48; i < Trains.Count && i < 72; i++)
                    {
                        Slot[i - 48].gameObject.SetActive(true);
                        Slot1[i - 48].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 48].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 48].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 48].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 48].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 48)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page3Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 72)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData4()
    {
        PageID = 4;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "4";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 72; i < Trains.Count && i < 96; i++)
                    {
                        Slot[i - 72].gameObject.SetActive(true);
                        Slot1[i - 72].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 72].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 72].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 72].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 72].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 72)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page4Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 96)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData5()
    {
        PageID = 5;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "5";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 96; i < Trains.Count && i < 120; i++)
                    {
                        Slot[i - 96].gameObject.SetActive(true);
                        Slot1[i - 96].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 96].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 96].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 96].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 96].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 96)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page5Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 120)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData6()
    {
        PageID = 6;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "6";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 120; i < Trains.Count && i < 144; i++)
                    {
                        Slot[i - 120].gameObject.SetActive(true);
                        Slot1[i - 120].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 120].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 120].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 120].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 120].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 120)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page6Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 144)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData7()
    {
        PageID = 7;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "7";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 144; i < Trains.Count && i < 168; i++)
                    {
                        Slot[i - 144].gameObject.SetActive(true);
                        Slot1[i - 144].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 144].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 144].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 144].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 144].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 144)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page7Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 168)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData8()
    {
        PageID = 8;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "8";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 168; i < Trains.Count && i < 192; i++)
                    {
                        Slot[i - 168].gameObject.SetActive(true);
                        Slot1[i - 168].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 168].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 168].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 168].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 168].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 168)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page8Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 192)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData9()
    {
        PageID = 9;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "9";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 192; i < Trains.Count && i < 216; i++)
                    {
                        Slot[i - 192].gameObject.SetActive(true);
                        Slot1[i - 192].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 192].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 192].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 192].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 192].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 192)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page9Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 216)
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData10()
    {
        PageID = 10;
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "10";
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Wagons", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    WagonData trainData = new WagonData();
                    trainData.DBTyp = reader.GetInt32(0);
                    trainData.DBFarbe = reader.GetString(1);
                    trainData.DBHersteller = reader.GetInt32(2);
                    trainData.DBErstellt = reader.GetString(3);
                    trainData.DBKatalognummer = reader.GetString(4);
                    trainData.DBSeriennummer = reader.GetString(5);
                    trainData.DBKaufTag = reader.GetInt32(6);
                    trainData.DBKaufMonat = reader.GetInt32(7);
                    trainData.DBKaufJahr = reader.GetInt32(8);
                    trainData.DBPreis = reader.GetChar(9);
                    trainData.DBKupplung = reader.GetInt32(10);
                    trainData.DBLicht = reader.GetInt32(11);
                    trainData.DBPreiser = reader.GetInt32(12);
                    trainData.DBSpurweite = reader.GetInt32(13);
                    trainData.DBIdentifyer = reader.GetString(16);
                    trainData.DBLagerort = reader.GetInt32(15);
                    Trains.Add(trainData);
                    for (int i = 216; i < Trains.Count && i < 240; i++)
                    {
                        Slot[i - 216].gameObject.SetActive(true);
                        Slot1[i - 216].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                        Slot2[i - 216].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                        Slot3[i - 216].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + vTag[Trains[i].DBKaufTag] + "." + vMonat[Trains[i].DBKaufMonat] + "" + vJahr[Trains[i].DBKaufJahr];
                        if (ShowImage == true)
                        {
                            //vBild[i - 216].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + ((i) + 1) + "." + Usettings.ImageType);
                            vBild[i - 216].GetComponent<RawImage>().texture = TrainPic[i];
                        }
                        if (Trains.Count >= 216)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page10Back.gameObject.SetActive(true);
                        }
                        if (Trains.Count >= 240)
                        {
                            Page10Vorwd.gameObject.SetActive(true);
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
                Logger.Error("MODUL Wagon_View :: GetWagonData" + PageID + "():  " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Wagonseite " + PageID + " an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void CleanScreen()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].gameObject.SetActive(false);
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

    public void GetLokData()
    {
        WagonTyp.value = Trains[SelectedID].DBTyp;
        Farbe.text = Trains[SelectedID].DBFarbe;
        Hersteller.value = Trains[SelectedID].DBHersteller;
        Spurweite.value = Trains[SelectedID].DBSpurweite;
        Katalognummer.text = Trains[SelectedID].DBKatalognummer;
        Seriennummer.text = Trains[SelectedID].DBSeriennummer;
        KaufdatumTag.value = Trains[SelectedID].DBKaufTag;
        KaufdatumMonat.value = Trains[SelectedID].DBKaufMonat;
        KaufdatumJahr.value = Trains[SelectedID].DBKaufJahr;
        Preis.text = Trains[SelectedID].DBPreis.ToString();
        Lager.value = Trains[SelectedID].DBLagerort;
        GUUID.text = Trains[SelectedID].DBIdentifyer;
        TopWindow.text = vHersteller[Trains[SelectedID].DBHersteller] + " " + Trains[SelectedID].DBKatalognummer + " Bearbeiten";
        filePath1 = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (SelectedID + 1) + "." + Usettings.ImageType);
        trainBild.GetComponent<RawImage>().texture = TrainPic[SelectedID];
        trainText.text = Katalognummer.text;
        if (Trains[SelectedID].DBKupplung == 1)
        {
            Kupplung.isOn = true;
        }
        if (Trains[SelectedID].DBLicht == 1)
        {
            Licht.isOn = true;
        }
        if (Trains[SelectedID].DBPreiser == 1)
        {
            Preiser.isOn = true;
        }
        Logger.Message("Lade Wagon Daten zum Bearbeiten", "GELB");
        PresenceManager.UpdatePresence("Edit Wagon", vHersteller[Trains[SelectedID].DBHersteller] + " " + Trains[SelectedID].DBKatalognummer, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DeleteTrain()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        string sql = "DELETE FROM Wagons WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer + "' AND  ERSTELLT='" + Trains[SelectedID].DBErstellt + "'  ";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Waggons/" + (SelectedID + 1) + ".png");
        StartManager.SystemMeldung.color = Color.red;
        StartManager.SystemMeldung.text = ("Wagon: " + Trains[SelectedID].DBKatalognummer + " In: " + Trains[SelectedID].DBFarbe + "wurde Gelöscht.!");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL WagonView ::  Wagon: " + Trains[SelectedID].DBKatalognummer + " Color= " + Trains[SelectedID].DBFarbe + " Created= " + Trains[SelectedID].DBErstellt + " Summe= " + Trains[SelectedID].DBPreis + " Deleted.!");
        }
        SelectedID = -1;
        CleanScreen();
        Logger.Message("Wagon wurde Gelöscht", "GRUEN");
    }

    public void DeleteSchutz()
    {
        SelectedID = -1;
    }

    public void UpdateTrain()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Wagons  SET FARBE = @FARBE , TYP = @Typ , HERSTELLER = @HERSTELLER , KATALOGNUMMER = @KATALOGNUMMER , SERIENNUMMER = @SERIENNUMMER , KAUFDAY = @KAUFDAY , KAUFMONAT = @KAUFMONAT ,KAUFJAHR = @KAUFJAHR , PREIS = @PREIS ,KUPPLUNG = @KUPPLUNG , LICHT = @LICHT , PREISER =@PREISER, SPURWEITE = @SPURWEITE, LAGERORT = @LAGERORT  WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer +  "' AND  ERSTELLT='" + Trains[SelectedID].DBErstellt +  "'  ";
            command.Parameters.AddWithValue("@FARBE", Farbe.text);
            command.Parameters.AddWithValue("@TYP", WagonTyp.value);
            command.Parameters.AddWithValue("@HERSTELLER", Hersteller.value);
            command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer.text);
            command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer.text);
            command.Parameters.AddWithValue("@PREIS", Preis.text);
            command.Parameters.AddWithValue("@KAUFDAY", KaufdatumTag.value);
            command.Parameters.AddWithValue("@KAUFMONAT", KaufdatumMonat.value);
            command.Parameters.AddWithValue("@KAUFJAHR", KaufdatumJahr.value);
            command.Parameters.AddWithValue("@KUPPLUNG", Kupplung.isOn);
            command.Parameters.AddWithValue("@LICHT", Licht.isOn);
            command.Parameters.AddWithValue("@PREISER", Preiser.isOn);
            command.Parameters.AddWithValue("@SPURWEITE", Spurweite.value);
            command.Parameters.AddWithValue("@LAGERORT", Lager.value);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Logger.Message("Fehler beim Bearbeiten des Wagons", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.Error("MODUL Wagon_View :: UpdateWagon():  " + ex + "\n");
                }
            }
            finally
            {
                dbConnection.Close();
                Logger.Message("Wagon wurde Erfolgreich bearbeited", "GRUEN");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Wagon_View ::  " + vHersteller[Trains[SelectedID].DBHersteller] + "'  = '" + Trains[SelectedID].DBKatalognummer + "'   = '" + Trains[SelectedID].DBErstellt + "  Edited.!");
                }
            }
        }
        SelectedID = -1;
        ReadAllTrainsNew();
    }

    public void ExportTrain()
    {
        dataexporter.Type = "WAGON";
        dataexporter.Typ = Trains[SelectedID].DBTyp;
        dataexporter.Farbe = Trains[SelectedID].DBFarbe;
        dataexporter.Erstellt = Trains[SelectedID].DBErstellt;
        dataexporter.Hersteller = Trains[SelectedID].DBHersteller;
        dataexporter.Spurweite = Trains[SelectedID].DBSpurweite;
        dataexporter.Katalognummer = Trains[SelectedID].DBKatalognummer;
        dataexporter.Seriennummer = Trains[SelectedID].DBSeriennummer;
        dataexporter.KaufTag = Trains[SelectedID].DBKaufTag;
        dataexporter.KaufMonat = Trains[SelectedID].DBKaufMonat;
        dataexporter.KaufJahr = Trains[SelectedID].DBKaufJahr;
        dataexporter.Preis = Trains[SelectedID].DBPreis;
        dataexporter.Kupplung = Trains[SelectedID].DBKupplung;
        dataexporter.Licht = Trains[SelectedID].DBLicht;
        dataexporter.Preiser = Trains[SelectedID].DBPreiser;
        dataexporter.Spurweite = Trains[SelectedID].DBSpurweite;
        string jsonData = JsonUtility.ToJson(dataexporter, true);
        File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Exporter/" + vHersteller[Trains[SelectedID].DBHersteller] + "-" + Trains[SelectedID].DBKatalognummer + "-" + Trains[SelectedID].DBFarbe + ".TRAIN", jsonData);
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Wagon: " + vHersteller[Trains[SelectedID].DBHersteller] + "-" + Trains[SelectedID].DBKatalognummer + "-" + Trains[SelectedID].DBFarbe + " is Final Exported.! ");
        }
        SelectedID = -1;
        Logger.Message("Wagon Exportiert, Ist im Export Ordner zu finden", "GRUEN");
    }

    public void GetTrainPictures()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: Check Wagon Images");
        }
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Wagons"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Wagons");
        }
        
        for (int i = 0; i < Trains.Count && i < 239; i++)
        {
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + Usettings.ImageType))
            {
                File.Copy(Application.streamingAssetsPath + "/Resources/Wagon.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + Usettings.ImageType);
                if (Logger.logIsEnabled == true)
                {
                    counter = (i + 1);
                }
            }
            else
            {
                counter = 0;
            }
        }
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: " + counter + " Images Created");
        }
    }

    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        return tex;
    }

    IEnumerator setImage(string url, int number)
    {
        Texture2D tex;
        tex = new Texture2D(2, 2, TextureFormat.DXT1, false);
        using (WWW www = new WWW("file:///" + url.Replace("\\", "/")))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            TrainPic[number] = tex;
        }
    }

    public void SendTrainToClient()
    {
        NH.TrySendTrainData("WAGON" + "," + Trains[SelectedID].DBTyp.ToString() + "," + Trains[SelectedID].DBFarbe.ToString() + "," + Trains[SelectedID].DBHersteller.ToString() + "," + Trains[SelectedID].DBKatalognummer.ToString() + "," + Trains[SelectedID].DBSeriennummer.ToString() + "," + Trains[SelectedID].DBKaufTag.ToString() + "," + Trains[SelectedID].DBKaufMonat.ToString() + "," + Trains[SelectedID].DBKaufJahr.ToString() + "," + Trains[SelectedID].DBPreis.ToString() + "," + Trains[SelectedID].DBKupplung.ToString() + "," + Trains[SelectedID].DBLicht.ToString() + "," + Trains[SelectedID].DBPreiser.ToString() + "," + Trains[SelectedID].DBSpurweite.ToString() + "," + Trains[SelectedID].DBIdentifyer.ToString() + "," + Trains[SelectedID].DBLagerort.ToString() + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Wagon_View :: RPC-Send Wagon");
        }
    }

    public void SendWWW()
    {
        uniqueID = DateTime.Now.ToString("HHmmssddMMyyyy");
        StartCoroutine(SendSelected());
    }

    public void verifyTrainData()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            if (Trains[i].DBIdentifyer == NH.CacheData14)
            {
                UPDATERPCTRAIN();
            }
        }
        ADDRPCTRAIN();
    }

    public void UPDATERPCTRAIN()
    {
        Logger.Message("Wagon Über RPC Emfangen", "LILA");
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Wagons  SET TYP = @TYP, FARBE = @FARBE, HERSTELLER = @HERSTELLER, KATALOGNUMMER = @KATALOGNUMMER, SERIENNUMMER = @SERIENNUMMER, KAUFDAY = @KAUFDAY, KAUFMONAT = @KAUFMONAT, KAUFJAHR = @KAUFJAHR, PREIS = @PREIS, KUPPLUNG = @KUPPLUNG, LICHT = @LICHT, PREISER = @PREISER, SPURWEITE = @SPURWEITE, LAGERORT = @LAGERORT, IDENTIFYER = @IDENTIFYER WHERE IDENTIFYER='" + NH.CacheData14 + "'  ";
            if (NH.CacheData1 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@TYP", Convert.ToInt32(NH.CacheData1));
            }
            else
            {

            }
            if (NH.CacheData2 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@FARBE", NH.CacheData2);
            }
            else
            {

            }
            if (NH.CacheData3 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@HERSTELLER", Convert.ToInt32(NH.CacheData3));
            }
            else
            {

            }
            if (NH.CacheData4 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KATALOGNUMMER", NH.CacheData4);
            }
            else
            {

            }
            if (NH.CacheData5 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SERIENNUMMER", NH.CacheData5);
            }
            else
            {

            }
            if (NH.CacheData6 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KAUFDAY", Convert.ToInt32(NH.CacheData6));
            }
            else
            {

            }
            if (NH.CacheData7 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KAUFMONAT", Convert.ToInt32(NH.CacheData7));
            }
            else
            {

            }
            if (NH.CacheData8 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KAUFJAHR", Convert.ToInt32(NH.CacheData8));
            }
            else
            {

            }
            if (NH.CacheData9 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PREIS", Convert.ToInt32(NH.CacheData9));
            }
            else
            {

            }
            if (NH.CacheData10 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KUPPLUNG", Convert.ToInt32(NH.CacheData10));
            }
            else
            {

            }
            if (NH.CacheData11 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@LICHT", Convert.ToInt32(NH.CacheData11));
            }
            else
            {

            }
            if (NH.CacheData12 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PREISER", Convert.ToInt32(NH.CacheData12));
            }
            else
            {

            }
            if (NH.CacheData13 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(NH.CacheData13));
            }
            else
            {

            }
            if (NH.CacheData15 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(NH.CacheData15));
            }
            else
            {

            }
            if (NH.CacheData14 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@IDENTIFYER", NH.CacheData14);
            }
            else
            {

            }
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
                ReadOn.color = Color.blue;
            }
            catch (SqliteException ex)
            {
                Logger.Message("Wagon Erfolgreich Empfangen, Fehler beim Update", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.Error("MODUL Wagon_View :: UPDATERPCWAGON():  " + ex + "\n");
                }
            }
            finally
            {
                RcpItems = +1;
                dbConnection.Close();
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Wagon_View :: RPC-Update" + NH.CacheData0 + " in: " + NH.CacheData2 + "  Upgedated.!");
                }
                ReadOn.color = Color.green;
            }
        }
        SelectedID = -1;
        ReadAllTrainsNew();
        Logger.Message("Wagon Erfolgreich Empfangen, und Upgedated", "GRUEN");
        NH.CreateNewConnection();
        SaveSettings();
    }

    public void ADDRPCTRAIN()
    {
        Logger.Message("Wagon Über RPC Emfangen", "LILA");
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Wagons (TYP, FARBE, HERSTELLER, KATALOGNUMMER, SERIENNUMMER, KAUFDAY, KAUFMONAT, KAUFJAHR, PREIS, KUPPLUNG, LICHT, PREISER, SPURWEITE, LAGERORT, IDENTIFYER) VALUES" + " (@TYP, @FARBE, @HERSTELLER, @KATALOGNUMMER, @SERIENNUMMER, @KAUFDAY, @KAUFMONAT, @KAUFJAHR, @PREIS, @KUPPLUNG, @LICHT, @PREISER, @SPURWEITE, @LAGERORT, @IDENTIFYER)";
            if (NH.CacheData1 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@TYP", Convert.ToInt32(NH.CacheData1));
            }
            else
            {

            }
            if (NH.CacheData2 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@FARBE", NH.CacheData2);
            }
            else
            {

            }
            if (NH.CacheData3 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@HERSTELLER", Convert.ToInt32(NH.CacheData3));
            }
            else
            {

            }
            if (NH.CacheData4 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KATALOGNUMMER", NH.CacheData4);
            }
            else
            {

            }
            if (NH.CacheData5 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SERIENNUMMER", NH.CacheData5);
            }
            else
            {

            }
            if (NH.CacheData6 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KAUFDAY", Convert.ToInt32(NH.CacheData6));
            }
            else
            {

            }
            if (NH.CacheData7 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KAUFMONAT", Convert.ToInt32(NH.CacheData7));
            }
            else
            {

            }
            if (NH.CacheData8 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KAUFJAHR", Convert.ToInt32(NH.CacheData8));
            }
            else
            {

            }
            if (NH.CacheData9 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PREIS", Convert.ToInt32(NH.CacheData9));
            }
            else
            {

            }
            if (NH.CacheData10 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KUPPLUNG", Convert.ToInt32(NH.CacheData10));
            }
            else
            {

            }
            if (NH.CacheData11 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@LICHT", Convert.ToInt32(NH.CacheData11));
            }
            else
            {

            }
            if (NH.CacheData12 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PREISER", Convert.ToInt32(NH.CacheData12));
            }
            else
            {

            }
            if (NH.CacheData13 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(NH.CacheData13));
            }
            else
            {

            }
            if (NH.CacheData15 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(NH.CacheData15));
            }
            else
            {

            }
            if (NH.CacheData14 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@IDENTIFYER", NH.CacheData14);
            }
            else
            {

            }
            if (Trains.Count <= Settings.LokLimit)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Logger.Message("Wagon Erfolgreich Empfangen, Fehler beim Speichern", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.Error("MODUL Wagon_View :: ADDRPCWAGON():  " + ex + "\n");
                    }
                }
                finally
                {
                    dbConnection.Close();
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Wagon_View :: RPC-Wagon " + NH.CacheData0 + " in: " + NH.CacheData2 + "  Gespeichert.!");
                    }
                }
            }
            else
            {
                RcpItems = +1;
                Logger.Message("Wagon Erfolgreich Empfangen, aber nicht Gespeichert Loklimit überschritten", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Wagon_View :: RPC-ADD Current Wagon Limit is to Low for your Entry ");
                }
            }
            Logger.Message("Wagon Erfolgreich Empfangen, und Gespeichert", "GRUEN");
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
                    Logger.PrintLog("MODUL Wagon_View :: ERROR by Save Settings: " + ex + "\n");
                    Debug.Log(ex);
                }
            }
            finally
            {
                NH.CreateNewConnection();
                dbConnection.Close();
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Wagon_View :: Save Finsch ");
                }
            }
        }
        Usettings.ReadSettings();
        RcpItems = Usettings.ImportRPC;
    }

    IEnumerator SendSelected()
    {
        string FinshURL = Settings.URL + "/insert.php?uniqueID=" + uniqueID + "&data=ISWAGON," + Trains[SelectedID].DBTyp.ToString() + "," + Trains[SelectedID].DBFarbe.ToString() + "," + Trains[SelectedID].DBHersteller.ToString() + "," + Trains[SelectedID].DBKatalognummer.ToString() + "," + Trains[SelectedID].DBSeriennummer.ToString() + "," + Trains[SelectedID].DBKaufTag.ToString() + "," + Trains[SelectedID].DBKaufMonat.ToString() + "," + Trains[SelectedID].DBKaufJahr.ToString() + "," + Trains[SelectedID].DBPreis.ToString() + "," + Trains[SelectedID].DBKupplung.ToString() + "," + Trains[SelectedID].DBLicht.ToString() + "," + Trains[SelectedID].DBPreiser.ToString() + "," + Trains[SelectedID].DBSpurweite.ToString() + "," + Trains[SelectedID].DBIdentifyer.ToString() + "," + Trains[SelectedID].DBLagerort.ToString() + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE";
        Debug.Log(FinshURL);
        WWW insert = new WWW(FinshURL);

        yield return insert;

        if (insert.error != null)
        {
            Logger.PrintLog("MODUL Wagon_View :: Error by send Wagon over WWW! Check your Internet Connection.!");
            Logger.PrintLog("MODUL Wagon_View :: " + insert.error);
        }
        if (insert.isDone)
        {
            Logger.PrintLog("MODUL Wagon_View :: Send Complete ExportKey = " + uniqueID);
            Win.SetActive(true);
            SendOK.text = uniqueID.ToString();
        }
    }
}