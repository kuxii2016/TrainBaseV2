/*
 * 
 *   TrainBase Import Manager Version 1 from 31.08.2018 written by Michael Kux
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


public class ImportManager : MonoBehaviour
{
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public SettingsManager SM;
    public LokView lokview;
    public LogWriterManager Logger;
    public Text ImportFolder;
    public int FoundRows = 0;
    public int selectedID;
    public int unselectedID;
    public Button[] slots;
    public ImportManager IM;
    public GameObject ImportWindow;
    public Text ImportWindowMessage;
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
    private DataExporter dataexporter;
    public string Reader = "";
    public Image ReadOn;
    public int Import;
    [Header("WWW-Importer")]
    public Text wwwTyp;
    public Text wwwBaureihe;
    public Text wwwFarbe;
    public Text wwwArtNR;
    public Text wwwHersteller;
    public Text wwwErfasst;
    public InputField ImportKey;
    public Toggle DeleteToggle;
    string CacheData0 = "NULL";
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

    void Start()
    {
        Import = SM.ImportXML;
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Import Manager -> Message is Normal.");
        }
        dataexporter = new DataExporter();
        selectedID = -1;
        ImportFolder.text = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Importer: ";
        GetRows();
    }

    public void GetRows()
    {
        ReadOn.color = Color.yellow;
        string[] imports = Directory.GetFiles(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Importer/");
        for (int i = 0; i < imports.Length; i++)
        {
            IM.slots[i].GetComponentInChildren<Text>().text = Path.GetFileName(imports[i]);
            FoundRows = (i + 1);
            slots[i].gameObject.SetActive(true);
        }
        StartManager.SystemMeldung.color = Color.cyan;
        StartManager.SystemMeldung.text = ("Gefundenes Rollmaterial.");
    }

    public void SelectedID(int id)
    {
        for (int u = 0; u < 53; u++)
        {
            IM.slots[u].GetComponentInChildren<Text>().color = Color.black;
            GetRows();
        }
        IM.slots[id].GetComponentInChildren<Text>().color = Color.blue;
        selectedID = id;
        ImportWindow.gameObject.SetActive(true);
        ImportWindowMessage.text = "Fremdes Rollmaterial:  " + IM.slots[id].GetComponentInChildren<Text>().text + "  in das Eigene System Einlesen?";
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Import_Manager :: Import from  " + IM.slots[id].GetComponentInChildren<Text>().text);
        }
        Reader = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Importer/" + IM.slots[id].GetComponentInChildren<Text>().text);
    }

    public void NoFix()
    {
        IM.slots[selectedID].GetComponentInChildren<Text>().color = Color.black;
        selectedID = -1;
    }

    public void cacheData()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Import_Manager :: Try Import from  " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Importer/") + (IM.slots[selectedID].GetComponentInChildren<Text>().text));
        }
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
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Import_Manager :: Data Cached, is ah " + Type + " from: " + Hersteller + " whit  item Number " + KatalogNummer + "" + Katalognummer);
        }
        if (Type == "TRAIN")
        {
            SaveTrain();
        }
        if (Type == "WAGON")
        {
            SaveWagon();
        }
    }

    public void SaveTrain()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Trains (BAUREIHE , FARBE , TYP , HERSTELLER , KATALOGNUMMER , SERIENNUMMER , PREIS , WARTUNGDAY ,WARTUNGMONAT , WARTUNGJEAR ,ADRESSE , PROTOKOLL , FAHRSTUFEN , DECHERSTELLER , RAUCH , SOUND , ROTWEISS ,PANDO , TELEX , KUPPLUNG , KTAG , KMONAT , KJAHR, SPURWEITE, IDENTIFYER, LAGERORT) VALUES" + " (@BAUREIHE , @FARBE , @TYP ,  @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @PREIS , @WARTUNGDAY , @WARTUNGMONAT , @WARTUNGJEAR , @ADRESSE , @PROTOKOLL , @FAHRSTUFEN , @DECHERSTELLER , @RAUCH , @SOUND , @ROTWEISS , @PANDO , @TELEX , @KUPPLUNG, @KTAG , @KMONAT , @KJAHR, @SPURWEITE, @IDENTIFYER, @LAGERORT)";
            command.Parameters.AddWithValue("@BAUREIHE", Baureihe);
            command.Parameters.AddWithValue("@FARBE", Farbe);
            command.Parameters.AddWithValue("@TYP", Energy);
            command.Parameters.AddWithValue("@HERSTELLER", Hersteller);
            command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer);
            command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer);
            command.Parameters.AddWithValue("@PREIS", Preis);
            command.Parameters.AddWithValue("@WARTUNGDAY", WartungDay);
            command.Parameters.AddWithValue("@WARTUNGMONAT", WartungMonth);
            command.Parameters.AddWithValue("@WARTUNGJEAR", WartungYear);
            command.Parameters.AddWithValue("@ADRESSE", Adresse);
            command.Parameters.AddWithValue("@PROTOKOLL", Protokoll);
            command.Parameters.AddWithValue("@FAHRSTUFEN", Fahrstufen);
            command.Parameters.AddWithValue("@DECHERSTELLER", DecoderHersteller);
            command.Parameters.AddWithValue("@RAUCH", Rauch);
            command.Parameters.AddWithValue("@SOUND", Sound);
            command.Parameters.AddWithValue("@ROTWEISS", LichtWechsel);
            command.Parameters.AddWithValue("@PANDO", ElektrischePandos);
            command.Parameters.AddWithValue("@TELEX", Telex);
            command.Parameters.AddWithValue("@KUPPLUNG", Telex);
            command.Parameters.AddWithValue("@KTAG", KaufDay);
            command.Parameters.AddWithValue("@KMONAT", KaufMonth);
            command.Parameters.AddWithValue("@KJAHR", KaufYear);
            command.Parameters.AddWithValue("@SPURWEITE", Spurweite);
            command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("@LAGERORT", 3);
            if (lokview.Trains.Count <= Settings.LokLimit)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.Error("MODUL Import_Manager :: SaveTrain()  " + ex + "\n");
                    }
                }
                finally
                {
                    Import = +1;
                    dbConnection.Close();
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Import_Manager :: " + Baureihe + "ADR: " + Adresse + " in: " + Farbe + "  Gespeichert.!");
                    }
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Import_Manager :: Current Train Limit is to Low for your Entry ");
                }
            }
        }
        ImportWindow.gameObject.SetActive(false);
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Import_Manager :: Import Done.! ");
        }
        SaveSettings();
    }

    public void SaveWagon()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Wagons (TYP , FARBE , HERSTELLER ,  KATALOGNUMMER , SERIENNUMMER , KAUFDAY , KAUFMONAT , KAUFJAHR , PREIS , KUPPLUNG , LICHT , PREISER, SPURWEITE, IDENTIFYER, LAGERORT) VALUES" + " (@TYP , @FARBE , @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @KAUFDAY , @KAUFMONAT , @KAUFJAHR , @PREIS , @KUPPLUNG , @LICHT , @PREISER, @SPURWEITE, @IDENTIFYER, @LAGERORT)";
            command.Parameters.AddWithValue("@TYP", Typ);
            command.Parameters.AddWithValue("@FARBE", Farbe);
            command.Parameters.AddWithValue("@HERSTELLER", Hersteller);
            command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer);
            command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer);
            command.Parameters.AddWithValue("@KAUFDAY", KaufTag);
            command.Parameters.AddWithValue("@KAUFMONAT", KaufMonat);
            command.Parameters.AddWithValue("@KAUFJAHR", KaufJahr);
            command.Parameters.AddWithValue("@PREIS", Preis);
            command.Parameters.AddWithValue("@KUPPLUNG", Kupplung);
            command.Parameters.AddWithValue("@LICHT", Licht);
            command.Parameters.AddWithValue("@PREISER", Preiser);
            command.Parameters.AddWithValue("@SPURWEITE", Spurweite);
            command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("@LAGERORT", 3);
            if (lokview.Trains.Count <= Settings.LokLimit)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.Error("MODUL Import_Manager :: SaveWagon()  " + ex + "\n");
                    }
                }
                finally
                {
                    Import = +1;
                    dbConnection.Close();
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Import_Manager :: save Wagon: " + Katalognummer + " in: " + Farbe + "  Gespeichert.! ");
                    }
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Import_Manager :: Ups Current Wagon Limit is to Low for your Entry ");
                }
            }
        }
        ImportWindow.gameObject.SetActive(false);
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Import_Manager :: Import Done.! ");
        }
        SaveSettings();
    }

    public void Refresch()
    {
        GetRows();
        StartManager.SystemMeldung.color = Color.cyan;
        StartManager.SystemMeldung.text = ("Alles Aktualisiert");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL ImportManager :: Row Manuell Refresch");
        }
    }

    public void SaveSettings()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Settings  SET IMPORTXML = @IMPORTXML";
            command.Parameters.AddWithValue("@IMPORTXML", Import);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.Error("MODUL Import_Manager :: SaveSettings()  " + ex + "\n");
                }
            }
            finally
            {
                dbConnection.Close();
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Import_Manager :: Save Finsch ");
                }
            }
        }
        SM.ReadSettings();
    }

    public void Search()
    {
        StartCoroutine(ReadData());
    }

    IEnumerator ReadData()
    {
        WWW read = new WWW(Settings.URL + "/read.php?uniqueID=" + ImportKey.text.ToString());
        yield return read;

        if (read.error != null)
        {
            ImportKey.text = "Kein Artikel Gefunden.!";
            Logger.PrintLogEnde();
            Logger.PrintLog("MODUL Import_Manager :: No Internet Connection to the Server.!");
            Logger.Message("Keine Internet Verbindung.!", "ROT");
            Logger.Error("MODUL Import_Manager :: ReadData()  No Connection to the Server \n");
        }
        else
        {
            Logger.PrintLogEnde();
            Logger.PrintLog("MODUL Import_Manager :: Connecting to the Server.!");

            if (read.text == "NOTHING,")
            {
                ImportKey.text = "Kein Artikel Gefunden.!";
                Logger.PrintLog("MODUL Import_Manager :: Import key Correct?");
                Logger.Message("Nichts Gefunden,Importkey Richtig?", "ROT");
                Logger.Error("MODUL Import_Manager :: ReadData()  Wrong Key \n");

                wwwTyp.color = Color.red;
                wwwTyp.text = "Nichts Gefunden.!";
            }
            else
            {
                Logger.PrintLog("MODUL Import_Manager :: Get Array Data, To Split this.");
                Logger.Message("Rollmaterial Gefunden Lade Daten", "GRUEN");
                string strOne = read.text;
                string[] strArrayOne = new string[] { "" };
                strArrayOne = strOne.Split(',');


                Logger.PrintLog("MODUL Import_Manager :: Empfangen " + read.bytesDownloaded + " bytes");

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
                wwwTyp.color = Color.black;

                if (CacheData1 == "ISTRAIN")
                {
                    wwwTyp.text = "Lok";
                }
                else
                {
                    wwwTyp.text = "Wagon";
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
        if(CacheData1 == "ISTRAIN")
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
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

                }
                if (CacheData22 != "NULL")
                {
                    command.Parameters.AddWithValue("@SOUND", Convert.ToInt32(CacheData22));
                }
                else
                {

                }
                if (CacheData23 != "NULL")
                {
                    command.Parameters.AddWithValue("@ROTWEISS", Convert.ToInt32(CacheData23));
                }
                else
                {

                }
                if (CacheData25 != "NULL")
                {
                    command.Parameters.AddWithValue("@PANDO", Convert.ToInt32(CacheData25));
                }
                else
                {

                }
                if (CacheData25 != "NULL")
                {
                    command.Parameters.AddWithValue("@TELEX", Convert.ToInt32(CacheData25));
                }
                else
                {

                }
                if (CacheData27 != "NULL")
                {
                    command.Parameters.AddWithValue("@KUPPLUNG", Convert.ToInt32(CacheData27));
                }
                else
                {

                }
                if (CacheData28 != "NULL")
                {
                    command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(CacheData28));
                }
                else
                {

                }
                if (CacheData29 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV2", Convert.ToInt32(CacheData29));
                }
                else
                {

                }
                if (CacheData30 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV3", Convert.ToInt32(CacheData30));
                }
                else
                {

                }
                if (CacheData31 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV4", Convert.ToInt32(CacheData31));
                }
                else
                {

                }
                if (CacheData32 != "NULL")
                {
                    command.Parameters.AddWithValue("@CV5", Convert.ToInt32(CacheData32));
                }
                else
                {

                }
                if (CacheData33 != "NULL")
                {
                    command.Parameters.AddWithValue("@IDENTIFYER", CacheData33);
                }
                else
                {

                }
                if (CacheData34 != "NULL")
                {
                    command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(CacheData34));
                }
                else
                {

                }
                if (lokview.Trains.Count <= Settings.LokLimit)
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
                            Logger.Error("MODUL Import_Manager :: ImportWWWItem()  Train Import from WWW " + ex + " \n");
                        }
                    }
                    finally
                    {
                        dbConnection.Close();
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("MODUL Import_Manager :: Lok erfolgreich Importiert  Gespeichert.!");
                        }
                    }
                }
                else
                {
                    Logger.Message("Lok Erfolgreich Empfangen, aber nicht Gespeichert Loklimit überschritten", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Import_Manager ::  Import from WWW Current Train Limit is to Low for your Entry ");
                    }
                }
                Logger.Message("Lok Erfolgreich Empfangen, und Gespeichert", "GRUEN");
            }
            if(DeleteToggle == DeleteToggle.isOn)
            {
                StartCoroutine(Clear());
            }
            else
            {
                Logger.PrintLog("MODUL Import_Manager :: Material wird nicht am Server Gelöscht,bleibt erhalten bis User es Löscht (RE-Import)");
            }
        }


        if (CacheData1 == "ISWAGON")
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
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

                }
                if (CacheData12 != "NULL")
                {
                    command.Parameters.AddWithValue("@LICHT", Convert.ToInt32(CacheData12));
                }
                else
                {

                }
                if (CacheData13 != "NULL")
                {
                    command.Parameters.AddWithValue("@PREISER", Convert.ToInt32(CacheData13));
                }
                else
                {

                }
                if (CacheData14 != "NULL")
                {
                    command.Parameters.AddWithValue("@SPURWEITE", Convert.ToInt32(CacheData14));
                }
                else
                {

                }
                if (CacheData16 != "NULL")
                {
                    command.Parameters.AddWithValue("@LAGERORT", Convert.ToInt32(CacheData16));
                }
                else
                {

                }
                if (CacheData15 != "NULL")
                {
                    command.Parameters.AddWithValue("@IDENTIFYER", CacheData15);
                }
                else
                {

                }
                if (lokview.Trains.Count <= Settings.LokLimit)
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
                            Logger.Error("MODUL Import_Manager :: ImportWWWItem()  Wagon Import from WWW " + ex + " \n");
                        }
                    }
                    finally
                    {
                        dbConnection.Close();
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("MODUL Import_Manager :: Lok erfolgreich Importiert  Gespeichert.!");
                        }
                    }
                }
                else
                {
                    Logger.Message("Lok Erfolgreich Empfangen, aber nicht Gespeichert Loklimit überschritten", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Import_Manager ::  Import from WWW Current Train Limit is to Low for your Entry ");
                    }
                }
                Logger.Message("Lok Erfolgreich Empfangen, und Gespeichert", "GRUEN");
            }
            if (DeleteToggle == DeleteToggle.isOn)
            {
                StartCoroutine(Clear());
            }
            else
            {
                Logger.PrintLog("MODUL Import_Manager :: Material wird nicht am Server Gelöscht,bleibt erhalten bis User es Löscht (RE-Import)");
            }
        }
    }

    IEnumerator Clear()
    {
        WWW read = new WWW(Settings.URL + "/remove.php?uniqueID=" + ImportKey.text.ToString());
        yield return read;

        if (read.error != null)
        {
            Logger.PrintLogEnde();
            Logger.PrintLog("MODUL Import_Manager :: No Internet Connection to the Server.!");
            Logger.Message("Keine Internet Verbindung.!", "ROT");
            Logger.Error("MODUL Import_Manager :: Clear()  No Connection to the Server \n");
        }
        else
        {
            Logger.PrintLogEnde();
            Logger.PrintLog("MODUL Import_Manager :: Stored Material Cleared!");
            
        }
    }
}