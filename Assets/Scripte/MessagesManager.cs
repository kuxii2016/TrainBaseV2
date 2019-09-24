/*
 * 
 *   TrainBase Message Manager Version 1 from 29.03.2018 written by Michael Kux
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

public class MessagesManager : MonoBehaviour {

    public LogWriterManager Logger;
    public  StartUpManager startmanager;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Message System -> Message is Normal.");
            Logger.PrintLogEnde();
            Logger.PrintLog("MODUL Message System :: 41 Messages Found.");
        }
    }

    //###############TOP Buttons
    public void LokListe()
    {
        Logger.Message("Öffnet die Lokliste, Zeigt alle Gespeicherten Loks", "BLAU");
    }

    public void NeueLok()
    {
        Logger.Message("Fügt eine Lok zur Liste hinzu", "BLAU");
    }

    public void WagonList()
    {
        Logger.Message("Öffnet die Wagonliste, Zeigt alle Gespeicherten Wagons", "BLAU");
    }

    public void NeuerWagon()
    {
        Logger.Message("Fügt einen Wagon zur Liste hinzu", "BLAU");
    }

    public void Einstellungen()
    {
        Logger.Message("Öffnet die Einstellungen für TrainBaseV2", "BLAU");
    }

    public void EinkaufsListe()
    {
        Logger.Message("Öffnet die Inventar liste, Zeigt alle Gespeicherten Items", "BLAU");
    }

    public void Statistik()
    {
        Logger.Message("Öffnet die Statistik, Zeigt nützliche dinge an", "BLAU");
    }

    public void Importer()
    {
        Logger.Message("Öffnet den Importer, Importiert Fremdes Rollmaterial aus dem Import Ordner", "BLAU");
    }

    public void Backups()
    {
        Logger.Message("Öffnet das Backupsystem, Hier kannst du alles Verwalten", "BLAU");
    }

    public void ProgrammInfos()
    {
        Logger.Message("Zeigt ein paar Infos an die mir Wichtig sind", "BLAU");
    }

    public void AnlagenDecoder()
    {
        Logger.Message("Verwaltet Anlagen Decoder", "BLAU");
    }

    //###############Settings
    public void AutoUpdateCheck()
    {
        Logger.Message("Überprüft bei Programmstart Automatisch auf neue Version", "GELB");
    }

    public void LeistungsDaten()
    {
        Logger.Message("Funktion gibt es bis jetzt noch nicht", "ROT");
    }

    public void ZeigeLoBilder()
    {
        Logger.Message("Zeige die Eigenen Lokbilder in der Liste an (Kann längere Ladezeiten Verursachen)", "GELB");
    }

    public void ErstelleLokPreis()
    {
        Logger.Message("Erstellt Eine Preisliste von den Loks in der Statistik", "GELB");
    }

    public void ErstelleWagonPreis()
    {
        Logger.Message("Erstellt Eine Preisliste von den Wagons in der Statistik", "GELB");
    }

    public void ErstelleItemPreis()
    {
        Logger.Message("Erstellt Eine Preisliste von den Items in der Statistik", "GELB");
    }

    public void Debugger()
    {
        Logger.Message("Aktiviert den Debugger (Für Error Report notwendig)", "GELB");
    }

    public void ImagesAutoRead()
    {
        Logger.Message("Prüft auf Fehlende Lokbilder bei Programmstart Automatisch", "GELB");
    }

    public void IErstelleBackup()
    {
        Logger.Message("Erstellt ein Backup, um im Fehlerfall wieder zu diesen Zeitpunkt zu wechseln", "GELB");
    }

    public void BilderNeuEinlesen()
    {
        Logger.Message("Überprüft auf Fehlende Bilder", "GELB");
    }

    public void PrüfeAufNeueVersion()
    {
        Logger.Message("Manuelle Update Überprüfung", "GELB");
    }

    public void ErrorRepoert()
    {
        Logger.Message("Erstellt einen Ordner zur Fehler behebung", "GELB");
    }

    public void Speichern()
    {
        Logger.Message("Speichert die Einstellungen", "GELB");
    }

    public void Sender()
    {
        Logger.Message("Diese Option ist zum Senden von Lokdaten an ein anderes Programm", "GELB");
    }

    public void Empfänger()
    {
        Logger.Message("Diese Option ist zum Empfangen von Lokdaten von einem anderen Programm", "GELB");
    }

    //###############LokList Wagonlist InventoryList
    public void Edit()
    {
        Logger.Message("Bearbeitet den Ausgewählten Eintrag", "GRUEN");
    }

    public void Delete()
    {
        Logger.Message("Löscht den Ausgewählten Eintrag", "GRUEN");
    }

    public void Export()
    {
        Logger.Message("Exportiert den Ausgewählten Eintrag, ist im Export Ordner zu Finden", "GRUEN");
    }

    public void Send()
    {
        Logger.Message("Sendet den Ausgewählten Eintrag, zu einem anderen Empfänger (Verbindung Erforderlich)", "GRUEN");
    }
}