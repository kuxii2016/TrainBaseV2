/*
 * 
 *   TrainBase Start up Manager Version 1 from 25.03.2018 written by Michael Kux
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

public class StartUpManager : MonoBehaviour
{
    public LogWriterManager Logger;
    public ProgrammSettings SettingsManager;
    public GameObject VersionToOld;
    public Text SystemMeldung;
    public string SavePfad = "";
    public string LastUpdate;
    public string NewDBProtokoll = "28052019";
    public string NewUpdate = "109";

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Startup_Manager -> Message is Normal.");
        }
        SavePfad = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName;
        CheckUserFolder();
        CheckDatabase();
    }

    public void ReadLastUpdate()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Settings", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    LastUpdate = reader.GetString(6);
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Startup_Manager :: Error read the Last Update" + ex + "\n");
            }
        }
        dbConnection.Close();
    }

    public void CheckUserFolder()
    {
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Updates"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Updates");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Exporter"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Exporter");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Importer"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Importer");
        }

        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Startup_Manager :: User Folder are all OK.! ");
        }
    }

    public void CheckDatabaseFile()
    {
        if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName))
        {
            Debug.Log("Database Here");
        }
    }

    public void CheckDatabase()
    {
        if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName))
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Startup_Manager :: I do Nothing Database exist.");
            }
            Debug.Log("Read");
            ReadLastUpdate();
            UpdateInstaller();
        }
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName))
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Startup_Manager :: First Start, Create Database.");
            }
            CreateTrainDataBase();
            Debug.Log("Create");
        }
    }

    public void CreateTrainDataBase()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "CREATE TABLE Trains(BAUREIHE TEXT, FARBE TEXT, TYP INT,  HERSTELLER INT,  ERSTELLT DATETIME DEFAULT CURRENT_TIMESTAMP, KATALOGNUMMER TEXT, SERIENNUMMER TEXT, KAUFDATUM TEXT, PREIS TEXT, WARTUNGDAY INT, WARTUNGMONAT INT, WARTUNGJEAR INT, ADRESSE TEXT, PROTOKOLL INT, FAHRSTUFEN INT, DECHERSTELLER TEXT, RAUCH INT, SOUND INT, ROTWEISS INT, PANDO INT, TELEX INT, KUPPLUNG INT, KTAG INT, KMONAT INT, KJAHR INT, SPURWEITE INT default 0, CV2 INT default 0, CV3 INT default 0, CV4 INT default 0, CV5 INT default 0, PIC TEXT default NULL, IDENTIFYER TEXT, LAGERORT INT default 0)";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        CreateWagonsDataBase();
        Debug.Log("Trains Done");
    }

    public void CreateWagonsDataBase()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "CREATE TABLE Wagons(TYP INT, FARBE TEXT, HERSTELLER INT,  ERSTELLT DATETIME DEFAULT CURRENT_TIMESTAMP, KATALOGNUMMER TEXT, SERIENNUMMER TEXT, KAUFDAY INT, KAUFMONAT INT, KAUFJAHR INT, PREIS TEXT, KUPPLUNG INT, LICHT INT, PREISER INT, SPURWEITE INT default 0, PIC TEXT, LAGERORT INT default 0, IDENTIFYER TEXT)";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        CreateSettingsDataBase();
        Debug.Log("Wagons Done");
    }

    public void CreateSettingsDataBase()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "CREATE TABLE Settings(AUTOCHECK INT default 1, TRAINS INT default 0, WAGONS INT default 0, DATENSEND INT default 0, SOUNDS INT default 0, INVENTORY INT default 0, LASTUPDATE TEXT default  109, KEY TEXT default  PUBLIC, IMPORTRPC INT default 0, IMPORTXML INT default 0, DBPROTOKOLL TEXT default 28052019, LOKLISTICONS INT default 0, IMAGETYPE INT default 0, WARTUNGSINTERVALL INT default 0)";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        CreateInventoryDataBase();
        Debug.Log("Settings Done");
        SaveUpdate();
    }

    public void CreateInventoryDataBase()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "CREATE TABLE Inventory(DATUM DATETIME DEFAULT CURRENT_TIMESTAMP, STUECK TEXT, BESCHREIBUNG TEXT, ARTKNR TEXT, PREIS TEXT, FREEa TEXT, FREEb TEXT, FREEc TEXT)";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        Debug.Log("Inventory Done");
        CreateDecoderDatabase();
    }

    public void SaveUpdate()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into  Settings (DBPROTOKOLL, LASTUPDATE) VALUES" + "(@DBPROTOKOLL, @LASTUPDATE)";
            command.Parameters.AddWithValue("@DBPROTOKOLL", NewDBProtokoll);
            command.Parameters.AddWithValue("@LASTUPDATE", NewUpdate);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Startup_Manager :: ERROR Write Update Data: " + ex + "\n");
                }
            }
            finally
            {
                dbConnection.Close();
                ReadLastUpdate();
            }
        }
    }

    public void UpdateInstaller()
    {
        if (LastUpdate == "108")
        {
            WriteImportRPC();
            WriteImportXML();
            WriteDBProtokoll();
            WriteLokListImagesIcon();
            WriteImageType();
            WriteWartungsIntervall();
            UpdateUpdate();
            UpdateInstaller();
        }

        if (LastUpdate == "109")
        {
            CreateDecoderDatabase();
            Logger.Message("Neues Update", "CYAN");
            UpdateInstaller();
        }

        if (LastUpdate == "110")
        {
            VersionToOld.gameObject.SetActive(false);
            Logger.Message("Programm ist Bereit", "CYAN");
        }
        else
        {
            VersionToOld.gameObject.SetActive(true);
        }
    }

    public void UpdateUpdate()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Settings  SET DBPROTOKOLL = @DBPROTOKOLL, LASTUPDATE = @LASTUPDATE";
            command.Parameters.AddWithValue("@DBPROTOKOLL", NewDBProtokoll);
            command.Parameters.AddWithValue("@LASTUPDATE", NewUpdate);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Startup_Manager :: ERROR Write Update Data: " + ex + "\n");
                }
            }
            finally
            {
                dbConnection.Close();
                ReadLastUpdate();
            }
        }
    }

    //# New Update Called 109
    public void WriteImportRPC()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "ALTER TABLE Settings ADD COLUMN IMPORTRPC INT default 0";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
    }

    public void WriteImportXML()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "ALTER TABLE Settings ADD COLUMN IMPORTXML INT default 0";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
    }

    public void WriteDBProtokoll()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "ALTER TABLE Settings ADD COLUMN DBPROTOKOLL TEXT default null";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
    }

    public void WriteLokListImagesIcon()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "ALTER TABLE Settings ADD COLUMN LOKLISTICONS INT default 0";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
    }

    public void WriteImageType()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "ALTER TABLE Settings ADD COLUMN IMAGETYPE INT default 0";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
    }

    public void WriteWartungsIntervall()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "ALTER TABLE Settings ADD COLUMN WARTUNGSINTERVALL INT default 0";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
    }

    public void CreateDecoderDatabase()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName));
        dbConnection.Open();
        string sql = "CREATE TABLE DECODER(DATUM DATETIME DEFAULT CURRENT_TIMESTAMP, BESCHREIBUNG TEXT default null, ARTKNR TEXT default null, TYPE INT default 0, OUT1 TEXT default null, OUT2 TEXT default null, OUT3 TEXT default null, OUT4 TEXT default null, OUT5 TEXT default null, OUT6 TEXT default null, OUT7 TEXT default null, OUT8 TEXT default null, OUT9 TEXT default null, OUT10 TEXT default null, OUT11 TEXT default null, OUT12 TEXT default null, OUT13 TEXT default null, OUT14 TEXT default null, OUT15 TEXT default null, OUT16 TEXT default null, OUT17 TEXT default null, IDENTIFYER TEXT default null, ADRESSE TEXT default null)";
        SqliteCommand Command = new SqliteCommand(sql, dbConnection);
        Command.ExecuteNonQuery();
        dbConnection.Close();
        Debug.Log("Decoderdatabase Done");

        UpdateUpdate();
    }
}