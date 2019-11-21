using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Mono.Data.Sqlite;
using System.Data;

public class Import_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public Import_Manager IM;
    public Train_List TL;
    public Wagon_List WL;

    [Header("WorkFlow")]
    public int PageOffset = 0;
    public int PageOffset2 = 15;
    public int CurrentPage = 1;
    public int selectedID;
    public int FoundRows = 0;
    public string Reader = "";
    [Header("Elements")]
    public Button[] Button;
    public Text Page;
    public string[] Imports;
    public GameObject DataPanel;
    public GameObject[] Wartung;
    public Text BaureiheText;
    public Text ColorText;
    public Text AdresseText;
    public Text Day;
    public Text Month;
    public Text Year;
    public Text WDay;
    public Text WMonth;
    public Text WYear;
    public Text KatalogNummerText;
    public Text SeriennummerText;
    public Toggle ImageCopy;
    [Header("Cache Data")]
    public string Type = "";
    public string Baureihe = "";
    public string Farbe = "";
    public int Energy;
    public int Hersteller;
    public string Erstellt = "";
    public int Spurweite;
    public string KatalogNummer = "";
    public string Seriennummer = "";
    public int KaufDay;
    public int KaufMonth;
    public int KaufYear;
    public string KaufPreis = "";
    public int WartungDay;
    public int WartungMonth;
    public int WartungYear;
    public string Adresse = "";
    public int Protokoll;
    public int Fahrstufen;
    public string DecoderHersteller = "";
    public int Rauch;
    public int Telex;
    public int Sound;
    public int LichtWechsel;
    public int ElektrischeKupplung;
    public int ElektrischePandos;
    public int Typ;
    public string Katalognummer = "";
    public int KaufTag;
    public int KaufMonat;
    public int KaufJahr;
    public int Preis;
    public int Kupplung;
    public int Licht;
    public int Preiser;
    public string CV1 = "";
    public string CV2 = "";
    public string CV3 = "";
    public string CV4 = "";
    public string CV5 = "";
    public RawImage Image2;
    private DataExporter dataexporter;
    private byte[] IMG;

    void Start()
    {
        startManager.Log("Lade Import_Manager -> Nachricht ist Normal.", "Load Import_Manager -> message is normal");
        ClearScreen();
        GetRows();
        dataexporter = new DataExporter();
    }

    public void GetRows()
    {
        Imports = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Importer/");
        for (int i = PageOffset; i < Imports.Length && i < PageOffset2; i++)
        {
            FoundRows = i + 1;
            Button[i - PageOffset].GetComponentInChildren<Text>().text = Path.GetFileName(Imports[i]);
            Button[i - PageOffset].gameObject.SetActive(true);
        }
    }

    public void ButtonID(int id)
    {
        selectedID = id;
        Debug.Log("DATA: " + id);
        DataPanel.SetActive(true);
        Reader = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Importer/" + IM.Button[id].GetComponentInChildren<Text>().text);
        cacheData();
    }

    public void cacheData()
    {
        try
        {
            dataexporter = JsonUtility.FromJson<DataExporter>(File.ReadAllText(Reader));
            Type = dataexporter.Type;
            Baureihe = dataexporter.Baureihe;
            Farbe = dataexporter.Farbe;
            Energy = dataexporter.Energy;
            Hersteller = dataexporter.Hersteller;
            Erstellt = dataexporter.Erstellt;
            Spurweite = dataexporter.Spurweite;
            KatalogNummer = dataexporter.KatalogNummer;
            Seriennummer = dataexporter.Seriennummer;
            KaufDay = dataexporter.KaufDay;
            KaufMonth = dataexporter.KaufMonth;
            KaufYear = dataexporter.KaufYear;
            KaufPreis = dataexporter.KaufPreis;
            WartungDay = dataexporter.WartungDay;
            WartungMonth = dataexporter.WartungMonth;
            WartungYear = dataexporter.WartungYear;
            Adresse = dataexporter.Adresse;
            Protokoll = dataexporter.Protokoll;
            Fahrstufen = dataexporter.Fahrstufen;
            DecoderHersteller = dataexporter.DecoderHersteller;
            Typ = dataexporter.Typ;
            Katalognummer = dataexporter.Katalognummer;
            KaufTag = dataexporter.KaufTag;
            KaufMonat = dataexporter.KaufMonat;
            KaufJahr = dataexporter.KaufJahr;
            Rauch = dataexporter.Rauch;
            Telex = dataexporter.Telex;
            Sound = dataexporter.Sound;
            LichtWechsel = dataexporter.LichtWechsel;
            ElektrischeKupplung = dataexporter.ElektrischeKupplung;
            ElektrischePandos = dataexporter.ElektrischePandos;
            Preis = dataexporter.Preis;
            Kupplung = dataexporter.Kupplung;
            Licht = dataexporter.Licht;
            Preiser = dataexporter.Preiser;
            CV1 = dataexporter.CV1;
            CV2 = dataexporter.CV2;
            CV3 = dataexporter.CV3;
            CV4 = dataexporter.CV4;
            CV5 = dataexporter.CV5;
            IMG = dataexporter.Image;
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(dataexporter.Image);
            Image2.texture = tex;
        }
        catch (Exception ex)
        {
            startManager.LogError("Fehler beim Lesen der Daten.", "Error by Read the Data", " Import_Manager :: cacheData; Error: " + ex);
            startManager.Error("cacheData(Import)", ex.ToString());
        }
        finally
        {
            startManager.Log("Modul Import_Manager :: Daten Erfolgreich Geladen, Öffne Daten Panel", "Modul Import_Manager :: Data Read, Open the Data Panel");
            if (Type == "TRAIN")
            {
                BaureiheText.text = "Baureihe: " + Baureihe;
                ColorText.text = "Farbe: " + Farbe;
                AdresseText.text = "Adresse: " + Adresse;
                Day.text = "Kauftag: " + (KaufDay + 1) + ".";
                Month.text = (KaufMonat + 1).ToString() + "." + (KaufJahr + 1990).ToString();
                Year.text = "";
                WDay.text = "Wartung: " + (WartungDay + 1) + ".";
                WMonth.text = (WartungMonth + 1).ToString() + "." + (WartungYear + 1990).ToString();
                WYear.text = "";
                SeriennummerText.text = "Seriennummer: " + Seriennummer;
                KatalogNummerText.text = "Katalognummer: " + KatalogNummer;
                //SaveTrain();
            }
            if (Type == "WAGON")
            {
                AdresseText.gameObject.SetActive(false);
                Wartung[0].SetActive(false);
                Wartung[1].SetActive(false);
                Wartung[2].SetActive(false);
                //SaveWagon();
                if (Typ == 0) { BaureiheText.text = "Wagon Art:  Personenwagon"; }
                if (Typ == 1) { BaureiheText.text = "Wagon Art:  Güterwagon"; }
                if (Typ == 2) { BaureiheText.text = "Wagon Art:  Sonstigerwagon"; }
                ColorText.text = "Farbe: " + Farbe;
                Day.text = "Kauftag: " + (KaufTag + 1) + ".";
                Month.text = (KaufMonat + 1).ToString() + "." + (KaufJahr + 1990).ToString();
                Year.text = "";

                SeriennummerText.text = "Seriennummer: " + Seriennummer;
                KatalogNummerText.text = "Katalognummer: " + Katalognummer;
            }
        }
    }

    void ClearScreen()
    {
        for (int i = 0; i < 15; i++)
        {
            Button[i].gameObject.SetActive(false);
        }
    }

    public void PageVor()
    {
        if (Imports.Length >= PageOffset)
        {
            PageOffset2 = PageOffset2 + 15;
            PageOffset = PageOffset + 15;
            CurrentPage = CurrentPage + 1;
            Page.text = CurrentPage.ToString();
            GetRows();
        }
        else
        {
            PageOffset2 = 15;
            PageOffset = 0;
            CurrentPage = 1;
            Page.text = CurrentPage.ToString();
            GetRows();
        }
    }

    public void PageBack()
    {
        if (PageOffset == 0)
        {
        }
        else
        {
            PageOffset2 = PageOffset2 - 15;
            PageOffset = PageOffset - 15;
            CurrentPage = CurrentPage - 1;
            Page.text = CurrentPage.ToString();
        }
    }

    public void Import()
    {
        if (Type == "TRAIN")
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Trains (BAUREIHE , FARBE , TYP , HERSTELLER , KATALOGNUMMER , SERIENNUMMER , PREIS , WARTUNGDAY ,WARTUNGMONAT , WARTUNGJEAR ,ADRESSE , PROTOKOLL , FAHRSTUFEN , DECHERSTELLER , RAUCH , SOUND , ROTWEISS ,PANDO , TELEX , KUPPLUNG , KTAG , KMONAT , KJAHR, SPURWEITE, IDENTIFYER, LAGERORT, CV2, CV3, CV4, CV5) VALUES" + " " +
                    "(@BAUREIHE , @FARBE , @TYP ,  @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @PREIS , @WARTUNGDAY , @WARTUNGMONAT , @WARTUNGJEAR , @ADRESSE , @PROTOKOLL , @FAHRSTUFEN , @DECHERSTELLER , @RAUCH , @SOUND , @ROTWEISS , @PANDO , @TELEX , @KUPPLUNG, @KTAG , @KMONAT , @KJAHR, @SPURWEITE, @IDENTIFYER, @LAGERORT, @CV2, @CV3, @CV4, @CV5)";
                command.Parameters.AddWithValue("@BAUREIHE", Baureihe);
                command.Parameters.AddWithValue("@FARBE", Farbe);
                command.Parameters.AddWithValue("@TYP", Energy);
                command.Parameters.AddWithValue("@HERSTELLER", Hersteller);
                command.Parameters.AddWithValue("@KATALOGNUMMER", KatalogNummer);
                command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer);
                command.Parameters.AddWithValue("@PREIS", Preis + KaufPreis);
                command.Parameters.AddWithValue("@WARTUNGDAY", WartungDay);
                command.Parameters.AddWithValue("@WARTUNGMONAT", WartungMonth);
                command.Parameters.AddWithValue("@WARTUNGJEAR", WartungYear);
                command.Parameters.AddWithValue("@ADRESSE", Adresse);
                command.Parameters.AddWithValue("@PROTOKOLL", Protokoll);
                command.Parameters.AddWithValue("@FAHRSTUFEN", Fahrstufen);
                command.Parameters.AddWithValue("@DECHERSTELLER", DecoderHersteller);
                command.Parameters.AddWithValue("@KTAG", KaufDay);
                command.Parameters.AddWithValue("@KMONAT", KaufMonth);
                command.Parameters.AddWithValue("@KJAHR", KaufYear);
                command.Parameters.AddWithValue("@SPURWEITE", Spurweite);
                command.Parameters.AddWithValue("@LAGERORT", 0);
                command.Parameters.AddWithValue("@CV2", CV2);
                command.Parameters.AddWithValue("@CV3", CV3);
                command.Parameters.AddWithValue("@CV4", CV4);
                command.Parameters.AddWithValue("@CV5", CV5);
                command.Parameters.AddWithValue("@RAUCH", 0);
                command.Parameters.AddWithValue("@SOUND", 0);
                command.Parameters.AddWithValue("@ROTWEISS", 0);
                command.Parameters.AddWithValue("@PANDO", 0);
                command.Parameters.AddWithValue("@TELEX", 0);
                command.Parameters.AddWithValue("@KUPPLUNG", 0);
                command.Parameters.AddWithValue("@IDENTIFYER", Guid.NewGuid().ToString());
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern.", "Error by Save Train.", " Import_Manager :: SaveEditTrain().Type == 'TRAIN' Error: " + ex);
                    startManager.Error("Import(Train);", "" + ex);
                }
                finally
                {
                    if (ImageCopy.isOn == true)
                    {
                        File.WriteAllBytes(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (TL.Trains.Count + 1) + "." + UserSettings.ImageType, IMG);
                    }
                    else
                    {
                        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (TL.Trains.Count + 1) + "." + UserSettings.ImageType))
                        {
                            File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (TL.Trains.Count + 1) + "." + UserSettings.ImageType);
                        }
                    }
                    startManager.Notify("Lok Importiert", "Train Importiert", "green", "green");
                }
                dbConnection.Close();
                dbConnection = null;
                TL.RefreshIndex();
            }
        }
        else
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Wagons  (TYP , FARBE , HERSTELLER ,  KATALOGNUMMER , SERIENNUMMER , KAUFDAY , KAUFMONAT , KAUFJAHR , PREIS , KUPPLUNG , LICHT , PREISER, SPURWEITE, IDENTIFYER , LAGERORT) VALUES" + " (@TYP , @FARBE , @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @KAUFDAY , @KAUFMONAT , @KAUFJAHR , @PREIS , @KUPPLUNG , @LICHT , @PREISER, @SPURWEITE, @IDENTIFYER , @LAGERORT)";
                command.Parameters.AddWithValue("@FARBE", Farbe);
                command.Parameters.AddWithValue("@TYP", Typ);
                command.Parameters.AddWithValue("@HERSTELLER", Hersteller);
                command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer);
                command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer);
                command.Parameters.AddWithValue("@PREIS", Preis);
                command.Parameters.AddWithValue("@KAUFDAY", KaufTag);
                command.Parameters.AddWithValue("@KAUFMONAT", KaufMonat);
                command.Parameters.AddWithValue("@KAUFJAHR", KaufJahr);
                command.Parameters.AddWithValue("@SPURWEITE", Spurweite);
                command.Parameters.AddWithValue("@LAGERORT", 0);
                command.Parameters.AddWithValue("@IDENTIFYER", Guid.NewGuid().ToString());
                command.Parameters.AddWithValue("@KUPPLUNG", Kupplung);
                command.Parameters.AddWithValue("@LICHT", Licht);
                command.Parameters.AddWithValue("@PREISER", Preiser);
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern.", "Error by Save.", " Import_Manager :: SaveEditTrain().Type == 'TRAIN' Error: " + ex);
                    startManager.Error("Import(Wagon);", "" + ex);
                }
                finally
                {
                    if (ImageCopy.isOn == true)
                    {
                        File.WriteAllBytes(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WL.Trains.Count + 1) + "." + UserSettings.ImageType, IMG);
                    }
                    else
                    {
                        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WL.Trains.Count + 1) + "." + UserSettings.ImageType))
                        {
                            File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WL.Trains.Count + 1) + "." + UserSettings.ImageType);
                        }
                    }
                    startManager.Notify("Wagon Gespeichert", "Wagon Saved", "green", "green");
                    WL.RefreschIndex();
                }
                dbConnection.Close();
                dbConnection = null;
            }
        }
    }
}