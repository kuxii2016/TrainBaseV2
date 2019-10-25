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
    public bool Wartung;
    public bool Checked;
}

public class Train_List : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    [Header("Lok View - Elemente")]
    public List<TrainData> Trains;
    public Texture2D[] CacheImage;
    public int PageOffset = 0;
    public int PageOffset2 = 12;
    public int CurrentPage = 1;
    public int SelectedID = -1;
    public Color ColorWartung;
    public Color NonWartung;
    [Header("List-Elements")]
    public Text Page;
    [Tooltip("SlotElemente zum Einschalten")]
    public GameObject[] Slot;
    [Tooltip("SlotElemente TextReihe 1")]
    public GameObject[] Slot1;
    [Tooltip("SlotElemente TextReihe 2")]
    public GameObject[] Slot2;
    [Tooltip("SlotElemente TextReihe 3")]
    public GameObject[] Slot3;
    [Tooltip("SlotElemente Picture")]
    public RawImage[] SlotBild;
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
    public Toggle[] DeleteEditToggle;
    public bool[] EditDeleteToggle;
    public GameObject[] WartungsPic;
    public GameObject[] LightSwitch;
    public GameObject[] Smoke;
    public Text[] TrainID;
    public Text CTrains;
    public Text WTrains;
    public Text NTrains;
    [Header("Edit-Panel-Inputs")]
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
    public RawImage TrainPic;
    public GameObject EditPanel;
    private DataExporter dataexporter;
    [Header("Workflow")]
    public int firstStart = 0;
    public bool IsPremium = false;
    public int WartungsTrains = 0;
    public int nonWartungsTrains = 0;
    public int CompleteTrains = 0;
    public bool IsEditMode;
    public string uniqueID;
    public GameObject Win;
    public InputField SendOK;

    void Start()
    {
        dataexporter = new DataExporter();
        startManager.Log("Lade Train_List -> Nachricht ist Normal.", "Load Train_List -> message is normal");
        ReadTrains();
        IsPremium = UserSettings.Premium;
        readIntervall();
    }

    private void FixedUpdate()
    {
        for (int i = PageOffset; i < Trains.Count && i < PageOffset2; i++)
        {
            TrainID[i - PageOffset].gameObject.SetActive(true);
            TrainID[i - PageOffset].text = "ID: " + (i + 1);
            SlotBild[i - PageOffset].texture = CacheImage[i];
            if (IsPremium == true)
            {
                if (Trains[i].DbSound == 1)
                {
                    Smoke[i - PageOffset].SetActive(true);
                }
                else
                {
                    Smoke[i - PageOffset].SetActive(false);
                }
                if (Trains[i].DbROTWEISS == 1)
                {
                    LightSwitch[i - PageOffset].SetActive(true);
                }
                else
                {
                    LightSwitch[i - PageOffset].SetActive(false);
                }
            }
        }

        for (int i = 0; i < Trains.Count; i++)
        {
            if (Trains[i].Checked == false)
            {
                DateTime date = DateTime.Now;
                DateTime date1 = new DateTime((Int32.Parse((Jahr[Trains[i].DbWartungJahr]).ToString()) + UserSettings.Maintenance), Int32.Parse(Monat[Trains[i].DbWartungMonat]), Int32.Parse(Tag[Trains[i].DbWartungTag]));
                Trains[i].Checked = true;
                if (firstStart != 1)
                {
                    CompleteTrains = CompleteTrains + 1;
                }
                if (date1 >= date)
                {
                    if (firstStart != 1)
                    {
                        nonWartungsTrains = nonWartungsTrains + 1;
                    }
                    Trains[i].Wartung = false;
                }
                else
                {
                    if (firstStart != 1)
                    {
                        WartungsTrains = WartungsTrains + 1;
                    }
                    Trains[i].Wartung = true;
                }
            }
        }

        for (int i = PageOffset; i < Trains.Count && i < PageOffset2; i++)
        {
            if (startManager.IsGerman == true)
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "." + Jahr[Trains[i].DbWartungJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
                if (Trains[i].Wartung == true)
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].SetActive(true);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                    }
                    else
                    {
                        Smoke[i - PageOffset].SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                    }
                }
                else
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        WartungsPic[i - PageOffset].SetActive(false);
                        LightSwitch[i - PageOffset].SetActive(false);
                        Smoke[i - PageOffset].SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    }
                }
            }
            else
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "Color: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Gauge: " + vSpur[Trains[i].DbSpurweite] + " | INR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Detected: " + Trains[i].DbAngelegt + " | Last Maintenance: " + Monat[Trains[i].DbWartungMonat] + "." + Tag[Trains[i].DbWartungTag] + "." + Jahr[Trains[i].DbWartungJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
                if (Trains[i].Wartung == true)
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].SetActive(true);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].SetActive(false);

                        Slot1[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                    }
                }
                else
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    }
                }
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < 12; i++)
        {
            EditDeleteToggle[i] = DeleteEditToggle[i].isOn;
            if (EditDeleteToggle[i] == true)
            {
                SelectedID = (i + PageOffset);
            }
        }

        if (IsPremium == true)
        {
            if (startManager.IsGerman == true)
            {
                CTrains.text = "Gefundene Loks: " + CompleteTrains;
                WTrains.text = "Loks mit Wartung: " + WartungsTrains;
                NTrains.text = "Loks ohne Wartung: " + nonWartungsTrains;
            }
            else
            {

                CTrains.text = "Found Trains: " + CompleteTrains;
                WTrains.text = "Trains with Maintenance: " + WartungsTrains;
                NTrains.text = "Trains without Maintenance: " + nonWartungsTrains;
            }
        }
        else
        {

            CTrains.text = "";
            WTrains.text = "";
            NTrains.text = "";
        }
    }

    public void readIntervall()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            if (Trains[i].Checked == false)
            {
                DateTime date = DateTime.Now;
                DateTime date1 = new DateTime((Int32.Parse((Jahr[Trains[i].DbWartungJahr]).ToString()) + UserSettings.Maintenance), Int32.Parse(Monat[Trains[i].DbWartungMonat]), Int32.Parse(Tag[Trains[i].DbWartungTag]));
                Trains[i].Checked = true;
                if (firstStart != 1)
                {
                    CompleteTrains = CompleteTrains + 1;
                }
                if (date1 >= date)
                {
                    if (firstStart != 1)
                    {
                        nonWartungsTrains = nonWartungsTrains + 1;
                    }
                    Trains[i].Wartung = false;
                    startManager.Log("Modul Train_List :: Lok ID: " + (i + 1) + " Wartung am: " + date1.ToString("dd.MM.yyyy"), "Modul Train_List :: Lok ID: " + i + "  Maintenance at: " + date1.ToString("MM.DD.yyyy"));
                }
                else
                {
                    if (firstStart != 1)
                    {
                        WartungsTrains = WartungsTrains + 1;
                    }
                    Trains[i].Wartung = true;
                    startManager.Log("Modul Train_List :: Lok ID: " + (i + 1) + " Hat das Maximal Datum Erreicht, Wartung Aktiv ***#WARNUNG#***", "Modul Train_List :: Lok ID: " + i + " Has reached the maximum date, maintenance active ***#WARNING#***");
                }
            }
        }
        startManager.Log("Modul Train_List :: Loks mit Wartung: " + WartungsTrains + " Loks ohne Wartung: " + nonWartungsTrains + " Gespeicherte Loks: " + CompleteTrains, "Modul Train_List :: Trains with Maintenance: " + WartungsTrains + " Trains without Maintenance: " + nonWartungsTrains + " Total Trains: " + CompleteTrains);
        firstStart = 1;
    }

    public void SetCurrentScreen()
    {
        ClearScreen();

        if (IsPremium == true)
        {
            if (startManager.IsGerman == true)
            {
                CTrains.text = "Gefundene Loks: " + CompleteTrains;
                WTrains.text = "Loks mit Wartung: " + WartungsTrains;
                NTrains.text = "Loks ohne Wartung: " + nonWartungsTrains;
            }
            else
            {

                CTrains.text = "Found Trains: " + CompleteTrains;
                WTrains.text = "Trains with Maintenance: " + WartungsTrains;
                NTrains.text = "Trains without Maintenance: " + nonWartungsTrains;
            }
        }

        for (int i = PageOffset; i < Trains.Count && i < PageOffset2; i++)
        {
            if (startManager.IsGerman == true)
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "." + Jahr[Trains[i].DbWartungJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
                if (Trains[i].Wartung == true)
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].SetActive(true);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].SetActive(false);

                        Slot1[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                    }
                }
                else
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    }
                }
            }
            else
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "Color: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Gauge: " + vSpur[Trains[i].DbSpurweite] + " | INR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Detected: " + Trains[i].DbAngelegt + " | Last Maintenance: " + Monat[Trains[i].DbWartungMonat] + "." + Tag[Trains[i].DbWartungTag] + "." + Jahr[Trains[i].DbWartungJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
                if (Trains[i].Wartung == true)
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].SetActive(true);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].SetActive(false);

                        Slot1[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                    }
                }
                else
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    }
                }
            }
        }
    }

    public void ReadTrains()
    {
        Trains = new List<TrainData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
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
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            startManager.LogError("Fehler beim Laden der Lokdaten.", "Error Loading Locomotive Data", " Train_List :: ReadAllTrains(); Error: " + ex);
        }
        finally
        {
            CacheImage = new Texture2D[Trains.Count];
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains"))
            {
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains");
            }

            for (int i = 0; i < Trains.Count; i++)
            {
                if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + UserSettings.ImageType))
                {
                    File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + UserSettings.ImageType);
                    startManager.Log("Modul Train_List :: Lok ID: " + i + " Kein Bild vorhanden, Erstelle standart Bild.", "Modul Train_List :: Lok ID: " + i + " No picture available, Create standard Picture");
                }
                else
                {
                    StartCoroutine(LoadImage((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + UserSettings.ImageType), i));
                }
            }
            dbConnection.Close();
            dbConnection = null;
        }
        startManager.Notify("Alle Loks Eingelesen", "All Locos are Read", "green", "green");
        startManager.Log("Modul Train_List :: Alle Loks Eingelesen.", "Modul Train_List :: All Locos are Read");
    }

    public void RefreschTrains()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            if (Trains[i].Checked == false)
            {
                Trains[i].Checked = true;
            }
        }

        if (IsPremium == true)
        {
            if (startManager.IsGerman == true)
            {
                CTrains.text = "Gefundene Loks: " + CompleteTrains;
                WTrains.text = "Loks mit Wartung: " + WartungsTrains;
                NTrains.text = "Loks ohne Wartung: " + nonWartungsTrains;
            }
            else
            {

                CTrains.text = "Found Trains: " + CompleteTrains;
                WTrains.text = "Trains with Maintenance: " + WartungsTrains;
                NTrains.text = "Trains without Maintenance: " + nonWartungsTrains;
            }
        }

        for (int i = PageOffset; i < Trains.Count && i < PageOffset2; i++)
        {
            if (startManager.IsGerman == true)
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "FARBE: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Spur: " + vSpur[Trains[i].DbSpurweite] + " | AtNR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Erfasst: " + Trains[i].DbAngelegt + " | Letzte Wartung am: " + Tag[Trains[i].DbWartungTag] + "." + Monat[Trains[i].DbWartungMonat] + "." + Jahr[Trains[i].DbWartungJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
                if (Trains[i].Wartung == true)
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].SetActive(true);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].SetActive(false);

                        Slot1[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                    }
                }
                else
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    }
                }
            }
            else
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + Trains[i].DbBaureihe + " | " + "Color: " + Trains[i].DbFarbe + " | " + "#: " + Trains[i].DbAdresse + " -> " + vProtokoll[Trains[i].DbProtokoll];
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DbHersteller] + " | Gauge: " + vSpur[Trains[i].DbSpurweite] + " | INR: " + Trains[i].DbKatalognummer + " | " + "SNR: " + Trains[i].DbSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Detected: " + Trains[i].DbAngelegt + " | Last Maintenance: " + Monat[Trains[i].DbWartungMonat] + "." + Tag[Trains[i].DbWartungTag] + "." + Jahr[Trains[i].DbWartungJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
                if (Trains[i].Wartung == true)
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].SetActive(true);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol0;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].SetActive(false);

                        Slot1[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = ColorWartung;
                    }
                }
                else
                {
                    if (IsPremium == true)
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                        Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    }
                    else
                    {
                        WartungsPic[i - PageOffset].gameObject.SetActive(false);
                        Slot1[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot2[i - PageOffset].GetComponent<Text>().color = NonWartung;
                        Slot3[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    }
                }
            }
        }
    }

    IEnumerator LoadImage(string url, int i)
    {
        Texture2D tex;
        tex = new Texture2D(2, 2, TextureFormat.DXT1, false);
        using (WWW www = new WWW("file:///" + url.Replace("\\", "/")))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            CacheImage[i] = tex;
        }
    }

    public void PageVorward()
    {
        if (Trains.Count >= PageOffset)
        {
            PageOffset2 = PageOffset2 + 12;
            PageOffset = PageOffset + 12;
            CurrentPage = CurrentPage + 1;
            Page.text = CurrentPage.ToString();
            SetCurrentScreen();
        }
        else
        {
            PageOffset2 = 12;
            PageOffset = 0;
            CurrentPage = 1;
            Page.text = CurrentPage.ToString();
            SetCurrentScreen();
        }
    }

    public void PageBack()
    {
        if (PageOffset == 0)
        {
        }
        else
        {
            PageOffset2 = PageOffset2 - 12;
            PageOffset = PageOffset - 12;
            CurrentPage = CurrentPage - 1;
            Page.text = CurrentPage.ToString();
            SetCurrentScreen();
        }
    }

    void ClearScreen()
    {
        for (int i = 0; i < 12; i++)
        {
            Slot[i].gameObject.SetActive(false);
            Slot1[i].GetComponent<Text>().text = "";
            Slot2[i].GetComponent<Text>().text = "";
            Slot3[i].GetComponent<Text>().text = "";
            DeleteEditToggle[i].isOn = false;
            LightSwitch[i].SetActive(false);
            Smoke[i].SetActive(false);
            TrainID[i].gameObject.SetActive(false);
        }
    }

    public void RefreschImages()
    {
        for (int i = 0; i < Trains.Count; i++)
        {
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + UserSettings.ImageType))
            {
                File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + UserSettings.ImageType);
                startManager.Log("Modul Train_List :: Lok ID: " + i + " Kein Bild vorhanden, Erstelle standart Bild.", "Modul Train_List :: Lok ID: " + i + " No picture available, Create standard Picture");
            }
            else
            {
                StartCoroutine(LoadImage((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + UserSettings.ImageType), i));
            }
        }
    }

    public void DeleteTrain()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Keine Lok ausgewählt", "No Train Selected", "red", "red");
        }
        else
        {
            try
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                dbConnection.Open();
                string sql = "DELETE FROM Trains WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer + "' AND  ERSTELLT='" + Trains[SelectedID].DbAngelegt + "'  ";
                SqliteCommand Command = new SqliteCommand(sql, dbConnection);
                Command.ExecuteNonQuery();
                dbConnection.Close();
                Trains.RemoveAt(SelectedID);
                readIntervall();
                SetCurrentScreen();
                File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (SelectedID + 1) + "." + UserSettings.ImageType);
                SelectedID = -1;
            }
            catch (SqliteException ex)
            {
                startManager.LogError("Modul Train_List :: Lok wurde Gelöscht id:" + SelectedID, "Modul Train_List :: Train Removed id:" + SelectedID, " Train_List :: DeleteTrain(); Error: " + ex);
            }
            finally
            {
                startManager.Notify("Lok wurde Gelöscht id:" + (SelectedID + 1), "Train Removed id:" + (SelectedID + 1), "green", "green");
                startManager.Log("Modul Train_List :: Lok wurde Gelöscht id:" + SelectedID, "Modul Train_List :: Train Removed id:" + SelectedID);
            }
            DeleteSchutz();
        }
    }

    public void DeleteSchutz()
    {
        SelectedID = -1;
    }

    public void SaveEditTrain()
    {
        if (IsEditMode == true)
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE Trains  SET BAUREIHE = @BAUREIHE, FARBE = @FARBE , TYP = @TYP , HERSTELLER = @HERSTELLER , KATALOGNUMMER = @KATALOGNUMMER , SERIENNUMMER = @SERIENNUMMER , PREIS = @PREIS , WARTUNGDAY = @WARTUNGDAY ,WARTUNGMONAT = @WARTUNGMONAT , WARTUNGJEAR = @WARTUNGJEAR , ADRESSE = @ADRESSE , PROTOKOLL = @PROTOKOLL , FAHRSTUFEN =@FAHRSTUFEN , DECHERSTELLER = @DECHERSTELLER , KTAG = @KTAG , KMONAT = @KMONAT , KJAHR = @KJAHR, SPURWEITE = @SPURWEITE, CV2 = @CV2,  CV3 = @CV3, CV4 = @CV4, CV5 = @CV5, RAUCH = @RAUCH , SOUND = @SOUND, ROTWEISS = @ROTWEISS, PANDO = @PANDO, TELEX = @TELEX, KUPPLUNG = @KUPPLUNG, LAGERORT = @LAGERORT WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer + "' AND ERSTELLT='" + Trains[SelectedID].DbAngelegt + "'  ";
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
                if (EditRauch.isOn == true)
                {
                    command.Parameters.AddWithValue("@RAUCH", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@RAUCH", 0);
                }

                if (EditSound.isOn == true)
                {
                    command.Parameters.AddWithValue("@SOUND", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@SOUND", 0);
                }

                if (EditLichtWechsel.isOn == true)
                {
                    command.Parameters.AddWithValue("@ROTWEISS", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@ROTWEISS", 0);
                }

                if (EditElektrischePandos.isOn == true)
                {
                    command.Parameters.AddWithValue("@PANDO", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@PANDO", 0);
                }

                if (EditTelex.isOn == true)
                {
                    command.Parameters.AddWithValue("@TELEX", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@TELEX", 0);
                }

                if (EditElektrischeKupplung.isOn == true)
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 0);
                }

                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern.", "Error by Save Train.", " Train_List :: SaveEditTrain().IsEditMode==True; Error: " + ex);
                }
                finally
                {
                    readIntervall();
                    SetCurrentScreen();
                    startManager.Notify("Lok wurde Bearbeited", "Train edited", "green", "green");
                }
                dbConnection.Close();
                dbConnection = null;
            }
            SelectedID = -1;
            IsEditMode = false;
        }
        else
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Trains (BAUREIHE , FARBE , TYP , HERSTELLER , KATALOGNUMMER , SERIENNUMMER , PREIS , WARTUNGDAY ,WARTUNGMONAT , WARTUNGJEAR ,ADRESSE , PROTOKOLL , FAHRSTUFEN , DECHERSTELLER , RAUCH , SOUND , ROTWEISS ,PANDO , TELEX , KUPPLUNG , KTAG , KMONAT , KJAHR, SPURWEITE, IDENTIFYER, LAGERORT, CV2, CV3, CV4, CV5) VALUES" + " " +
                    "(@BAUREIHE , @FARBE , @TYP ,  @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @PREIS , @WARTUNGDAY , @WARTUNGMONAT , @WARTUNGJEAR , @ADRESSE , @PROTOKOLL , @FAHRSTUFEN , @DECHERSTELLER , @RAUCH , @SOUND , @ROTWEISS , @PANDO , @TELEX , @KUPPLUNG, @KTAG , @KMONAT , @KJAHR, @SPURWEITE, @IDENTIFYER, @LAGERORT, @CV2, @CV3, @CV4, @CV5)";
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
                command.Parameters.AddWithValue("@LAGERORT", Lager.value);
                command.Parameters.AddWithValue("@ADRESSE", CV1.text);
                command.Parameters.AddWithValue("@CV2", 0);
                command.Parameters.AddWithValue("@CV3", 0);
                command.Parameters.AddWithValue("@CV4", 0);
                command.Parameters.AddWithValue("@CV5", 0);
                if (EditRauch.isOn == true)
                {
                    command.Parameters.AddWithValue("@RAUCH", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@RAUCH", 0);
                }

                if (EditSound.isOn == true)
                {
                    command.Parameters.AddWithValue("@SOUND", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@SOUND", 0);
                }

                if (EditLichtWechsel.isOn == true)
                {
                    command.Parameters.AddWithValue("@ROTWEISS", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@ROTWEISS", 0);
                }

                if (EditElektrischePandos.isOn == true)
                {
                    command.Parameters.AddWithValue("@PANDO", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@PANDO", 0);
                }

                if (EditTelex.isOn == true)
                {
                    command.Parameters.AddWithValue("@TELEX", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@TELEX", 0);
                }

                if (EditElektrischeKupplung.isOn == true)
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 0);
                }

                command.Parameters.AddWithValue("@IDENTIFYER", Guid.NewGuid().ToString());
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern.", "Error by Save Train.", " Train_List :: SaveEditTrain().IsEditMode==False; Error: " + ex);
                }
                finally
                {
                    if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (Trains.Count + 1) + "." + UserSettings.ImageType))
                    {
                        File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (Trains.Count + 1) + "." + UserSettings.ImageType);
                    }
                    startManager.Notify("Lok Gespeichert", "Train Saved", "green", "green");
                }
                dbConnection.Close();
                dbConnection = null;
            }
            SelectedID = -1;
            ReadTrains();
        }
        SetCurrentScreen();
    }

    public void GetLokData()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Keine Lok ausgewählt", "No Train Selected", "red", "red");
        }
        else
        {
            IsEditMode = true;
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

            if (Trains[SelectedID].DbRauch == 1)
            {
                EditRauch.isOn = true;
            }
            else
            {
                EditRauch.isOn = false;
            }

            if (Trains[SelectedID].DbSound == 1)
            {
                EditSound.isOn = true;
            }
            else
            {
                EditSound.isOn = false;
            }

            if (Trains[SelectedID].DbROTWEISS == 1)
            {
                EditLichtWechsel.isOn = true;
            }
            else
            {
                EditLichtWechsel.isOn = false;
            }

            if (Trains[SelectedID].DbPandos == 1)
            {
                EditElektrischePandos.isOn = true;
            }
            else
            {
                EditElektrischePandos.isOn = false;
            }

            if (Trains[SelectedID].DbTelex == 1)
            {
                EditTelex.isOn = true;
            }
            else
            {
                EditTelex.isOn = false;
            }
        
            if (Trains[SelectedID].DbElekKupplung == 1)
            {
                EditElektrischeKupplung.isOn = true;
            }
            else
            {
                EditElektrischeKupplung.isOn = false;
            }

            TrainPic.GetComponent<RawImage>().texture = CacheImage[SelectedID];
            for (int i = 0; i < 12; i++)
            {
                DeleteEditToggle[i].isOn = false;
            }
        }
    }

    public void SendWWW()
    {
        uniqueID = DateTime.Now.ToString("HHmmssddMMyyyy");
        StartCoroutine(SendSelected());
    }

    IEnumerator SendSelected()
    {
        string FinshURL = startManager.WebExporterUrl + "/insert.php?uniqueID=" + uniqueID + "&data=ISTRAIN," + Trains[SelectedID].DbBaureihe.ToString() + "," + Trains[SelectedID].DbFarbe.ToString() + "," + Trains[SelectedID].DbLokTyp.ToString() + "," + Trains[SelectedID].DbHersteller.ToString() + "," + Trains[SelectedID].DbKatalognummer.ToString() + "," + Trains[SelectedID].DbSeriennummer.ToString() + "," + Trains[SelectedID].DbKaufTag.ToString() + "," + Trains[SelectedID].DbKaufMonat.ToString() + "," + Trains[SelectedID].DbKaufJahr.ToString() + "," + Trains[SelectedID].DbPreis.ToString() + "," + Trains[SelectedID].DbWartungTag.ToString() + "," + Trains[SelectedID].DbWartungMonat.ToString() + "," + Trains[SelectedID].DbWartungJahr.ToString() + "," + Trains[SelectedID].DbAdresse.ToString() + "," + Trains[SelectedID].DbProtokoll.ToString() + "," + Trains[SelectedID].DbFahrstufen.ToString() + "," + "EMPTY" + "," + Trains[SelectedID].DbDecHersteller.ToString() + "," + Trains[SelectedID].DbAngelegt.ToString() + "," + Trains[SelectedID].DbRauch.ToString() + "," + Trains[SelectedID].DbSound.ToString() + "," + Trains[SelectedID].DbROTWEISS.ToString() + "," + "EMPTY" + "," + Trains[SelectedID].DbPandos.ToString() + "," + Trains[SelectedID].DbTelex.ToString() + "," + Trains[SelectedID].DbElekKupplung.ToString() + "," + Trains[SelectedID].DbSpurweite.ToString() + "," + Trains[SelectedID].DbCV2.ToString() + "," + Trains[SelectedID].DbCV3.ToString() + "," + Trains[SelectedID].DbCV4.ToString() + "," + Trains[SelectedID].DbCV5.ToString() + "," + Trains[SelectedID].DBIdentifyer.ToString() + "," + Trains[SelectedID].DBLagerort.ToString();
        Debug.Log(FinshURL);
        WWW insert = new WWW(FinshURL);

        yield return insert;

        if (insert.error != null)
        {
            startManager.LogError("Keine Verbindung zum Server möglich!.", "No Connection to the Server.", " Train_List :: SendSelected(); Error: " + insert.error);
        }
        if (insert.isDone)
        {
            startManager.Notify("Lok Gesendet", "Train Send", "green", "green");
            Win.SetActive(true);
            SendOK.text = uniqueID.ToString();
        }
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
        dataexporter.Image = File.ReadAllBytes((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (SelectedID + 1) + "." + UserSettings.ImageType));
        string jsonData = JsonUtility.ToJson(dataexporter, true);
        File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Exporter/" + vHersteller[Trains[SelectedID].DbHersteller] + "-" + Trains[SelectedID].DbKatalognummer + ".TRAIN", jsonData);
        SelectedID = -1;
        startManager.Notify("Lok als Datei Exportiert", "Train as File Exported", "green", "green");
    }
}