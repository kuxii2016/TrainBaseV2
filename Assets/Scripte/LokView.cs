/*
 * 
 *   TrainBase TrainView Manager Version 1 from 27.03.2018 written by Michael Kux
 *    *   Last Edit 31.08.2018
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
using System.Threading;
using DiscordPresence;

[System.Serializable]
public class TrainData : System.Object
{
    public string DbBaureihe;
    public string DbFarbe;
    public int DbLokTyp;
    public int DbHersteller;
    public string DbKatalognummer;
    public string DbSeriennummer;
    public int DbKaufTag;
    public int DbKaufMonat;
    public int DbKaufJahr;
    public int DbPreis;
    public int DbWartungTag;
    public int DbWartungMonat;
    public int DbWartungJahr;
    public string DbAdresse;
    public int DbProtokoll;
    public int DbFahrstufen;
    public string DbDecHersteller;
    public string DbDecID;
    public string DbAngelegt;
    public int DbRauch;
    public int DbSound;
    public int DbROTWEISS;
    public int DbBeleuchtung;
    public int DbPandos;
    public int DbTelex;
    public int DbElekKupplung;
    public Int32 DbSpurweite;
    public Int32 DbCV2;
    public Int32 DbCV3;
    public Int32 DbCV4;
    public Int32 DbCV5;
    public string DBIdentifyer;
    public int DBLagerort;
}

public class LokView : MonoBehaviour
{
    public LogWriterManager Logger;
    public GameObject TrainList;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public SettingsManager Usettings;
    public Network_Handler NH;
    public Discord_Menue_Update DMU;
    [Header("Lok EDIT")]
    public InputField EditBaureihe;
    public InputField EditFarbe;
    public Dropdown EditEnergy;
    public Dropdown EditHersteller;
    public Dropdown EditSpurweite;
    public InputField EditKatalogNummer;
    public InputField EditSeriennummer;
    public Dropdown EditKDay;
    public Dropdown EditKMonth;
    public Dropdown EditKYear;
    public InputField EditPreis;
    public Dropdown EditWDay;
    public Dropdown EditWMonth;
    public Dropdown EditWYear;
    public InputField EditAdresse;
    public Dropdown EditProtokoll;
    public Dropdown EditFahrstufen;
    public InputField EditDecoderHersteller;
    public Toggle EditRauch;
    public Toggle EditTelex;
    public Toggle EditSound;
    public Toggle EditLichtWechsel;
    public Toggle EditElektrischeKupplung;
    public Toggle EditElektrischePandos;
    public InputField CV1;
    public InputField CV2;
    public InputField CV3;
    public InputField CV4;
    public InputField CV5;
    public Dropdown Lager;
    public Text GUUID;
    public Text TopWindow;
    public string EditDBBaureihe;
    public string EditDbFarbe;
    public int EditDbLokTyp;
    public int EditDbHersteller;
    public string EditDbKatalognummer;
    public string EditDbSeriennummer;
    public int EditDbKaufTag;
    public int EditDbKaufMonat;
    public int EditDbKaufJahr;
    public int EditDbPreis;
    public int EditDbWartungTag;
    public int EditDbWartungMonat;
    public int EditDbWartungJahr;
    public string EditDbAdresse;
    public int EditDbProtokoll;
    public int EditDbFahrstufen;
    public string EditDbDecHersteller;
    public string EditDbDecID;
    public string EditDbAngelegt;
    public int EditDbRauch;
    public int EditDbSound;
    public int EditDbROTWEISS;
    public int EditDbBeleuchtung;
    public int EditDbPandos;
    public int EditDbTelex;
    public int EditDbElekKupplung;
    public int EditDbSpurweite;
    [Header("Lok View - Elemente")]
    public List<TrainData> Trains;
    [Tooltip("SlotElemente zum Einschalten")]
    public GameObject[] Slot;
    [Tooltip("SlotElemente TextReihe 1")]
    public GameObject[] Slot1;
    [Tooltip("SlotElemente TextReihe 2")]
    public GameObject[] Slot2;
    [Tooltip("SlotElemente TextReihe 3")]
    public GameObject[] Slot3;
    [Tooltip("SlotElemente Picture")]
    public RawImage[] vBild;
    [Tooltip("Edit Picture")]
    public RawImage trainBild;
    [Tooltip("Edit Picture Text")]
    public Text trainText;
    [Tooltip("HerstellerListe")]
    public string[] vHersteller;
    [Tooltip("Protokol Typen")]
    public string[] vProtokoll;
    [Tooltip("Spurweite")]
    public string[] vSpur;
    [Tooltip("Tage")]
    public string[] Tag;
    [Tooltip("Monate")]
    public string[] Monat;
    [Tooltip("Jahre")]
    public string[] Jahr;
    [Tooltip("SelectedButtons")]
    public Toggle[] DeleteEditToggle;
    [Tooltip("Selected ID")]
    public int SelectedID = -1;
    [Tooltip("PageID")]
    public int PageID = 0;
    public bool[] Select;
    public int RowLoks = 0;
    public int WartungsIntervall = 0;
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
    private DataExporter dataexporter;
    public Image ReadOn;
    public bool ShowImage = false;
    private string filePath;
    private string filePath1;
    public Color ColorWartung;
    public Color NonWartung;
    public string[] tmpGuid;
    public bool UpdateInProgress = false;
    public int counter = 0;
    public int RcpItems;
    [Header("WWW-Send")]
    public GameObject Win;
    public InputField SendOK;
    public Texture2D[] TrainPic;
    public string uniqueID = "";

    void Start()
    {
        WartungsIntervall = Usettings.WartungsInterVall;
        ShowImage = Usettings.ZeigeLokbilderBool;
        dataexporter = new DataExporter();
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
        ReadOn.color = Color.green;
        RcpItems = Usettings.ImportRPC;
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

    public void ReadAllTrainsNew()
    {
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            ReadOn.color = Color.blue;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read all Trains" + ex + "\n");
            }
            Logger.Message("Fehler beim Laden der Lokliste", "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        ReadCustomPic();
    }

    void readIntervall()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            int Jears = int.Parse(Jahr[Trains[i].DbWartungJahr]);
            int old = ((Jears + WartungsIntervall));
            int now = (DateTime.Now.Year);
            if (old >= now)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View :: Train ID: " + (i + 1) + " | " + Trains[i].DbBaureihe + " -> Next: " + (old) + " -> Last: " + now + " = " + (old + 1 - now) + " Years to the next Inspection ");
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View :: Train ID: " + (i + 1) + " | " + Trains[i].DbBaureihe + " -> Next: " + (old) + " -> Last: " + now + " = " + (now - old) + " Years since last Inspection.!!!                               ***WARNING***");
                }
            }
        }
        Logger.PrintLogEnde();
    }

    public void ReadCustomPic()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            StartCoroutine(setImage((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + Usettings.ImageType), i));
        }
        Logger.PrintLogEnde();

        Logger.PrintLog("MODUL Lok_View :: Loadet " + (Trains.Count + 1) + " Train Pictures.");
    }

    public void GetTrainData1()
    {
        PageID = 1;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "1";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 0; i < Trains.Count && i < 24; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            Slot1[i].GetComponent<Text>().color = NonWartung;
                            Slot2[i].GetComponent<Text>().color = NonWartung;
                            Slot3[i].GetComponent<Text>().color = NonWartung;
                            Slot[i].gameObject.SetActive(true);
                            Slot1[i].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + Usettings.ImageType);
                                vBild[i].GetComponent<RawImage>().texture = TrainPic[i] ;
                            }
                        }
                        else
                        {
                            Slot1[i].GetComponent<Text>().color = ColorWartung;
                            Slot2[i].GetComponent<Text>().color = ColorWartung;
                            Slot3[i].GetComponent<Text>().color = ColorWartung;
                            Slot[i].gameObject.SetActive(true);
                            Slot1[i].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                              // vBild[i].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + Usettings.ImageType);
                               vBild[i].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#1 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData2()
    {
        PageID = 2;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "2";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 24; i < Trains.Count && i < 48; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 24].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        //WARTUNGAKTIV
                        if (old >= now)
                        {
                            //Select[i - 24] = DeleteEditToggle[i].isOn;
                            Slot1[i - 24].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 24].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 24].GetComponent<Text>().color = NonWartung;
                            Slot[i - 24].gameObject.SetActive(true);
                            Slot1[i - 24].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 24].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 24].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                               // vBild[i - 24].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 24].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 24].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 24].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 24].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 24].gameObject.SetActive(true);
                            //Slot4[i - 24].GetComponent<Texture>(). = (vBild[Trains[i].DbLokTyp]);
                            Slot1[i - 24].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 24].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 24].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                               // vBild[i - 24].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 24].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData3()
    {
        PageID = 3;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "3";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 48; i < Trains.Count && i < 72; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 48].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            Slot1[i - 48].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 48].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 48].GetComponent<Text>().color = NonWartung;
                            Slot[i - 48].gameObject.SetActive(true);
                            Slot1[i - 48].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 48].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 48].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 48].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 48].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 48].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 48].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 48].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 48].gameObject.SetActive(true);
                            Slot1[i - 48].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 48].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 48].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 48].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 48].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData4()
    {
        PageID = 4;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "4";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 72; i < Trains.Count && i < 96; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 72].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            //Select[i - 72] = DeleteEditToggle[i].isOn;
                            Slot1[i - 72].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 72].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 72].GetComponent<Text>().color = NonWartung;
                            Slot[i - 72].gameObject.SetActive(true);
                            Slot1[i - 72].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 72].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 72].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 72].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 72].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 72].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 72].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 72].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 72].gameObject.SetActive(true);
                            Slot1[i - 72].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 72].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 72].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 72].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 72].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData5()
    {
        PageID = 5;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "5";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 96; i < Trains.Count && i < 120; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 96].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            //Select[i - 96] = DeleteEditToggle[i].isOn;
                            Slot1[i - 96].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 96].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 96].GetComponent<Text>().color = NonWartung;
                            Slot[i - 96].gameObject.SetActive(true);
                            Slot1[i - 96].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 96].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 96].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 96].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 96].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 96].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 96].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 96].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 96].gameObject.SetActive(true);
                            Slot1[i - 96].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 96].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 96].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 96].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 96].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData6()
    {
        PageID = 6;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "6";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 120; i < Trains.Count && i < 144; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 120].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            //Select[i - 120] = DeleteEditToggle[i].isOn;
                            Slot1[i - 120].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 120].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 120].GetComponent<Text>().color = NonWartung;
                            Slot[i - 120].gameObject.SetActive(true);
                            Slot1[i - 120].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 120].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 120].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 120].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 120].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 120].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 120].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 120].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 120].gameObject.SetActive(true);
                            Slot1[i - 120].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 120].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 120].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 120].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 120].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData7()
    {
        PageID = 7;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "7";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 144; i < Trains.Count && i < 168; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 144].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            //Select[i - 144] = DeleteEditToggle[i].isOn;
                            Slot1[i - 144].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 144].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 144].GetComponent<Text>().color = NonWartung;
                            Slot[i - 144].gameObject.SetActive(true);
                            Slot1[i - 144].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 144].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 144].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 144].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 144].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 144].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 144].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 144].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 144].gameObject.SetActive(true);
                            Slot1[i - 144].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 144].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 144].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 144].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 144].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData8()
    {
        PageID = 8;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "8";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 168; i < Trains.Count && i < 192; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 168].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            //Select[i - 168] = DeleteEditToggle[i].isOn;
                            Slot1[i - 168].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 168].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 168].GetComponent<Text>().color = NonWartung;
                            Slot[i - 168].gameObject.SetActive(true);
                            Slot1[i - 168].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 168].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 168].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 168].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 168].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 168].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 168].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 168].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 168].gameObject.SetActive(true);
                            Slot1[i - 168].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 168].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 168].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 168].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 168].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData9()
    {
        PageID = 9;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "9";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 192; i < Trains.Count && i < 216; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 192].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        //WARTUNGAKTIV
                        if (old >= now)
                        {
                            //Select[i - 192] = DeleteEditToggle[i].isOn;
                            Slot1[i - 192].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 192].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 192].GetComponent<Text>().color = NonWartung;
                            Slot[i - 192].gameObject.SetActive(true);
                            Slot1[i - 192].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 192].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 192].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 192].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 192].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 192].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 192].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 192].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 192].gameObject.SetActive(true);
                            Slot1[i - 192].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 192].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 192].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 192].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 192].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page " + PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void GetTrainData10()
    {
        PageID = 10;
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        CleanScreen();
        TrainList.gameObject.SetActive(true);
        PageNumber.GetComponent<Text>().text = "10";
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Trains", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    TrainData trainData = new TrainData();
                    trainData.DbBaureihe = reader.GetString(0);
                    trainData.DbFarbe = reader.GetString(1);
                    trainData.DbLokTyp = reader.GetInt32(2);
                    trainData.DbHersteller = reader.GetInt32(3);
                    trainData.DbAngelegt = reader.GetString(4);
                    trainData.DbKatalognummer = reader.GetString(5);
                    trainData.DbSeriennummer = reader.GetString(6);
                    trainData.DbPreis = reader.GetChar(8);
                    trainData.DbWartungTag = reader.GetInt32(9);
                    trainData.DbWartungMonat = reader.GetInt32(10);
                    trainData.DbWartungJahr = reader.GetInt32(11);
                    trainData.DbAdresse = reader.GetString(12);
                    trainData.DbProtokoll = reader.GetInt32(13);
                    trainData.DbFahrstufen = reader.GetInt32(14);
                    trainData.DbDecHersteller = reader.GetString(15);
                    trainData.DbRauch = reader.GetInt32(16);
                    trainData.DbSound = reader.GetInt32(17);
                    trainData.DbROTWEISS = reader.GetInt32(18);
                    trainData.DbPandos = reader.GetInt32(19);
                    trainData.DbTelex = reader.GetInt32(20);
                    trainData.DbElekKupplung = reader.GetInt32(21);
                    trainData.DbKaufTag = reader.GetInt32(22);
                    trainData.DbKaufMonat = reader.GetInt32(23);
                    trainData.DbKaufJahr = reader.GetInt32(24);
                    trainData.DbSpurweite = reader.GetInt32(25);
                    trainData.DbCV2 = reader.GetInt32(26);
                    trainData.DbCV3 = reader.GetInt32(27);
                    trainData.DbCV4 = reader.GetInt32(28);
                    trainData.DbCV5 = reader.GetInt32(29);
                    trainData.DBIdentifyer = reader.GetString(31);
                    trainData.DBLagerort = reader.GetInt32(32);
                    Trains.Add(trainData);
                    for (int i = 216; i < Trains.Count && i < 240; i++)
                    {
                        int Jears = int.Parse(Jahr[Trains[i - 216].DbWartungJahr]);
                        int old = ((Jears + WartungsIntervall));
                        int now = (DateTime.Now.Year);
                        if (old >= now)
                        {
                            //Select[i - 216] = DeleteEditToggle[i].isOn;
                            Slot1[i - 216].GetComponent<Text>().color = NonWartung;
                            Slot2[i - 216].GetComponent<Text>().color = NonWartung;
                            Slot3[i - 216].GetComponent<Text>().color = NonWartung;
                            Slot[i - 216].gameObject.SetActive(true);
                            Slot1[i - 216].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 216].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 216].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 216].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 216].GetComponent<RawImage>().texture = TrainPic[i];
                            }
                        }
                        else
                        {
                            Slot1[i - 216].GetComponent<Text>().color = ColorWartung;
                            Slot2[i - 216].GetComponent<Text>().color = ColorWartung;
                            Slot3[i - 216].GetComponent<Text>().color = ColorWartung;
                            Slot[i - 216].gameObject.SetActive(true);
                            Slot1[i - 216].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                            Slot2[i - 216].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                            Slot3[i - 216].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "" + Jahr[Trains[i].DbWartungJahr];
                            if (ShowImage == true)
                            {
                                //vBild[i - 216].GetComponent<RawImage>().texture = LoadPNG(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + ((i) + 1) + "." + Usettings.ImageType);
                                vBild[i - 216].GetComponent<RawImage>().texture = TrainPic[i];
                            }
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
                Logger.PrintLog("MODUL Lok_View :: ERROR by Read Trains: Page#10 " + ex +"\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " +PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Lokseite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Page "+ PageID + " Found " + Trains.Count + " Rows");
        }
    }

    public void CleanScreen()
    {
        WartungsIntervall = Usettings.WartungsInterVall;
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
        ReadOn.color = Color.green;
    }

    public void GetLokData()
    {
        EditBaureihe.text = Trains[SelectedID].DbBaureihe;
        EditFarbe.text = Trains[SelectedID].DbFarbe;
        EditEnergy.value = Trains[SelectedID].DbLokTyp;
        EditHersteller.value = Trains[SelectedID].DbHersteller;
        EditSpurweite.value = Trains[SelectedID].DbSpurweite;
        EditKatalogNummer.text = Trains[SelectedID].DbKatalognummer;
        EditSeriennummer.text = Trains[SelectedID].DbSeriennummer;
        EditKDay.value = Trains[SelectedID].DbKaufTag;
        EditKMonth.value = Trains[SelectedID].DbKaufMonat;
        EditKYear.value = Trains[SelectedID].DbKaufJahr;
        EditPreis.text = Trains[SelectedID].DbPreis.ToString();
        EditWDay.value = Trains[SelectedID].DbWartungTag;
        EditWMonth.value = Trains[SelectedID].DbWartungMonat;
        EditWYear.value = Trains[SelectedID].DbWartungJahr;
        EditAdresse.text = Trains[SelectedID].DbAdresse;
        EditProtokoll.value = Trains[SelectedID].DbProtokoll;
        EditFahrstufen.value = Trains[SelectedID].DbFahrstufen;
        EditDecoderHersteller.text = Trains[SelectedID].DbDecHersteller;
        CV2.text = Trains[SelectedID].DbCV2.ToString();
        CV3.text = Trains[SelectedID].DbCV3.ToString();
        CV4.text = Trains[SelectedID].DbCV4.ToString();
        CV5.text = Trains[SelectedID].DbCV5.ToString();
        CV1.text = Trains[SelectedID].DbAdresse;
        GUUID.text = Trains[SelectedID].DBIdentifyer;
        Lager.value = Trains[SelectedID].DBLagerort;
        TopWindow.text = Trains[SelectedID].DbBaureihe + " Bearbeitung";
        filePath1 = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (SelectedID + 1) + "." + Usettings.ImageType);
        trainBild.GetComponent<RawImage>().texture = TrainPic[SelectedID];
        trainText.text = EditBaureihe.text;
        Logger.Message("Lade Lok Daten zum Bearbeiten", "GELB");
        PresenceManager.UpdatePresence("Edit Train", Trains[SelectedID].DbBaureihe, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DeleteTrain()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        string sql = "DELETE FROM Trains WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer +  "' AND  ERSTELLT='" + Trains[SelectedID].DbAngelegt + "'  ";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (SelectedID + 1) + ".png");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Train Delete: " + Trains[SelectedID].DbBaureihe + " Color: " + Trains[SelectedID].DbFarbe + " from " + Trains[SelectedID].DbAngelegt + " Deleted.!");
        }
        SelectedID = -1;
        CleanScreen();
        Logger.Message("Lok wurde Gelöscht", "GRUEN");
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
            command.CommandText = "UPDATE Trains  SET BAUREIHE = @BAUREIHE, FARBE = @FARBE , TYP = @TYP , HERSTELLER = @HERSTELLER , KATALOGNUMMER = @KATALOGNUMMER , SERIENNUMMER = @SERIENNUMMER , PREIS = @PREIS , WARTUNGDAY = @WARTUNGDAY ,WARTUNGMONAT = @WARTUNGMONAT , WARTUNGJEAR = @WARTUNGJEAR , ADRESSE = @ADRESSE , PROTOKOLL = @PROTOKOLL , FAHRSTUFEN =@FAHRSTUFEN , DECHERSTELLER = @DECHERSTELLER , KTAG = @KTAG , KMONAT = @KMONAT , KJAHR = @KJAHR, SPURWEITE = @SPURWEITE, CV2 = @CV2,  CV3 = @CV3, CV4 = @CV4, CV5 = @CV5, LAGERORT = @LAGERORT WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer + "' AND ERSTELLT='" + Trains[SelectedID].DbAngelegt + "'  ";
            command.Parameters.AddWithValue("@BAUREIHE", EditBaureihe.text);
            command.Parameters.AddWithValue("@FARBE", EditFarbe.text);
            command.Parameters.AddWithValue("@TYP", EditEnergy.value);
            command.Parameters.AddWithValue("@HERSTELLER", EditHersteller.value);
            command.Parameters.AddWithValue("@KATALOGNUMMER", EditKatalogNummer.text);
            command.Parameters.AddWithValue("@SERIENNUMMER", EditSeriennummer.text);
            command.Parameters.AddWithValue("@PREIS", EditPreis.text);
            command.Parameters.AddWithValue("@WARTUNGDAY", EditWDay.value);
            command.Parameters.AddWithValue("@WARTUNGMONAT", EditWMonth.value);
            command.Parameters.AddWithValue("@WARTUNGJEAR", EditWYear.value);
            command.Parameters.AddWithValue("@ADRESSE", EditAdresse.text);
            command.Parameters.AddWithValue("@PROTOKOLL", EditProtokoll.value);
            command.Parameters.AddWithValue("@FAHRSTUFEN", EditFahrstufen.value);
            command.Parameters.AddWithValue("@DECHERSTELLER", EditDecoderHersteller.text);
            command.Parameters.AddWithValue("@KTAG", EditKDay.value);
            command.Parameters.AddWithValue("@KMONAT", EditKMonth.value);
            command.Parameters.AddWithValue("@KJAHR", EditWYear.value);
            command.Parameters.AddWithValue("@SPURWEITE", EditSpurweite.value);
            command.Parameters.AddWithValue("@ADRESSE", CV1.text);
            command.Parameters.AddWithValue("@CV2", CV2.text);
            command.Parameters.AddWithValue("@CV3", CV3.text);
            command.Parameters.AddWithValue("@CV4", CV4.text);
            command.Parameters.AddWithValue("@CV5", CV5.text);
            command.Parameters.AddWithValue("@LAGERORT", Lager.value);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
                ReadOn.color = Color.blue;
            }
            catch (SqliteException ex)
            {
                Logger.Message("Fehler beim Bearbeiten der Lok", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View :: Fehler beim Bearbeiten " + ex + "\n");
                }
            }
            finally
            {
                dbConnection.Close();
                Logger.Message("Lok wurde Erfolgreich bearbeited", "GRUEN");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View ::  " + EditBaureihe.text + " ADR: " + EditAdresse.text + " in: " + EditFarbe.text + "  Editiert.!");
                }
            }
        }
        SelectedID = -1;
        ReadAllTrainsNew();
    }

    public void ExportTrain()
    {
        dataexporter.Type = "TRAIN";
        dataexporter.Baureihe = Trains[SelectedID].DbBaureihe;
        dataexporter.Farbe = Trains[SelectedID].DbFarbe;
        dataexporter.Energy = Trains[SelectedID].DbLokTyp;
        dataexporter.Hersteller = Trains[SelectedID].DbHersteller;
        dataexporter.Erstellt = Trains[SelectedID].DbAngelegt;
        dataexporter.Spurweite = Trains[SelectedID].DbSpurweite;
        dataexporter.KatalogNummer = Trains[SelectedID].DbKatalognummer;
        dataexporter.Seriennummer = Trains[SelectedID].DbSeriennummer;
        dataexporter.KaufDay = Trains[SelectedID].DbKaufTag;
        dataexporter.KaufMonth = Trains[SelectedID].DbKaufMonat;
        dataexporter.KaufYear = Trains[SelectedID].DbKaufJahr;
        dataexporter.KaufPreis = Trains[SelectedID].DbPreis.ToString();
        dataexporter.WartungDay = Trains[SelectedID].DbWartungTag;
        dataexporter.WartungMonth = Trains[SelectedID].DbWartungMonat;
        dataexporter.WartungYear = Trains[SelectedID].DbWartungJahr;
        dataexporter.Adresse = Trains[SelectedID].DbAdresse;
        dataexporter.Protokoll = Trains[SelectedID].DbProtokoll;
        dataexporter.Fahrstufen = Trains[SelectedID].DbFahrstufen;
        dataexporter.Fahrstufen = Trains[SelectedID].DbFahrstufen;
        dataexporter.DecoderHersteller = Trains[SelectedID].DbDecHersteller;
        dataexporter.Rauch = Trains[SelectedID].DbRauch;
        dataexporter.Telex = Trains[SelectedID].DbTelex;
        dataexporter.Sound = Trains[SelectedID].DbSound;
        dataexporter.LichtWechsel = Trains[SelectedID].DbROTWEISS;
        dataexporter.ElektrischeKupplung = Trains[SelectedID].DbElekKupplung;
        dataexporter.ElektrischePandos = Trains[SelectedID].DbPandos;
        dataexporter.CV2 = Trains[SelectedID].DbCV2.ToString();
        dataexporter.CV3 = Trains[SelectedID].DbCV3.ToString();
        dataexporter.CV4 = Trains[SelectedID].DbCV4.ToString();
        dataexporter.CV5 = Trains[SelectedID].DbCV5.ToString();
        string jsonData = JsonUtility.ToJson(dataexporter, true);
        File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Exporter/" + vHersteller[Trains[SelectedID].DbHersteller] + "-" + Trains[SelectedID].DbKatalognummer + ".TRAIN", jsonData);
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Train: " + vHersteller[Trains[SelectedID].DbHersteller] + "-" + Trains[SelectedID].DbKatalognummer + " is Final Exported.!");
        }
        SelectedID = -1;
        Logger.Message("Lok Exportiert, Ist im Export Ordner zu finden", "GRUEN");
    }

    public void ReadIntervall()
    {
        if (File.Exists(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub"))
        {
            string data = File.ReadAllText(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub");
            WartungsIntervall = int.Parse(data);
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Lok_View :: Intervall Data Found. Set to " + (data));
            }
        }
        else
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Lok_View :: No Intervall Data Found. Set default to 1 Year(s).");
            }
            WartungsIntervall = 1;
        }
    }

    public void GetTrainPictures()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: Check Lok Images");
        }
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains");
        }

        for (int i = 0; i < Trains.Count && i < 239; i++)
        {
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + Usettings.ImageType))
            {
                File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + Usettings.ImageType);
                counter = (i+1);
            }
            else
            {
                counter = 0;
            }
        }
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: " + counter + " Images Created");
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
        NH.TrySendTrainData("TRAIN" + "," + Trains[SelectedID].DbBaureihe.ToString() + "," + Trains[SelectedID].DbFarbe.ToString() + "," + Trains[SelectedID].DbLokTyp.ToString() + "," + Trains[SelectedID].DbHersteller.ToString() + "," + Trains[SelectedID].DbKatalognummer.ToString() + "," + Trains[SelectedID].DbSeriennummer.ToString() + "," + Trains[SelectedID].DbKaufTag.ToString() + "," + Trains[SelectedID].DbKaufMonat.ToString() + "," + Trains[SelectedID].DbKaufJahr.ToString() + "," + Trains[SelectedID].DbPreis.ToString() + "," + Trains[SelectedID].DbWartungTag.ToString() + "," + Trains[SelectedID].DbWartungMonat.ToString() + "," + Trains[SelectedID].DbWartungJahr.ToString() + "," + Trains[SelectedID].DbAdresse.ToString() + "," + Trains[SelectedID].DbProtokoll.ToString() + "," + Trains[SelectedID].DbFahrstufen.ToString() + "," + "EMPTY" + "," + Trains[SelectedID].DbDecHersteller.ToString() + "," + Trains[SelectedID].DbAngelegt.ToString() + "," + Trains[SelectedID].DbRauch.ToString() + "," + Trains[SelectedID].DbSound.ToString() + "," + Trains[SelectedID].DbROTWEISS.ToString() + "," + "EMPTY" + "," + Trains[SelectedID].DbPandos.ToString() + "," + Trains[SelectedID].DbTelex.ToString() + "," + Trains[SelectedID].DbElekKupplung.ToString() + "," + Trains[SelectedID].DbSpurweite.ToString() + "," + Trains[SelectedID].DbCV2.ToString() + "," + Trains[SelectedID].DbCV3.ToString() + "," + Trains[SelectedID].DbCV4.ToString() + "," + Trains[SelectedID].DbCV5.ToString() + "," + Trains[SelectedID].DBIdentifyer.ToString() + "," + Trains[SelectedID].DBLagerort.ToString());
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Lok_View :: RPC-Send Lok");
            //Logger.PrintLog("TRAIN" + "," + Trains[SelectedID].DbBaureihe.ToString() + "," + Trains[SelectedID].DbFarbe.ToString() + "," + Trains[SelectedID].DbLokTyp.ToString() + "," + Trains[SelectedID].DbHersteller.ToString() + "," + Trains[SelectedID].DbKatalognummer.ToString() + "," + Trains[SelectedID].DbSeriennummer.ToString() + "," + Trains[SelectedID].DbKaufTag.ToString() + "," + Trains[SelectedID].DbKaufMonat.ToString() + "," + Trains[SelectedID].DbKaufJahr.ToString() + "," + Trains[SelectedID].DbPreis.ToString() + "," + Trains[SelectedID].DbWartungTag.ToString() + "," + Trains[SelectedID].DbWartungMonat.ToString() + "," + Trains[SelectedID].DbWartungJahr.ToString() + "," + Trains[SelectedID].DbAdresse.ToString() + "," + Trains[SelectedID].DbProtokoll.ToString() + "," + Trains[SelectedID].DbFahrstufen.ToString() + "," + Trains[SelectedID].DbDecHersteller.ToString() + "," + Trains[SelectedID].DbDecID.ToString() + "," + Trains[SelectedID].DbAngelegt.ToString() + "," + Trains[SelectedID].DbRauch.ToString() + "," + Trains[SelectedID].DbSound.ToString() + "," + Trains[SelectedID].DbROTWEISS.ToString() + "," + Trains[SelectedID].DbBeleuchtung.ToString() + "," + Trains[SelectedID].DbPandos.ToString() + "," + Trains[SelectedID].DbTelex.ToString() + "," + Trains[SelectedID].DbElekKupplung.ToString() + "," + Trains[SelectedID].DbSpurweite.ToString() + "," + Trains[SelectedID].DbCV2.ToString() + "," + Trains[SelectedID].DbCV3.ToString() + "," + Trains[SelectedID].DbCV4.ToString() + "," + Trains[SelectedID].DbCV5.ToString() + "," + Trains[SelectedID].DBIdentifyer.ToString() + "," + Trains[SelectedID].DBLagerort.ToString());
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
            if (Trains[i].DBIdentifyer == NH.CacheData33)
            {
                UPDATERPCTRAIN();
            }
        }
        ADDRPCTRAIN();
    }

    public void UPDATERPCTRAIN()
    {
        Logger.Message("Lok Über RPC Emfangen", "LILA");
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Trains  SET BAUREIHE = @BAUREIHE, FARBE = @FARBE, TYP = @TYP, HERSTELLER = @HERSTELLER, KATALOGNUMMER = @KATALOGNUMMER, SERIENNUMMER = @SERIENNUMMER,PREIS = @PREIS, WARTUNGDAY = @WARTUNGDAY, WARTUNGMONAT = @WARTUNGMONAT, WARTUNGJEAR = @WARTUNGJEAR, ADRESSE = @ADRESSE, PROTOKOLL = @PROTOKOLL, FAHRSTUFEN = @FAHRSTUFEN, DECHERSTELLER = @DECHERSTELLER, RAUCH = @RAUCH, SOUND = @SOUND, ROTWEISS = @ROTWEISS, PANDO = @PANDO, TELEX = @TELEX, KUPPLUNG = @KUPPLUNG, KTAG = @KTAG, KMONAT = @KMONAT, KJAHR = @KJAHR, SPURWEITE = @SPURWEITE, CV2 = @CV2, CV3 = @CV3, CV4 = @CV4, CV5 = @CV5, IDENTIFYER = @IDENTIFYER, LAGERORT = @LAGERORT WHERE IDENTIFYER='" + NH.CacheData30 + "'  ";
            if (NH.CacheData1 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@BAUREIHE", NH.CacheData1);
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
                command.Parameters.AddWithValue("@TYP", Convert.ToInt32(NH.CacheData3));
            }
            else
            {

            }
            if (NH.CacheData4 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@HERSTELLER", Convert.ToInt32(NH.CacheData4));
            }
            else
            {

            }
            if (NH.CacheData5 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KATALOGNUMMER", NH.CacheData5);
            }
            else
            {

            }
            if (NH.CacheData6 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SERIENNUMMER", NH.CacheData6);
            }
            else
            {

            }
            if (NH.CacheData7 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KTAG", Convert.ToInt32(NH.CacheData7));
            }
            else
            {

            }
            if (NH.CacheData8 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KMONAT", Convert.ToInt32(NH.CacheData8));
            }
            else
            {

            }
            if (NH.CacheData9 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KJAHR", Convert.ToInt32(NH.CacheData9));
            }
            else
            {

            }
            if (NH.CacheData10 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PREIS", Convert.ToInt32(NH.CacheData10));
            }
            else
            {

            }
            if (NH.CacheData11 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@WARTUNGDAY", Convert.ToInt32(NH.CacheData11));
            }
            else
            {

            }
            if (NH.CacheData12 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@WARTUNGMONAT", Convert.ToInt32(NH.CacheData12));
            }
            else
            {

            }
            if (NH.CacheData13 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@WARTUNGJEAR", Convert.ToInt32(NH.CacheData13));
            }
            else
            {

            }
            if (NH.CacheData14 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@ADRESSE", NH.CacheData14);
            }
            else
            {

            }
            if (NH.CacheData15 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PROTOKOLL", Convert.ToInt32(NH.CacheData15));
            }
            else
            {

            }
            if (NH.CacheData16 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@FAHRSTUFEN", Convert.ToInt32(NH.CacheData16));
            }
            else
            {

            }
            if (NH.CacheData17 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@DECHERSTELLER", NH.CacheData17);
            }
            else
            {

            }
            if (NH.CacheData20 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@RAUCH", Convert.ToInt32(NH.CacheData20));
            }
            else
            {

            }
            if (NH.CacheData21 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SOUND", Convert.ToInt32(NH.CacheData21));
            }
            else
            {

            }
            if (NH.CacheData22 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@ROTWEISS", Convert.ToInt32(NH.CacheData22));
            }
            else
            {

            }
            if (NH.CacheData24 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PANDO", Convert.ToInt32(NH.CacheData24));
            }
            else
            {

            }
            if (NH.CacheData25 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@TELEX", Convert.ToInt32(NH.CacheData25));
            }
            else
            {

            }
            if (NH.CacheData26 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KUPPLUNG", Convert.ToInt32(NH.CacheData26));
            }
            else
            {

            }
            if (NH.CacheData27 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(NH.CacheData27));
            }
            else
            {

            }
            if (NH.CacheData28 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV2", Convert.ToInt32(NH.CacheData28));
            }
            else
            {

            }
            if (NH.CacheData29 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV3", Convert.ToInt32(NH.CacheData29));
            }
            else
            {

            }
            if (NH.CacheData30 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV4", Convert.ToInt32(NH.CacheData30));
            }
            else
            {

            }
            if (NH.CacheData31 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV5", Convert.ToInt32(NH.CacheData31));
            }
            else
            {

            }
            if (NH.CacheData32 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@IDENTIFYER", NH.CacheData32);
            }
            else
            {

            }
            if (NH.CacheData33 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(NH.CacheData33));
            }
            else
            {

            }
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Logger.Message("Lok Erfolgreich Empfangen, Fehler beim Update", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View ::  RPC-Update Fehler beim Update " + ex + "\n");
                }
            }
            finally
            {
                RcpItems = +1;
                dbConnection.Close();
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View :: RPC-Update" + NH.CacheData0 + " in: " + NH.CacheData2 + "  Upgedated.!");
                }
                ReadOn.color = Color.green;
            }
        }
        SelectedID = -1;
        ReadAllTrainsNew();
        Logger.Message("Lok Erfolgreich Empfangen, und Upgedated", "GRUEN");
        NH.CreateNewConnection();
        SaveSettings();
    }

    public void ADDRPCTRAIN()
    {
        Logger.Message("Lok Über RPC Emfangen", "LILA");
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Trains (BAUREIHE, FARBE, TYP, HERSTELLER, KATALOGNUMMER, SERIENNUMMER, PREIS, WARTUNGDAY, WARTUNGMONAT, WARTUNGJEAR, ADRESSE, PROTOKOLL, FAHRSTUFEN, DECHERSTELLER, RAUCH, SOUND, ROTWEISS, PANDO, TELEX, KUPPLUNG, KTAG, KMONAT, KJAHR, SPURWEITE, CV2, CV3, CV4, CV5, IDENTIFYER, LAGERORT) VALUES" + " (@BAUREIHE, @FARBE, @TYP, @HERSTELLER, @KATALOGNUMMER, @SERIENNUMMER, @PREIS, @WARTUNGDAY, @WARTUNGMONAT, @WARTUNGJEAR, @ADRESSE, @PROTOKOLL, @FAHRSTUFEN, @DECHERSTELLER, @RAUCH, @SOUND, @ROTWEISS, @PANDO, @TELEX, @KUPPLUNG, @KTAG, @KMONAT, @KJAHR, @SPURWEITE, @CV2, @CV3, @CV4, @CV5, @IDENTIFYER, @LAGERORT)";
            if (NH.CacheData1 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@BAUREIHE", NH.CacheData1);
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
                command.Parameters.AddWithValue("@TYP", Convert.ToInt32(NH.CacheData3));
            }
            else
            {

            }
            if (NH.CacheData4 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@HERSTELLER", Convert.ToInt32(NH.CacheData4));
            }
            else
            {

            }
            if (NH.CacheData5 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KATALOGNUMMER", NH.CacheData5);
            }
            else
            {

            }
            if (NH.CacheData6 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SERIENNUMMER", NH.CacheData6);
            }
            else
            {

            }
            if (NH.CacheData7 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KTAG", Convert.ToInt32(NH.CacheData7));
            }
            else
            {

            }
            if (NH.CacheData8 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KMONAT", Convert.ToInt32(NH.CacheData8));
            }
            else
            {

            }
            if (NH.CacheData9 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KJAHR", Convert.ToInt32(NH.CacheData9));
            }
            else
            {

            }
            if (NH.CacheData10 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PREIS", Convert.ToInt32(NH.CacheData10));
            }
            else
            {

            }
            if (NH.CacheData11 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@WARTUNGDAY", Convert.ToInt32(NH.CacheData11));
            }
            else
            {

            }
            if (NH.CacheData12 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@WARTUNGMONAT", Convert.ToInt32(NH.CacheData12));
            }
            else
            {

            }
            if (NH.CacheData13 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@WARTUNGJEAR", Convert.ToInt32(NH.CacheData13));
            }
            else
            {

            }
            if (NH.CacheData14 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@ADRESSE", NH.CacheData14);
            }
            else
            {

            }
            if (NH.CacheData15 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PROTOKOLL", Convert.ToInt32(NH.CacheData15));
            }
            else
            {

            }
            if (NH.CacheData16 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@FAHRSTUFEN", Convert.ToInt32(NH.CacheData16));
            }
            else
            {

            }
            if (NH.CacheData17 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@DECHERSTELLER", NH.CacheData17);
            }
            else
            {

            }
            if (NH.CacheData20 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@RAUCH", Convert.ToInt32(NH.CacheData20));
            }
            else
            {

            }
            if (NH.CacheData21 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SOUND", Convert.ToInt32(NH.CacheData21));
            }
            else
            {

            }
            if (NH.CacheData22 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@ROTWEISS", Convert.ToInt32(NH.CacheData22));
            }
            else
            {

            }
            if (NH.CacheData24 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@PANDO", Convert.ToInt32(NH.CacheData24));
            }
            else
            {

            }
            if (NH.CacheData25 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@TELEX", Convert.ToInt32(NH.CacheData25));
            }
            else
            {

            }
            if (NH.CacheData26 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@KUPPLUNG", Convert.ToInt32(NH.CacheData26));
            }
            else
            {

            }
            if (NH.CacheData27 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(NH.CacheData27));
            }
            else
            {

            }
            if (NH.CacheData28 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV2", Convert.ToInt32(NH.CacheData28));
            }
            else
            {

            }
            if (NH.CacheData29 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV3", Convert.ToInt32(NH.CacheData29));
            }
            else
            {

            }
            if (NH.CacheData30 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV4", Convert.ToInt32(NH.CacheData30));
            }
            else
            {

            }
            if (NH.CacheData31 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@CV5", Convert.ToInt32(NH.CacheData31));
            }
            else
            {

            }
            if (NH.CacheData32 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@IDENTIFYER", NH.CacheData32);
            }
            else
            {

            }
            if (NH.CacheData33 != "MOTHERFUCKERABFUCK")
            {
                command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(NH.CacheData33));
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
                    Logger.Message("Lok Erfolgreich Empfangen, Fehler beim Speichern", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Lok_View :: RPC-Lok Fehler beim Speichern " + ex + "\n");
                    }
                }
                finally
                {
                    RcpItems = +1;
                    dbConnection.Close();
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Lok_View :: RPC-Lok " + NH.CacheData0 + " in: " + NH.CacheData2 + "  Gespeichert.!");
                    }
                }
            }
            else
            {
                Logger.Message("Lok Erfolgreich Empfangen, aber nicht Gespeichert Loklimit überschritten", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View :: RPC-ADD Current Train Limit is to Low for your Entry ");
                }
            }
            Logger.Message("Lok Erfolgreich Empfangen, und Gespeichert", "GRUEN");
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
                    Logger.PrintLog("MODUL Lok_View :: ERROR by Save Settings: " + ex + "\n");
                    Debug.Log(ex);
                }
            }
            finally
            {
                dbConnection.Close();
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Lok_View :: Save Finsch ");
                }
            }
        }
        Usettings.ReadSettings();
        RcpItems = Usettings.ImportRPC;
    }

    IEnumerator SendSelected()
    {
        string FinshURL = Settings.URL + "/insert.php?uniqueID=" + uniqueID + "&data=ISTRAIN," + Trains[SelectedID].DbBaureihe.ToString() + "," + Trains[SelectedID].DbFarbe.ToString() + "," + Trains[SelectedID].DbLokTyp.ToString() + "," + Trains[SelectedID].DbHersteller.ToString() + "," + Trains[SelectedID].DbKatalognummer.ToString() + "," + Trains[SelectedID].DbSeriennummer.ToString() + "," + Trains[SelectedID].DbKaufTag.ToString() + "," + Trains[SelectedID].DbKaufMonat.ToString() + "," + Trains[SelectedID].DbKaufJahr.ToString() + "," + Trains[SelectedID].DbPreis.ToString() + "," + Trains[SelectedID].DbWartungTag.ToString() + "," + Trains[SelectedID].DbWartungMonat.ToString() + "," + Trains[SelectedID].DbWartungJahr.ToString() + "," + Trains[SelectedID].DbAdresse.ToString() + "," + Trains[SelectedID].DbProtokoll.ToString() + "," + Trains[SelectedID].DbFahrstufen.ToString() + "," + "EMPTY" + "," + Trains[SelectedID].DbDecHersteller.ToString() + "," + Trains[SelectedID].DbAngelegt.ToString() + "," + Trains[SelectedID].DbRauch.ToString() + "," + Trains[SelectedID].DbSound.ToString() + "," + Trains[SelectedID].DbROTWEISS.ToString() + "," + "EMPTY" + "," + Trains[SelectedID].DbPandos.ToString() + "," + Trains[SelectedID].DbTelex.ToString() + "," + Trains[SelectedID].DbElekKupplung.ToString() + "," + Trains[SelectedID].DbSpurweite.ToString() + "," + Trains[SelectedID].DbCV2.ToString() + "," + Trains[SelectedID].DbCV3.ToString() + "," + Trains[SelectedID].DbCV4.ToString() + "," + Trains[SelectedID].DbCV5.ToString() + "," + Trains[SelectedID].DBIdentifyer.ToString() + "," + Trains[SelectedID].DBLagerort.ToString();
        Debug.Log(FinshURL);
        WWW insert = new WWW(FinshURL);

        yield return insert;

        if(insert.error != null)
        {
            Logger.PrintLog("MODUL Lok_View :: Error by send Train over WWW! Check your Internet Connection.!");
            Logger.PrintLog("MODUL Lok_View :: " + insert.error);
        }
        if(insert.isDone)
        { 
            Logger.PrintLog("MODUL Lok_View :: Send Complete ExportKey = " + uniqueID);
            Win.SetActive(true);
            SendOK.text = uniqueID.ToString();
        }
    }
}