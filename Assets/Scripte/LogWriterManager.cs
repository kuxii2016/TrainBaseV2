/*
 * 
 *   TrainBase LogWriter Manager Version 1 from 30.03.2018 written by Michael Kux
 *    *   Last Edit 31.08.2018
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

public class LogWriterManager : MonoBehaviour {

    public ProgrammSettings loader;
    [Header("Writer Version")]
    public string WriterVersion = "2.0.2";
    [Tooltip("LogPfad")]
    public string LogPfad = "";
    public string FidexLogPfad;
    [Tooltip("Ángeschalted")]
    public bool logIsEnabled = true;
    [Tooltip("LogFileString")]
    public string CurrentLogFile = "";
    public string LogEndingLine = "KtXb";
    [Tooltip("LogfilePfad")]
    public Text  LogDirectory;
    [Header("Log Window Elements")]
    public Text LogPfadView;
    public GameObject LogWindow;
    public Text LogTextLine;
    public Scrollbar TextScrol;
    public string line1 = " |---------------------------------------|";
    public string line2 = "  TrainBase V2 Modellbahn Katalog";
    public string line3 = "  Custom Entwickler Log";
    public string line4 = "  This file is for Error reporting";
    public string line5 = "  (C)2018 - 2019 by KuxiiSoft";
    public string line6 = " |---------------------------------------|";
    public string line7 = "";
    public string UserCounter = "";
    public string WinUser = "";
    public string UnixUser = "";
    public string SonstUser = "";
    public string OS = "";
    public bool RotateLog = false;
    public Image pMessageImage;
    public Text pMessage;
    Color ColorParser;

    void Start()
    {
        LogPfad = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "Logs/";
        CurrentLogFile = "current" + "." + LogEndingLine;
        FidexLogPfad = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Logs/" + CurrentLogFile;
        LogRotate();
    }

    void Update()
    {
        if(logIsEnabled == true && Input.GetKeyDown(KeyCode.F12))
        {
            ReadInput();
            LogWindow.gameObject.SetActive(true);
        }
    }

    public void CreateDebug()
    {
        FileStream fs = new FileStream(FidexLogPfad, FileMode.Append, FileAccess.Write, FileShare.Write);
        fs.Close();
        StreamWriter sw = new StreamWriter(FidexLogPfad, true, Encoding.ASCII);
        sw.Write(" " + line1 + "\n");
        sw.Write(" " + line2 + "\n");
        sw.Write(" " + line3 + "\n");
        sw.Write(" " + line4 + "\n");
        sw.Write(" " + line5 + "\n");
        sw.Write(" " + line6 + "\n");
        sw.Write(" " + " Programm run on ah: " + "\n");
        sw.Write(" " + " Running OS: " + SystemInfo.operatingSystem + "\n");
        sw.Write(" " + " CPU: " + SystemInfo.processorType + " ~" + (SystemInfo.processorFrequency / 1000) + " GhZ" + "\n");
        sw.Write(" " + " Max Ram : " + SystemInfo.systemMemorySize + " MB" + "\n");
        sw.Write(" " + " GPU Driver: " + SystemInfo.graphicsDeviceName + "\n");
        sw.Write(" " + " 3D Beschleunigung: " + SystemInfo.graphicsDeviceType + "\n");
        sw.Write(" " + " Shader Level: " + SystemInfo.graphicsShaderLevel + "\n");
        sw.Write(" " + " GPU Ram: " + SystemInfo.graphicsMemorySize + " MB" + "\n");
        sw.Write(" " + " SLI or Crossfire: " + SystemInfo.graphicsMultiThreaded + "\n");
        sw.Write("\n");
        sw.Close();
        PrintLog("ENABLE LogWriter_Manager -> Message is Normal.");
        if (RotateLog == true)
        {
            PrintLog("MODUL LogWriter_Manager :: Clear Old Log dumps,and Files.");
        }
        if (RotateLog == false)
        {
            PrintLog("MODUL LogWriter_Manager :: Start a new Logfile Writer.");
        }
    }

    public void ReadInput()
    {
        LogTextLine.text = File.ReadAllText(FidexLogPfad);
    }
    
    public void PrintLog(string log)
    {
        FileStream fs = new FileStream(LogPfad + CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
        fs.Close();
        StreamWriter sw = new StreamWriter(LogPfad + CurrentLogFile, true, Encoding.ASCII);
        sw.Write(DateTime.Now.ToString("HH:mm:ss : ") + log + " \n");
        sw.Close();
    }

    public void LogRotate()
    {
        if (File.Exists(LogPfad + "OLD-Log" + "." + "dump"))
        {
            File.Delete(LogPfad + "OLD-Log" + "." + "dump");
        }

        if (File.Exists(LogPfad + "current" + "." + LogEndingLine))
        {
            File.Copy(LogPfad + "current" + "." + LogEndingLine, LogPfad + "OLD-Log" + "." + "dump");
            File.Delete(LogPfad + "current" + "." + LogEndingLine);
            RotateLog = true;
        }
        else
        {
            RotateLog = false;
            FileStream fs = new FileStream(FidexLogPfad, FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(FidexLogPfad, true, Encoding.ASCII);
            sw.Close();
        }
        CreateDebug();
    }

    public void PrintLogEnde()
    {
        FileStream fs = new FileStream(LogPfad + CurrentLogFile, FileMode.Append, FileAccess.Write, FileShare.Write);
        fs.Close();
        StreamWriter sw = new StreamWriter(LogPfad + CurrentLogFile, true, Encoding.ASCII);
        sw.Write("\n");
        sw.Close();
    }

    public void Message(string text, string color)
    {
        string ptext = text;
        string pcolor = color;
        StartCoroutine(StatusMessage(ptext, pcolor));
    }

    IEnumerator StatusMessage(string text, string pcolor)
    {
        if(pcolor == "ROT")
        {
            ColorParser = Color.red;
        }
        if (pcolor == "GRUEN")
        {
            ColorParser = Color.green;
        }
        if (pcolor == "GELB")
        {
            ColorParser = Color.yellow;
        }
        if (pcolor == "LILA")
        {
            ColorParser = Color.magenta;
        }
        if (pcolor == "CYAN")
        {
            ColorParser = Color.cyan;
        }
        if (pcolor == "BLAU")
        {
            ColorParser = Color.blue;
        }
        if (pcolor == "SCHWARTZ")
        {
            ColorParser = Color.black;
        }
        pMessageImage.color = ColorParser;
        pMessage.color = ColorParser;
        pMessage.text = text;
        yield return new WaitForSeconds(10);
        pMessageImage.color = Color.white;
        pMessage.color = Color.black;
        pMessage.text = "";
    }
}