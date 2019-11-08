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

public class Stats_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public Train_List TrainList;
    public Wagon_List WagonList;
    public Inventory_Manager InventoryList;
    public Decoder_Manager DecoderList;

    [Header("Elements")]
    public Slider[] DirectDraw;
    public Text MaxValue;
    public Text[] HerstellerSumme;
    public Text ItemSumm;
    public Text IteamSt;
    public Text DecoderSumm;
    public Text DecoderSt;
    public Text LokSumm;
    public Text LokSt;
    public Text WagonSumm;
    public Text WagonSt;
    public Text StGesammt;
    public Text PriceGesammt;

    [Header("WorkFlow")]
    public int Trains;
    public int Wagons;
    public int Items;
    public int Decoder;
    public int TotalSaved;
    public int TrainPrice;
    public int WagonPrice;
    public int ItemPrice;
    public int CompletePrice;
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
    public float MaxBar;
    public bool IsLoadet = false;
    public int CountedTrains = 0;
    public int CountedWagon = 0;

    void Start ()
    {
        startManager.Log("Lade Stats_Manager -> Nachricht ist Normal.", "Load Stats_Manager -> message is normal");
        ReadDataValues();
    }

    void Update()
    {
        if (IsLoadet == true)
        {
            if (DirectDraw[0].value != Ade)
            {
                DirectDraw[0].value += 1;
                HerstellerSumme[0].text = "Ade: " + (DirectDraw[0].value);
            }

            if (DirectDraw[1].value != Bachmann)
            {
                DirectDraw[1].value += 1;
                HerstellerSumme[1].text = "Bachmann: " + (DirectDraw[1].value);
            }

            if (DirectDraw[2].value != Bemo)
            {
                DirectDraw[2].value += 1;
                HerstellerSumme[2].text = "Bemo: " + (DirectDraw[2].value);
            }

            if (DirectDraw[3].value != Brawa)
            {
                DirectDraw[3].value += 1;
                HerstellerSumme[3].text = "Brawa: " + (DirectDraw[3].value);
            }

            if (DirectDraw[4].value != Faller)
            {
                DirectDraw[4].value += 1;
                HerstellerSumme[4].text = "Faller: " + (DirectDraw[4].value);
            }

            if (DirectDraw[5].value != Fleischmann)
            {
                DirectDraw[5].value += 1;
                HerstellerSumme[5].text = "Fleischmann: " + (DirectDraw[5].value);
            }

            if (DirectDraw[6].value != Hornby)
            {
                DirectDraw[6].value += 1;
                HerstellerSumme[6].text = "Hornby: " + (DirectDraw[6].value);
            }

            if (DirectDraw[7].value != Jouef)
            {
                DirectDraw[7].value += 1;
                HerstellerSumme[7].text = "Jouef: " + (DirectDraw[7].value);
            }

            if (DirectDraw[8].value != Kleinbahn)
            {
                DirectDraw[8].value += 1;
                HerstellerSumme[8].text = "Kleinbahn: " + (DirectDraw[8].value);
            }

            if (DirectDraw[9].value != Liliput)
            {
                DirectDraw[9].value += 1;
                HerstellerSumme[9].text = "Liliput: " + (DirectDraw[9].value);
            }

            if (DirectDraw[10].value != Lima)
            {
                DirectDraw[10].value += 1;
                HerstellerSumme[10].text = "Lima: " + (DirectDraw[10].value);
            }

            if (DirectDraw[11].value != MiniTrix)
            {
                DirectDraw[11].value += 1;
                HerstellerSumme[11].text = "MiniTrix: " + (DirectDraw[11].value);
            }

            if (DirectDraw[12].value != Märklin)
            {
                DirectDraw[12].value += 1;
                HerstellerSumme[12].text = "Märklin: " + (DirectDraw[12].value);
            }

            if (DirectDraw[13].value != Piko)
            {
                DirectDraw[13].value += 1;
                HerstellerSumme[12].text = "Piko: " + (DirectDraw[12].value);
            }

            if (DirectDraw[14].value != Rivarosi)
            {
                DirectDraw[14].value += 1;
                HerstellerSumme[14].text = "Rivarosi: " + (DirectDraw[14].value);
            }

            if (DirectDraw[15].value != Roco)
            {
                DirectDraw[15].value += 1;
                HerstellerSumme[15].text = "Roco: " + (DirectDraw[15].value);
            }

            if (DirectDraw[16].value != Röwa)
            {
                DirectDraw[16].value += 1;
                HerstellerSumme[16].text = "Röwa: " + (DirectDraw[16].value);
            }

            if (CountedTrains != TrainList.Trains.Count)
            {
                CountedTrains += 1;
            }

            if (CountedWagon != WagonList.Trains.Count)
            {
                CountedWagon += 1;
            }

            if (Items != InventoryList.Item.Count)
            {
                Items += 1;
            }

            if (Decoder != DecoderList.dbDecoder.Count)
            {
                Decoder += 1;
            }

            if (TrainPrice != TrainList.TotalSumme)
            {
                if (UserSettings.PreisLoks.isOn == true)
                {
                    TrainPrice += 1;
                }
                else
                {
                    TrainPrice = 0;
                }
            }

            if (WagonPrice != WagonList.TotalSumme)
            {
                if (UserSettings.PreisWagons.isOn == true)
                {
                    WagonPrice += 1;
                }
                else
                {
                    WagonPrice = 0;
                }
            }

            if (ItemPrice != InventoryList.CompleteSumm)
            {
                if (UserSettings.PreisInventar.isOn == true)
                {
                    ItemPrice += 1;
                }
                else
                {
                    ItemPrice = 0;
                }
            }

            if (CompletePrice != (TrainPrice + WagonPrice + ItemPrice))
            {
                CompletePrice += 1;
            }

            if (TotalSaved != (Trains + Wagons + InventoryList.Item.Count + DecoderList.dbDecoder.Count))
            {
                TotalSaved += 1;
            }
        }

        MaxValue.text = "        Trains: " + CountedTrains + "         Wagons: " + CountedWagon;
        DecoderSt.text = Decoder.ToString() + " st.";
        IteamSt.text = Items.ToString() + " st.";
        LokSt.text = CountedTrains.ToString() + " st.";
        WagonSt.text = CountedWagon.ToString() + " st.";
        StGesammt.text = TotalSaved.ToString() + " st.";
        PriceGesammt.text = CompletePrice.ToString() + " €";
        WagonSumm.text = WagonPrice.ToString() + " €";
        LokSumm.text = TrainPrice.ToString() + " €";
        DecoderSumm.text = "";
        ItemSumm.text = ItemPrice.ToString() + " €";
    }

    public void Unload()
    {
        IsLoadet = false;
        for (int x = 0; x < DirectDraw.Length; x++)
        {
            DirectDraw[x].value = 0;
            HerstellerSumme[x].text = "";
        }
        ItemPrice = 0;
        TrainPrice = 0;
        WagonPrice = 0;
        Brawa = 0;
        Fleischmann = 0;
        Hornby = 0;
        Jouef = 0;
        Kleinbahn = 0;
        Lima = 0;
        MiniTrix = 0;
        Märklin = 0;
        Piko = 0;
        Roco = 0;
        Röwa = 0;
        Trix = 0;
        Ade = 0;
        Bachmann = 0;
        Bemo = 0;
        Brawa = 0;
        Faller = 0;
        Liliput = 0;
        Rivarosi = 0;
        MaxBar = 0;
        CountedTrains = 0;
        CountedWagon = 0;
        Items = 0;
        Decoder = 0;
        CompletePrice = 0;
        TotalSaved = 0;
        ReadDataValues();
        startManager.Notify("Generiere Daten & Diagramm", "TrainBaseV2 Started and ready", "green", "green");
        IsLoadet = true;
    }

    void ReadDataValues()
    {
        try
        {
            Trains = TrainList.Trains.Count;
            Wagons = WagonList.Trains.Count;

            for (int s = 0; s < TrainList.Trains.Count; s++)
            {
                if (TrainList.Trains[s].DbHersteller == 0 == true)
                {
                    Brawa += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 1 == true)
                {
                    Fleischmann += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 2 == true)
                {
                    Hornby += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 3 == true)
                {
                    Jouef += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 4 == true)
                {
                    Kleinbahn += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 5 == true)
                {
                    Lima += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 6 == true)
                {
                    MiniTrix += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 7 == true)
                {
                    Märklin += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 8 == true)
                {
                    Piko += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 9 == true)
                {
                    Roco += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 10 == true)
                {
                    Röwa += 1;
                }
                if (TrainList.Trains[s].DbHersteller == 11 == true)
                {
                    Trix += 1;
                }
            }

            for (int s = 0; s < WagonList.Trains.Count; s++)
            {
                if (WagonList.Trains[s].DBHersteller == 0 == true)
                {
                    Ade += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 1 == true)
                {
                    Bachmann += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 2 == true)
                {
                    Bemo += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 3 == true)
                {
                    Brawa += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 4 == true)
                {
                    Faller += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 5 == true)
                {
                    Fleischmann += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 6 == true)
                {
                    Hornby += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 7 == true)
                {
                    Jouef += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 8 == true)
                {
                    Kleinbahn += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 9 == true)
                {
                    Liliput += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 10 == true)
                {
                    Lima += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 11 == true)
                {
                    Märklin += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 12 == true)
                {
                    Piko += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 13 == true)
                {
                    Rivarosi += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 14 == true)
                {
                    Roco += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 15 == true)
                {
                    Trix += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 16 == true)
                {
                    Röwa += 1;
                }
                if (WagonList.Trains[s].DBHersteller == 17 == true)
                {
                    MiniTrix += 1;
                }
            }

            for (int x = 0; x < DirectDraw.Length; x++)
            {
                MaxBar = Mathf.Round(TrainList.Trains.Count + WagonList.Trains.Count);
                DirectDraw[x].maxValue = (MaxBar);
            }
            IsLoadet = true;
        }
        catch (Exception ex)
        {
            startManager.LogError("Fehler beim Laden der Daten.", "Error Loading  Data", " Stats_Manager :: ReadDataValues(); Error: " + ex);
        }
        finally
        {
            startManager.Log("Modul Stats_Manager :: Alle Daten Gelesen.", "Modul Stats_Manager :: All Data are Read");
        }
    }
}