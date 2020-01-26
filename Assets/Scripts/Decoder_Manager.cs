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
public class DecoderData : System.Object
{
    public string dbDatum;
    public string dbBeschreibung;
    public string dbArtkNR;
    public int dbType;
    public string db1;
    public string db2;
    public string db3;
    public string db4;
    public string db5;
    public string db6;
    public string db7;
    public string db8;
    public string db9;
    public string db10;
    public string db11;
    public string db12;
    public string db13;
    public string db14;
    public string db15;
    public string db16;
    public string db17;
    public string dbIdentifyer;
    public string dbAdress;
}

public class Decoder_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public Network_Manager NM;
    [Header("Slots")]
    public GameObject[] Slot;
    public RawImage[] m83;
    public RawImage[] m84;
    public RawImage[] S88;
    public Text[] Description;
    public Text[] Adresse;
    public Text[] Date;
    public Text[] ID;
    public InputField[] InputFields;
    public Text[] PlaceHolder;
    public Toggle[] SelectedToggle;
    public Text PageID;
    public GameObject Edit_M83;
    public GameObject Edit_M84;
    public GameObject Edit_S88;
    [Header("Decoder List Items")]
    public List<DecoderData> dbDecoder;
    public int PageOffset = 0;
    public int PageOffset2 = 12;
    public int CurrentPage = 1;
    public int SelectedID = -1;
    public string DecoderType = "Null";
    public bool Edit = false;
    
    private void FixedUpdate()
    {
        for (int i = PageOffset; i < dbDecoder.Count && i < PageOffset2; i++)
        {
            Slot[i - PageOffset].gameObject.SetActive(true);

            Description[i - PageOffset].text = dbDecoder[i].dbBeschreibung;
            Adresse[i - PageOffset].text = "# " + dbDecoder[i].dbAdress;
            Date[i - PageOffset].text = dbDecoder[i].dbDatum;
            ID[i - PageOffset].text = "ID: " + i;

            if (dbDecoder[i].dbType == 0)
            {
                S88[i - PageOffset].gameObject.SetActive(true);
            }

            if (dbDecoder[i].dbType == 1)
            {
                m83[i - PageOffset].gameObject.SetActive(true);
            }

            if (dbDecoder[i].dbType == 2)
            {
                m84[i - PageOffset].gameObject.SetActive(true);
            }
        }
    }

    void Start()
    {
        startManager.Log("Lade Decoder_Liste -> Nachricht ist Normal.", "Load Decoder_List -> message is normal");
        SetStartScreen();
        ReadAllItems();
        IsS88();
    }

    void Update()
    {
        for (int i = 0; i < SelectedToggle.Length; i++)
        {
            if (SelectedToggle[i].isOn == true)
            {
                SelectedID = (i + PageOffset);
            }
        }
    }

    public void Delete()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Kein Decoder ausgewählt", "No Decoder Selected", "red", "red");
        }
        else
        {
            try
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                dbConnection.Open();
                string sql = "DELETE FROM DECODER WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "' AND  DATUM='" + dbDecoder[SelectedID].dbDatum + "'  ";
                SqliteCommand Command = new SqliteCommand(sql, dbConnection);
                Command.ExecuteNonQuery();
                dbConnection.Close();
                dbDecoder.RemoveAt(SelectedID);
                ReadAllItems();
                SelectedID = -1;
            }
            catch (SqliteException ex)
            {
                startManager.LogError("Modul Decoder_Liste :: Decoder wurde Gelöscht id:" + SelectedID, "Modul Decoder_Liste :: Decoder Removed id:" + SelectedID, " Decoder_Liste :: Delete(); Error: " + ex);
                startManager.Error("Delete(Decoder);", "" + ex);
            }
            finally
            {
                startManager.Notify("Decoder wurde Gelöscht id:" + (SelectedID + 1), "Decoder Removed id:" + (SelectedID + 1), "green", "green");
            }
            startManager.Log("Modul Decoder_Liste :: Decoder wurde Gelöscht id:" + SelectedID, "Modul Decoder_Liste :: Decoder Removed id:" + SelectedID);
        }
        SelectedID = -1;
        SelectedToggle[0].isOn = false;
        SelectedToggle[1].isOn = false;
        SelectedToggle[2].isOn = false;
        SelectedToggle[3].isOn = false;
        SelectedToggle[4].isOn = false;
        SelectedToggle[5].isOn = false;
    }

    public void ReadAllItems()
    {
        dbDecoder = new List<DecoderData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM DECODER", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    DecoderData RdbDecoder = new DecoderData();
                    RdbDecoder.dbDatum = reader.GetString(0);
                    RdbDecoder.dbBeschreibung = reader.GetString(1);
                    RdbDecoder.dbType = reader.GetInt32(3);
                    RdbDecoder.db1 = reader.GetString(4);
                    RdbDecoder.db2 = reader.GetString(5);
                    RdbDecoder.db3 = reader.GetString(6);
                    RdbDecoder.db4 = reader.GetString(7);
                    RdbDecoder.db5 = reader.GetString(8);
                    RdbDecoder.db6 = reader.GetString(9);
                    RdbDecoder.db7 = reader.GetString(10);
                    RdbDecoder.db8 = reader.GetString(11);
                    RdbDecoder.db9 = reader.GetString(12);
                    RdbDecoder.db10 = reader.GetString(13);
                    RdbDecoder.db11 = reader.GetString(14);
                    RdbDecoder.db12 = reader.GetString(15);
                    RdbDecoder.db13 = reader.GetString(16);
                    RdbDecoder.db14 = reader.GetString(17);
                    RdbDecoder.db15 = reader.GetString(18);
                    RdbDecoder.db16 = reader.GetString(19);
                    RdbDecoder.dbIdentifyer = reader.GetString(21);
                    if (reader[22].GetType() != typeof(DBNull))
                    {
                        RdbDecoder.dbAdress = reader.GetString(22);
                    }
                    else
                    {
                        RdbDecoder.dbAdress = "0";
                    }
                    dbDecoder.Add(RdbDecoder);
                }
            }
        }
        catch (SqliteException ex)
        {
            startManager.LogError("Fehler beim Laden der Decoder.", "Error Loading Decoder Data", " Decoder_Liste :: ReadAllItems(); Error: " + ex);
            startManager.Error("ReadAll(Decoder);", "" + ex);
        }
        finally
        {
            startManager.Log("Modul Decoder_Liste :: Alle Decoder Eingelesen.", "Modul Decoder_List :: All Decoders are Read");
            dbConnection.Close();
            dbConnection.Dispose();
            dbConnection = null;
        }

        startManager.Log("Modul Decoder_Liste :: "+ (dbDecoder.Count) +" Gespeicherte Decoder Gefunden.", "Modul Decoder_List :: " + (dbDecoder.Count) + " Saved Decoder Found.");
    }

    public void PageVorward()
    {
        if (dbDecoder.Count >= PageOffset)
        {
            PageOffset2 = PageOffset2 + 6;
            PageOffset = PageOffset + 6;
            CurrentPage = CurrentPage + 1;
            PageID.text = CurrentPage.ToString();
        }
        else
        {
            PageOffset2 = 6;
            PageOffset = 0;
            CurrentPage = 1;
            PageID.text = CurrentPage.ToString();
        }
    }

    public void PageBack()
    {
        if (PageOffset == 0)
        {
        }
        else
        {
            PageOffset2 = PageOffset2 - 6;
            PageOffset = PageOffset - 6;
            CurrentPage = CurrentPage - 1;
            PageID.text = CurrentPage.ToString();
        }
    }

    public void SetStartScreen()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].SetActive(false);
            m83[i].gameObject.SetActive(false);
            m84[i].gameObject.SetActive(false);
            S88[i].gameObject.SetActive(false);
        }
    }

    public void IsS88()
    {
        DecoderType = "S88";
        InputFields[13].gameObject.SetActive(true);
        InputFields[14].gameObject.SetActive(true);
        InputFields[15].gameObject.SetActive(true);
        InputFields[16].gameObject.SetActive(true);
        InputFields[17].gameObject.SetActive(true);
        PlaceHolder[0].text = "Port : 0 -> Masse";
        PlaceHolder[1].text = "Port : 1";
        PlaceHolder[2].text = "Port : 2";
        PlaceHolder[3].text = "Port : 3";
        PlaceHolder[4].text = "Port : 4";
        PlaceHolder[5].text = "Port : 5";
        PlaceHolder[6].text = "Port : 6";
        PlaceHolder[7].text = "Port : 7";
        PlaceHolder[8].text = "Port : 8";
        PlaceHolder[9].text = "Port : 9";
        PlaceHolder[10].text = "Port : 10";
        PlaceHolder[11].text = "Port : 11";
        PlaceHolder[12].text = "Port : 12";
        PlaceHolder[13].text = "Port : 13";
        PlaceHolder[14].text = "Port : 14";
        PlaceHolder[15].text = "Port : 15";
        PlaceHolder[16].text = "Port : 16";

    }

    public void IsM83()
    {
        DecoderType = "M83";
        InputFields[13].gameObject.SetActive(false);
        InputFields[14].gameObject.SetActive(false);
        InputFields[15].gameObject.SetActive(false);
        InputFields[16].gameObject.SetActive(false);
        InputFields[17].gameObject.SetActive(false);
        PlaceHolder[0].text = "Port : 1";
        PlaceHolder[1].text = "Port : 1";
        PlaceHolder[2].text = "Port : 1";
        PlaceHolder[3].text = "Port : 2";
        PlaceHolder[4].text = "Port : 2";
        PlaceHolder[5].text = "Port : 2";
        PlaceHolder[6].text = "Port : 3";
        PlaceHolder[7].text = "Port : 3";
        PlaceHolder[8].text = "Port : 3";
        PlaceHolder[9].text = "Port : 4";
        PlaceHolder[10].text = "Port : 4";
        PlaceHolder[11].text = "Port : 4";
    }

    public void IsM84()
    {
        DecoderType = "M84";
        InputFields[13].gameObject.SetActive(false);
        InputFields[14].gameObject.SetActive(false);
        InputFields[15].gameObject.SetActive(false);
        InputFields[16].gameObject.SetActive(false);
        InputFields[17].gameObject.SetActive(false);
        PlaceHolder[0].text = "Port : 1";
        PlaceHolder[1].text = "Port : 1";
        PlaceHolder[2].text = "Port : 1";
        PlaceHolder[3].text = "Port : 2";
        PlaceHolder[4].text = "Port : 2";
        PlaceHolder[5].text = "Port : 2";
        PlaceHolder[6].text = "Port : 3";
        PlaceHolder[7].text = "Port : 3";
        PlaceHolder[8].text = "Port : 3";
        PlaceHolder[9].text = "Port : 4";
        PlaceHolder[10].text = "Port : 4";
        PlaceHolder[11].text = "Port : 4";
    }

    public void SaveDecoder()
    {
        if (Edit == false)
        {
            if (DecoderType == "S88")
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into DECODER (BESCHREIBUNG , TYPE , OUT1 , OUT2 , OUT3 , OUT4 , OUT5 ,OUT6 , OUT7 ,OUT8 , OUT9 , OUT10 , OUT11 , OUT12 , OUT13 , OUT14 ,OUT15 , OUT16 , IDENTIFYER ) VALUES" + " (@BESCHREIBUNG , @TYPE , @OUT1 , @OUT2 , @OUT3 , @OUT4 , @OUT5 ,@OUT6 , @OUT7 ,@OUT8 , @OUT9 , @OUT10 , @OUT11 , @OUT12 , @OUT13 , @OUT14 ,@OUT15 , @OUT16 , @IDENTIFYER)";
                    command.Parameters.AddWithValue("@BESCHREIBUNG", InputFields[0].text);
                    command.Parameters.AddWithValue("@TYPE", 0);
                    command.Parameters.AddWithValue("@OUT1", InputFields[1].text);
                    command.Parameters.AddWithValue("@OUT2", InputFields[2].text);
                    command.Parameters.AddWithValue("@OUT3", InputFields[3].text);
                    command.Parameters.AddWithValue("@OUT4", InputFields[4].text);
                    command.Parameters.AddWithValue("@OUT5", InputFields[5].text);
                    command.Parameters.AddWithValue("@OUT6", InputFields[6].text);
                    command.Parameters.AddWithValue("@OUT7", InputFields[7].text);
                    command.Parameters.AddWithValue("@OUT8", InputFields[8].text);
                    command.Parameters.AddWithValue("@OUT9", InputFields[9].text);
                    command.Parameters.AddWithValue("@OUT10", InputFields[10].text);
                    command.Parameters.AddWithValue("@OUT11", InputFields[11].text);
                    command.Parameters.AddWithValue("@OUT12", InputFields[12].text);
                    command.Parameters.AddWithValue("@OUT13", InputFields[13].text);
                    command.Parameters.AddWithValue("@OUT14", InputFields[14].text);
                    command.Parameters.AddWithValue("@OUT15", InputFields[15].text);
                    command.Parameters.AddWithValue("@OUT16", InputFields[16].text);
                    command.Parameters.AddWithValue("@OUT16", InputFields[17].text);
                    command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        startManager.LogError("Fehler beim Speichern des Decoder.", "Error Saving Decoder Data", " Decoder_Liste :: SaveDecoder::S88(); Error: " + ex);
                        startManager.Error("Savedecoder(S88);", "" + ex);
                    }
                    finally
                    {
                        dbConnection.Close();
                        startManager.Log("Modul Decoder_Liste :: Decoder Gespeichert", "Modul Decoder_List :: Decoder Saved");
                    }
                }
            }

            if (DecoderType == "M83")
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into DECODER (BESCHREIBUNG , TYPE , OUT1 , OUT2 , OUT3 , OUT4 , OUT5 ,OUT6 , OUT7 ,OUT8 , OUT9 , OUT10 , OUT11 , OUT12 , OUT13 , OUT14 ,OUT15 , OUT16 , IDENTIFYER ) VALUES" + " (@BESCHREIBUNG , @TYPE , @OUT1 , @OUT2 , @OUT3 , @OUT4 , @OUT5 ,@OUT6 , @OUT7 ,@OUT8 , @OUT9 , @OUT10 , @OUT11 , @OUT12 , @OUT13 , @OUT14 ,@OUT15 , @OUT16 , @IDENTIFYER)";
                    command.Parameters.AddWithValue("@BESCHREIBUNG", InputFields[0].text);
                    command.Parameters.AddWithValue("@TYPE", 1);
                    command.Parameters.AddWithValue("@OUT1", InputFields[1].text);
                    command.Parameters.AddWithValue("@OUT2", InputFields[2].text);
                    command.Parameters.AddWithValue("@OUT3", InputFields[3].text);
                    command.Parameters.AddWithValue("@OUT4", InputFields[4].text);
                    command.Parameters.AddWithValue("@OUT5", InputFields[5].text);
                    command.Parameters.AddWithValue("@OUT6", InputFields[6].text);
                    command.Parameters.AddWithValue("@OUT7", InputFields[7].text);
                    command.Parameters.AddWithValue("@OUT8", InputFields[8].text);
                    command.Parameters.AddWithValue("@OUT9", InputFields[9].text);
                    command.Parameters.AddWithValue("@OUT10", InputFields[10].text);
                    command.Parameters.AddWithValue("@OUT11", InputFields[11].text);
                    command.Parameters.AddWithValue("@OUT12", InputFields[12].text);
                    command.Parameters.AddWithValue("@OUT13", "null");
                    command.Parameters.AddWithValue("@OUT14", "null");
                    command.Parameters.AddWithValue("@OUT15", "null");
                    command.Parameters.AddWithValue("@OUT16", "null");
                    command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        startManager.LogError("Fehler beim Speichern des Decoder.", "Error Saving Decoder Data", " Decoder_Liste :: SaveDecoder::M83(); Error: " + ex);
                        startManager.Error("Savedecoder(M83);", "" + ex);
                    }
                    finally
                    {
                        dbConnection.Close();
                        startManager.Log("Modul Decoder_Liste :: Decoder Gespeichert", "Modul Decoder_List :: Decoder Saved");
                    }
                }
            }

            if (DecoderType == "M84")
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into DECODER (BESCHREIBUNG , TYPE , OUT1 , OUT2 , OUT3 , OUT4 , OUT5 ,OUT6 , OUT7 ,OUT8 , OUT9 , OUT10 , OUT11 , OUT12 , OUT13 , OUT14 ,OUT15 , OUT16 , IDENTIFYER  ) VALUES" + " (@BESCHREIBUNG , @TYPE , @OUT1 , @OUT2 , @OUT3 , @OUT4 , @OUT5 ,@OUT6 , @OUT7 ,@OUT8 , @OUT9 , @OUT10 , @OUT11 , @OUT12 , @OUT13 , @OUT14 ,@OUT15 , @OUT16 , @IDENTIFYER )";
                    command.Parameters.AddWithValue("@BESCHREIBUNG", InputFields[0].text);
                    command.Parameters.AddWithValue("@TYPE", 2);
                    command.Parameters.AddWithValue("@OUT1", InputFields[1].text);
                    command.Parameters.AddWithValue("@OUT2", InputFields[2].text);
                    command.Parameters.AddWithValue("@OUT3", InputFields[3].text);
                    command.Parameters.AddWithValue("@OUT4", InputFields[4].text);
                    command.Parameters.AddWithValue("@OUT5", InputFields[5].text);
                    command.Parameters.AddWithValue("@OUT6", InputFields[6].text);
                    command.Parameters.AddWithValue("@OUT7", InputFields[7].text);
                    command.Parameters.AddWithValue("@OUT8", InputFields[8].text);
                    command.Parameters.AddWithValue("@OUT9", InputFields[9].text);
                    command.Parameters.AddWithValue("@OUT10", InputFields[10].text);
                    command.Parameters.AddWithValue("@OUT11", InputFields[11].text);
                    command.Parameters.AddWithValue("@OUT12", InputFields[12].text);
                    command.Parameters.AddWithValue("@OUT13", "null");
                    command.Parameters.AddWithValue("@OUT14", "null");
                    command.Parameters.AddWithValue("@OUT15", "null");
                    command.Parameters.AddWithValue("@OUT16", "null");
                    command.Parameters.AddWithValue("@IDENTIFYER", System.Guid.NewGuid().ToString());
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        startManager.LogError("Fehler beim Speichern des Decoder.", "Error Saving Decoder Data", " Decoder_Liste :: SaveDecoder::M84(); Error: " + ex);
                        startManager.Error("Savedecoder(M84);", "" + ex);
                    }
                    finally
                    {
                        dbConnection.Close();
                        startManager.Log("Modul Decoder_Liste :: Decoder Gespeichert", "Modul Decoder_List :: Decoder Saved");
                    }
                }
            }
        }
        else
        {
            Edit = false;
            if (dbDecoder[SelectedID].dbType == 0)
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE DECODER SET BESCHREIBUNG = @BESCHREIBUNG, OUT1 = @OUT1, OUT2 = @OUT2 , OUT3 = @OUT3 , OUT4 = @OUT4 , OUT5 = @OUT5 ,OUT6 = @OUT6 , OUT7 = @OUT7 ,OUT8 = @OUT8 , OUT9 = @OUT9 , OUT10 = @OUT10 , OUT11 = @OUT11 , OUT12 = @OUT12 , OUT13 = @OUT13 , OUT14 = @OUT14 ,OUT15 = @OUT15 , OUT16 = @OUT16  WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "' AND DATUM='" + dbDecoder[SelectedID].dbDatum + "'  ";
                    command.Parameters.AddWithValue("@BESCHREIBUNG", InputFields[0].text);
                    command.Parameters.AddWithValue("@TYPE", 0);
                    command.Parameters.AddWithValue("@OUT1", InputFields[1].text);
                    command.Parameters.AddWithValue("@OUT2", InputFields[2].text);
                    command.Parameters.AddWithValue("@OUT3", InputFields[3].text);
                    command.Parameters.AddWithValue("@OUT4", InputFields[4].text);
                    command.Parameters.AddWithValue("@OUT5", InputFields[5].text);
                    command.Parameters.AddWithValue("@OUT6", InputFields[6].text);
                    command.Parameters.AddWithValue("@OUT7", InputFields[7].text);
                    command.Parameters.AddWithValue("@OUT8", InputFields[8].text);
                    command.Parameters.AddWithValue("@OUT9", InputFields[9].text);
                    command.Parameters.AddWithValue("@OUT10", InputFields[10].text);
                    command.Parameters.AddWithValue("@OUT11", InputFields[11].text);
                    command.Parameters.AddWithValue("@OUT12", InputFields[12].text);
                    command.Parameters.AddWithValue("@OUT13", InputFields[13].text);
                    command.Parameters.AddWithValue("@OUT14", InputFields[14].text);
                    command.Parameters.AddWithValue("@OUT15", InputFields[15].text);
                    command.Parameters.AddWithValue("@OUT16", InputFields[16].text);
                    command.Parameters.AddWithValue("@OUT16", InputFields[17].text);
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        startManager.LogError("Fehler beim Bearbeiten des Decoder.", "Error Edit Decoder Data", " Decoder_Liste :: SaveDecoder::Edit = true; S88(); Error: " + ex);
                        startManager.Error("UpdateDecoder(S88);", "" + ex);
                    }
                    finally
                    {
                        dbConnection.Close();
                        startManager.Log("Modul Decoder_Liste :: Decoder Gespeichert", "Modul Decoder_List :: Decoder Saved");
                    }
                }
            }

            if (dbDecoder[SelectedID].dbType == 1)
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE DECODER SET BESCHREIBUNG = @BESCHREIBUNG, OUT1 = @OUT1, OUT2 = @OUT2 , OUT3 = @OUT3 , OUT4 = @OUT4 , OUT5 = @OUT5 ,OUT6 = @OUT6 , OUT7 = @OUT7 ,OUT8 = @OUT8 , OUT9 = @OUT9 , OUT10 = @OUT10 , OUT11 = @OUT11 , OUT12 = @OUT12 , OUT13 = @OUT13 , OUT14 = @OUT14 ,OUT15 = @OUT15 , OUT16 = @OUT16  WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "' AND DATUM='" + dbDecoder[SelectedID].dbDatum + "'  ";
                    command.Parameters.AddWithValue("@BESCHREIBUNG", InputFields[0].text);
                    command.Parameters.AddWithValue("@TYPE", 1);
                    command.Parameters.AddWithValue("@OUT1", InputFields[1].text);
                    command.Parameters.AddWithValue("@OUT2", InputFields[2].text);
                    command.Parameters.AddWithValue("@OUT3", InputFields[3].text);
                    command.Parameters.AddWithValue("@OUT4", InputFields[4].text);
                    command.Parameters.AddWithValue("@OUT5", InputFields[5].text);
                    command.Parameters.AddWithValue("@OUT6", InputFields[6].text);
                    command.Parameters.AddWithValue("@OUT7", InputFields[7].text);
                    command.Parameters.AddWithValue("@OUT8", InputFields[8].text);
                    command.Parameters.AddWithValue("@OUT9", InputFields[9].text);
                    command.Parameters.AddWithValue("@OUT10", InputFields[10].text);
                    command.Parameters.AddWithValue("@OUT11", InputFields[11].text);
                    command.Parameters.AddWithValue("@OUT12", InputFields[12].text);
                    command.Parameters.AddWithValue("@OUT13", "null");
                    command.Parameters.AddWithValue("@OUT14", "null");
                    command.Parameters.AddWithValue("@OUT15", "null");
                    command.Parameters.AddWithValue("@OUT16", "null");
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        startManager.LogError("Fehler beim Bearbeiten des Decoder.", "Error Edit Decoder Data", " Decoder_Liste :: SaveDecoder::Edit = true; M83(); Error: " + ex);
                        startManager.Error("UpdateDecoder(M83);", "" + ex);
                    }
                    finally
                    {
                        dbConnection.Close();
                        startManager.Log("Modul Decoder_Liste :: Decoder Gespeichert", "Modul Decoder_List :: Decoder Saved");
                    }
                }
            }

            if (dbDecoder[SelectedID].dbType == 2)
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE DECODER SET BESCHREIBUNG = @BESCHREIBUNG, OUT1 = @OUT1, OUT2 = @OUT2 , OUT3 = @OUT3 , OUT4 = @OUT4 , OUT5 = @OUT5 ,OUT6 = @OUT6 , OUT7 = @OUT7 ,OUT8 = @OUT8 , OUT9 = @OUT9 , OUT10 = @OUT10 , OUT11 = @OUT11 , OUT12 = @OUT12 , OUT13 = @OUT13 , OUT14 = @OUT14 ,OUT15 = @OUT15 , OUT16 = @OUT16  WHERE IDENTIFYER='" + dbDecoder[SelectedID].dbIdentifyer + "' AND DATUM='" + dbDecoder[SelectedID].dbDatum + "'  ";
                    command.Parameters.AddWithValue("@BESCHREIBUNG", InputFields[0].text);
                    command.Parameters.AddWithValue("@TYPE", 2);
                    command.Parameters.AddWithValue("@OUT1", InputFields[1].text);
                    command.Parameters.AddWithValue("@OUT2", InputFields[2].text);
                    command.Parameters.AddWithValue("@OUT3", InputFields[3].text);
                    command.Parameters.AddWithValue("@OUT4", InputFields[4].text);
                    command.Parameters.AddWithValue("@OUT5", InputFields[5].text);
                    command.Parameters.AddWithValue("@OUT6", InputFields[6].text);
                    command.Parameters.AddWithValue("@OUT7", InputFields[7].text);
                    command.Parameters.AddWithValue("@OUT8", InputFields[8].text);
                    command.Parameters.AddWithValue("@OUT9", InputFields[9].text);
                    command.Parameters.AddWithValue("@OUT10", InputFields[10].text);
                    command.Parameters.AddWithValue("@OUT11", InputFields[11].text);
                    command.Parameters.AddWithValue("@OUT12", InputFields[12].text);
                    command.Parameters.AddWithValue("@OUT13", "null");
                    command.Parameters.AddWithValue("@OUT14", "null");
                    command.Parameters.AddWithValue("@OUT15", "null");
                    command.Parameters.AddWithValue("@OUT16", "null");
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        startManager.LogError("Fehler beim Bearbeiten des Decoder.", "Error Edit Decoder Data", " Decoder_Liste :: SaveDecoder::Edit = true; M84(); Error: " + ex);
                        startManager.Error("UpdateDecoder(M84);", "" + ex);
                    }
                    finally
                    {
                        dbConnection.Close();
                        startManager.Log("Modul Decoder_Liste :: Decoder Gespeichert", "Modul Decoder_List :: Decoder Saved");
                    }
                }
            }
        }
        SelectedID = -1;
        SelectedToggle[0].isOn = false;
        SelectedToggle[1].isOn = false;
        SelectedToggle[2].isOn = false;
        SelectedToggle[3].isOn = false;
        SelectedToggle[4].isOn = false;
        SelectedToggle[5].isOn = false;
    }

    public void GetSelcetedData()
    {
        Edit = true;
        if (dbDecoder[SelectedID].dbType == 0) //S88 Decoder 
        {
            Edit_S88.gameObject.SetActive(true);
            Edit_M84.gameObject.SetActive(false);
            Edit_M83.gameObject.SetActive(false);
            IsS88();
            InputFields[2].text = dbDecoder[SelectedID].db1;
            InputFields[3].text = dbDecoder[SelectedID].db2;
            InputFields[4].text = dbDecoder[SelectedID].db3;
            InputFields[5].text = dbDecoder[SelectedID].db4;
            InputFields[6].text = dbDecoder[SelectedID].db5;
            InputFields[7].text = dbDecoder[SelectedID].db6;
            InputFields[8].text = dbDecoder[SelectedID].db7;
            InputFields[9].text = dbDecoder[SelectedID].db8;
            InputFields[10].text = dbDecoder[SelectedID].db9;
            InputFields[11].text = dbDecoder[SelectedID].db10;
            InputFields[12].text = dbDecoder[SelectedID].db11;
            InputFields[13].text = dbDecoder[SelectedID].db12;
            InputFields[14].text = dbDecoder[SelectedID].db13;
            InputFields[15].text = dbDecoder[SelectedID].db14;
            InputFields[16].text = dbDecoder[SelectedID].db15;
            InputFields[17].text = dbDecoder[SelectedID].db16;
            InputFields[0].text = dbDecoder[SelectedID].dbBeschreibung;
        }

        if (dbDecoder[SelectedID].dbType == 1) //M83 Decoder
        {
            IsM83();
            Edit_M83.gameObject.SetActive(true);
            Edit_M84.gameObject.SetActive(false);
            Edit_S88.gameObject.SetActive(false);
            InputFields[1].text = dbDecoder[SelectedID].db1;
            InputFields[2].text = dbDecoder[SelectedID].db2;
            InputFields[3].text = dbDecoder[SelectedID].db3;
            InputFields[4].text = dbDecoder[SelectedID].db4;
            InputFields[5].text = dbDecoder[SelectedID].db5;
            InputFields[6].text = dbDecoder[SelectedID].db6;
            InputFields[7].text = dbDecoder[SelectedID].db7;
            InputFields[8].text = dbDecoder[SelectedID].db8;
            InputFields[9].text = dbDecoder[SelectedID].db9;
            InputFields[10].text = dbDecoder[SelectedID].db10;
            InputFields[11].text = dbDecoder[SelectedID].db11;
            InputFields[12].text = dbDecoder[SelectedID].db12;
            InputFields[0].text = dbDecoder[SelectedID].dbBeschreibung;
        }

        if (dbDecoder[SelectedID].dbType == 2) //M84 Decoder
        {
            IsM84();
            Edit_M84.gameObject.SetActive(true);
            Edit_M83.gameObject.SetActive(false);
            Edit_S88.gameObject.SetActive(false);
            InputFields[1].text = dbDecoder[SelectedID].db1;
            InputFields[2].text = dbDecoder[SelectedID].db2;
            InputFields[3].text = dbDecoder[SelectedID].db3;
            InputFields[4].text = dbDecoder[SelectedID].db4;
            InputFields[5].text = dbDecoder[SelectedID].db5;
            InputFields[6].text = dbDecoder[SelectedID].db6;
            InputFields[7].text = dbDecoder[SelectedID].db7;
            InputFields[8].text = dbDecoder[SelectedID].db8;
            InputFields[9].text = dbDecoder[SelectedID].db9;
            InputFields[10].text = dbDecoder[SelectedID].db10;
            InputFields[11].text = dbDecoder[SelectedID].db11;
            InputFields[12].text = dbDecoder[SelectedID].db12;
            InputFields[0].text = dbDecoder[SelectedID].dbBeschreibung;
        }
    }

    public void SendDecoder(int id)
    {
        NM.TrySendData("Decoder" + "?" + dbDecoder[id].dbDatum.ToString() + "?" + dbDecoder[id].dbBeschreibung.ToString() + "?" +"null" + "?" + dbDecoder[id].dbType.ToString() + "?" + dbDecoder[id].db1.ToString() + "?" + dbDecoder[id].db2.ToString() + "?" + dbDecoder[id].db3.ToString() + "?" + dbDecoder[id].db4.ToString() + "?" + dbDecoder[id].db5.ToString() + "?" + dbDecoder[id].db6.ToString() + "?" + dbDecoder[id].db7.ToString() + "?" + dbDecoder[id].db8.ToString() + "?" + dbDecoder[id].db9.ToString() + "?" + dbDecoder[id].db10.ToString() + "?" + dbDecoder[id].db11.ToString() + "?" + dbDecoder[id].db12.ToString() + "?" + dbDecoder[id].db13.ToString() + "?" + dbDecoder[id].db14.ToString() + "?" + dbDecoder[id].db15.ToString() + "?" + dbDecoder[id].db16.ToString() + "?" + "null" + "?" + dbDecoder[id].dbIdentifyer.ToString() + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null");
    }
}