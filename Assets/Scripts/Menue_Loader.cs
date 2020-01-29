using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Menue_Loader : MonoBehaviour {

    [Header("Depents")]
    public Start_Manager startManager;
    [Header("Buttons")]
    public Button Train_List;
    public Button Wagon_List;
    public Button Inventory_List;
    public Button Decoder_List;
    public Button Settings_View;
    public Button Stats_View;
    public Button Backup_View;
    public Button About_View;
    public Button Version;
    [Header("Menue-Button-LokList")]
    public Button[] add;
    public Button[] edit;
    public Button[] remove;
    public Button[] file;
    public Button[] rpc;
    public Button[] www;
    [Header("Menue-Button-Lokedit")]
    public Text[] Baureihe;
    public Text[] Farbe;
    public Text[] Hersteller;
    public Text[] EnergyArt;
    public Text[] Katalognummer;
    public Text[] Decoder;
    public Text[] Adresse;
    public Text[] Seriennummer;
    public Text[] Preis;
    public Text[] Protokoll;
    public Text[] Fahstufen;
    public Text[] LagerOrt;
    public Text[] Kauftag;
    public Text[] WartungTag;
    public Text[] Rauch;
    public Text[] Telex;
    public Text[] Sound;
    public Text[] LichtWechsel;
    public Text[] ElekKupplung;
    public Text[] ElektPando;
    [Header("Menue-Button-Wagonedit")]
    public Text WeditColor;
    public Text WeditHersteller;
    public Text WeditKatalognummer;
    public Text WeditSeriennummer;
    public Text WeditPreis;
    public Text WEditWagonType;
    public Text WeditLager;
    public Text WeditKaufTag;
    public Text WeditPreiser;
    public Text WeditLicht;
    public Text WeditStromKupplung;


    void Start()
    {
        if (startManager.IsGerman == true)
        {
            Train_List.GetComponentInChildren<Text>().text = "Lok Liste";
            Wagon_List.GetComponentInChildren<Text>().text = "Wagon Liste";
            Inventory_List.GetComponentInChildren<Text>().text = "Inventar Liste";
            Decoder_List.GetComponentInChildren<Text>().text = "Decoder Liste";
            Settings_View.GetComponentInChildren<Text>().text = "Einstellungen";
            Stats_View.GetComponentInChildren<Text>().text = "Statistik Übersicht";
            Backup_View.GetComponentInChildren<Text>().text = "Backup Übersicht";
            About_View.GetComponentInChildren<Text>().text = "Über TrainbaseV2";
        }

        if (startManager.IsEnglisch == true)
        {
            Train_List.GetComponentInChildren<Text>().text = "Locomotive list";
            Wagon_List.GetComponentInChildren<Text>().text = "Wagon list";
            Inventory_List.GetComponentInChildren<Text>().text = "Inventory list";
            Decoder_List.GetComponentInChildren<Text>().text = "Decoder list";
            Settings_View.GetComponentInChildren<Text>().text = "Settings";
            Stats_View.GetComponentInChildren<Text>().text = "Statistics overview";
            Backup_View.GetComponentInChildren<Text>().text = "Backup overview";
            About_View.GetComponentInChildren<Text>().text = "About TrainBaseV2";
        }
        SetMenueButton();
    }

    public void OnklickTrainList()
    {
        startManager.Notify("Zeigt Gespeicherte Loks", "Shows Saved Locos", "blue", "blue");
    }

    public void OnklickWagonList()
    {
        startManager.Notify("Zeigt Gespeicherte Wagons", "Shows Saved Wagons", "blue", "blue");
    }

    public void OnklickInventoryList()
    {
        startManager.Notify("Zeigt Gespeicherte Einkäufe", "Shows Saved Purchases", "blue", "blue");
    }

    public void OnklickDecoderList()
    {
        startManager.Notify("Zeigt Gespeicherte Decoder", "Shows Saved Decoders", "blue", "blue");
    }

    public void OnklickSettings()
    {
        startManager.Notify("Öffnet das Einstellungs Fenster", "Opens the Settings Window", "blue", "blue");
    }

    public void OnklickStatsList()
    {
        startManager.Notify("Zeigt diverse Statistiken", "Shows Various Statistics", "blue", "blue");
    }

    public void OnklickBackupList()
    {
        startManager.Notify("Zeigt Gespeicherte Backups", "Shows Saved Backups", "blue", "blue");
    }

    public void OnklickAbout()
    {
        startManager.Notify("Infos über TrainBaseV2", "Information about TrainBaseV2", "blue", "blue");
    }

    public void OnklickDebuger()
    {
        startManager.Notify("Öffnet den Log-Reader", "Opens the Log Reader", "blue", "blue");
    }

    public void OnklickVersion()
    {
        Version.GetComponentInChildren<Text>().color = Color.yellow;
        startManager.Notify("Öffnet den Änderungs Verlauf", "Opens the Change History", "blue", "blue");
    }

    public void OnklickVersionExit()
    {
        Version.GetComponentInChildren<Text>().color = Color.black;
    }

    public void OnEnterReCreateLocosImages()
    {
        startManager.Notify("Erstellt Standart Bilder für Loks neu", "Rebuilds Standard Images for Locos", "blue", "blue");
    }

    public void OnEnterReCreateWagonImages()
    {
        startManager.Notify("Erstellt Standart Bilder für Wagons neu", "Rebuilds Standard Images for Wagons", "blue", "blue");
    }

    public void OnEnterReFreshLocosImages()
    {
        startManager.Notify("Läd Eigene Bilder neu", "Reload own images new", "blue", "blue");
    }

    public void OnEnterReFreshWagonImages()
    {
        startManager.Notify("Läd Eigene Wagon Bilder neu", "Reload own Wagon images", "blue", "blue");
    }

    public void OnEnterCreateErrorReport()
    {
        startManager.Notify("Erstellt alle nötigen Dateien für einen Error Report", "Created all needed File for ah Error Report", "blue", "blue");
    }

    public void OnEnterCkeckVersion()
    {
        startManager.Notify("Schaue Online nach Neuer Version", "Watch Online for New Version", "blue", "blue");
    }

    public void OnEnterSaveSettings()
    {
        startManager.Notify("Speichert die Aktuellen Einstellungen", "Saves the Current Settings", "blue", "blue");
    }

    public void SetMenueButton()
    {
        if (startManager.IsGerman == true)
        {
            add[0].GetComponentInChildren<Text>().text = "Neuer Eintrag";
            add[1].GetComponentInChildren<Text>().text = "Neuer Eintrag";
            edit[0].GetComponentInChildren<Text>().text = "Bearbeiten";
            edit[1].GetComponentInChildren<Text>().text = "Bearbeiten";
            remove[0].GetComponentInChildren<Text>().text = "Löschen";
            remove[1].GetComponentInChildren<Text>().text = "Löschen";
            file[0].GetComponentInChildren<Text>().text = "als Datei";
            file[1].GetComponentInChildren<Text>().text = "als Datei";
            file[2].GetComponentInChildren<Text>().text = "als Datei";
            file[3].GetComponentInChildren<Text>().text = "als Datei";
            rpc[0].GetComponentInChildren<Text>().text = "über Rpc";
            rpc[1].GetComponentInChildren<Text>().text = "über Rpc";
            rpc[2].GetComponentInChildren<Text>().text = "über Rpc";
            rpc[3].GetComponentInChildren<Text>().text = "über Rpc";
            www[0].GetComponentInChildren<Text>().text = "über Web";
            www[1].GetComponentInChildren<Text>().text = "über Web";
            www[2].GetComponentInChildren<Text>().text = "über Web";
            www[3].GetComponentInChildren<Text>().text = "über Web";
            Baureihe[0].text = "Baureihe";
            Farbe[0].text = "Farbe";
            Hersteller[0].text = "Hersteller";
            EnergyArt[0].text = "Antriebs System";
            Katalognummer[0].text = "Katalognummer";
            Decoder[0].text = "Decoder";
            Adresse[0].text = "Adresse";
            Seriennummer[0].text = "Seriennummer";
            Preis[0].text = "Preis";
            Protokoll[0].text = "Protokoll";
            Fahstufen[0].text = "Fahrstufen";
            LagerOrt[0].text = "Lagerort";
            Kauftag[0].text = "Kauf Tag";
            WartungTag[0].text = "Wartungs Tag";
            Rauch[0].text = "hat Rauch";
            Telex[0].text = "hat Telex";
            Sound[0].text = "hat Sound";
            LichtWechsel[0].text = "hat Lichtwechsel";
            ElekKupplung[0].text = "hat Elektrische Kupplungen";
            ElektPando[0].text = "hat Elektrische Pandos";
            WeditColor.text = " Farbe";
            WeditHersteller.text = " Hersteller";
            WeditKatalognummer.text = " Katalognummer";
            WeditSeriennummer.text = " Seriennummer";
            WeditPreis.text = "Preis";
            WEditWagonType.text = "Wagon Typ";
            WeditLager.text = "Lagerort";
            WeditKaufTag.text = "Kauf Tag";
            WeditPreiser.text = "Wagon ist mit Preiser";
            WeditLicht.text = "Wagon hat Innenbeleuchtung";
            WeditStromKupplung.text = "Wagon hat Stromführende Kupplung";
        }
        else
        {
            add[0].GetComponentInChildren<Text>().text = "Add";
            edit[0].GetComponentInChildren<Text>().text = "Edit";
            remove[0].GetComponentInChildren<Text>().text = "Delete";
            add[1].GetComponentInChildren<Text>().text = "Add";
            edit[1].GetComponentInChildren<Text>().text = "Edit";
            remove[1].GetComponentInChildren<Text>().text = "Delete";
            file[0].GetComponentInChildren<Text>().text = "as File";
            file[1].GetComponentInChildren<Text>().text = "as File";
            rpc[0].GetComponentInChildren<Text>().text = "as Rpc";
            rpc[1].GetComponentInChildren<Text>().text = "as Rpc";
            www[0].GetComponentInChildren<Text>().text = "as Web";
            www[1].GetComponentInChildren<Text>().text = "as Web";
            file[2].GetComponentInChildren<Text>().text = "as File";
            file[3].GetComponentInChildren<Text>().text = "as File";
            rpc[2].GetComponentInChildren<Text>().text = "as Rpc";
            rpc[3].GetComponentInChildren<Text>().text = "as Rpc";
            www[2].GetComponentInChildren<Text>().text = "as Web";
            www[3].GetComponentInChildren<Text>().text = "as Web";
            Baureihe[0].text = "Model Series";
            Farbe[0].text = "Color";
            Hersteller[0].text = "Manufacturer";
            EnergyArt[0].text = "Drive system";
            Katalognummer[0].text = "Catalog Number";
            Decoder[0].text = "Decoder";
            Adresse[0].text = "Address";
            Seriennummer[0].text = "Serial Number";
            Preis[0].text = "Price";
            Protokoll[0].text = "Protocol";
            Fahstufen[0].text = "Speed Steps";
            LagerOrt[0].text = "Storage Location";
            Kauftag[0].text = "Buy day";
            WartungTag[0].text = "Maintenance day";
            Rauch[0].text = "has Smoke";
            Telex[0].text = "has Telex";
            Sound[0].text = "has Sound";
            LichtWechsel[0].text = "has Lightchange";
            ElekKupplung[0].text = "has Electrical couplings";
            ElektPando[0].text = "has Electrical Pandos";
            WeditColor.text = " Color";
            WeditHersteller.text = " Manufacturer";
            WeditKatalognummer.text = " Catalog Number";
            WeditSeriennummer.text = " Serial Number";
            WeditPreis.text = "Price";
            WEditWagonType.text = "Wagon Type";
            WeditLager.text = "Storage Location";
            WeditKaufTag.text = "Buy day";
            WeditPreiser.text = "has Preiser";
            WeditLicht.text = "has Interiour Light";
            WeditStromKupplung.text = "has Elec. Coupler";
        }
    }

    public void AnonPrice()
    {
        startManager.Notify("Wird für den Durchschnittswert des Artikel Erfasst (Verkaufs Hilfe)", "Recorded for the product's average value (Sales Help)", "blue", "blue");
    }

    public void WActive()
    {
        startManager.Notify("HTML Color Code Für Aktive Wartung", "HTML Color Code For Active Maintenance", "blue", "blue");
    }

    public void WNonActive()
    {
        startManager.Notify("HTML Color Code Für Normale Anzeige", "HTML Color Code For Normal Display", "blue", "blue");
    }

    public void Sender()
    {
        startManager.Notify("Startet bei erfolgreicher Verbindung das Senden der Gesamten Listen zum Empfänger", "If successful, starts sending the entire lists to the recipient", "blue", "blue");
    }

    public void Receiver()
    {
        startManager.Notify("Startet den Empfänger zum Empfänger der gesendeten Listen", "Starts the receiver to the recipient of the sent lists", "blue", "blue");
    }
}