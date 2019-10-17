/*
 * 
 *   TrainBase Decoder Manager Version 1 from 09.06.2019 written by Michael Kux
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
public class DecoderData : System.Object
{
    public string dbDatum;
    public string dbBeschreibung;
    public string dbArtkNR;
    public int dbType;
    public string db1;
    public string db2;
    public string db3;
    public string db4;
    public string db5;
    public string db6;
    public string db7;
    public string db8;
    public string db9;
    public string db10;
    public string db11;
    public string db12;
    public string db13;
    public string db14;
    public string db15;
    public string db16;
    public string db17;
    public string dbIdentifyer;
}
public class DecoderManager : MonoBehaviour
{
    public LogWriterManager Logger;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public SettingsManager USettings;
    public Network_Handler NH;
    public SettingsManager SM;
    public Discord_Menue_Update DMU;
    [Header("Decoder List Items")]
    public List<DecoderData> dbDecoder;
    public GameObject[] slots;
    public GameObject[] Datum;
    public GameObject[] Beschreibung;
    public GameObject[] ArtkNR;
    public Toggle[] Toggle;
    public bool[] Selected;
    public int SelectedID = 0;
    public int PageID = 0;
    public int Decoder;
    [Header("Input Fields S88")]
    public InputField S88Port1;
    public InputField S88Port2;
    public InputField S88Port3;
    public InputField S88Port4;
    public InputField S88Port5;
    public InputField S88Port6;
    public InputField S88Port7;
    public InputField S88Port8;
    public InputField S88Port9;
    public InputField S88Port10;
    public InputField S88Port11;
    public InputField S88Port12;
    public InputField S88Port13;
    public InputField S88Port14;
    public InputField S88Port15;
    public InputField S88Port16;
    public InputField S88Description;
    public int S88ID = 0;
    [Header("Input Fields M83")]
    public InputField M83Port1;
    public InputField M83Port2;
    public InputField M83Port3;
    public InputField M83Port4;
    public InputField M83Description;
    public int M83ID = 1;
    [Header("Input Fields M84")]
    public InputField M84Port1;
    public InputField M84Port2;
    public InputField M84Port3;
    public InputField M84Port4;
    public InputField M84Description;
    public int M84ID = 2;
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
    [Header("Edit Panel Windows")]
    public Text TopWinDesc;
    public GameObject Edit_S88;
    public InputField Edit_S88Port1;
    public InputField Edit_S88Port2;
    public InputField Edit_S88Port3;
    public InputField Edit_S88Port4;
    public InputField Edit_S88Port5;
    public InputField Edit_S88Port6;
    public InputField Edit_S88Port7;
    public InputField Edit_S88Port8;
    public InputField Edit_S88Port9;
    public InputField Edit_S88Port10;
    public InputField Edit_S88Port11;
    public InputField Edit_S88Port12;
    public InputField Edit_S88Port13;
    public InputField Edit_S88Port14;
    public InputField Edit_S88Port15;
    public InputField Edit_S88Port16;
    public InputField Edit_S88Description;
    public GameObject Edit_M83;
    public InputField Edit_M83Port1;
    public InputField Edit_M83Port2;
    public InputField Edit_M83Port3;
    public InputField Edit_M83Port4;
    public InputField Edit_M83Description;
    public GameObject Edit_M84;
    public InputField Edit_M84Port1;
    public InputField Edit_M84Port2;
    public InputField Edit_M84Port3;
    public InputField Edit_M84Port4;
    public InputField Edit_M84Description;
    //Static Shit here :P
    string Port1 = "Port: 1";
    string Port2 = "Port: 2";
    string Port3 = "Port: 3";
    string Port4 = "Port: 4";
    string Port5 = "Port: 5";
    string Port6 = "Port: 6";
    string Port7 = "Port: 7";
    string Port8 = "Port: 8";
    string Port9 = "Port: 9";
    string Port10 = "Port: 10";
    string Port11 = "Port: 11";
    string Port12 = "Port: 12";
    string Port13 = "Port: 13";
    string Port14 = "Port: 14";
    string Port15 = "Port: 15";
    string Port16 = "Port: 16";
    string DText = "Custom Text";

    void Start ()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Decoder_Manager -> Message is Normal.");
        }
        CleanScreen();
        ReadAllItems();
        readIntervall();
        SetButtonText();
    }

    public void SetButtonText()
    {
        //S88
        S88Port1.text = Port1;
        S88Port2.text = Port2;
        S88Port3.text = Port3;
        S88Port4.text = Port4;
        S88Port5.text = Port5;
        S88Port6.text = Port6;
        S88Port7.text = Port7;
        S88Port8.text = Port8;
        S88Port9.text = Port9;
        S88Port10.text = Port10;
        S88Port11.text = Port11;
        S88Port12.text = Port12;
        S88Port13.text = Port13;
        S88Port14.text = Port14;
        S88Port15.text = Port15;
        S88Port16.text = Port16;
        S88Description.text = DText;
        //M83
        M83Port1.text = Port4;
        M83Port2.text = Port3;
        M83Port3.text = Port2;
        M83Port4.text = Port1;
        M83Description.text = DText;
        //M84
        M84Port1.text = Port4;
        M84Port2.text = Port3;
        M84Port3.text = Port2;
        M84Port4.text = Port1;
        M84Description.text = DText;

    }

    public void ReadAllItems()
    {
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    dbDecoder.Add(RdbDecoder);
                    Decoder = dbDecoder.Count;
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read all Items:  " + ex + "\n");
            }
        }
        dbConnection.Close();
        dbConnection = null;
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Items Loadet");
        }
    }

    void readIntervall()
    {
        for (int i = 0; i < dbDecoder.Count; i++)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Decoder_Manager :: DECODER ID: " + (i + 1) + " | " + dbDecoder[i].dbBeschreibung + " | " + dbDecoder[i].dbArtkNR);
            }
            Decoder = i + 1;
        }
        Logger.PrintLogEnde();
    }

    public void StatsFix()
    {
        ReadAllItems();
        for (int i = 0; i < dbDecoder.Count; i++)
        {
            Decoder = i + 1;
        }
        Logger.PrintLogEnde();
    }

    void Update ()
    {
        if (PageID == 1)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d;
                }
            }
        }

        if (PageID == 2)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 24;
                }
            }
        }

        if (PageID == 3)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 48;
                }
            }
        }

        if (PageID == 4)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 72;
                }
            }
        }

        if (PageID == 5)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 96;
                }
            }
        }

        if (PageID == 6)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 120;
                }
            }
        }

        if (PageID == 7)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 144;
                }
            }
        }

        if (PageID == 8)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 168;
                }
            }
        }

        if (PageID == 9)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 192;
                }
            }
        }

        if (PageID == 10)
        {
            for (int d = 0; d < Selected.Length; d++)
            {
                Selected[d] = Toggle[d].isOn;
                if (Toggle[d].isOn)
                {
                    SelectedID = d + 216;
                }
            }
        }
    }

    public void SaveS88()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into DECODER (BESCHREIBUNG , TYPE , OUT1 , OUT2 , OUT3 , OUT4 , OUT5 ,OUT6 , OUT7 ,OUT8 , OUT9 , OUT10 , OUT11 , OUT12 , OUT13 , OUT14 ,OUT15 , OUT16 , IDENTIFYER ) VALUES" + " (@BESCHREIBUNG , @TYPE , @OUT1 , @OUT2 , @OUT3 , @OUT4 , @OUT5 ,@OUT6 , @OUT7 ,@OUT8 , @OUT9 , @OUT10 , @OUT11 , @OUT12 , @OUT13 , @OUT14 ,@OUT15 , @OUT16 , @IDENTIFYER)";
            command.Parameters.AddWithValue("@BESCHREIBUNG", S88Description.text);
            command.Parameters.AddWithValue("@TYPE", S88ID);
            command.Parameters.AddWithValue("@OUT1", S88Port1.text);
            command.Parameters.AddWithValue("@OUT2", S88Port2.text);
            command.Parameters.AddWithValue("@OUT3", S88Port3.text);
            command.Parameters.AddWithValue("@OUT4", S88Port4.text);
            command.Parameters.AddWithValue("@OUT5", S88Port5.text);
            command.Parameters.AddWithValue("@OUT6", S88Port6.text);
            command.Parameters.AddWithValue("@OUT7", S88Port7.text);
            command.Parameters.AddWithValue("@OUT8", S88Port8.text);
            command.Parameters.AddWithValue("@OUT9", S88Port9.text);
            command.Parameters.AddWithValue("@OUT10", S88Port10.text);
            command.Parameters.AddWithValue("@OUT11", S88Port11.text);
            command.Parameters.AddWithValue("@OUT12", S88Port12.text);
            command.Parameters.AddWithValue("@OUT13", S88Port13.text);
            command.Parameters.AddWithValue("@OUT14", S88Port14.text);
            command.Parameters.AddWithValue("@OUT15", S88Port15.text);
            command.Parameters.AddWithValue("@OUT16", S88Port16.text);
            command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
            if (dbDecoder.Count <= 240)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Logger.Message("Fehler beim Schreiben in die Datenbank", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Decoder_Manager :: Save new Decoder: " + ex + "\n");
                    }
                }
                finally
                {
                    dbConnection.Close();
                    Logger.Message("Neuer Decoder Erfolgreich Angelegt.", "GRUEN");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("S88 Decoder Erfolgreich Angelegt.");
                    }
                }
            }
            else
            {
                Logger.Message("Fehler beim Speichern des Decoders, Aktuelles limit ist bei 240 Decodern.!", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Decoder_Manager :: Current Train Limit is to Low for your Entry ");
                }
            }
        }
    }

    public void SaveM83()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into DECODER (BESCHREIBUNG , TYPE , OUT1 , OUT2 , OUT3 , OUT4 , OUT5 ,OUT6 , OUT7 ,OUT8 , OUT9 , OUT10 , OUT11 , OUT12 , OUT13 , OUT14 ,OUT15 , OUT16 , IDENTIFYER ) VALUES" + " (@BESCHREIBUNG , @TYPE , @OUT1 , @OUT2 , @OUT3 , @OUT4 , @OUT5 ,@OUT6 , @OUT7 ,@OUT8 , @OUT9 , @OUT10 , @OUT11 , @OUT12 , @OUT13 , @OUT14 ,@OUT15 , @OUT16 , @IDENTIFYER)";
            command.Parameters.AddWithValue("@BESCHREIBUNG", M83Description.text);
            command.Parameters.AddWithValue("@TYPE", M83ID);
            command.Parameters.AddWithValue("@OUT1", M83Port1.text);
            command.Parameters.AddWithValue("@OUT2", M83Port2.text);
            command.Parameters.AddWithValue("@OUT3", M83Port3.text);
            command.Parameters.AddWithValue("@OUT4", M83Port4.text);
            command.Parameters.AddWithValue("@OUT5", "null");
            command.Parameters.AddWithValue("@OUT6", "null");
            command.Parameters.AddWithValue("@OUT7", "null");
            command.Parameters.AddWithValue("@OUT8", "null");
            command.Parameters.AddWithValue("@OUT9", "null");
            command.Parameters.AddWithValue("@OUT10", "null");
            command.Parameters.AddWithValue("@OUT11", "null");
            command.Parameters.AddWithValue("@OUT12", "null");
            command.Parameters.AddWithValue("@OUT13", "null");
            command.Parameters.AddWithValue("@OUT14", "null");
            command.Parameters.AddWithValue("@OUT15", "null");
            command.Parameters.AddWithValue("@OUT16", "null");
            command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
            if (dbDecoder.Count <= 240)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Logger.Message("Fehler beim Schreiben in die Datenbank", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Decoder_Manager :: Save new Decoder: " + ex + "\n");
                    }
                }
                finally
                {
                    dbConnection.Close();
                    Logger.Message("Neuer Decoder Erfolgreich Angelegt.", "GRUEN");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("M83 Decoder Erfolgreich Angelegt.");
                    }
                }
            }
            else
            {
                Logger.Message("Fehler beim Speichern des Decoders, Aktuelles limit ist bei 240 Decodern.!", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Decoder_Manager :: Current Train Limit is to Low for your Entry ");
                }
            }
        }
    }

    public void SaveM84()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into DECODER (BESCHREIBUNG , TYPE , OUT1 , OUT2 , OUT3 , OUT4 , OUT5 ,OUT6 , OUT7 ,OUT8 , OUT9 , OUT10 , OUT11 , OUT12 , OUT13 , OUT14 ,OUT15 , OUT16 , IDENTIFYER  ) VALUES" + " (@BESCHREIBUNG , @TYPE , @OUT1 , @OUT2 , @OUT3 , @OUT4 , @OUT5 ,@OUT6 , @OUT7 ,@OUT8 , @OUT9 , @OUT10 , @OUT11 , @OUT12 , @OUT13 , @OUT14 ,@OUT15 , @OUT16 , @IDENTIFYER )";
            command.Parameters.AddWithValue("@BESCHREIBUNG", M84Description.text);
            command.Parameters.AddWithValue("@TYPE", M84ID);
            command.Parameters.AddWithValue("@OUT1", M84Port1.text);
            command.Parameters.AddWithValue("@OUT2", M84Port2.text);
            command.Parameters.AddWithValue("@OUT3", M84Port3.text);
            command.Parameters.AddWithValue("@OUT4", M84Port4.text);
            command.Parameters.AddWithValue("@OUT5", "null");
            command.Parameters.AddWithValue("@OUT6", "null");
            command.Parameters.AddWithValue("@OUT7", "null");
            command.Parameters.AddWithValue("@OUT8", "null");
            command.Parameters.AddWithValue("@OUT9", "null");
            command.Parameters.AddWithValue("@OUT10", "null");
            command.Parameters.AddWithValue("@OUT11", "null");
            command.Parameters.AddWithValue("@OUT12", "null");
            command.Parameters.AddWithValue("@OUT13", "null");
            command.Parameters.AddWithValue("@OUT14", "null");
            command.Parameters.AddWithValue("@OUT15", "null");
            command.Parameters.AddWithValue("@OUT16", "null");
            command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
            if (dbDecoder.Count <= 240)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    Logger.Message("Fehler beim Schreiben in die Datenbank", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Decoder_Manager :: Save new Decoder: " + ex + "\n");
                    }
                }
                finally
                {
                    dbConnection.Close();
                    Logger.Message("Neuer Decoder Erfolgreich Angelegt.", "GRUEN");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("M84 Decoder Erfolgreich Angelegt.");
                    }
                }
            }
            else
            {
                Logger.Message("Fehler beim Speichern des Decoders, Aktuelles limit ist bei 240 Decodern.!", "ROT");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Decoder_Manager :: Current Train Limit is to Low for your Entry ");
                }
            }
        }
    }

    public void ReadItemList1()
    {
        PageID = 1;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 0; i < dbDecoder.Count && i < 24; i++)
                    {
                        slots[i].gameObject.SetActive(true);
                        Datum[i].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if(dbDecoder[i].dbType == 0)
                        {
                            ArtkNR[i].GetComponent<Text>().text = " S88 ";
                        }
                        else if(dbDecoder[i].dbType == 1)
                        {
                            ArtkNR[i].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 0)
                        {
                            Page1Vorwd.gameObject.SetActive(false);
                            PageNumber.gameObject.SetActive(false);
                            Page1Back.gameObject.SetActive(false);
                        }
                        if (dbDecoder.Count >= 25)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
           Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }

    public void ReadItemList2()
    {
        PageID = 2;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 24; i < dbDecoder.Count && i < 48; i++)
                    {
                        slots[i - 24].gameObject.SetActive(true);
                        Datum[i - 24].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i - 24].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if (dbDecoder[i - 24].dbType == 0)
                        {
                            ArtkNR[i - 24].GetComponent<Text>().text = " S88 ";
                        }
                        else if (dbDecoder[i - 24].dbType == 1)
                        {
                            ArtkNR[i - 24].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i - 24].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 25)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page2Back.gameObject.SetActive(true);
                        }
                        if (dbDecoder.Count >= 48)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }

    public void ReadItemList3()
    {
        PageID = 3;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 48; i < dbDecoder.Count && i < 72; i++)
                    {
                        slots[i - 48].gameObject.SetActive(true);
                        Datum[i - 48].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i - 48].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if (dbDecoder[i - 48].dbType == 0)
                        {
                            ArtkNR[i - 48].GetComponent<Text>().text = " S88 ";
                        }
                        else if (dbDecoder[i - 48].dbType == 1)
                        {
                            ArtkNR[i - 48].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i - 48].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 49)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page3Back.gameObject.SetActive(true);
                        }
                        if (dbDecoder.Count >= 72)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }

    public void ReadItemList4()
    {
        PageID = 4;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 72; i < dbDecoder.Count && i < 96; i++)
                    {
                        slots[i - 72].gameObject.SetActive(true);
                        Datum[i - 72].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i - 72].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if (dbDecoder[i - 72].dbType == 0)
                        {
                            ArtkNR[i - 72].GetComponent<Text>().text = " S88 ";
                        }
                        else if (dbDecoder[i - 72].dbType == 1)
                        {
                            ArtkNR[i - 72].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i - 72].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 73)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page4Back.gameObject.SetActive(true);
                        }
                        if (dbDecoder.Count >= 96)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }

    public void ReadItemList5()
    {
        PageID = 5;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 96; i < dbDecoder.Count && i < 120; i++)
                    {
                        slots[i - 96].gameObject.SetActive(true);
                        Datum[i - 96].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i - 96].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if (dbDecoder[i - 96].dbType == 0)
                        {
                            ArtkNR[i - 96].GetComponent<Text>().text = " S88 ";
                        }
                        else if (dbDecoder[i - 96].dbType == 1)
                        {
                            ArtkNR[i - 96].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i - 96].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 97)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page5Back.gameObject.SetActive(true);
                        }
                        if (dbDecoder.Count >= 120)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }

    public void ReadItemList6()
    {
        PageID = 6;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 120; i < dbDecoder.Count && i < 144; i++)
                    {
                        slots[i - 120].gameObject.SetActive(true);
                        Datum[i - 120].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i - 120].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if (dbDecoder[i - 120].dbType == 0)
                        {
                            ArtkNR[i - 120].GetComponent<Text>().text = " S88 ";
                        }
                        else if (dbDecoder[i - 120].dbType == 1)
                        {
                            ArtkNR[i - 120].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i - 120].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 121)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page6Back.gameObject.SetActive(true);
                        }
                        if (dbDecoder.Count >= 144)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }

    public void ReadItemList7()
    {
        PageID = 7;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 144; i < dbDecoder.Count && i < 168; i++)
                    {
                        slots[i - 144].gameObject.SetActive(true);
                        Datum[i - 144].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i - 144].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if (dbDecoder[i - 144].dbType == 0)
                        {
                            ArtkNR[i - 144].GetComponent<Text>().text = " S88 ";
                        }
                        else if (dbDecoder[i - 144].dbType == 1)
                        {
                            ArtkNR[i - 144].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i - 144].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 145)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page7Back.gameObject.SetActive(true);
                        }
                        if (dbDecoder.Count >= 168)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }

    public void ReadItemList8()
    {
        PageID = 8;
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: " + PageID, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        PageNumber.text = PageID.ToString();
        CleanScreen();
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    dbDecoder.Add(RdbDecoder);
                    for (int i = 168; i < dbDecoder.Count && i < 192; i++)
                    {
                        slots[i - 168].gameObject.SetActive(true);
                        Datum[i - 168].GetComponent<Text>().text = " ID: " + (i + 1) + " | " + dbDecoder[i].dbDatum + "  ";
                        Beschreibung[i - 168].GetComponent<Text>().text = " " + dbDecoder[i].dbBeschreibung + "  ";
                        if (dbDecoder[i - 168].dbType == 0)
                        {
                            ArtkNR[i - 168].GetComponent<Text>().text = " S88 ";
                        }
                        else if (dbDecoder[i - 168].dbType == 1)
                        {
                            ArtkNR[i - 168].GetComponent<Text>().text = " M83 ";
                        }
                        else
                        {
                            ArtkNR[i - 168].GetComponent<Text>().text = "M84";
                        }
                        if (dbDecoder.Count >= 169)
                        {
                            PageNumber.gameObject.SetActive(true);
                            Page8Back.gameObject.SetActive(true);
                        }
                        if (dbDecoder.Count >= 192)
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
                Logger.PrintLog("MODUL Decoder_Manager :: ERROR by Read Items: Page# " + PageID + " " + ex + "\n");
            }
            Logger.Message("Fehler beim Lesen von Seite " + PageID, "ROT");
        }
        dbConnection.Close();
        dbConnection = null;
        Logger.Message("Zeige Decoder Seite " + PageID + "an", "CYAN");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager :: Page " + PageID + " Found " + dbDecoder.Count + " Rows");
        }
    }
    
    public void DeleteItem()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        string sql = "DELETE FROM DECODER WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "'  ";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        Logger.Message("Eintrag Erfogreich Gelöscht", "ROT");
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Decoder_Manager ::  Decoder: Ident " + dbDecoder[SelectedID].dbIdentifyer + " from Date: " + dbDecoder[SelectedID].dbDatum + " wurde aus dem System Gelöscht.!");
        }
        SelectedID = -1;
    }

    public void DeleteSchutz()
    {
        SelectedID = -1;
    }

    public void GetSelcetedData()
    {
        if (dbDecoder[SelectedID].dbType == 0) //S88 Decoder 
        {
            Edit_S88.gameObject.SetActive(true);
            Edit_M84.gameObject.SetActive(false);
            Edit_M83.gameObject.SetActive(false);
            PresenceManager.UpdatePresence("Edit Decoder", "S88 ID: " + (SelectedID + 1), DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
            TopWinDesc.text = "Edit Anlagen Decoder S88 ID: " + (SelectedID + 1);
            Edit_S88Port1.text = dbDecoder[SelectedID].db1;
            Edit_S88Port2.text = dbDecoder[SelectedID].db2;
            Edit_S88Port3.text = dbDecoder[SelectedID].db3;
            Edit_S88Port4.text = dbDecoder[SelectedID].db4;
            Edit_S88Port5.text = dbDecoder[SelectedID].db5;
            Edit_S88Port6.text = dbDecoder[SelectedID].db6;
            Edit_S88Port7.text = dbDecoder[SelectedID].db7;
            Edit_S88Port8.text = dbDecoder[SelectedID].db8;
            Edit_S88Port9.text = dbDecoder[SelectedID].db9;
            Edit_S88Port10.text = dbDecoder[SelectedID].db10;
            Edit_S88Port11.text = dbDecoder[SelectedID].db11;
            Edit_S88Port12.text = dbDecoder[SelectedID].db12;
            Edit_S88Port13.text = dbDecoder[SelectedID].db13;
            Edit_S88Port14.text = dbDecoder[SelectedID].db14;
            Edit_S88Port15.text = dbDecoder[SelectedID].db15;
            Edit_S88Port16.text = dbDecoder[SelectedID].db16;
            Edit_S88Description.text = dbDecoder[SelectedID].dbBeschreibung;
        }

        if (dbDecoder[SelectedID].dbType == 1) //M83 Decoder
        {
            Edit_M83.gameObject.SetActive(true);
            Edit_M84.gameObject.SetActive(false);
            Edit_S88.gameObject.SetActive(false);
            PresenceManager.UpdatePresence("Edit Decoder", "M83 ID: " + (SelectedID + 1), DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
            TopWinDesc.text = "Edit Anlagen Decoder M83 ID: " + (SelectedID + 1);
            Edit_M83Port1.text = dbDecoder[SelectedID].db1;
            Edit_M83Port2.text = dbDecoder[SelectedID].db2;
            Edit_M83Port3.text = dbDecoder[SelectedID].db3;
            Edit_M83Port4.text = dbDecoder[SelectedID].db4;
            Edit_M83Description.text = dbDecoder[SelectedID].dbBeschreibung;
        }

        if (dbDecoder[SelectedID].dbType == 2) //M84 Decoder
        {
            Edit_M84.gameObject.SetActive(true);
            Edit_M83.gameObject.SetActive(false);
            Edit_S88.gameObject.SetActive(false);
            PresenceManager.UpdatePresence("Edit Decoder", "M84 ID: " + (SelectedID + 1), DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
            TopWinDesc.text = "Edit Anlagen Decoder M84 ID: " + (SelectedID +1);

            Edit_M84Port1.text = dbDecoder[SelectedID].db4;
            Edit_M84Port2.text = dbDecoder[SelectedID].db3;
            Edit_M84Port3.text = dbDecoder[SelectedID].db2;
            Edit_M84Port4.text = dbDecoder[SelectedID].db1;
            Edit_M84Description.text = dbDecoder[SelectedID].dbBeschreibung;
        }
    }

    public void SaveNewUpdate()
    {
        if(dbDecoder[SelectedID].dbType == 0 )
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE DECODER SET BESCHREIBUNG = @BESCHREIBUNG, OUT1 = @OUT1, OUT2 = @OUT2 , OUT3 = @OUT3 , OUT4 = @OUT4 , OUT5 = @OUT5 ,OUT6 = @OUT6 , OUT7 = @OUT7 ,OUT8 = @OUT8 , OUT9 = @OUT9 , OUT10 = @OUT10 , OUT11 = @OUT11 , OUT12 = @OUT12 , OUT13 = @OUT13 , OUT14 = @OUT14 ,OUT15 = @OUT15 , OUT16 = @OUT16  WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "' AND DATUM='" + dbDecoder[SelectedID].dbDatum + "'  ";
                command.Parameters.AddWithValue("@BESCHREIBUNG", Edit_S88Description.text);
                command.Parameters.AddWithValue("@OUT1", Edit_S88Port1.text);
                command.Parameters.AddWithValue("@OUT2", Edit_S88Port2.text);
                command.Parameters.AddWithValue("@OUT3", Edit_S88Port3.text);
                command.Parameters.AddWithValue("@OUT4", Edit_S88Port4.text);
                command.Parameters.AddWithValue("@OUT5", Edit_S88Port5.text);
                command.Parameters.AddWithValue("@OUT6", Edit_S88Port6.text);
                command.Parameters.AddWithValue("@OUT7", Edit_S88Port7.text);
                command.Parameters.AddWithValue("@OUT8", Edit_S88Port8.text);
                command.Parameters.AddWithValue("@OUT9", Edit_S88Port9.text);
                command.Parameters.AddWithValue("@OUT10", Edit_S88Port10.text);
                command.Parameters.AddWithValue("@OUT11", Edit_S88Port11.text);
                command.Parameters.AddWithValue("@OUT12", Edit_S88Port12.text);
                command.Parameters.AddWithValue("@OUT13", Edit_S88Port13.text);
                command.Parameters.AddWithValue("@OUT14", Edit_S88Port14.text);
                command.Parameters.AddWithValue("@OUT15", Edit_S88Port15.text);
                command.Parameters.AddWithValue("@OUT16", Edit_S88Port16.text);
                if (dbDecoder.Count <= 240)
                {
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Logger.Message("Fehler beim Schreiben in die Datenbank", "ROT");
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("MODUL Decoder_Manager :: Error by Edit Decoder: " + ex + "\n");
                        }
                    }
                    finally
                    {
                        dbConnection.Close();
                        Logger.Message("Decoder Updated.", "GRUEN");
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("S88 Decoder Erfolgreich Bearbeitet.");
                        }
                    }
                }
                else
                {
                    Logger.Message("Fehler beim Speichern des Decoders, Aktuelles limit ist bei 240 Decodern.!", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Decoder_Manager :: Current Decoder Limit is to Low for your Entry ");
                    }
                }
            }
        }

        if(dbDecoder[SelectedID].dbType == 1)
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE DECODER SET BESCHREIBUNG = @BESCHREIBUNG, OUT1 = @OUT1, OUT2 = @OUT2 , OUT3 = @OUT3 , OUT4 = @OUT4  WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "' AND DATUM='" + dbDecoder[SelectedID].dbDatum + "'  ";
                command.Parameters.AddWithValue("@BESCHREIBUNG", Edit_M83Description.text);
                command.Parameters.AddWithValue("@OUT1", Edit_M83Port1.text);
                command.Parameters.AddWithValue("@OUT2", Edit_M83Port2.text);
                command.Parameters.AddWithValue("@OUT3", Edit_M83Port3.text);
                command.Parameters.AddWithValue("@OUT4", Edit_M83Port4.text);
                if (dbDecoder.Count <= 240)
                {
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Logger.Message("Fehler beim Schreiben in die Datenbank", "ROT");
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("MODUL Decoder_Manager :: Error by Edit Decoder: " + ex + "\n");
                        }
                    }
                    finally
                    {
                        dbConnection.Close();
                        Logger.Message("Decoder Updated.", "GRUEN");
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("M83 Decoder Erfolgreich Bearbeitet.");
                        }
                    }
                }
                else
                {
                    Logger.Message("Fehler beim Speichern des Decoders, Aktuelles limit ist bei 240 Decodern.!", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Decoder_Manager :: Current Decoder Limit is to Low for your Entry ");
                    }
                }
            }
        }

        if(dbDecoder[SelectedID].dbType == 2)
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE DECODER SET BESCHREIBUNG = @BESCHREIBUNG, OUT1 = @OUT1, OUT2 = @OUT2 , OUT3 = @OUT3 , OUT4 = @OUT4 WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "' AND DATUM='" + dbDecoder[SelectedID].dbDatum + "'  ";
                command.Parameters.AddWithValue("@BESCHREIBUNG", Edit_M84Description.text);
                command.Parameters.AddWithValue("@OUT1", Edit_M84Port1.text);
                command.Parameters.AddWithValue("@OUT2", Edit_M84Port2.text);
                command.Parameters.AddWithValue("@OUT3", Edit_M84Port3.text);
                command.Parameters.AddWithValue("@OUT4", Edit_M84Port4.text);
                if (dbDecoder.Count <= 240)
                {
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        Logger.Message("Fehler beim Schreiben in die Datenbank", "ROT");
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("MODUL Decoder_Manager :: Error by Edit Decoder: " + ex + "\n");
                        }
                    }
                    finally
                    {
                        dbConnection.Close();
                        Logger.Message("Decoder Updated.", "GRUEN");
                        if (Logger.logIsEnabled == true)
                        {
                            Logger.PrintLog("M84 Decoder Erfolgreich Bearbeitet.");
                        }
                    }
                }
                else
                {
                    Logger.Message("Fehler beim Speichern des Decoders, Aktuelles limit ist bei 240 Decodern.!", "ROT");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL Decoder_Manager :: Current Decoder Limit is to Low for your Entry ");
                    }
                }
            }
        }
    }

    public void CleanScreen()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(false);
            Toggle[i].isOn = false;
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
}
