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

public class Settings_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Train_List trainList;
    [Header("Buttons")]
    public Button ReCreateTrainImages;
    public Button ReCreateWagonImages;
    public Button RefreshTrainImages;
    public Button RefreshWagonImages;
    public Button CheckVersion;
    public Button SaveSettings;
    [Header("Outputs")]
    public Text DBInfos;
    public Text DBSize;
    public Text DBUpdate;
    public Text DBProtokoll;
    public Text DBPath;
    public Text DBVersion;
    public Text KeyInfos;
    public Text KeyNumber;
    public Text KeyDate;
    public Text ProgrammType;
    public InputField UUID;
    public Text Sonstiges;
    public Text ImageFolder;
    public Text IPAdress;
    [Header("User-Input")]
    public Toggle WriteError;
    public Toggle WriteLog;
    public Toggle DedectLang;
    public Toggle AutoUpdateCheck;
    public Toggle ListenBilder;
    public Toggle PreisLoks;
    public Toggle PreisWagons;
    public Toggle PreisInventar;
    public Toggle AnnonPrice;
    public Toggle Autosync;
    public Toggle AutoReport;
    public Dropdown Lang;
    public Dropdown ImageTyp;
    public Dropdown Wartungs;
    public InputField MessageTimer;
    public Text WartungColorText;
    public Text NormalColorText;
    public Text MessageTimerText;
    public Text LangText;
    public Text ImageText;
    public Text WartungText;
    public InputField SenderIP;
    [Header("Color-Settings")]
    public InputField WartungColor;
    public InputField NonWartungColor;
    public Image WartungColorPic;
    public Image NonWartungColorPic;
    public Color newCol0;
    public Color newCol1;
    public string htmlValue0;
    public string htmlValue1;
    DateTime left;
    public int Maintenance;
    public string Language;
    public string ImageType = "png";
    public long TrainPicSize = 0;
    public long WaggonPicSize = 0;
    public bool Premium = false;
    private config configData;
    public string DB_Protokoll;
    public string DB_Update;
    int Message = 1;
    [Header("Display")]
    public Toggle Fullscreen;
    public Dropdown Resulotion;
    public Text Displaylabel;
    public Text resolutionText;
    public Text ToggleText;
    public Resolution[] resolutions;
    public bool IsLoadet = false;

    void Start ()
    {
        configData = new config();
        if (startManager.IsGerman)
        {
            Language = "German";
            AnnonPrice.GetComponentInChildren<Text>().text = "Anonyme Preis erfassung";
            Displaylabel.text = "Display Einstellungen";
            resolutionText.text = "Auflösung:";
            ToggleText.text = "Vollbild";

            WriteError.GetComponentInChildren<Text>().text = "Erstelle Fehler in einer Log Datei";
            WriteLog.GetComponentInChildren<Text>().text = "Erstelle System Log Files";
            DedectLang.GetComponentInChildren<Text>().text = "Erkenne Sprache Automatisch";
            AutoUpdateCheck.GetComponentInChildren<Text>().text = "Automatische Überprüfung auf neue Updates";
            ListenBilder.GetComponentInChildren<Text>().text = "Zeige Eigene Bilder in den Listen";
            PreisLoks.GetComponentInChildren<Text>().text = "Erstelle Lok Preis-Liste";
            PreisWagons.GetComponentInChildren<Text>().text = "Erstelle Wagon Preis-Liste";
            PreisInventar.GetComponentInChildren<Text>().text = "Erstelle Inventar Preis-Liste";

            WartungColorText.text = "Wartung - Color";
            NormalColorText.text = "Normal - Color";
            MessageTimerText.text = "Nachrichten Timer";
            LangText.text = "Sprache";
            ImageText.text = "Bilder Typ";
            WartungText.text = "Wartungs Intervall";

        }
        else
        {
            Language = "Englisch";
            ReCreateTrainImages.GetComponentInChildren<Text>().text = "Create Stantart Loco Pictures";
            ReCreateWagonImages.GetComponentInChildren<Text>().text = "Create Wagon Pictures";
            RefreshTrainImages.GetComponentInChildren<Text>().text = "Read Loco Images new";
            RefreshWagonImages.GetComponentInChildren<Text>().text = "Read Wagon Images new";
            CheckVersion.GetComponentInChildren<Text>().text = "Check for ah new Version";
            SaveSettings.GetComponentInChildren<Text>().text = "Save Settings";
            AnnonPrice.GetComponentInChildren<Text>().text = "Anonym Price entry";
            Displaylabel.text = "Display Settings";
            resolutionText.text = "Resolution:";
            ToggleText.text = "Fullscreen";

            WriteError.GetComponentInChildren<Text>().text = "Create errors in a Log File";
            WriteLog.GetComponentInChildren<Text>().text = "Create System Log File";
            DedectLang.GetComponentInChildren<Text>().text = "Recognize language automatically";
            AutoUpdateCheck.GetComponentInChildren<Text>().text = "Automatic check for new Updates";
            ListenBilder.GetComponentInChildren<Text>().text = "Show own Pictures in the lists";
            PreisLoks.GetComponentInChildren<Text>().text = "Create Lok price list";
            PreisWagons.GetComponentInChildren<Text>().text = "Create Wagon Price List";
            PreisInventar.GetComponentInChildren<Text>().text = "Create Inventory price list";

            WartungColorText.text = "Maintenance - Color";
            NormalColorText.text = "Normal - Color";
            MessageTimerText.text = "News Timer";
            LangText.text = "Language";
            ImageText.text = "Pictures type";
            WartungText.text = "Maintenance Interval";
        }
        startManager.Log("Lade Settings_Manager -> Nachricht ist Normal.", "Load Settings_Manager -> message is normal");
        CalculateImageSize();
        ReadSettings();
        LoadSettings();
        SetOutput();
    }
	
	void Update ()
    {
        if(ImageTyp.value == 0)
        {
            ImageType = "png";
        }
        else
        {
            ImageType = "jpg";
        }
        htmlValue0 = "#" + ColorUtility.TryParseHtmlString(WartungColor.text, out newCol0);
        htmlValue1 =  "#" + ColorUtility.TryParseHtmlString(NonWartungColor.text, out newCol1);
        WartungColorPic.color = newCol0;
        NonWartungColorPic.color = newCol1;
}

    private void OnEnable()
    {
        Resulotion.onValueChanged.AddListener(delegate { ResulotiononValueChanged(); });
        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            Resulotion.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }
    }

    public void ResulotiononValueChanged()
    {
        Screen.SetResolution(resolutions[Resulotion.value].width, resolutions[Resulotion.value].height, Screen.fullScreen);
        configData.Resolution = Resulotion.value;
    }

    public void LoadSettings()
    {
        if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "config.xml"))
        {
            configData = JsonUtility.FromJson<config>(File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "config.xml"));
            DedectLang.isOn = configData.AutoDedectLanguage;
            if (configData.IsGerman == true)
            {
                Lang.value = 0;
            }
            else
            {
                Lang.value = 1;
            }
            AutoReport.isOn = configData.AutoError;
            Autosync.isOn = configData.Autosync;
            AnnonPrice.isOn = configData.AVGPrice;
            WriteError.isOn = configData.WriteErrorLog;
            WriteLog.isOn = configData.WriteLog;
            AutoUpdateCheck.isOn = configData.UpdateCheck;
            ListenBilder.isOn = configData.ListImages;
            PreisLoks.isOn = configData.TrainPrice;
            PreisWagons.isOn = configData.WagonPrice;
            PreisInventar.isOn = configData.InventoryPrice;
            ImageTyp.value = configData.ImageTyp;
            Wartungs.value = configData.Maintenance;
            Maintenance = (configData.Maintenance) + 1;
            MessageTimer.text = configData.MessageTimer.ToString();
            WartungColor.text = configData.MaintenanceColor;
            NonWartungColor.text = configData.NonMaintenanceColor;
            Fullscreen.isOn = configData.FullScreen;
            Resulotion.RefreshShownValue();
            IsLoadet = true;
        }
        else
        {
            SaveSettingsBTN();
        }
    }

    public void ReadSettings()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Settings", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader[6].GetType() != typeof(DBNull))
                    {
                        DB_Update = reader.GetString(6);
                    }
                    if (reader[10].GetType() != typeof(DBNull))
                    {
                        DB_Protokoll = reader.GetString(10);
                    }
                }
                //startManager.Log("Modul Settings_Manager ::" + AutoUpdateBool + " | ZeigeBilder = " + ZeigeLokbilderBool + " | ErstelleLokPreis = " + ErstelleLokPriceBool + " | ErstelleWagonPreis = " + ErstelleWagonPriceBool + " | ErstelleItemPreis = " + ErstelleInvenoryPriceBool + " | Debugger = " + DebuggerBool + " | BilderAutomatischPrüfen = " + ImagesAutoReadBool + " | BilderType: " + ImageType + " | Wartungsintervall: " + (WartungsInterVall + 1) + " Jahr", ("Modul Settings_Manager :: AutoUpdateCheck: " + AutoUpdateBool + " | LokListIcon = " + ZeigeLokbilderBool + " | GenerateLokPrice = " + ErstelleLokPriceBool + " | GenerateWagonPrice = " + ErstelleWagonPriceBool + " | GenerateItemPrice = " + ErstelleInvenoryPriceBool + " | Debugger = " + DebuggerBool + " | ImageAutoCheck = " + ImagesAutoReadBool + " | ImageType: " + ImageType + " | Maintenance: " + (WartungsInterVall + 1) + " Years"));
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            startManager.LogError("Fehler beim Laden der Einstellungen.", "Error Loading Settings Data", " Settings_Manager :: ReadSettings; Error: " + ex);
            startManager.Error("ReadDatabase(Settings_Manager);", "" + ex);
        }
        dbConnection.Close();
        dbConnection.Dispose();
        dbConnection = null;
    }

    public void SetOutput()
    {
        var fileInfo = new System.IO.FileInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db");
        DateTime date1 = new DateTime((DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Year, (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Month, (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Day);
        DateTime date2 = DateTime.Today;
        int daysDiff = ((TimeSpan)(date1 - date2)).Days;
        if (daysDiff <= 0)
        {
            ProgrammType.text = "Version: Public/Free";
            Premium = false;
        }
        else
        {
            ProgrammType.text = "Version: Supporter/Donated";
            Premium = true;
        }

        if (Language == "German")
        {
            DBInfos.text = "Datenbank Information";
            DBSize.text = "Größe: " + (fileInfo.Length / 1000).ToString() + " kb";
            DBUpdate.text = "";
            DBProtokoll.text = "";
            DBPath.text = "Pfad: " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/" + "Database");
            KeyInfos.text = "Donation Key Info";
            if (daysDiff <= 0)
            {
                KeyDate.text = "Key Ablaufdatum:  ";
                KeyNumber.text = "Key: ";
            }
            else
            {
                KeyDate.text = "Key Ablaufdatum: " + (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Day + "." + (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Month + "." + (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Year + " :  " + daysDiff + " Tage";
                KeyNumber.text = "Key: " + startManager.DonateKey;
            }
            UUID.text = "Serialnummer: " + startManager.UserUUID.ToString();
            ImageFolder.text = "Bilder Ordner Größe: " + ((WaggonPicSize + TrainPicSize) / 1000 / 1024).ToString() + " Mb";
            IPAdress.text = "IP Adresse: " + Network.player.ipAddress.ToString();
            Sonstiges.text = "Sonstige";
        }
        else
        {
            DBInfos.text = "Database Information";
            DBSize.text = "Size: " + (fileInfo.Length / 1000).ToString() + " kb";
            DBUpdate.text = "";
            DBProtokoll.text = "";
            DBPath.text = "Path: " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/" + "Database");
            KeyInfos.text = "Donation Key Info";
            if (daysDiff <= 0)
            {
                KeyDate.text = "Key Expiry Date:  ";
                KeyNumber.text = "Key: ";
            }
            else
            {
                KeyDate.text = "Key Expiry Date: " + (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Day + "." + (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Month + "." + (DateTime.Now.AddDays(Int32.Parse(startManager.DonateDate))).Year + " :  " + daysDiff + " Days";
                KeyNumber.text = "Key: " + startManager.DonateKey;
            }
            UUID.text = "Serialnumber: " + startManager.UserUUID.ToString();
            ImageFolder.text = "Pictures Folder Size: " + ((WaggonPicSize + TrainPicSize) / 1000 / 1024).ToString() + " Mb";
            IPAdress.text = "IP Adress: " + Network.player.ipAddress.ToString();
            Sonstiges.text = "Other";
        }
        startManager.Log("Modul Settings_Manager :: Einstellungen Geladen set: ERROR:"+WriteError.isOn + " | LOG:" + WriteLog.isOn + " | AutoLang:" + startManager.AutoDedectLanguage + " | AutoUpdate:" + AutoUpdateCheck.isOn + " | EigeneBilder:" + ListenBilder.isOn + " | LokPreise:" + PreisLoks.isOn + " | WagonPreise:" + PreisWagons.isOn + " | InventarPreise:" + PreisInventar.isOn + " | UpdateURL:" + startManager.UpdateUrl + " | KeyURL:" + startManager.KeyURL + " ", "Modul Settings_Manager :: Settings Loadet set: ERROR:" + WriteError.isOn + " | LOG:" + WriteLog.isOn + " | AutoLang:" + startManager.AutoDedectLanguage + " | AutoUpdate:" + AutoUpdateCheck.isOn + " | EigeneBilder:" + ListenBilder.isOn + " | LokPreise:" + PreisLoks.isOn + " | WagonPreise:" + PreisWagons.isOn + " | InventarPreise:" + PreisInventar.isOn + " | UpdateURL:" + startManager.UpdateUrl + " | KeyURL:" + startManager.KeyURL + " ");

        DBProtokoll.text = "Protokoll: " + DB_Protokoll;
        DBUpdate.text = "Update: " + DB_Update;
    }

    public void ReCreateTrainPic()
    {
        for (int i = 0; i < trainList.Trains.Count; i++)
        {
            //Remove Train PNG Images
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + "png"))
            {
                File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + "png");
            }

            //Remove Train JPG Images
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + "jpg"))
            {
                File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (i + 1) + "." + "jpg");
            }
        }
    }

    public void ReCreateWagonPic()
    {
        for (int i = 0; i < trainList.Trains.Count; i++)
        {
            //Remove Wagon PNG Images
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + "png"))
            {
                File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + "png");
            }

            //Remove Wagon JPG Images
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + "jpg"))
            {
                File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (i + 1) + "." + "jpg");
            }
        }
    }

    public void RefreshTrainPic()
    {
        trainList.RefreshIndex();
    }

    public void SaveSettingsBTN()
    {
        try
        {
            if(AutoReport.isOn == true)
            {
                configData.AutoError = true;
            }
            else
            {
                configData.AutoError = false;
            }

            if(DedectLang.isOn == true)
            {
                configData.AutoDedectLanguage = true;
            }
            else
            {
                configData.AutoDedectLanguage = false;
            }

            if (Lang.value == 0)
            {
                configData.IsEnglisch = false;
                configData.IsGerman = true;
            }
            else
            {
                configData.IsGerman = false;
                configData.IsEnglisch = true;
            }

            if (WriteError.isOn == true)
            {
                configData.WriteErrorLog = true;
            }
            else
            {
                configData.WriteErrorLog = false;
            }

            if (WriteLog.isOn == true)
            {
                configData.WriteLog = true;
            }
            else
            {
                configData.WriteLog = false;
            }

            if (AutoUpdateCheck.isOn == true)
            {
                configData.UpdateCheck = true;
            }
            else
            {
                configData.UpdateCheck = false;
            }

            if (ListenBilder.isOn == true)
            {
                configData.ListImages = true;
            }
            else
            {
                configData.ListImages = false;
            }

            if (PreisLoks.isOn == true)
            {
                configData.TrainPrice = true;
            }
            else
            {
                configData.TrainPrice = false;
            }

            if (PreisWagons.isOn == true)
            {
                configData.WagonPrice = true;
            }
            else
            {
                configData.WagonPrice = false;
            }

            if (PreisInventar.isOn == true)
            {
                configData.InventoryPrice = true;
            }
            else
            {
                configData.InventoryPrice = false;
            }

            if(AnnonPrice.isOn == true)
            {
                configData.AVGPrice = true;
            }
            else
            {
                configData.AVGPrice = true;
            }

            if (Autosync.isOn == true)
            {
                configData.Autosync = true;
            }
            else
            {
                configData.Autosync = false;
            }

            configData.ImageTyp = ImageTyp.value;
            configData.Maintenance = Wartungs.value;
            configData.Resolution = Resulotion.value;
            if (Fullscreen.isOn == true)
            {
                configData.FullScreen = true;
            }
            else
            {
                configData.FullScreen = false;
            }
            configData.MessageTimer = Int32.Parse((Message + Int32.Parse(MessageTimer.text)).ToString());

            configData.NonMaintenanceColor = NonWartungColor.text;
            configData.MaintenanceColor = WartungColor.text;
            string jsonData = JsonUtility.ToJson(configData, true);
            File.WriteAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "config.xml", jsonData);
        }
        catch (Exception ex)
        {
            startManager.LogError("Fehler beim Speichern der Einstellungen.", "Error Saving Settings.", "Settings_Manager Train_List :: Error SaveSettingsBTN(); " + ex);
            startManager.Error("SaveSettings(Settings_Manager);", "" + ex);
        }
        finally
        {
            startManager.Log("Modul Settings_Manager :: Einstellungen Erfolgreich Gespeichert.", "Settings_Manager :: Settings Successfully saved.");
            startManager.Notify("Einstellungen Gespeichert.!", "Settings Saved.!", "green", "green");
        }
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

    public void CalculateImageSize()
    {
        WaggonPicSize = DirSize(new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Wagons"));
        TrainPicSize = DirSize(new DirectoryInfo(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Trains"));
    }

    public void CreateErrorReport()
    {
        try
        {
            if (!Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error-Report"))
            {
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error-Report");
                if (File.Exists(startManager.LogPath + "OLD-Log"))
                    File.Copy(startManager.LogPath + "OLD-Log", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error-Report/" + "_old");
                if (File.Exists(startManager.LogPath + "last.Log"))
                    File.Copy(startManager.LogPath + "last.log", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error-Report/" + "_curr");
                if (File.Exists(startManager.LogPath + "error.log"))
                    File.Copy(startManager.LogPath + "error.log", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error-Report/" + "_err");
                if (File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"))
                    File.Copy(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db", System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + "/Error-Report/" + "_db");
            }
        }
        catch(Exception ex)
        {
            startManager.LogError("Fehler beim Erstellen des Fehler Report.", "Error to Create the Error Report", " Settings_Manager :: CreateErrorReport; Error: " + ex);
            startManager.Error("CreateErrorReport(Settings_Manager);", "" + ex);
        }
        finally
        {
            startManager.Log("Modul Settings_Manager :: Error Report  Erfolgreich Erstellt.", "Settings_Manager :: Error Report Successfully saved.");
            startManager.Notify("Error Report Gespeichert.!", "Error Report Saved.!", "green", "green");
        }
    }
}