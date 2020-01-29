//Überarbeitete Version vom 17.11.2019 19:53
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

public class Wagon_List : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public Network_Manager NM;
    [Header("Lok View - Elemente")]
    public List<WagonData> Trains;
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
    public string[] vTyp;
    public Toggle[] DeleteEditToggle;
    public bool[] EditDeleteToggle;
    public Text CTrains;
    public Text WTrains;
    public Text NTrains;
    public Text[] TrainID;
    [Header("Edit-Panel-Inputs")]
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
    public int TotalSumme = 0;
    public GameObject Win;
    public InputField SendOK;
    public string ImageType;
    public Texture2D StandArtPic;

    void Start()
    {
        dataexporter = new DataExporter();
        startManager.Log("Lade Wagon_List -> Nachricht ist Normal.", "Load Wagon_List -> message is normal");
        if (startManager._IsReady == true)
        {
            RefreschIndex();
        }
        else
        {
            startManager.Notify("Warnung Alte Datenbank Erkannt, Bitte Löschen ist nicht nutzbar", "Old Databse Dedected, The Database is Not Useable", "blue", "blue");
        }
        IsPremium = UserSettings.Premium;
    }

    private void Update()
    {
        for (int i = 0; i < 12; i++)
        {
            EditDeleteToggle[i] = DeleteEditToggle[i].isOn;
            if (EditDeleteToggle[i] == true)
            {
                SelectedID = (i + PageOffset);
            }
        }
        ImageType = UserSettings.ImageType;
        CompleteTrains = Trains.Count;
        if (IsPremium == true)
        {
            if (startManager.IsGerman == true)
            {
                CTrains.text = "Gefundene Wagons: " + CompleteTrains;
                WTrains.text = "";
                NTrains.text = "";
            }
            else
            {
                CTrains.text = "Found Wagons: " + CompleteTrains;
                WTrains.text = "";
                NTrains.text = "";
            }
        }
        else
        {
            CTrains.text = "";
            WTrains.text = "";
            NTrains.text = "";
        }

        for (int i = PageOffset; i < Trains.Count && i < PageOffset2; i++)
        {
            if (startManager.IsGerman == true)
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + Tag[Trains[i].DBKaufTag] + "." + Monat[Trains[i].DBKaufMonat] + "." + Jahr[Trains[i].DBKaufJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
                TrainID[i - PageOffset].text = "ID: " + (i + 1).ToString();

                if (IsPremium == true)
                {
                    Slot1[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    Slot2[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                    Slot3[i - PageOffset].GetComponent<Text>().color = UserSettings.newCol1;
                }
                else
                {

                    Slot1[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    Slot2[i - PageOffset].GetComponent<Text>().color = NonWartung;
                    Slot3[i - PageOffset].GetComponent<Text>().color = NonWartung;
                }
            }
            else
            {
                Slot[i - PageOffset].gameObject.SetActive(true);
                Slot1[i - PageOffset].GetComponent<Text>().text = " " + vTyp[Trains[i].DBTyp] + " | " + "FARBE: " + Trains[i].DBFarbe;
                Slot2[i - PageOffset].GetComponent<Text>().text = " " + vHersteller[Trains[i].DBHersteller] + " | Spur: " + vSpur[Trains[i].DBSpurweite] + " | AtNR: " + Trains[i].DBKatalognummer + " | " + "SNR: " + Trains[i].DBSeriennummer;
                Slot3[i - PageOffset].GetComponent<Text>().text = " Erfasst: " + Trains[i].DBErstellt + " | Kauf am: " + Tag[Trains[i].DBKaufTag] + "." + Monat[Trains[i].DBKaufMonat] + "." + Jahr[Trains[i].DBKaufJahr];
                SlotBild[i - PageOffset].texture = CacheImage[i];
            }
        }
    }

    public void RefreschIndex()
    {
        Trains = new List<WagonData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
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
                    TotalSumme = TotalSumme + reader.GetChar(9);
                    Trains.Add(trainData);
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            startManager.LogError("Fehler beim Laden der Wagondaten.", "Error Loading Wagondata Data", " Wagon_List :: ReadTrains(); Error: " + ex);
            startManager.Error("RefreshIndex(Wagons);", "" + ex);
        }
        finally
        {
            LoadIcon();
            dbConnection.Close();
            dbConnection.Dispose();
            dbConnection = null;
            startManager.Notify("Alle Wagons Eingelesen", "All Wagons are Read", "green", "green");
            startManager.Log("Modul Wagon_List :: " + Trains.Count + " Wagons Gefunden", "Modul Wagon_List :: " + Trains.Count + " Wagons Found");
        }
    }

    private void LoadIcon()
    {
        try
        {
            CacheImage = new Texture2D[Trains.Count];
            for (int i = 0; i < Trains.Count; i++)
            {
                if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + ImageType))
                {
                    CacheImage[i] = StandArtPic;
                }
                else
                {
                    StartCoroutine(LoadImage((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + ImageType), i));
                }
            }
        }
        catch(Exception ex)
        {
            startManager.Error("LoadIcon(Wagons)", ex.ToString());
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
        for (int i = 0; i < 12; i++)
        {
            DeleteEditToggle[i].isOn = false;
            Slot[i].gameObject.SetActive(false);
        }
        if (Trains.Count >= PageOffset)
        {
            PageOffset2 = PageOffset2 + 12;
            PageOffset = PageOffset + 12;
            CurrentPage = CurrentPage + 1;
            Page.text = CurrentPage.ToString();
        }
        else
        {
            PageOffset2 = 12;
            PageOffset = 0;
            CurrentPage = 1;
            Page.text = CurrentPage.ToString();
        }
    }

    public void PageBack()
    {
        for (int i = 0; i < 12; i++)
        {
            DeleteEditToggle[i].isOn = false;
            Slot[i].gameObject.SetActive(false);
        }
        if (PageOffset == 0)
        {
        }
        else
        {
            PageOffset2 = PageOffset2 - 12;
            PageOffset = PageOffset - 12;
            CurrentPage = CurrentPage - 1;
            Page.text = CurrentPage.ToString();
        }
    }

    public void DeleteTrain()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Keinen Wagon ausgewählt", "No Wagon Selected", "red", "red");
        }
        else
        {
            try
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                dbConnection.Open();
                string sql = "DELETE FROM Wagons WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer + "' AND  ERSTELLT='" + Trains[SelectedID].DBErstellt + "'  ";
                SqliteCommand Command = new SqliteCommand(sql, dbConnection);
                Command.ExecuteNonQuery();
                dbConnection.Close();
                File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (SelectedID + 1) + "." + UserSettings.ImageType);
            }
            catch (SqliteException ex)
            {
                startManager.LogError("Modul Wagon_List :: Wadon id:" + SelectedID + " wurde nicht Gelöscht", "Modul Wagon_List :: Wagon id:" + SelectedID + " can not Remove", " Wagon_List :: DeleteTrain(); Error: " + ex);
            }
            finally
            {
                startManager.Notify("Lok wurde Gelöscht id:" + (SelectedID + 1), "Train Removed id:" + (SelectedID + 1), "green", "green");
                startManager.Log("Modul Wagon_List :: Wagin id: " + SelectedID + " wurde Erfolgreich Gelöscht.!", "Modul Wagon_List :: Wagon id: " + SelectedID + " Removed.!");
            }
            
        }
        SelectedID = -1;
        for (int i = 0; i < 12; i++)
        {
            EditDeleteToggle[i] = DeleteEditToggle[i].isOn;
            if (EditDeleteToggle[i] == true)
            {
                SelectedID = (i + PageOffset);
            }
        }
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
                command.CommandText = "UPDATE Wagons  SET FARBE = @FARBE , TYP = @Typ , HERSTELLER = @HERSTELLER , KATALOGNUMMER = @KATALOGNUMMER , SERIENNUMMER = @SERIENNUMMER , KAUFDAY = @KAUFDAY , KAUFMONAT = @KAUFMONAT ,KAUFJAHR = @KAUFJAHR , PREIS = @PREIS ,KUPPLUNG = @KUPPLUNG , LICHT = @LICHT , PREISER =@PREISER, SPURWEITE = @SPURWEITE, LAGERORT = @LAGERORT  WHERE IDENTIFYER='" + Trains[SelectedID].DBIdentifyer + "' AND  ERSTELLT='" + Trains[SelectedID].DBErstellt + "'  ";
                command.Parameters.AddWithValue("@FARBE", Farbe.text);
                command.Parameters.AddWithValue("@TYP", WagonTyp.value);
                command.Parameters.AddWithValue("@HERSTELLER", Hersteller.value);
                command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer.text);
                command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer.text);
                command.Parameters.AddWithValue("@PREIS", Preis.text);
                command.Parameters.AddWithValue("@KAUFDAY", KaufdatumTag.value);
                command.Parameters.AddWithValue("@KAUFMONAT", KaufdatumMonat.value);
                command.Parameters.AddWithValue("@KAUFJAHR", KaufdatumJahr.value);
                command.Parameters.AddWithValue("@SPURWEITE", Spurweite.value);
                command.Parameters.AddWithValue("@LAGERORT", Lager.value);
                if (Kupplung.isOn == true)
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 0);
                }
                if (Licht.isOn == true)
                {
                    command.Parameters.AddWithValue("@LICHT", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@LICHT", 0);
                }
                if (Preiser.isOn == true)
                {
                    command.Parameters.AddWithValue("@PREISER", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@PREISER", 0);
                }
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern.", "Error by Save.", " Wagon_List :: SaveEditTrain().IsEditMode==True; Error: " + ex);
                    startManager.Error("EditWagon(Wagons);", "" + ex);
                }
                finally
                {
                    startManager.Notify("Wagon wurde Bearbeited", "Wagon edited", "green", "green");
                }
                dbConnection.Close();
                dbConnection = null;
            }
            SelectedID = -1;
            for (int i = 0; i < 12; i++)
            {
                EditDeleteToggle[i] = DeleteEditToggle[i].isOn;
                if (EditDeleteToggle[i] == true)
                {
                    SelectedID = (i + PageOffset);
                }
            }
            RefreschIndex();
        }
        else
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Wagons  (TYP , FARBE , HERSTELLER ,  KATALOGNUMMER , SERIENNUMMER , KAUFDAY , KAUFMONAT , KAUFJAHR , PREIS , KUPPLUNG , LICHT , PREISER, SPURWEITE, IDENTIFYER , LAGERORT) VALUES" + " (@TYP , @FARBE , @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @KAUFDAY , @KAUFMONAT , @KAUFJAHR , @PREIS , @KUPPLUNG , @LICHT , @PREISER, @SPURWEITE, @IDENTIFYER , @LAGERORT)";
                command.Parameters.AddWithValue("@FARBE", Farbe.text);
                command.Parameters.AddWithValue("@TYP", WagonTyp.value);
                command.Parameters.AddWithValue("@HERSTELLER", Hersteller.value);
                command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer.text);
                command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer.text);
                command.Parameters.AddWithValue("@PREIS", Preis.text);
                command.Parameters.AddWithValue("@KAUFDAY", KaufdatumTag.value);
                command.Parameters.AddWithValue("@KAUFMONAT", KaufdatumMonat.value);
                command.Parameters.AddWithValue("@KAUFJAHR", KaufdatumJahr.value);
                command.Parameters.AddWithValue("@SPURWEITE", Spurweite.value);
                command.Parameters.AddWithValue("@LAGERORT", Lager.value);
                command.Parameters.AddWithValue("@IDENTIFYER", Guid.NewGuid().ToString());
                if(Kupplung.isOn == true)
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 0);
                }
                if (Licht.isOn == true)
                {
                    command.Parameters.AddWithValue("@LICHT", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@LICHT", 0);
                }
                if (Preiser.isOn == true)
                {
                    command.Parameters.AddWithValue("@PREISER", 1);
                }
                else
                {
                    command.Parameters.AddWithValue("@PREISER", 0);
                }
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern.", "Error by Save Train.", " Wagon_List :: SaveEditTrain().IsEditMode==false; Error: " + ex);
                    startManager.Error("Save(Wagons);", "" + ex);
                }
                finally
                {
                    if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (Trains.Count + 1) + "." + UserSettings.ImageType))
                    {
                        File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (Trains.Count + 1) + "." + UserSettings.ImageType);
                    }
                    startManager.Notify("Wagon Gespeichert", "Wagon Saved", "green", "green");
                }
                dbConnection.Close();
                dbConnection = null;
            }
            SelectedID = -1;
            for (int i = 0; i < 12; i++)
            {
                EditDeleteToggle[i] = DeleteEditToggle[i].isOn;
                if (EditDeleteToggle[i] == true)
                {
                    SelectedID = (i + PageOffset);
                }
            }
        }
        RefreschIndex();
    }

    public void GetLokData()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Keinen Wagon ausgewählt", "No Wagon Selected", "red", "red");
        }
        else
        {
            IsEditMode = true;
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
            TrainPic.GetComponent<RawImage>().texture = CacheImage[SelectedID];
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
        string FinshURL = "http://" + startManager.WebExporterUrl + "exporter" + "/insert.php?uniqueID=" + uniqueID + "&data=ISWAGON," + Trains[SelectedID].DBTyp.ToString() + "," + Trains[SelectedID].DBFarbe.ToString() + "," + Trains[SelectedID].DBHersteller.ToString() + "," + Trains[SelectedID].DBKatalognummer.ToString() + "," + Trains[SelectedID].DBSeriennummer.ToString() + "," + Trains[SelectedID].DBKaufTag.ToString() + "," + Trains[SelectedID].DBKaufMonat.ToString() + "," + Trains[SelectedID].DBKaufJahr.ToString() + "," + Trains[SelectedID].DBPreis.ToString() + "," + Trains[SelectedID].DBKupplung.ToString() + "," + Trains[SelectedID].DBLicht.ToString() + "," + Trains[SelectedID].DBPreiser.ToString() + "," + Trains[SelectedID].DBSpurweite.ToString() + "," + Trains[SelectedID].DBIdentifyer.ToString() + "," + Trains[SelectedID].DBLagerort.ToString() + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE" + "," + "NONE";
        Debug.Log(FinshURL);
        WWW insert = new WWW(FinshURL);

        yield return insert;

        if (insert.error != null)
        {
            startManager.LogError("Keine Verbindung zum Server möglich!.", "No Connection to the Server.", " Train_List :: SendSelected(); Error: " + insert.error);
            startManager.Error("SendSelected(Wagons)", insert.error.ToString());
        }
        if (insert.isDone)
        {
            startManager.Notify("Wagon Gesendet", "Wagon Send", "green", "green");
            Win.SetActive(true);
            SendOK.text = uniqueID.ToString();
        }
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
        dataexporter.Spurweite = Trains[SelectedID].DBSpurweite;
        dataexporter.Kupplung = Trains[SelectedID].DBKupplung;
        dataexporter.Licht = Trains[SelectedID].DBLicht;
        dataexporter.Preiser = Trains[SelectedID].DBPreiser;
        dataexporter.Image = File.ReadAllBytes((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (SelectedID + 1) + "." + UserSettings.ImageType));
        string jsonData = JsonUtility.ToJson(dataexporter, true);
        File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Exporter/" + vHersteller[Trains[SelectedID].DBHersteller] + "-" + Trains[SelectedID].DBKatalognummer + "-" + Trains[SelectedID].DBFarbe + ".TRAIN", jsonData);
        SelectedID = -1;
        startManager.Notify("Wagon als Datei Exportiert", "Wagon as File Exported", "green", "green");
    }

    public void SendWagon(int id)
    {
        NM.TrySendData("WAGON" + "?" + Trains[id].DBTyp.ToString() + "?" + Trains[id].DBFarbe.ToString() + "?" + Trains[id].DBHersteller.ToString() + "?" + Trains[id].DBErstellt.ToString() + "?" + Trains[id].DBKatalognummer.ToString() + "?" + Trains[id].DBSeriennummer.ToString() + "?" + Trains[id].DBKaufTag.ToString() + "?" + Trains[id].DBKaufMonat.ToString() + "?" + Trains[id].DBKaufJahr.ToString() + "?" + Trains[id].DBPreis.ToString() + "?" + Trains[id].DBKupplung.ToString() + "?" + Trains[id].DBLicht.ToString() + "?" + Trains[id].DBPreiser.ToString() + "?" + Trains[id].DBSpurweite.ToString() + "?" + Trains[id].DBIdentifyer.ToString() + "?" + Trains[id].DBLagerort.ToString() + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null");
    }

    public void ForceRPCSend()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Kein Wagon ausgewählt","No Wagon Selecdet","yellow", "yellow");
        }
        else
        {
            int id = SelectedID;
            NM.TrySendData("WAGON" + "?" + Trains[id].DBTyp.ToString() + "?" + Trains[id].DBFarbe.ToString() + "?" + Trains[id].DBHersteller.ToString() + "?" + Trains[id].DBErstellt.ToString() + "?" + Trains[id].DBKatalognummer.ToString() + "?" + Trains[id].DBSeriennummer.ToString() + "?" + Trains[id].DBKaufTag.ToString() + "?" + Trains[id].DBKaufMonat.ToString() + "?" + Trains[id].DBKaufJahr.ToString() + "?" + Trains[id].DBPreis.ToString() + "?" + Trains[id].DBKupplung.ToString() + "?" + Trains[id].DBLicht.ToString() + "?" + Trains[id].DBPreiser.ToString() + "?" + Trains[id].DBSpurweite.ToString() + "?" + Trains[id].DBIdentifyer.ToString() + "?" + Trains[id].DBLagerort.ToString() + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null");
        }
    }
}