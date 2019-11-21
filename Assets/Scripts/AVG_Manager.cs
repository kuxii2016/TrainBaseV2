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

[System.Serializable]
public class ExportData : System.Object
{
    public string DbKatalognummer;
    public string DbHersteller;
    public int DbPreis;
    public string DBIdentifyer;
}
public class AVG_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public Train_List TrainList;
    public Wagon_List WagonList;
    [Header("Workflow")]
    public List<ExportData> Export;

    [Header("Workflow")]
    public bool IsActive = false;

    [Header("Elements")]
    public InputField ItemNumber;
    public Text Manufracture;
    public Text Output;

    void Start()
    {
        startManager.Log("Lade AVG_Manager -> Nachricht ist Normal.", "Load AVG_Manager -> message is normal");
        if (UserSettings.AnnonPrice.isOn == true)
        {
            IsActive = true;
            startManager.Log("Modul AVG_Manager :: Preis Analysierer ist Aktiv, Danke fuers Mitwirken.! :) ", "Modul AVG_Manager :: Price analyzer is active, thanks for contributing.! :)");
            ReadTrains();
            ReadWagons();
            CheckNewUpdates();
        }
        else
        {
            IsActive = false;
            startManager.Log("Modul AVG_Manager :: Preis Analysierer ist De-Aktiv. :( ", "Modul AVG_Manager :: Price analyzer is De-Active. :( ");
            startManager.Log("Modul AVG_Manager :: Es werden keine Daten Gesendet! ", "Modul AVG_Manager :: No data will be sent.!");
        }
    }
    
    void ReadTrains()
    {
        Export = new List<ExportData>();

        for (int i = 0; i < TrainList.Trains.Count; i++)
        {
            ExportData export = new ExportData();
            export.DbKatalognummer = TrainList.vHersteller[TrainList.Trains[i].DbHersteller];
            export.DbHersteller = TrainList.Trains[i].DbKatalognummer;
            export.DbPreis = TrainList.Trains[i].DbPreis;
            export.DBIdentifyer = TrainList.Trains[i].DBIdentifyer;
            Export.Add(export);
        }
        startManager.Log("Modul AVG_Manager :: "+ TrainList.Trains.Count + " Loks Gefunden", "Modul AVG_Manager :: " + TrainList.Trains.Count + " Trains Found");
    }

    void ReadWagons()
    {
        for (int i = 0; i < WagonList.Trains.Count; i++)
        {
            ExportData export = new ExportData();
            export.DbKatalognummer = WagonList.vHersteller[WagonList.Trains[i].DBHersteller];
            export.DbHersteller = WagonList.Trains[i].DBKatalognummer;
            export.DbPreis = WagonList.Trains[i].DBPreis;
            export.DBIdentifyer = WagonList.Trains[i].DBIdentifyer;
            Export.Add(export);
        }
        startManager.Log("Modul AVG_Manager :: " + WagonList.Trains.Count + " Wagons Gefunden", "Modul AVG_Manager :: " + WagonList.Trains.Count + " Wagons Found");
    }

    void CheckNewUpdates()
    {
        if(IsActive == true)
        {
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/TrainBaseV2"))
            {
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/TrainBaseV2");
            }

            for (int i = 0; i < Export.Count; i++)
            {
                if(!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/TrainBaseV2" + "/" + Export[i].DBIdentifyer) )
                {
                    Debug.Log(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/TrainBaseV2");
                    StartCoroutine(SendSelected(Export[i].DBIdentifyer, Export[i].DbKatalognummer, Export[i].DbHersteller, Export[i].DbPreis));

                    FileStream fs = new FileStream(startManager.LogPath + "Export.log", FileMode.Append, FileAccess.Write, FileShare.Write);
                    fs.Close();
                    StreamWriter sw = new StreamWriter(startManager.LogPath + "Export.log", true, Encoding.ASCII);
                    sw.Write(DateTime.Now.ToString("HH:mm - dd/MM/yyyy ") + "Modul AVG_Manager :: Neue Daten Gefunden: " + Export[i].DBIdentifyer + "," + Export[i].DbKatalognummer + "," + Export[i].DbHersteller + "," + Export[i].DbPreis + " \n");
                    sw.Close();

                    FileStream fss = new FileStream(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + "/TrainBaseV2" + "/" + Export[i].DBIdentifyer, FileMode.Append, FileAccess.Write, FileShare.Write);
                    fss.Close();


                }
            }
            //startManager.Log("Modul AVG_Manager :: Gesendet: " + Export.Count + "Einträge", "Modul AVG_Manager :: Send: " + Export.Count + "Entrys");
        }
    }

    public void SendData()
    {
        StartCoroutine(GetData(ItemNumber.text, Manufracture.text));
    }

    IEnumerator SendSelected(string uniqueID, string Manufacturer, string ItemNumber, int Price)
    {
        string FinshURL = "http://" + startManager.AVGPriceURL + "calculator" + "/insert.php?uniqueID=" + uniqueID + "&ItemNumber=" + ItemNumber + "&Manufacturer=" + Manufacturer + "&Price=" + Price;

        WWW insert = new WWW(FinshURL);

        yield return insert;

        if (insert.error != null)
        {
            startManager.LogError("Keine Verbindung zum Server möglich!.", "No Connection to the Server.", " Train_List :: SendSelected(); Error: " + insert.error);
            startManager.Error("SendSelected(AVG);", insert.error);
        }
        if (insert.isDone)
        {
        }
    }

    private IEnumerator GetData(string ItemNumber, string Manufacturer)
    {
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/55.0.2883.87 Safari/537.36";
        Dictionary<string, string> ht = new Dictionary<string, string>();
        ht["User-Agent"] = userAgent;
        {
            WWW www = new WWW("http://" + startManager.AVGPriceURL + "calculator" + "/check.php?ItemNumber=" + ItemNumber + "&Manufacturer=" + Manufacturer, null, ht);
            yield return www;
            if (www.error != null)
            {
                startManager.LogError("Fehler beim Abfragen der Daten.", "Error while querying the data", " AVG_Manager :: SendSelected(); Error: " + www.error);
                startManager.Error("GetData(AVG)", www.error);
            }
            else
            {
                Output.text = www.text;
            }
        }
    }
}