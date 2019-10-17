/*
 * 
 *   TrainBase SaveTrain Manager Version 1 from 25.03.2018 written by Michael Kux
 *    *   Last Edit 31.08.2018
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

public class NewTrain : MonoBehaviour
{
    public LogWriterManager Logger;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public LokView lokview;
    public InputField Baureihe;
    public InputField Farbe;
    public Dropdown LokTyp;
    public Dropdown Hersteller;
    public Dropdown Spurweite;
    public InputField Katalognummer;
    public InputField Seriennummer;
    public Dropdown KaufdatumTag;
    public Dropdown KaufdatumMonat;
    public Dropdown KaufdatumJahr;
    public InputField Preis;
    public Dropdown WartungTag;
    public Dropdown WartungMonat;
    public Dropdown WartungJahr;
    public InputField Adresse;
    public Dropdown Protokoll;
    public Dropdown Fahrstufen;
    public InputField DecHersteller;
    public Toggle Rauch;
    public Toggle Sound;
    public Toggle ROTWEISS;
    public Toggle Pandos;
    public Toggle Telex;
    public Toggle ElekKupplung;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE AddTrain -> Message is Normal.");
        }
    }

    public void SaveTrain()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Trains (BAUREIHE , FARBE , TYP , HERSTELLER , KATALOGNUMMER , SERIENNUMMER , PREIS , WARTUNGDAY ,WARTUNGMONAT , WARTUNGJEAR ,ADRESSE , PROTOKOLL , FAHRSTUFEN , DECHERSTELLER , RAUCH , SOUND , ROTWEISS ,PANDO , TELEX , KUPPLUNG , KTAG , KMONAT , KJAHR, SPURWEITE, IDENTIFYER, LAGERORT) VALUES" + " (@BAUREIHE , @FARBE , @TYP ,  @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @PREIS , @WARTUNGDAY , @WARTUNGMONAT , @WARTUNGJEAR , @ADRESSE , @PROTOKOLL , @FAHRSTUFEN , @DECHERSTELLER , @RAUCH , @SOUND , @ROTWEISS , @PANDO , @TELEX , @KUPPLUNG, @KTAG , @KMONAT , @KJAHR, @SPURWEITE, @IDENTIFYER, @LAGERORT)";
            command.Parameters.AddWithValue("@BAUREIHE", Baureihe.text);
            command.Parameters.AddWithValue("@FARBE", Farbe.text);
            command.Parameters.AddWithValue("@TYP", LokTyp.value);
            command.Parameters.AddWithValue("@HERSTELLER", Hersteller.value);
            command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer.text);
            command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer.text);
            command.Parameters.AddWithValue("@PREIS", Preis.text);
            command.Parameters.AddWithValue("@WARTUNGDAY", WartungTag.value);
            command.Parameters.AddWithValue("@WARTUNGMONAT", WartungMonat.value);
            command.Parameters.AddWithValue("@WARTUNGJEAR", WartungJahr.value);
            command.Parameters.AddWithValue("@ADRESSE", Adresse.text);
            command.Parameters.AddWithValue("@PROTOKOLL", Protokoll.value);
            command.Parameters.AddWithValue("@FAHRSTUFEN", Fahrstufen.value);
            command.Parameters.AddWithValue("@DECHERSTELLER", DecHersteller.text);
            command.Parameters.AddWithValue("@RAUCH", Rauch.isOn);
            command.Parameters.AddWithValue("@SOUND", Sound.isOn);
            command.Parameters.AddWithValue("@ROTWEISS", ROTWEISS.isOn);
            command.Parameters.AddWithValue("@PANDO", Pandos.isOn);
            command.Parameters.AddWithValue("@TELEX", Telex.isOn);
            command.Parameters.AddWithValue("@KUPPLUNG", ElekKupplung.isOn);
            command.Parameters.AddWithValue("@KTAG", WartungTag.value);
            command.Parameters.AddWithValue("@KMONAT", WartungMonat.value); 
            command.Parameters.AddWithValue("@KJAHR", KaufdatumJahr.value);
            command.Parameters.AddWithValue("@SPURWEITE", Spurweite.value);
            command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
            command.Parameters.AddWithValue("@LAGERORT", 0);
            if (lokview.Trains.Count <= Settings.LokLimit)
            {
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    StartManager.SystemMeldung.color = Color.red;
                    StartManager.SystemMeldung.text = ("Error: 12 Write to Bank");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL AddTrain :: Save new Train: " + ex + "\n");
                    }
                }
                finally
                {
                    dbConnection.Close();
                    StartManager.SystemMeldung.color = Color.green;
                    StartManager.SystemMeldung.text = (Baureihe.text + " ADR: " + Adresse.text + " in: " + Farbe.text + "  Gespeichert.!");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL AddTrain :: " + Baureihe.text + "ADR: " + Adresse.text + " in: " + Farbe.text + "  Gespeichert.!");
                    }
                }
            }
            else
            {
                StartManager.SystemMeldung.color = Color.red;
                StartManager.SystemMeldung.text = ("Fehler beim Speichern der Lok Aktuelles limit ist bei 240 Loks.!");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL AddTrain :: Current Train Limit is to Low for your Entry ");
                }
            }
        }
    }
}