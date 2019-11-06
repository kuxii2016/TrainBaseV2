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
    [Header("Slots")]
    public GameObject[] Slot;
    public RawImage[] m83;
    public RawImage[] m84;
    public RawImage[] S88;
    public Text[] Description;
    public Text[] Adresse;
    public Text[] Date;
    public Text[] ID;
    public Toggle[] SelectedToggle;
    public Text PageID;
    [Header("Decoder List Items")]
    public List<DecoderData> dbDecoder;
    public int PageOffset = 0;
    public int PageOffset2 = 12;
    public int CurrentPage = 1;
    public int SelectedID = -1;


    private void FixedUpdate()
    {
        for (int i = PageOffset; i < dbDecoder.Count && i < PageOffset2; i++)
        {
            Slot[i - PageOffset].gameObject.SetActive(true);

            Description[i - PageOffset].text = dbDecoder[i].dbBeschreibung;
            Adresse[i - PageOffset].text = "# " + dbDecoder[i].dbAdress;
            Date[i - PageOffset].text = dbDecoder[i].dbDatum;
            ID[i - PageOffset].text = "ID: " + i;

            if(dbDecoder[i].dbType == 0)
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

    void Start ()
    {
        startManager.Log("Lade Decoder_Liste -> Nachricht ist Normal.", "Load Decoder_List -> message is normal");
        SetStartScreen();
        ReadAllItems();
    }
	
	void Update ()
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
            }
            finally
            {
                startManager.Notify("Decoder wurde Gelöscht id:" + (SelectedID + 1), "Decoder Removed id:" + (SelectedID + 1), "green", "green");
                startManager.Log("Modul Decoder_Liste :: Decoder wurde Gelöscht id:" + SelectedID, "Modul Decoder_Liste :: Decoder Removed id:" + SelectedID);
            }
        }
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
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            startManager.LogError("Fehler beim Laden der Decoder.", "Error Loading Decoder Data", " Decoder_Liste :: ReadAllItems(); Error: " + ex);
        }
        finally
        {
            startManager.Log("Modul Decoder_Liste :: Alle Decoder Eingelesen.", "Modul Decoder_List :: All Decoders are Read");
        }

        dbConnection.Close();
        dbConnection = null;
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
}
