using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Translator : MonoBehaviour
{
    public bool German;
    public bool Other;
    public bool AutoDedect;
    public string Lang;
    public LogWriterManager Logger;
    private SettingsData settingsData;
    [Header("MainMenue")]
    public Text LokListe;
    public Text NewLok;
    public Text WagonList;
    public Text NewWagon;
    public Text Settings;
    public Text EinkaufsList;
    public Text Decoder;
    public Text Stats;
    public Text Importer;
    public Text Backups;
    public Text ProgrammInfo;
    [Header("Trainslist")]
    public Text MainWinTrainList;
    public Text ExportFile;
    public Text ExportRPC;
    public Text ExportWWW;
    [Header("TrainADD")]
    public Text MainWinAddTrain;
    public Text Baureihe;
    public Text Farbe;
    public Text EnergyArt;
    public Text Hersteller;
    public Text Katalognummer;
    public Text Seriennummer;
    public Text Kaufdatum;
    public Text Preis;
    public Text Wartung;
    public Text Adresse;
    public Text Protokoll;
    public Text Fahrstufen;
    public Text DecoderHersteller;
    public Text Rauch;
    public Text Telex;
    public Text Sound;
    public Text Lichtwechsel;
    public Text ElektrischeKupplung;
    public Text ElektrischePandos;
    [Header("Settings")]
    public Text MainWinSettings;
    public Text UpdateCheck;
    public Text AnonymeData;
    public Text Lokbilder;
    public Text LokPrice;
    public Text WagonPrice;
    public Text EinkaufsPreis;
    public Text Debugger;
    public Text BilderAutoRead;
    public Text WartungsIntervall;
    public Text DatenbankSize;
    public Text DatenBankPath;
    public Text LastUpdate;
    public Text ImageFolderSize;
    public Text CheckVersion;
    public Text CreateBackup;
    public Text ReadPicsNew;
    public Text LoadPicsNew;
    public Text Save;
    public Text Receiver;
    public Text Sender;
    [Header("WagonList")]
    public Text MainWinWagonList;
    public Text ExportFile1;
    public Text ExportRPC1;
    public Text ExportWWW1;
    [Header("WagonEdit")]
    public Text MainWinAddWagon;
    public Text WagonFarbe;
    public Text WagonHersteller;
    public Text WagonSpurweite;
    public Text WagonKatalognummer;
    public Text WagonSeriennummer;
    public Text WagonKaufdatum;
    public Text WagonPreis;
    public Text WagonElektoKuppler;
    public Text WagonInnenLicht;
    public Text WagonPreiser;
    [Header("Stats")]
    public Text MainWinStats;
    public Text LoksSaved;
    public Text WagonsSaved;
    public Text BuysSaved;
    public Text SavedDecoder;
    public Text Import;
    public Text Received;
    public Text TrainsPiad;
    public Text WagonsPaid;
    public Text BuyPaid;
    public Text CompleteSumm;
    [Header("Über TrainBase")]
    public Text About;
    public Text Info;

    void Start()
    {
        settingsData = new SettingsData();
        LoadSettingsData();
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Translator_Manager -> Message is Normal.");
            if (AutoDedect == true)
            {
                if (Application.systemLanguage == SystemLanguage.German)
                {
                    German = true;
                    Lang = "de_DE";
                    Logger.PrintLog("MODUL Translator_Manager :: Setze Sprache auf Deutsch." + "\n");
                }
                else
                {
                    Other = true;
                    Lang = "en_EN";
                    Logger.PrintLog("MODUL Translator_Manager :: Set Laguane to Englisch." + "\n");
                    SetLaguane();
                }
            }
            else
            {
                if (German == true)
                {
                    German = true;
                    Lang = "de_DE";
                    Logger.PrintLog("MODUL Translator_Manager :: Setze Sprache auf Deutsch." + "\n");
                }
                else
                {
                    Other = true;
                    Lang = "en_EN";
                    Logger.PrintLog("MODUL Translator_Manager :: Set Laguane to Englisch." + "\n");
                    SetLaguane();
                }
            }
        }
    }

    void SetLaguane()
    {
        MainWinTrainList.text = "Saved locomotives";
        LokListe.text = "Trainlist";
        NewLok.text = "New Train";
        WagonList.text = "Wagon List";
        NewWagon.text = "New Wagon";
        Settings.text = "Settings";
        EinkaufsList.text = "Shopping List";
        Decoder.text = "Decoder List";
        Stats.text = "Stats View";
        Importer.text = "Importer";
        Backups.text = "Backups";
        ProgrammInfo.text = "Program Info";
        ExportFile.text = "as File";
        ExportRPC.text = "via RPC";
        ExportWWW.text = "via WWW";

        MainWinAddTrain.text = "Add ah new Train";
        Baureihe.text = "Series";
        Farbe.text = "Color";
        EnergyArt.text = "Energy art";
        Hersteller.text = "Manufacturer";
        Katalognummer.text = "Catalog Number";
        Seriennummer.text = "Serial Number";
        Kaufdatum.text = "Buy Date";
        Preis.text = "Price";
        Wartung.text = "Maintenance Day";
        Adresse.text = "Adresse";
        Protokoll.text = "Protokoll";
        Fahrstufen.text = "Speed Steps";
        DecoderHersteller.text = "Decoder Manufacturer ";
        Rauch.text = "has Smoke";
        Telex.text = "has Telex";
        Sound.text = "has Sound";
        Lichtwechsel.text = "has Lighrtswitch";
        ElektrischeKupplung.text = "has Elek. Couppler";
        ElektrischePandos.text = "has Elek. Pando.";

        MainWinSettings.text = "Programm Settings";
        UpdateCheck.text = "Auto Update Check";
        AnonymeData.text = "Send Anonymous Data";
        Lokbilder.text = "Show Train/Wagon Images in the List";
        LokPrice.text = "Generate Train Price List";
        WagonPrice.text = "Generate Wagon Price List";
        EinkaufsPreis.text = "Create Shopping List Prices";
        Debugger.text = "Enable Debuger";
        BilderAutoRead.text = "Images Autoload";
        WartungsIntervall.text = "Maintenance Interval";
        DatenbankSize.text = "Database Size";
        DatenBankPath.text = "Database Path";
        LastUpdate.text = "Last DB Update";
        ImageFolderSize.text = "Image Folder Size";
        CheckVersion.text = "Check new Version";
        CreateBackup.text = "Create Backup";
        ReadPicsNew.text = "Reload Train Wagon Images New";
        LoadPicsNew.text = "Create New Clean Images";
        Save.text = "Save Settings";
        Receiver.text = "Receive Mode";
        Sender.text = "Sender Mode";

        MainWinWagonList.text = "Saved Wagons";
        ExportFile1.text = "as File";
        ExportRPC1.text = "via RPC";
        ExportWWW1.text = "via WWW";

        MainWinAddWagon.text = "Add new Wagon";
              WagonFarbe.text = "Color:";
        WagonHersteller.text = "Manufacturer:";
        WagonSpurweite.text = "Gauge:";
        WagonKatalognummer.text = "Catalog Number:";
        WagonSeriennummer.text = "Serial Number:";
        WagonKaufdatum.text = "Buy Date:";
        WagonPreis.text = "Price:";
        WagonElektoKuppler.text = "Electric Coupling:";
        WagonInnenLicht.text = "Interior Lighting:";
        WagonPreiser.text = "has Preiser:";

              MainWinStats.text = "Statistics Overview";
        LoksSaved.text = "Stored locomotives:";
        WagonsSaved.text = "Saved Wagons:";
        BuysSaved.text = "Saved Purchases:";
        SavedDecoder.text = "Saved Decoder:";
        Import.text = "Imported things:";
        Received.text = "Received things:";
        TrainsPiad.text = "For locomotives issued:";
        WagonsPaid.text = "For Wagons issued:";
        BuyPaid.text = "For Purchases issued:";
        CompleteSumm.text = "Complete Summe:";

      About.text = "About TrainBaseV2";
        Info.text = "About TrainBaseV2: \n \n \n 1) -Updates \n The program was created and extended by a private individual. \n New updates may appear over time. \n The developer assumes no liability for loss of data in an update. \n  \n2) - Decompiling Dissabler Reflectors Debugger  \n \n Decompile, Dissociate debugging is not allowedand gives no response to demand. \n  \n 3) - Internet usage  \n The program does not need internet access to provide the functionalityhowever,  \n to detect new updates. \n I've allowed myself to build an anonymous statistic that shows me what:Users, \n program starts and the launches of different versions...this statistic is completely visible under: \n http://trainbase.bplaced.net  \n  \n 4) - mistakes, wishes  \n Can I have to be notified but notHowever, \n it would be good if these are reported. \n  \n 5) - Update and Downloads  \n Updates or program downloads only on the pages specified by me.  \n I DO NOT allow my programs on any other than to offer my websites for download.  \n Download Pages:My private pool contains release build as well as beta versions that do not appear on Github  \n http://trainbase.rf.gd to the downloads at Github I will only post stable build posted by myself.  \nhttps://github.com/kuxii2016/TrainBaseV2  \n  \n 6) -reading  \nI allow the passing on of my program.  \n  \n 9) - Development  \n I will continue to develop this program alone,I am not looking for partners sponsors or other.";
    }

    public void LoadSettingsData()
    {
        settingsData = JsonUtility.FromJson<SettingsData>(File.ReadAllText(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/" + "config.xml"));
        settingsData.Autodedect = AutoDedect;
    }
}