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
            //unselectedID = u;
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
        if(Type == "TRAIN")
        {
            SaveTrain();
        }
        if(Type == "WAGON")
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
                        Logger.PrintLog("MODUL Import_Manager :: Save new Train: " + "\n" + ex);
                    }
                }
                finally
                {
                    Import =+1;
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
                        Logger.PrintLog("MODUL Import_Manager :: Save new Wagon: " + ex + "\n");
                    }
                }
                finally
                {
                    Import =+1;
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
        StartManager.SystemMeldung.text = ( "Alles Aktualisiert");
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
                    Logger.PrintLog("MODUL Import_Manager :: ERROR by Save Settings: " + ex + "\n");
                    Debug.Log(ex);
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
}