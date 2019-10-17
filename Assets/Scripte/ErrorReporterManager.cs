/*
 * 
 *   TrainBase Error Report Manager Version 1 from 31.08.2018 written by Michael Kux
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
using System.IO.Compression;

public class ErrorReporterManager : MonoBehaviour {

    public LogWriterManager Logger;
    public ProgrammSettings SettingsManager;
    public string Errorfile = "";
    public string ErrorFolder = "";

    void Start ()
    {
        Errorfile = Logger.LogPfad + Logger.CurrentLogFile;
        ErrorFolder = (System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error").ToString();
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Error Reporter Manager -> Message is Normal.");
        }
    }

    public void CreateErrorReport()
    {
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error");
            File.Copy(Logger.LogPfad + Logger.CurrentLogFile, (System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error/" + "last.log"));
            File.Copy((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + SettingsManager.DatabasesName), (System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error/" + "db.dll"));
            Logger.PrintLog("MODUL Error_Reporter_Manager :: Report Created, Send this on 'kux.michael2@googlemail.com'");
        }
    }
}