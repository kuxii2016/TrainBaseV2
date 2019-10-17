/*
 * 
 *   TrainBase FuhrPark Manager Version 1 from 25.03.2018 written by Michael Kux
 *    *   Last Edit 31.08.2018
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
using DiscordPresence;

public class FuhrParkManager : MonoBehaviour {

    public LogWriterManager Logger;
    public ProgrammSettings SettingsManager;
    public LokView lokView;
    public WagonView wagonview;
    public SettingsManager Settings;
    public InventoryManager IvMG;
    public Discord_Menue_Update DMU;
    public DecoderManager DM;
    public Text FoundLoks;
    public Text FoundWagons;
    public Text BuyTrains;
    public Text BuyWagons;
    public Text Inventorysumme;
    public Text Items;
    public Text SavedDecoder;
    public Text SummeGesammt;
    public GameObject SternText;
    public Dropdown HerstellerView;
    public Slider[] Bar;
    public Text[] HerstellerSumme;
    public int RowLoks = 0;
    public int RowWagons = 0;
    public int RowItems = 0;
    private string wert;
    public Int32 Loksum = 0;
    public Int32 Wagonsum = 0;
    public int Ade = 0;
    public int Bachmann = 0;
    public int Bemo = 0;
    public int Brawa = 0;
    public int Faller = 0;
    public int Fleischmann = 0;
    public int Hornby = 0;
    public int Jouef = 0;
    public int Kleinbahn = 0;
    public int Liliput = 0;
    public int Lima = 0;
    public int MiniTrix = 0;
    public int Märklin = 0;
    public int Piko = 0;
    public int Rivarosi = 0;
    public int Roco = 0;
    public int Röwa = 0;
    public int Trix = 0;
    public Image ReadOn;
    public Text ImportRPC;
    public Text ImportXML;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Statistik List -> Message is Normal.");
        }
        RowLoks = lokView.RowLoks;
        RowWagons = wagonview.RowLoks;
        Items.text = IvMG.Items.ToString();
        for (int x = 0; x < Bar.Length; x++)
        {
            Bar[x].maxValue = (lokView.Trains.Count + wagonview.Trains.Count);
        }

        for (int s = 0; s < lokView.Trains.Count; s++)
        {
            if (lokView.Trains[s].DbHersteller == 0 == true)
            {
                Brawa += 1;
            }
            if (lokView.Trains[s].DbHersteller == 1 == true)
            {
                Fleischmann += 1;
            }
            if (lokView.Trains[s].DbHersteller == 2 == true)
            {
                Hornby += 1;
            }
            if (lokView.Trains[s].DbHersteller == 3 == true)
            {
                Jouef += 1;
            }
            if (lokView.Trains[s].DbHersteller == 4 == true)
            {
                Kleinbahn += 1;
            }
            if (lokView.Trains[s].DbHersteller == 5 == true)
            {
                Lima += 1;
            }
            if (lokView.Trains[s].DbHersteller == 6 == true)
            {
                MiniTrix += 1;
            }
            if (lokView.Trains[s].DbHersteller == 7 == true)
            {
                Märklin += 1;
            }
            if (lokView.Trains[s].DbHersteller == 8 == true)
            {
                Piko += 1;
            }
            if (lokView.Trains[s].DbHersteller == 9 == true)
            {
                Roco += 1;
            }
            if (lokView.Trains[s].DbHersteller == 10 == true)
            {
                Röwa += 1;
            }
            if (lokView.Trains[s].DbHersteller == 11 == true)
            {
                Trix += 1;
            }
        }

        for (int s = 0; s < wagonview.Trains.Count; s++)
        {
            if (wagonview.Trains[s].DBHersteller == 0 == true)
            {
                Ade += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 1 == true)
            {
                Bachmann += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 2 == true)
            {
                Bemo += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 3 == true)
            {
                Brawa += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 4 == true)
            {
                Faller += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 5 == true)
            {
                Fleischmann += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 6 == true)
            {
                Hornby += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 7 == true)
            {
                Jouef += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 8 == true)
            {
                Kleinbahn += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 9 == true)
            {
                Liliput += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 10 == true)
            {
                Lima += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 11 == true)
            {
                Märklin += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 12 == true)
            {
                Piko += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 13 == true)
            {
                Rivarosi += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 14 == true)
            {
                Roco += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 15 == true)
            {
                Trix += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 16 == true)
            {
                Röwa += 1;
            }
            if (wagonview.Trains[s].DBHersteller == 17 == true)
            {
                MiniTrix += 1;
            }
        }

        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("MODUL Statistik_Manager :: Read Diagramm Data, for Train/Wagons(s).");
        }
    }

    void Update()
    {
        SavedDecoder.text = DM.Decoder.ToString();
        RowItems = IvMG.InVD.Count;
        Items.text = RowItems.ToString();
        ImportRPC.text = Settings.ImportRPC.ToString();
        ImportXML.text = Settings.ImportXML.ToString();
        SummeGesammt.GetComponent<Text>().text = (Loksum + Wagonsum + IvMG.SiteSumme).ToString() + " €";
        RowLoks = lokView.RowLoks;
        Inventorysumme.GetComponent<Text>().text = IvMG.SiteSumme.ToString() + " €";
        RowWagons = wagonview.RowLoks;
        FoundLoks.GetComponent<Text>().text = RowLoks.ToString();
        FoundWagons.GetComponent<Text>().text = RowWagons.ToString();
        if (Settings.ErstelleLokPrice == true)
        {
            BuyTrains.GetComponent<Text>().text = Loksum.ToString() + " €";
            if (Loksum == 0)
            {
                ReadOn.color = Color.blue;
                for (int s = 0; s < lokView.Trains.Count; s++)
                {
                    Loksum += lokView.Trains[s].DbPreis;
                }
            }
        }
        else
        {
            BuyTrains.GetComponent<Text>().text = "Preisliste Deaktiviert.(*)";
            SternText.gameObject.SetActive(true);
        }

        if (Settings.ErstelleWagonPrice == true)
        {
            BuyWagons.GetComponent<Text>().text = Wagonsum.ToString() + " €";
            if (Wagonsum == 0)
            {
                ReadOn.color = Color.blue;
                for (int s = 0; s < wagonview.Trains.Count; s++)
                {
                    Wagonsum += wagonview.Trains[s].DBPreis;
                }
            }
        }
        else
        {
            BuyWagons.GetComponent<Text>().text = "Preisliste Deaktiviert.(*)";
            SternText.gameObject.SetActive(true);
        }
        Bar[0].value = Ade;
        HerstellerSumme[0].text = "Ade: " +Ade;
        Bar[1].value = Bachmann;
        HerstellerSumme[1].text = "Bachmann: " +Bachmann;
        Bar[2].value = Bemo;
        HerstellerSumme[2].text = "Bemo: " + Bemo;
        Bar[3].value = Brawa;
        HerstellerSumme[3].text = "Brawa: " + Brawa;
        Bar[4].value = Faller;
        HerstellerSumme[4].text = "Faller: " + Faller;
        Bar[5].value = Fleischmann;
        HerstellerSumme[5].text = "Fleischmann: " + Fleischmann;
        Bar[6].value = Hornby;
        HerstellerSumme[6].text = "Hornby: " + Hornby;
        Bar[7].value = Jouef;
        HerstellerSumme[7].text = "Jouef: " + Jouef;
        Bar[8].value = Kleinbahn;
        HerstellerSumme[8].text = "Kleinbahn: " + Kleinbahn;
        Bar[9].value = Liliput;
        HerstellerSumme[9].text = "Liliput: " + Liliput;
        Bar[10].value = Lima;
        HerstellerSumme[10].text = "Lima: " + Lima;
        Bar[11].value = MiniTrix;
        HerstellerSumme[11].text = "MiniTrix: " + MiniTrix;
        Bar[12].value = Märklin;
        HerstellerSumme[12].text = "Märklin: " + Märklin;
        Bar[13].value = Piko;
        HerstellerSumme[13].text = "Piko: " + Piko;
        Bar[14].value = Rivarosi;
        HerstellerSumme[14].text = "Rivarosi: " + Rivarosi;
        Bar[15].value = Roco;
        HerstellerSumme[15].text = "Roco: " + Roco;
        Bar[16].value = Röwa;
        HerstellerSumme[16].text = "Röwa: " + Röwa;
        Bar[17].value = Trix;
        HerstellerSumme[17].text = "Trix: " + Trix;
    }

    public void newScore()
    {
        PresenceManager.UpdatePresence("View Stats", "Trains: " + RowLoks + "Wagons:" + RowWagons, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        if (Settings.ErstelleLokPrice == true)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Statistik_Manager :: Get Price list from Trains.");
            }
            BuyTrains.GetComponent<Text>().text = Loksum.ToString() + " €";
            if (Loksum == 0)
            {
                ReadOn.color = Color.blue;
                for (int s = 0; s < lokView.Trains.Count; s++)
                {
                    Loksum += lokView.Trains[s].DbPreis;
                }
            }
            ReadOn.color = Color.green;
        }
        else
        {
            BuyTrains.GetComponent<Text>().text = "Preisliste Deaktiviert.(*)";
            SternText.gameObject.SetActive(true);
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Statistik_Manager :: Price List Trains Disabled..");
            }
        }
    }

    public void NewScore2()
    {
        PresenceManager.UpdatePresence("View Stats", "Trains: " + RowLoks + "Wagons:" + RowWagons, DMU.Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
        if (Settings.ErstelleWagonPrice == true)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Statistik_Manager ::  Get Price list from Wagons.");
            }
            BuyWagons.GetComponent<Text>().text = Wagonsum.ToString() + " €";
            if (Wagonsum == 0)
            {
                ReadOn.color = Color.blue;
                for (int s = 0; s < wagonview.Trains.Count; s++)
                {
                    Wagonsum += wagonview.Trains[s].DBPreis;
                }
            }
            ReadOn.color = Color.green;
        }
        else
        {
            BuyWagons.GetComponent<Text>().text = "Preisliste Deaktiviert.(*)";
            SternText.gameObject.SetActive(true);
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Statistik_Manager ::  Price List Wagons Disabled.");
            }
        }
    }
}