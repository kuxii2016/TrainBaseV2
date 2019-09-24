/*
 * 
 *   TrainBase Settings Manager Version 1 from 25.03.2018 written by Michael Kux
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
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Threading;

public class SettingsManager : MonoBehaviour
{
    public LogWriterManager Logger;
    public ProgrammSettings Settings;
    public GameObject DebuggerIcon;
    public StartUpManager StartManager;
    public LokView Lokview;
    public WagonView Wagonview;
    [Header("DB-Werte")]
    public int AutoUpdateValue;
    public int LeistungsDatenValue;
    public int ZeigeLokbilderValue;
    public int ErstelleLokPriceValue;
    public int ErstelleWagonPriceValue;
    public int InventoryPriceList;
    public int DebuggerValue;
    public int ImagesAutoReadValue;
    public int RowImageType;
    public int WartungsInterVall;
    public string LastUpdate;
    public string Key;
    public int ImportRPC;
    public int ImportXML;
    public string DBProtokoll;
    [Header("DB-BOOLS")]
    public bool AutoUpdateBool = false;
    public bool LeistungsDatenBool = false;
    public bool ZeigeLokbilderBool = false;
    public bool ErstelleLokPriceBool = false;
    public bool ErstelleWagonPriceBool = false;
    public bool ErstelleInvenoryPriceBool = false;
    public bool DebuggerBool = true;
    public bool ImagesAutoReadBool = false;
    [Header("Setting-Toggels")]
    public Toggle AutoUpdate;
    public Toggle LeistungsDaten;
    public Toggle ZeigeLokbilder;
    public Toggle ErstelleLokPrice;
    public Toggle ErstelleWagonPrice;
    public Toggle ErstelleInvenoryPrice;
    public Toggle Debugger;
    public Toggle ImagesAutoRead;
    [Header("Settings-Texte")]
    public Text DBsize;
    public Text DBpath;
    public Text DBversion;
    public Text DBupdate;
    public Text ImageFolder;
    public Text ProgrammTyp;
    public Dropdown WIntervall;
    public Slider ImageSetting;
    public Text SliderText;
    public string ImageType = "";
    public string PicValue;
    public long TrainPicSize = 0;
    public long WaggonPicSize = 0;
    [Header("Newtwork Settings")]
    public Text LanAdress;
    public Text NetworkType;
    public InputField ServerIP;
    public string IP;
    [Header("Backupfunktion")]
    public GameObject BackUpMessage;
    public Text WarningMessage;
    public Button JA;
    public Button Nein;
    public int ImageCount = 1;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE User_Settings -> Message is Normal.");
            Logger.PrintLogEnde();
            Logger.PrintLog("MODUL User_Settings :: Load Programm Infos try to Enable.");
            Logger.PrintLog("MODUL User_Settings :: Set Data Current Version to: " + Settings.Version + " | Datenbankname: " + Settings.DatabasesName + " | UpdateURL: " + Settings.UpdateURL );
        }
        ReadSettings();
        DBInfo();
        GetIPAdress();

        if (Logger.logIsEnabled == true)
        {
            DebuggerIcon.gameObject.SetActive(true);
            Debugger.isOn = true;
        }
        else
        {
            DebuggerIcon.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (ImageSetting.value == 0)
        {
            ImageType = "png";
            SliderText.text = "png";
        }
        else
        {
            ImageType = "jpg";
            SliderText.text = "jpg";
        }
        CalculateImageSize();
    }

    public void ReadSettings()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Settings", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader[0].GetType() != typeof(DBNull))
                    {
                        AutoUpdateValue = reader.GetInt32(0);
                        if(AutoUpdateValue == 1)
                        {
                            AutoUpdateBool = true;
                            AutoUpdate.isOn = true;
                        }
                    }

                    if (reader[1].GetType() != typeof(DBNull))
                    {
                        ErstelleLokPriceValue = reader.GetInt32(1);
                        if(ErstelleLokPriceValue == 1)
                        {
                            ErstelleLokPriceBool = true;
                            ErstelleLokPrice.isOn = true;
                        }
                    }

                    if (reader[2].GetType() != typeof(DBNull))
                    {
                        ErstelleWagonPriceValue = reader.GetInt32(2);
                        if(ErstelleWagonPriceValue == 1)
                        {
                            ErstelleWagonPriceBool = true;
                            ErstelleWagonPrice.isOn = true;
                        }
                    }

                    if (reader[3].GetType() != typeof(DBNull))
                    {
                        DebuggerValue = reader.GetInt32(3);
                        if(DebuggerValue == 1)
                        {
                            DebuggerBool = true;
                            Debugger.isOn = true;
                            Logger.logIsEnabled = true;
                        }
                    }

                    if (reader[4].GetType() != typeof(DBNull))
                    {
                        ImagesAutoReadValue = reader.GetInt32(4);
                        if(ImagesAutoReadValue == 1)
                        {
                            ImagesAutoReadBool = true;
                            ImagesAutoRead.isOn = true;
                        }
                    }

                    if (reader[5].GetType() != typeof(DBNull))
                    {
                        InventoryPriceList = reader.GetInt32(5);
                        if(InventoryPriceList == 1)
                        {
                            ErstelleInvenoryPriceBool = true;
                            ErstelleInvenoryPrice.isOn = true;
                        }
                    }

                    if (reader[6].GetType() != typeof(DBNull))
                    {
                        LastUpdate = reader.GetString(6);
                        DBupdate.text = LastUpdate.ToString();
                    }

                    if (reader[7].GetType() != typeof(DBNull))
                    {
                        Key = reader.GetString(7);
                        ProgrammTyp.text = Key;
                    }

                    if (reader[8].GetType() != typeof(DBNull))
                    {
                        ImportRPC = reader.GetInt32(8);
                    }

                    if (reader[9].GetType() != typeof(DBNull))
                    {
                        ImportXML = reader.GetInt32(9);
                    }

                    if (reader[10].GetType() != typeof(DBNull))
                    {
                        DBProtokoll = reader.GetString(10);
                        DBversion.text = DBProtokoll;
                    }

                    if (reader[11].GetType() != typeof(DBNull))
                    {
                        ZeigeLokbilderValue = reader.GetInt32(11);
                        if(ZeigeLokbilderValue == 1)
                        {
                            ZeigeLokbilderBool = true;
                            ZeigeLokbilder.isOn = true;
                        }
                    }

                    if (reader[12].GetType() != typeof(DBNull))
                    {
                        RowImageType = reader.GetInt32(12);
                        ImageSetting.value = RowImageType;
                        if(RowImageType == 0)
                        {
                            ImageType = "png";
                            SliderText.text = "png";
                        }
                        if (RowImageType == 1)
                        {
                            ImageType = "jpg";
                            SliderText.text = "jpg";
                        }
                    }
                    
                    if (reader[13].GetType() != typeof(DBNull))
                    {
                        WartungsInterVall = reader.GetInt32(13);
                        WIntervall.value = WartungsInterVall;
                    }

                }
                Logger.PrintLog("MODUL User_Settings :: set: Update = " + AutoUpdateBool + " | LokListIcon = " + ZeigeLokbilderBool + " | GenerateLokPrice = " + ErstelleLokPriceBool + " | GenerateWagonPrice = " + ErstelleWagonPriceBool + " | GenerateItemPrice = " + ErstelleInvenoryPriceBool + " | Debugger = " + DebuggerBool + " | ImageAutoCheck = " + ImagesAutoReadBool + " | BilderType: " + ImageType + " | Wartungsintervall: " + (WartungsInterVall + 1) + " Jahr");
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL User_Settings ::  " + ex + "\n");
            }
        }
        dbConnection.Close();
        dbConnection = null;
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL User_Settings :: Setting successfully Loadet");
        }
    }

    public void GetIPAdress()
    {
        NetworkType.text = "Lan Verbindung";
        LanAdress.text = Network.player.ipAddress.ToString();
        IP = Network.player.ipAddress.ToString();
    }

    public void DBInfo()
    {
        var fileInfo = new System.IO.FileInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName);
        DBsize.text = (fileInfo.Length / 1000).ToString() + " kb";
        DBpath.GetComponent<Text>().text = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName);
    }

    public void SaveSettings()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Settings  SET AUTOCHECK = @AUTOCHECK, TRAINS = @TRAINS , WAGONS = @WAGONS , DATENSEND = @DATENSEND , SOUNDS = @SOUNDS , INVENTORY = @INVENTORY , LOKLISTICONS = @LOKLISTICONS, IMAGETYPE = @IMAGETYPE, WARTUNGSINTERVALL = @WARTUNGSINTERVALL" ;
            command.Parameters.AddWithValue("@AUTOCHECK", AutoUpdate.isOn);
            command.Parameters.AddWithValue("@TRAINS", ErstelleLokPrice.isOn);
            command.Parameters.AddWithValue("@WAGONS", ErstelleWagonPrice.isOn);
            command.Parameters.AddWithValue("@DATENSEND", Debugger.isOn);
            command.Parameters.AddWithValue("@SOUNDS", ImagesAutoRead.isOn);
            command.Parameters.AddWithValue("@INVENTORY", ErstelleInvenoryPrice.isOn);
            command.Parameters.AddWithValue("@LOKLISTICONS", ZeigeLokbilder.isOn);
            command.Parameters.AddWithValue("@IMAGETYPE", ImageSetting.value);
            command.Parameters.AddWithValue("@WARTUNGSINTERVALL", WIntervall.value);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Settings :: ERROR by Save Settings: " + ex + "\n");
                    Debug.Log(ex);
                }
            }
            finally
            {
                dbConnection.Close();
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Settings :: Save Finsch ");
                }
            }
        }
    }

    public void CreateBackup()
    {
        try
        {
            if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" + System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year))
            {
                BackUpMessage.gameObject.SetActive(true);
                WarningMessage.text = "Backup vom: " + System.DateTime.Now.Day + " - " + System.DateTime.Now.Month + " - " + System.DateTime.Now.Year + " " +
                    "Wurde bereits gefunden, Wollen sie das Backup überschreiben?";
                if (Logger.logIsEnabled == true)
                {
                    FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                    fs.Close();
                    StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                    sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL Backup :: Backup Exestiert Bereits. \n");
                    sw.Close();
                }
            }
            else
            {
                File.Copy(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName, System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" +  System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year);
                if (Logger.logIsEnabled == true)
                {
                    FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                    fs.Close();
                    StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                    sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL Backup :: Try to Create Database Backup. \n");
                    sw.Close();
                }
            }
        }
        catch(Exception ex)
        {
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL Backup :: Error by Create Database Backup: " +  ex + "\n \n");
                sw.Close();
            }
        }
        finally
        {
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL Backup :: Backup Created \n");
                sw.Close();
            }
        }
    }

    public void ReplaceOldBackup()
    {
        try
        {
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups/" +  System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year);
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL Backup :: Delete Old Backup. \n");
                sw.Close();
            }
        }
        catch (Exception ex)
        {
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL Backup :: Error beim Löschen des Alten Backups."  + ex + "\n \n" );
                sw.Close();
            }
        }
        finally
        {
            if (Logger.logIsEnabled == true)
            {
                FileStream fs = new FileStream(Logger.LogPfad + Logger.CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Logger.LogPfad + Logger.CurrentLogFile, true, Encoding.ASCII);
                sw.Write(System.DateTime.Now.Hour + ":" + System.DateTime.Now.Minute + ":" + System.DateTime.Now.Second + " " + "MODUL Backup :: Gelöscht \n");
                sw.Close();
            }
            CreateBackup();
            BackUpMessage.gameObject.SetActive(false);
            WarningMessage.text = "";
        }
    }

    public void SetIntervall()
    {
        Directory.CreateDirectory(Application.dataPath + "/" + "Config");
        if (!File.Exists(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub"))
        {
            FileStream fs = new FileStream(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub", FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub", true, Encoding.ASCII);
            sw.Write(WIntervall.value);
            sw.Close();
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Settings :: Set Wartungs Intewall @Value -> " + WIntervall.value);
            }
        }
        else
        {
            File.Delete(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub");
            FileStream fs = new FileStream(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub", FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(Application.dataPath + "/" + "Config" + "/" + "WartungsIntervall.pub", true, Encoding.ASCII);
            sw.Write(WIntervall.value);
            sw.Close();
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Settings :: Set Wartungs Intewall @Value -> " + WIntervall.value);
            }
        }
    }

    public void CalculateImageSize()
    {
        WaggonPicSize = DirSize(new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Wagons"));
        TrainPicSize = DirSize(new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains"));
        ImageFolder.text = ((WaggonPicSize + TrainPicSize) / 1000 / 1024).ToString() + " Mb";
    }

    public static long DirSize(DirectoryInfo p)
    {
        long size = 0;
        FileInfo[] fis = p.GetFiles();
        foreach (FileInfo fi in fis)
        {
            size += fi.Length;
        }
        DirectoryInfo[] dis = p.GetDirectories();
        foreach (DirectoryInfo di in dis)
        {
            size += DirSize(di);
        }
        return size;
    }

    public void DeleteImages()
    {
        for (int k = 0; k < 239; k++)
        {
            ImageCount = +k;
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/Trains/" + k + ".png");
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/Trains/" + k + ".jpg");
        }
        
        for (int i = 0; i < 239; i++)
        {
            ImageCount = +i;
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/Wagons/" + i + ".png");
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/Wagons/" + i + ".jpg");
        }
        Lokview.GetTrainPictures();
        Wagonview.GetTrainPictures();
        Logger.Message((ImageCount / 4).ToString() + " Bilder neu Erstellt", "LILA");
        ImageCount = 1;
        CalculateImageSize();
    }
}