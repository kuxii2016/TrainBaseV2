using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Diagnostics;
using UnityEngine.UI;
using UnityEngine;
using Microsoft.Win32;
using System.Text;
using System.Net;

public class Start_Manager : MonoBehaviour
{

    [Header("Programm-Settings")]
    public string ProgrammVersion;
    public string UpdateUrl;
    public string WebExporterUrl;
    public string StatsUrl;
    public string KeyURL;
    public string AVGPriceURL;
    public string ErrorURL;
    public string Language;
    public string ConfigLanguage;
    public string CopyrightLine;
    public string UserUUID;
    public string DonateKey = "0";
    public string DonateDate = "0";
    public string LogPath;
    public int MessageTime = 15;
    public bool ProgrammIsInEditorMode;
    public bool WriteLog;
    public bool WriteError;
    public bool UpdateAvailable;
    public bool AutoDedectLanguage;
    public bool IsGerman;
    public bool IsEnglisch;
    public bool Premium = false;
    public bool AutoError = false;
    private config configData;
    [Header("Haupt-Panel-Elements")]
    public Text Copyright;
    public Text Version;
    public Text SystemMessage;
    public Image NotifyIcon;
    public Image DebugIcon;
    Color ColorParser;
    Color ColorParserIcon;
    public GameObject ErrorView;
    public Text ErrorTyp;
    public Text ErrorDSC;
    public string Send = "Aus";
    [Header("Verlauf-Panel")]
    public Text History;

    void Start()
    {
        configData = new config();
        LogPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "Logs/";
        CheckFolder();
        CheckKey();
        GetLanguage();
        StartCoroutine(GetKey());
        LogRotate();
        SetScreen();
        Log("Lade Start_Manager -> Nachricht ist Normal.", "Load Start_Manager -> message is normal");
        Notify("TrainBaseV2 gestartet und Bereit ", "TrainBaseV2 Started and ready", "green", "green");
        if(Application.isEditor == true)
        {
            ProgrammIsInEditorMode = true;
            Log("Modul Start_Manager :: Programm ist im Editor Mode, Keine Statistik, Update Check Verfügbar.", "Modul Start_Manager :: Programm is im Editor Mode, No Statistik, Update Check");
        }

    }

    void GetLanguage()
    {
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "config.xml"))
        {
            if (Application.systemLanguage == SystemLanguage.German)
            {
                IsGerman = true;
                IsEnglisch = false;
                AutoDedectLanguage = true;
            }
            if (Application.systemLanguage != SystemLanguage.German)
            {
                IsEnglisch = true;
                IsGerman = false;
                AutoDedectLanguage = true;
            }
            configData.IsGerman = IsGerman;
            configData.IsEnglisch = IsEnglisch;
            configData.AutoDedectLanguage = AutoDedectLanguage;
            configData.WriteErrorLog = WriteError;
            configData.WriteLog = WriteLog;
            configData.AutoError = AutoError;
            string jsonData = JsonUtility.ToJson(configData, true);
            File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "config.xml", jsonData);
        }
        else
        {
            LoadConfig();
        }
    }

    public void LogError(string MessageDE, string MessageEN, string FileMessage)
    {
        if (WriteError == true)
        {
            FileStream fs = new FileStream(LogPath + "error.log", FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(LogPath + "error.log", true, Encoding.ASCII);
            sw.Write(DateTime.Now.ToString("HH:mm - dd/MM/yyyy ") + "***FATAL*** " + FileMessage + " \n");
            sw.Close();
        }
        StartCoroutine(StatusMessage(MessageEN, MessageDE, "red", "red"));
    }

    public void Log(string MessageDE, string MessageEN)
    {
        if (WriteLog == true)
        {
            if (IsGerman == true)
            {
                FileStream fs = new FileStream(LogPath + "last.log", FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(LogPath + "last.log", true, Encoding.ASCII);
                sw.Write(DateTime.Now.ToString("HH:mm ") + MessageDE + " \n");
                sw.Close();
            }
            else
            {
            }

            if (IsEnglisch == true)
            {
                FileStream fs = new FileStream(LogPath + "last.log", FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(LogPath + "last.log", true, Encoding.ASCII);
                sw.Write(DateTime.Now.ToString("HH:mm ") + MessageEN + " \n");
                sw.Close();
            }
            else
            {
            }
        }
    }

    public void Notify(string MessageDE, string MessageEN, string SystemColor, string NotifyColor)
    {
        StartCoroutine(StatusMessage(MessageEN, MessageDE, SystemColor, NotifyColor));
    }

    void CheckFolder()
    {
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Wagons"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/" + "Images/" + "Wagons");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Inventory"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/" + "Images/" + "Inventory");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Backups");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Logs"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Logs");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"))
        {
            File.Copy(Application.streamingAssetsPath + "/Resources/clear.db", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Exporter"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Exporter");
        }

        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Importer"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Importer");
        }
    }

    void LogRotate()
    {
        if (File.Exists(LogPath + "OLD-Log"))
        {
            File.Delete(LogPath + "OLD-Log");
        }

        if (File.Exists(LogPath + "last.log"))
        {
            File.Copy(LogPath + "last.log", LogPath + "OLD-Log");
            File.Delete(LogPath + "last.log");
            FileStream fs = new FileStream(LogPath + "last.log", FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(LogPath + "last.log", true, Encoding.ASCII);
            sw.Close();
        }
        else
        {
            FileStream fs = new FileStream(LogPath + "last.log", FileMode.Append, FileAccess.Write, FileShare.Write);
            fs.Close();
            StreamWriter sw = new StreamWriter(LogPath + "last.log", true, Encoding.ASCII);
            sw.Close();
        }
    }

    IEnumerator StatusMessage(string text0, string text1, string pcolor, string nColor)
    {
        if (pcolor == "red")
        {
            ColorParser = Color.red;
        }
        if (pcolor == "green")
        {
            ColorParser = Color.green;
        }
        if (pcolor == "yellow")
        {
            ColorParser = Color.yellow;
        }
        if (pcolor == "magenta")
        {
            ColorParser = Color.magenta;
        }
        if (pcolor == "cyan")
        {
            ColorParser = Color.cyan;
        }
        if (pcolor == "blue")
        {
            ColorParser = Color.blue;
        }
        if (pcolor == "black")
        {
            ColorParser = Color.black;
        }
        if (pcolor == "gray")
        {
            ColorParser = Color.gray;
        }
        if (pcolor == "white")
        {
            ColorParser = Color.white;
        }

        if (nColor == "red")
        {
            ColorParserIcon = Color.red;
        }
        if (nColor == "green")
        {
            ColorParserIcon = Color.green;
        }
        if (nColor == "yellow")
        {
            ColorParserIcon = Color.yellow;
        }
        if (nColor == "magenta")
        {
            ColorParserIcon = Color.magenta;
        }
        if (nColor == "cyan")
        {
            ColorParserIcon = Color.cyan;
        }
        if (nColor == "blue")
        {
            ColorParserIcon = Color.blue;
        }
        if (nColor == "black")
        {
            ColorParserIcon = Color.black;
        }
        if (nColor == "gray")
        {
            ColorParserIcon = Color.gray;
        }
        if (nColor == "white")
        {
            ColorParserIcon = Color.white;
        }
        if (nColor == "clear")
        {
            ColorParserIcon = Color.clear;
        }
        if (IsGerman == true)
        {
            NotifyIcon.color = ColorParserIcon;
            SystemMessage.color = ColorParser;
            SystemMessage.text = text1;
        }
        else
        {
            //Pah tu Nix
        }
        if (IsEnglisch == true)
        {
            NotifyIcon.color = ColorParserIcon;
            SystemMessage.color = ColorParser;
            SystemMessage.text = text0;
        }
        else
        {
            //Pah tu Nix
        }
        yield return new WaitForSeconds(MessageTime);
        if (IsGerman == true)
        {
            SystemMessage.color = Color.black;
            NotifyIcon.color = Color.clear;
            SystemMessage.text = "";
        }
        else
        {
            //Pah tu Nix
        }
        if (IsEnglisch == true)
        {
            SystemMessage.color = Color.black;
            NotifyIcon.color = Color.clear;
            SystemMessage.text = "";
        }
        else
        {
            //Pah tu Nix
        }
    }

    void CheckKey()
    {
        RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\TrainBaseV2");
        if (key != null)
        {
            UserUUID = key.GetValue("uuid").ToString();
            DonateDate = key.GetValue("date").ToString();
            DonateKey = key.GetValue("key").ToString();
            Language = key.GetValue("lang").ToString();
        }
        else
        {
            StartCoroutine(RegisterNewUser());
            RegisterKey();
        }
    }

    void RegisterKey()
    {
        UserUUID = Guid.NewGuid().ToString();
        Language = Application.systemLanguage.ToString();
        RegistryKey newkey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\TrainBaseV2");
        newkey.SetValue("key", "0000-0000-0000");
        newkey.SetValue("date", "0");
        newkey.SetValue("uuid", UserUUID);
        newkey.SetValue("lang", Language);
        newkey.Close();
    }

    void LoadConfig()
    {
        configData = JsonUtility.FromJson<config>(File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "config.xml"));
        IsGerman = configData.IsGerman;
        IsEnglisch = configData.IsEnglisch;
        WriteError = configData.WriteErrorLog;
        WriteLog = configData.WriteLog;
        MessageTime = configData.MessageTimer;
        WriteLog = configData.WriteLog;
        WriteError = configData.WriteErrorLog;
        AutoDedectLanguage = configData.AutoDedectLanguage;
        AutoError = configData.AutoError;
    }

    void SetScreen()
    {
        Copyright.text = CopyrightLine;
        Version.text = "Version: " + ProgrammVersion;
        if (WriteLog == false)
        {
            DebugIcon.color = Color.red;
        }
        else
        {
            DebugIcon.color = Color.green;
        }
    }

    public void GetHistory()
    {
        StartCoroutine(GetWebHistory());
    }

    private IEnumerator GetWebHistory()
    {
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        Dictionary<string, string> ht = new Dictionary<string, string>();
        ht["User-Agent"] = userAgent;
        {
            WWW www = new WWW("https://pastebin.com/raw/9EvbqpqQ", null, ht);
            yield return www;
            if (www.error != null)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                History.text = www.text;
            }
        }
    }

    IEnumerator GetKey()
    {
        WWW read = new WWW("http://" + KeyURL + "key" + "/check.php?uuid=" + UserUUID);
        yield return read;

        if(read.error != null)
        {
            Log("Modul Settings_Manager :: Programmversion Kann nicht Geprueft werden, Internet Verbunden?", "Modul Settings_Manager :: Can not Check Key, Internet Connection?.");
        }
        else
        {
            RegistryKey newkey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\\TrainBaseV2");
            if (newkey != null)
            {
                string strOne = read.text;
                string[] strArrayOne = new string[] { "" };
                strArrayOne = strOne.Split(',');
                DonateKey = strArrayOne[0];
                DonateDate = strArrayOne[1];
                newkey.SetValue("key", strArrayOne[0], RegistryValueKind.String);
                newkey.SetValue("date", strArrayOne[1], RegistryValueKind.String);
                newkey.Close();
                DateTime date1 = new DateTime((DateTime.Now.AddDays(Int32.Parse(DonateDate))).Year, (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Month, (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Day);
                DateTime date2 = DateTime.Today;
                int daysDiff = ((TimeSpan)(date1 - date2)).Days;
                Log("Modul Settings_Manager :: Regestrierte Version Key bis: " + (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Day + "." + (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Month + "." + (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Year + " | " + daysDiff + " Tage Verbleibend", "Modul Settings_Manager :: Regestrated Version Key up: " + (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Day + "." + (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Month + "." + (DateTime.Now.AddDays(Int32.Parse(DonateDate))).Year + " | " + daysDiff + " Days Remaining");
                if(daysDiff <= 10)
                {
                    Log("Modul Settings_Manager :: Version Lauft in: " + daysDiff + " ab, ab dann steht nur noch das Programm mit der Grundversion zur Verfügung, Key kann Verlängert werden.!", "Modul Settings_Manager :: Version Runs Into: " + daysDiff + " From then on, only the program with the basic version is available, Key can be extended.!");
                }
                if (daysDiff <= 0)
                {
                    Premium = false;
                }
                else
                {
                    Premium = true;
                }
            }
        }
        if (ProgrammIsInEditorMode == false)
        {
            StartCoroutine(RegisterNewStart());
        }
    }

    private IEnumerator RegisterNewUser()
    {
        string FinshURL = "http://" + KeyURL +"/" + "sysstats" + "/insert.php?CPU=" + (SystemInfo.processorType + "@ " + (SystemInfo.processorFrequency / 1000) + " Ghz") + "&GPU=" + (SystemInfo.graphicsDeviceName + " & " + (SystemInfo.graphicsMemorySize + 1024) / 1024 + " GB") + "&RAM=" + ( (SystemInfo.systemMemorySize + 1024) / 1024 + " GB") + "&OS=" + (SystemInfo.operatingSystem);
        WWW insert = new WWW(FinshURL);
        UnityEngine.Debug.Log(FinshURL);
        yield return insert;

        if (insert.error != null)
        {
            LogError("Keine Verbindung zum Server möglich!.", "No Connection to the Server.", " Start_Manager :: RegisterNewUser(); Error: " + insert.error);
        }
        if (insert.isDone)
        {
        }
    }

    private IEnumerator RegisterNewStart()
    {
        string FinshURL = "http://" + KeyURL + "/" + "sysstats" + "/start.php?os=" + SystemInfo.operatingSystemFamily + "&version=" + ProgrammVersion + "&uuid=" + UserUUID + "&premium=" + Premium;
        WWW insert = new WWW(FinshURL);
        UnityEngine.Debug.Log(FinshURL);
        yield return insert;

        if (insert.error != null)
        {
            LogError("Keine Verbindung zum Server möglich!.", "No Connection to the Server.", " Start_Manager :: RegisterNewStart(); Error: " + insert.error);
        }
        if (insert.isDone)
        {
        }
    }

    public void Error(string Activity, string Error)
    {
        if(AutoError == true)
        {
            Send = "AN";
            StartCoroutine(SendError(Activity, Error));
        }
        else
        {
            Send = "AUS";
        }
        ErrorTyp.text = "Fehler: " + Activity;
        ErrorDSC.text = "Fehler wird Automatisch Gemeldet: " + Send + "\n \n" + Error;
        ErrorView.gameObject.SetActive(true);
    }

    private IEnumerator SendError(string Error, string Activity)
    {
        string FinshURL = "http://" + ErrorURL + "error" + "/insert.php?error=" + Activity + "&data=" + Error + " ";
        WWW insert = new WWW(FinshURL);

        yield return insert;

        if (insert.error != null)
        {
            LogError("Keine Verbindung zum Server möglich!.", "No Connection to the Server.", " Train_List :: SendSelected(); Error: " + insert.error);
        }
    }
}