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


public class WebImport_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public Train_List TL;
    public Wagon_List WL;
    [Header("Elements")]
    public InputField ImportKey;
    public Text ItemType;
    public Text wwwBaureihe;
    public Text wwwFarbe;
    public Text wwwArtNR;
    public Text wwwErfasst;
    public Toggle DeleteToggle;
    [Header("Workflow")]
    public string CacheData0 = "NULL";
    public string CacheData1 = "NULL";
    public string CacheData2 = "NULL";
    public string CacheData3 = "NULL";
    public string CacheData4 = "NULL";
    public string CacheData5 = "NULL";
    public string CacheData6 = "NULL";
    public string CacheData7 = "NULL";
    public string CacheData8 = "NULL";
    public string CacheData9 = "NULL";
    public string CacheData10 = "NULL";
    public string CacheData11 = "NULL";
    public string CacheData12 = "NULL";
    public string CacheData13 = "NULL";
    public string CacheData14 = "NULL";
    public string CacheData15 = "NULL";
    public string CacheData16 = "NULL";
    public string CacheData17 = "NULL";
    public string CacheData18 = "NULL";
    public string CacheData19 = "NULL";
    public string CacheData20 = "NULL";
    public string CacheData21 = "NULL";
    public string CacheData22 = "NULL";
    public string CacheData23 = "NULL";
    public string CacheData24 = "NULL";
    public string CacheData25 = "NULL";
    public string CacheData26 = "NULL";
    public string CacheData27 = "NULL";
    public string CacheData28 = "NULL";
    public string CacheData29 = "NULL";
    public string CacheData30 = "NULL";
    public string CacheData31 = "NULL";
    public string CacheData32 = "NULL";
    public string CacheData33 = "NULL";
    public string CacheData34 = "NULL";
    
    void Start ()
    {
        startManager.Log("Lade WebImport_Manager -> Nachricht ist Normal.", "Load WebImport_Manager -> message is normal");
    }
	
    public IEnumerator ReadData()
    {
        WWW read = new WWW("http://" + startManager.WebExporterUrl+ "exporter/read.php?uniqueID=" + ImportKey.text.ToString());
        yield return read;

        if (read.error != null)
        {
            ImportKey.text = "Kein Artikel Gefunden.!";
            startManager.Log("WebImport_Manager :: Keine Verbindung zum Server", "WebImport_Manager :: No Server Connection");
            startManager.LogError("Keine Verbindung zum Server", "No Server Connection", " WebImport_Manager :: ReadData(); Error: " + read.error);
            startManager.Notify("Keine Verbindung zum Server", "No Server Connection", "red", "red");
        }
        else
        {
            if (read.text == "NOTHING,")
            {
                ImportKey.text = "Kein Artikel Gefunden.!";
                startManager.Notify("Nichts Gefunden", "Nothing Found", "red", "red");
            }
            else
            {
                startManager.Notify("Daten Gefunden", "Data Found", "green", "green");
                string strOne = read.text;
                string[] strArrayOne = new string[] { "" };
                strArrayOne = strOne.Split(',');
                startManager.Log("WebImport_Manager :: Empfangen " + read.bytesDownloaded + " bytes", "WebImport_Manager :: Recieve " + read.bytesDownloaded + " bytes");

                CacheData0 = strArrayOne[0];
                CacheData1 = strArrayOne[1];
                CacheData2 = strArrayOne[2];
                CacheData3 = strArrayOne[3];
                CacheData4 = strArrayOne[4];
                CacheData5 = strArrayOne[5];
                CacheData6 = strArrayOne[6];
                CacheData7 = strArrayOne[7];
                CacheData8 = strArrayOne[8];
                CacheData9 = strArrayOne[9];
                CacheData10 = strArrayOne[10];
                CacheData11 = strArrayOne[11];
                CacheData12 = strArrayOne[12];
                CacheData13 = strArrayOne[13];
                CacheData14 = strArrayOne[14];
                CacheData15 = strArrayOne[15];
                CacheData16 = strArrayOne[16];
                CacheData17 = strArrayOne[17];
                CacheData18 = strArrayOne[18];
                CacheData19 = strArrayOne[19];
                CacheData20 = strArrayOne[20];
                CacheData21 = strArrayOne[21];
                CacheData22 = strArrayOne[22];
                CacheData23 = strArrayOne[23];
                CacheData24 = strArrayOne[24];
                CacheData25 = strArrayOne[25];
                CacheData26 = strArrayOne[26];
                CacheData27 = strArrayOne[27];
                CacheData28 = strArrayOne[28];
                CacheData29 = strArrayOne[29];
                CacheData30 = strArrayOne[30];
                CacheData31 = strArrayOne[31];
                CacheData32 = strArrayOne[32];
                CacheData33 = strArrayOne[33];
                CacheData34 = strArrayOne[34];

                if (CacheData1 == "ISTRAIN")
                {
                    ItemType.text = "Lok";
                }
                else
                {
                    ItemType.text = "Wagon";
                }
                wwwBaureihe.text = CacheData2;
                wwwFarbe.text = CacheData3;
                wwwArtNR.text = CacheData6;
                wwwErfasst.text = CacheData20;

            }
        }
    }

    public void ImportWWWItem()
    {
        if (CacheData1 == "ISTRAIN")
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Trains (BAUREIHE, FARBE, TYP, HERSTELLER, KATALOGNUMMER, SERIENNUMMER, PREIS, WARTUNGDAY, WARTUNGMONAT, WARTUNGJEAR, ADRESSE, PROTOKOLL, FAHRSTUFEN, DECHERSTELLER, RAUCH, SOUND, ROTWEISS, PANDO, TELEX, KUPPLUNG, KTAG, KMONAT, KJAHR, SPURWEITE, CV2, CV3, CV4, CV5, IDENTIFYER, LAGERORT) VALUES" + " (@BAUREIHE, @FARBE, @TYP, @HERSTELLER, @KATALOGNUMMER, @SERIENNUMMER, @PREIS, @WARTUNGDAY, @WARTUNGMONAT, @WARTUNGJEAR, @ADRESSE, @PROTOKOLL, @FAHRSTUFEN, @DECHERSTELLER, @RAUCH, @SOUND, @ROTWEISS, @PANDO, @TELEX, @KUPPLUNG, @KTAG, @KMONAT, @KJAHR, @SPURWEITE, @CV2, @CV3, @CV4, @CV5, @IDENTIFYER, @LAGERORT)";
                if (CacheData2 != "NULL")
                {
                    command.Parameters.AddWithValue("@BAUREIHE", CacheData2);
                }

                else
                {

                }

                if (CacheData3 != "NULL")
                {
                    command.Parameters.AddWithValue("@FARBE", CacheData3);
                }

                else
                {

                }

                if (CacheData4 != "NULL")
                {
                    command.Parameters.AddWithValue("@TYP", Convert.ToInt32(CacheData4));
                }

                else
                {

                }

                if (CacheData5 != "NULL")
                {
                    command.Parameters.AddWithValue("@HERSTELLER", Convert.ToInt32(CacheData5));
                }

                else
                {

                }

                if (CacheData6 != "NULL")
                {
                    command.Parameters.AddWithValue("@KATALOGNUMMER", CacheData6);
                }

                else
                {

                }

                if (CacheData7 != "NULL")
                {
                    command.Parameters.AddWithValue("@SERIENNUMMER", CacheData7);
                }

                else
                {

                }

                if (CacheData8 != "NULL")
                {
                    command.Parameters.AddWithValue("@KTAG", Convert.ToInt32(CacheData8));
                }

                else
                {

                }

                if (CacheData9 != "NULL")
                {
                    command.Parameters.AddWithValue("@KMONAT", Convert.ToInt32(CacheData9));
                }

                else
                {

                }

                if (CacheData10 != "NULL")
                {
                    command.Parameters.AddWithValue("@KJAHR", Convert.ToInt32(CacheData10));
                }

                else
                {

                }

                if (CacheData11 != "NULL")
                {
                    command.Parameters.AddWithValue("@PREIS", Convert.ToInt32(CacheData11));
                }

                else
                {

                }

                if (CacheData12 != "NULL")
                {
                    command.Parameters.AddWithValue("@WARTUNGDAY", Convert.ToInt32(CacheData12));
                }

                else
                {

                }

                if (CacheData13 != "NULL")
                {
                    command.Parameters.AddWithValue("@WARTUNGMONAT", Convert.ToInt32(CacheData13));
                }

                else
                {

                }

                if (CacheData14 != "NULL")
                {
                    command.Parameters.AddWithValue("@WARTUNGJEAR", Convert.ToInt32(CacheData14));
                }

                else
                {

                }

                if (CacheData15 != "NULL")
                {
                    command.Parameters.AddWithValue("@ADRESSE", CacheData15);
                }

                else
                {

                }

                if (CacheData16 != "NULL")
                {
                    command.Parameters.AddWithValue("@PROTOKOLL", Convert.ToInt32(CacheData16));
                }

                else
                {

                }

                if (CacheData17 != "NULL")
                {
                    command.Parameters.AddWithValue("@FAHRSTUFEN", Convert.ToInt32(CacheData17));
                }

                else
                {

                }

                if (CacheData18 != "NULL")
                {
                    command.Parameters.AddWithValue("@DECHERSTELLER", CacheData18);
                }

                else
                {

                }

                if (CacheData21 != "NULL")
                {
                    command.Parameters.AddWithValue("@RAUCH", Convert.ToInt32(CacheData21));
                }

                else
                {
                    command.Parameters.AddWithValue("@RAUCH", 0);
                }

                if (CacheData22 != "NULL")
                {
                    command.Parameters.AddWithValue("@SOUND", Convert.ToInt32(CacheData22));
                }

                else
                {
                    command.Parameters.AddWithValue("@SOUND", 0);
                }

                if (CacheData23 != "NULL")
                {
                    command.Parameters.AddWithValue("@ROTWEISS", Convert.ToInt32(CacheData23));
                }

                else
                {
                    command.Parameters.AddWithValue("@ROTWEISS", 0);
                }

                if (CacheData25 != "NULL")
                {
                    command.Parameters.AddWithValue("@PANDO", Convert.ToInt32(CacheData25));
                }

                else
                {
                    command.Parameters.AddWithValue("@PANDO", 0);
                }

                if (CacheData25 != "NULL")
                {
                    command.Parameters.AddWithValue("@TELEX", Convert.ToInt32(CacheData25));
                }

                else
                {
                    command.Parameters.AddWithValue("@TELEX", 0);
                }

                if (CacheData27 != "NULL")
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", Convert.ToInt32(CacheData27));
                }

                else
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 0);
                }

                if (CacheData28 != "NULL")
                {
                    command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(CacheData28));
                }

                else
                {
                    command.Parameters.AddWithValue("@SPURWEITE", 0);
                }

                if (CacheData29 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV2", Convert.ToInt32(CacheData29));
                }

                else
                {
                    command.Parameters.AddWithValue("@CV2", 0);
                }

                if (CacheData30 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV3", Convert.ToInt32(CacheData30));
                }

                else
                {
                    command.Parameters.AddWithValue("@CV3", 0);
                }

                if (CacheData31 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV4", Convert.ToInt32(CacheData31));
                }

                else
                {
                    command.Parameters.AddWithValue("@CV4", 0);
                }

                if (CacheData32 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV5", Convert.ToInt32(CacheData32));
                }

                else
                {
                    command.Parameters.AddWithValue("@CV5", 0);
                }

                if (CacheData33 != "NULL")
                {
                    command.Parameters.AddWithValue("@IDENTIFYER", CacheData33);
                }

                else
                {
                    command.Parameters.AddWithValue("@IDENTIFYER", Guid.NewGuid().ToString());
                }

                if (CacheData34 != "NULL")
                {
                    command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(CacheData34));
                }

                else
                {
                    command.Parameters.AddWithValue("@LAGERORT", 0);
                }

                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.Log("WebImport_Manager :: Lok nicht Gespeichert", "WebImport_Manager :: Error Save Train");
                    startManager.LogError("Keine Verbindung zum Server", "No Server Connection", " WebImport_Manager :: ImportWWWItem();ChacheData0 Error: " + ex);
                    startManager.Notify("Lok nicht Gespeichert", "Error Save Train", "red", "red");
                }
                finally
                {
                    File.Copy(Application.streamingAssetsPath + "/Resources/Train.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (TL.Trains.Count + 1) + "." + UserSettings.ImageType);
                    startManager.Log("WebImport_Manager :: Lok Saved", "WebImport_Manager :: Train Saved");
                    startManager.Notify("Lok Gespeichert", "Train Saved", "green", "green");
                    dbConnection.Close();
                }
            }
            if (DeleteToggle == DeleteToggle.isOn)
            {
                StartCoroutine(Clear());
            }
            else
            {
                startManager.Log("WebImport_Manager :: Eintrag nicht Erfernen", "WebImport_Manager :: Entry no Remove");
            }
        }
        
        if (CacheData1 == "ISWAGON")
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Wagons (TYP, FARBE, HERSTELLER, KATALOGNUMMER, SERIENNUMMER, KAUFDAY, KAUFMONAT, KAUFJAHR, PREIS, KUPPLUNG, LICHT, PREISER, SPURWEITE, LAGERORT, IDENTIFYER) VALUES" + " (@TYP, @FARBE, @HERSTELLER, @KATALOGNUMMER, @SERIENNUMMER, @KAUFDAY, @KAUFMONAT, @KAUFJAHR, @PREIS, @KUPPLUNG, @LICHT, @PREISER, @SPURWEITE, @LAGERORT, @IDENTIFYER)";

                if (CacheData2 != "NULL")
                {
                    command.Parameters.AddWithValue("@TYP", Convert.ToInt32(CacheData2));
                }

                else
                {

                }

                if (CacheData3 != "NULL")
                {
                    command.Parameters.AddWithValue("@FARBE", CacheData3);
                }

                else
                {

                }

                if (CacheData4 != "NULL")
                {
                    command.Parameters.AddWithValue("@HERSTELLER", Convert.ToInt32(CacheData4));
                }

                else
                {

                }

                if (CacheData5 != "NULL")
                {
                    command.Parameters.AddWithValue("@KATALOGNUMMER", CacheData5);
                }

                else
                {

                }

                if (CacheData6 != "NULL")
                {
                    command.Parameters.AddWithValue("@SERIENNUMMER", CacheData6);
                }

                else
                {

                }

                if (CacheData7 != "NULL")
                {
                    command.Parameters.AddWithValue("@KAUFDAY", Convert.ToInt32(CacheData7));
                }

                else
                {

                }

                if (CacheData8 != "NULL")
                {
                    command.Parameters.AddWithValue("@KAUFMONAT", Convert.ToInt32(CacheData8));
                }

                else
                {

                }

                if (CacheData9 != "NULL")
                {
                    command.Parameters.AddWithValue("@KAUFJAHR", Convert.ToInt32(CacheData9));
                }

                else
                {

                }

                if (CacheData10 != "NULL")
                {
                    command.Parameters.AddWithValue("@PREIS", Convert.ToInt32(CacheData10));
                }

                else
                {

                }

                if (CacheData11 != "NULL")
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", Convert.ToInt32(CacheData11));
                }

                else
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", 0);
                }

                if (CacheData12 != "NULL")
                {
                    command.Parameters.AddWithValue("@LICHT", Convert.ToInt32(CacheData12));
                }

                else
                {
                    command.Parameters.AddWithValue("@LICHT", 0);
                }

                if (CacheData13 != "NULL")
                {
                    command.Parameters.AddWithValue("@PREISER", Convert.ToInt32(CacheData13));
                }

                else
                {
                    command.Parameters.AddWithValue("@PREISER", 0);
                }

                if (CacheData14 != "NULL")
                {
                    command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(CacheData14));
                }

                else
                {
                    command.Parameters.AddWithValue("@SPURWEITE", 0);
                }

                if (CacheData16 != "NULL")
                {
                    command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(CacheData16));
                }

                else
                {
                    command.Parameters.AddWithValue("@LAGERORT", 0);
                }

                if (CacheData15 != "NULL")
                {
                    command.Parameters.AddWithValue("@IDENTIFYER", CacheData15);
                }

                else
                {
                    command.Parameters.AddWithValue("@IDENTIFYER", Guid.NewGuid().ToString());
                }

                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.Log("WebImport_Manager :: Wagon nicht Gespeichert", "WebImport_Manager :: Error Save Wagon");
                    startManager.LogError("Keine Verbindung zum Server", "No Server Connection", " WebImport_Manager :: ImportWWWItem();ChacheData1 Error: " + ex);
                    startManager.Notify("Wagon nicht Gespeichert", "Error Save Wagon", "red", "red");
                }
                finally
                {
                    File.Copy(Application.streamingAssetsPath + "/Resources/Wagon.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WL.Trains.Count + 1) + "." + UserSettings.ImageType);
                    startManager.Log("WebImport_Manager :: Wagon Saved", "WebImport_Manager :: Wagon Saved");
                    startManager.Notify("Wagon Gespeichert", "Wagon Saved", "green", "green");
                }
            }
            if (DeleteToggle == DeleteToggle.isOn)
            {
                StartCoroutine(Clear());
            }
            else
            {
                startManager.Log("WebImport_Manager :: Eintrag nicht Erfernen", "WebImport_Manager :: Entry no Remove");
            }
        }
    }

    IEnumerator Clear()
    {
        WWW read = new WWW("http://" + startManager.WebExporterUrl + "exporter/remove.php?uniqueID=" + ImportKey.text.ToString());
        yield return read;

        if (read.error != null)
        {
            startManager.Log("WebImport_Manager :: Keine Verbindung zum Server", "WebImport_Manager :: No Server Connection");
            startManager.LogError("Keine Verbindung zum Server", "No Server Connection", " WebImport_Manager :: ReadData(); Error: " + read.error);
            startManager.Notify("Keine Verbindung zum Server", "No Server Connection", "red", "red");
            startManager.Error("Clear(WebImport)", read.error.ToString());
        }
        else
        {
            startManager.Log("WebImport_Manager :: Eintrag Gelöscht", "WebImport_Manager :: Entry  Removed");
        }
    }

    public void Search()
    {
        StartCoroutine(ReadData());
    }
}