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

public class NewWagon : MonoBehaviour
{
    public LogWriterManager Logger;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public WagonView lokview;
    public InputField Farbe;
    public Dropdown WagonTyp;
    public Dropdown Hersteller;
    public Dropdown Spurweite;
    public InputField Katalognummer;
    public InputField Seriennummer;
    public Dropdown KaufdatumTag;
    public Dropdown KaufdatumMonat;
    public Dropdown KaufdatumJahr;
    public InputField Preis;
    public Toggle Kupplung;
    public Toggle Licht;
    public Toggle Preiser;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE AddWagon -> Message is Normal.");
        }
    }

    public void SaveWagon()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + Settings.DatabasesName));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Wagons (TYP , FARBE , HERSTELLER ,  KATALOGNUMMER , SERIENNUMMER , KAUFDAY , KAUFMONAT , KAUFJAHR , PREIS , KUPPLUNG , LICHT , PREISER, SPURWEITE, IDENTIFYER , LAGERORT) VALUES" + " (@TYP , @FARBE , @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @KAUFDAY , @KAUFMONAT , @KAUFJAHR , @PREIS , @KUPPLUNG , @LICHT , @PREISER, @SPURWEITE, @IDENTIFYER , @LAGERORT)";
            command.Parameters.AddWithValue("@TYP", WagonTyp.value);
            command.Parameters.AddWithValue("@FARBE", Farbe.text);
            command.Parameters.AddWithValue("@HERSTELLER", Hersteller.value);
            command.Parameters.AddWithValue("@KATALOGNUMMER", Katalognummer.text);
            command.Parameters.AddWithValue("@SERIENNUMMER", Seriennummer.text);
            command.Parameters.AddWithValue("@KAUFDAY", KaufdatumTag.value);
            command.Parameters.AddWithValue("@KAUFMONAT", KaufdatumMonat.value);
            command.Parameters.AddWithValue("@KAUFJAHR", KaufdatumJahr.value);
            command.Parameters.AddWithValue("@PREIS", Preis.text);
            command.Parameters.AddWithValue("@KUPPLUNG", Kupplung.isOn);
            command.Parameters.AddWithValue("@LICHT", Licht.isOn);
            command.Parameters.AddWithValue("@PREISER", Preiser.isOn);
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
                        Logger.Error("MODUL AddWagon :: SaveWagon():  " + ex + "\n");
                    }
                }
                finally
                {
                    dbConnection.Close();
                    StartManager.SystemMeldung.color = Color.green;
                    StartManager.SystemMeldung.text = ("Wagon: " + Katalognummer.text + " in: " + Farbe.text + "  Gespeichert.!");
                    if (Logger.logIsEnabled == true)
                    {
                        Logger.PrintLog("MODUL AddWagon :: save Wagon: " + Katalognummer.text + " in: " + Farbe.text + "  Gespeichert.!");
                    }
                }
            }
            else
            {
                StartManager.SystemMeldung.color = Color.red;
                StartManager.SystemMeldung.text = ("Fehler beim Speichern des Wagons, Aktuelles limit ist bei 240 Wagons.!");
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL AddWagon :: Ups Current Wagon Limit is to Low for your Entry ");
                }
            }
        }
    }
}