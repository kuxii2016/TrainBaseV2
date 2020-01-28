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
public class InventoryData : System.Object
{
    public string dbDatum;
    public string dbStueck;
    public string dbBeschreibung;
    public string dbArtkNR;
    public int dbPreis;
    public string dbGuid;
    public Texture2D Image;
}

public class Inventory_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public List<InventoryData> Item;
    public Network_Manager NM;
    [Header("Elements")]
    public GameObject[] Slot;
    public RawImage[] SlotImage;
    public Text[] Created;
    public Text[] Description;
    public Text[] Stock;
    public Text[] Number;
    public Text[] Price;
    public Text[] ID;
    public Toggle[] Selected;
    public Text PageIndex;
    public Text SeitenWert;
    public Button Remove;
    public Button Add;
    [Header("Inventory add Items")]
    public InputField ARTIKELNUMMER;
    public InputField STUECKZAHL;
    public InputField BESCHREIBUNG;
    public InputField PREIS;
    public Text EditWindow;
    public Text UpdateBTN;
    [Header("Workflow")]
    public bool[] SelectedBool;
    public int PageOffset = 0;
    public int PageOffset2 = 10;
    public int CurrentPage = 1;
    public int SelectedID = -1;
    public int CompleteSumm = 0;
    public int SiteSumm = 0;
    public Color Normal;
    public bool EditMode = false;
    public Texture2D[] CacheImage;
    public Texture2D StandArtPic;

    void Start()
    {
        startManager.Log("Lade Inventory_Manager -> Nachricht ist Normal.", "Load Inventory_Manager -> message is normal");
        if (startManager._IsReady == true)
        {
            RefreschIndex();
        }
        else
        {
            startManager.Notify("Warnung Alte Datenbank Erkannt, Bitte Löschen ist nicht nutzbar", "Old Databse Dedected, The Database is Not Useable", "blue", "blue");
        }
        for (int i = 0; i < Item.Count; i++)
        {
            CompleteSumm = CompleteSumm + Item[i].dbPreis;
        }
        if (startManager.IsGerman == true)
        {
            Remove.GetComponentInChildren<Text>().text = "Löschen";
            Add.GetComponentInChildren<Text>().text = "Neuer Eintrag";
        }
        else
        {
            Remove.GetComponentInChildren<Text>().text = "Delete";
            Add.GetComponentInChildren<Text>().text = "New Entry";
        }
    }

    public void RefreschIndex()
    {
        Item = new List<InventoryData>();
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
        dbConnection.Open();
        try
        {
            SqliteCommand cmd = new SqliteCommand("SELECT * FROM Inventory", dbConnection);
            SqliteDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    InventoryData inVD = new InventoryData();
                    inVD.dbDatum = reader.GetString(0);
                    inVD.dbStueck = reader.GetString(1);
                    inVD.dbBeschreibung = reader.GetString(2);
                    inVD.dbArtkNR = reader.GetString(3);
                    inVD.dbPreis = reader.GetChar(4);
                    if(reader[5].GetType() == typeof(DBNull))
                    {
                        inVD.dbGuid = "null";
                    }
                    else
                    {
                        inVD.dbGuid = reader.GetString(5);
                    }
                    Item.Add(inVD);
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            startManager.LogError("Fehler beim Laden der Items.", "Error Loading Item Data", " Inventory_Manager :: ReadAllItems(); Error: " + ex);
            startManager.Error("RefreshIndex(InventoryList);", "" + ex);
        }
        finally
        {
            SelectedBool = new bool[Item.Count];
            CacheImage = new Texture2D[Item.Count];
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Inventory"))
            {
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Inventory");
            }

            for (int i = 0; i < Item.Count; i++)
            {
                if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/" + (i + 1) + "." + UserSettings.ImageType))
                {
                    CacheImage[i] = StandArtPic;
                }
                else
                {
                    StartCoroutine(LoadImage((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/" + (i + 1) + "." + UserSettings.ImageType), i));
                }
            }
            dbConnection.Close();
            dbConnection.Dispose();
            dbConnection = null;
            startManager.Log("Modul Inventory_Manager :: Alle Items Gelesen.", "Modul Inventory_Manager :: All Items are Read");
            startManager.Log("Modul Inventory_Manager :: " + Item.Count + " Items Gefunden", "Modul Inventory_Manager :: " + Item.Count + " Items Found");
            CheckData();
        }
    }

    public void PageVorward()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].SetActive(false);
            Selected[i].isOn = false;
        }
        if (Item.Count >= PageOffset)
        {
            PageOffset2 = PageOffset2 + 10;
            PageOffset = PageOffset + 10;
            CurrentPage = CurrentPage + 1;
            PageIndex.text = CurrentPage.ToString();
        }
        else
        {
            PageOffset2 = 10;
            PageOffset = 0;
            CurrentPage = 1;
            PageIndex.text = CurrentPage.ToString();
        }
    }

    public void PageBack()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].SetActive(false);
            Selected[i].isOn = false;
        }
        if (PageOffset == 0)
        {
        }
        else
        {
            PageOffset2 = PageOffset2 - 10;
            PageOffset = PageOffset - 10;
            CurrentPage = CurrentPage - 1;
            PageIndex.text = CurrentPage.ToString();
        }
    }

    public void PicID(int id)
    {
        SelectedID = id + PageOffset;
    }

    IEnumerator LoadImage(string url, int i)
    {
        Texture2D tex;
        tex = new Texture2D(2, 2, TextureFormat.DXT1, false);
        using (WWW www = new WWW("file:///" + url.Replace("\\", "/")))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            CacheImage[i] = tex;
        }
    }

    public void DeleteItem()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Kein Eintrag Ausgewählt", "No Entry Selected", "red", "red");
        }
        else
        {
            try
            {
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
                dbConnection.Open();
                string sql = "DELETE FROM Inventory WHERE DATUM='" + Item[SelectedID].dbDatum + "' AND FREEa='" + Item[SelectedID].dbGuid + "'  ";
                SqliteCommand Command = new SqliteCommand(sql, dbConnection);
                Command.ExecuteNonQuery();
                dbConnection.Close();
            }
            catch (Exception ex)
            {
                startManager.LogError("Fehler beim Löschen des Items.", "Error Delete Item Data", " Inventory_Manager :: DeleteItem(); Error: " + ex);
                startManager.Error("Delete(InventoryList);", "" + ex);
            }
            finally
            {
                startManager.Log("Modul Inventory_Manager :: Item Geloescht.", "Modul Inventory_Manager :: Item Removed");
            }
        }
        RefreschIndex();
        SelectedID = -1;
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].SetActive(false);
            Selected[i].isOn = false;
        }
    }

    public void SaveItem()
    {
        if (EditMode == false)
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "INSERT into Inventory (ARTKNR , STUECK , BESCHREIBUNG , PREIS, FREEa) VALUES" + " (@ARTKNR , @STUECK , @BESCHREIBUNG ,  @PREIS, @FREEa)";
                command.Parameters.AddWithValue("@ARTKNR", ARTIKELNUMMER.text);
                command.Parameters.AddWithValue("@STUECK", STUECKZAHL.text);
                command.Parameters.AddWithValue("@BESCHREIBUNG", BESCHREIBUNG.text);
                command.Parameters.AddWithValue("@PREIS", PREIS.text);
                command.Parameters.AddWithValue("@FREEa", Guid.NewGuid().ToString());
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern des Items.", "Error Saving Item Data", " Inventory_Manager :: SaveItem(); Error: " + ex);
                    startManager.Error("Save(InventoryList);", "" + ex);
                }
                finally
                {
                    dbConnection.Close();
                    startManager.Log("Modul Inventory_Manager :: Item Gespeichert.", "Modul Inventory_Manager :: Item Saved");
                }
            }
            RefreschIndex();
            SelectedID = -1;
            for (int i = 0; i < Slot.Length; i++)
            {
                Slot[i].SetActive(false);
                Selected[i].isOn = false;
            }
            ARTIKELNUMMER.text = "";
            STUECKZAHL.text = "";
            BESCHREIBUNG.text = "";
            PREIS.text = "";
        }
        else
        {
            UpdateCurrentItem();
        }
    }
    
    void Update()
    {
        for (int i = 0; i < Selected.Length; i++)
        {
            if (Selected[i].isOn == true)
            {
                SelectedID = (i + PageOffset);
            }
        }

        for (int i = PageOffset; i < Item.Count && i < PageOffset2; i++)
        {
            Slot[i - PageOffset].SetActive(true);
            ID[i - PageOffset].text = "ID: " + (i +1);
            Created[i - PageOffset].text = "Gespeichert: " + Item[i].dbDatum;
            Description[i - PageOffset].text = "Titel: " + Item[i].dbBeschreibung;
            Stock[i - PageOffset].text = "Stück: " + Item[i].dbStueck;
            Number[i - PageOffset].text = "Artikel NR: " + Item[i].dbArtkNR;
            Price[i - PageOffset].text = "Preis: " + Item[i].dbPreis.ToString() + " €";
            SlotImage[i - PageOffset].texture = CacheImage[i];
            SiteSumm = SiteSumm + Item[i].dbPreis;
            SeitenWert.text = "Total Items: " + Item.Count + " Seitenwert: " + SiteSumm + "€ Gesammt Wert: " + CompleteSumm + "€";
            if (UserSettings.Premium == true)
            {
                Created[i - PageOffset].color = UserSettings.newCol1;
                Description[i - PageOffset].color = UserSettings.newCol1;
                Stock[i - PageOffset].color = UserSettings.newCol1;
                Number[i - PageOffset].color = UserSettings.newCol1;
                Price[i - PageOffset].color = UserSettings.newCol1;
            }
            else
            {
                Created[i - PageOffset].color = Normal;
                Description[i - PageOffset].color = Normal;
                Stock[i - PageOffset].color = Normal;
                Number[i - PageOffset].color = Normal;
                Price[i - PageOffset].color = Normal;
            }
        }
        SiteSumm = 0;
    }

    public void GetData()
    {
        if (SelectedID == -1)
        {
            startManager.Notify("Kein Eintrag Ausgewählt", "No Entry Selected", "red", "red");
        }
        else
        {
            EditWindow.text = "Eintrag Bearbeiten";
            UpdateBTN.text = "Speichern";
            EditMode = true;
            ARTIKELNUMMER.text = Item[SelectedID].dbArtkNR;
            STUECKZAHL.text = Item[SelectedID].dbStueck;
            BESCHREIBUNG.text = Item[SelectedID].dbBeschreibung;
            PREIS.text = Item[SelectedID].dbPreis.ToString();
        }
    }

    public void SendItem(int id)
    {
        NM.TrySendData("ITEM" + "?" + Item[id].dbStueck.ToString() + "?" + Item[id].dbBeschreibung.ToString() + "?" + Item[id].dbArtkNR.ToString() + "?" + Item[id].dbPreis.ToString() + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null" + "?" + "null");
    }

    public void CheckData()
    {
        for (int i = 0; i < Item.Count; i++)
        {
            if(Item[i].dbGuid == "null")
            {
                var uuid = Guid.NewGuid().ToString();
                Item[i].dbGuid = uuid;
                UpdateItem(Item[i].dbDatum, Item[i].dbBeschreibung, uuid, Item[i].dbArtkNR, Item[i].dbStueck, Item[i].dbPreis.ToString());
            }
        }
    }

    public void UpdateItem(string date, string desc, string uuid, string Art, string stk, string price)
    {
        if (startManager._IsReady == true)
        {
            SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
            using (SqliteCommand command = new SqliteCommand())
            {
                command.Connection = dbConnection;
                command.CommandType = CommandType.Text;
                command.CommandText = "UPDATE Inventory set ARTKNR = @ARTKNR ,STUECK  = @STUECK ,BESCHREIBUNG = @BESCHREIBUNG,PREIS = @PREIS,FREEa = @FREEa WHERE BESCHREIBUNG='" + desc + "' AND DATUM='" + date + "'  ";
                command.Parameters.AddWithValue("@ARTKNR", Art);
                command.Parameters.AddWithValue("@STUECK", stk);
                command.Parameters.AddWithValue("@BESCHREIBUNG", desc);
                command.Parameters.AddWithValue("@PREIS", price);
                command.Parameters.AddWithValue("@FREEa", uuid);
                try
                {
                    dbConnection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqliteException ex)
                {
                    startManager.LogError("Fehler beim Speichern des Items.", "Error Saving Item Data", " Inventory_Manager :: UpdateItem(); Error: " + ex);
                    startManager.Error("UpdateItem(InventoryList);", "" + ex);
                }
                finally
                {
                    dbConnection.Close();
                    startManager.Log("Modul Inventory_Manager :: Item Upgedated.", "Modul Inventory_Manager :: Item Upgedated");
                }
            }
        }
    }

    void UpdateCurrentItem()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "UPDATE Inventory set ARTKNR = @ARTKNR ,STUECK  = @STUECK ,BESCHREIBUNG = @BESCHREIBUNG,PREIS = @PREIS WHERE FREEa='" + Item[SelectedID].dbGuid + "' AND DATUM='" + Item[SelectedID].dbDatum + "'  ";
            command.Parameters.AddWithValue("@ARTKNR", ARTIKELNUMMER.text);
            command.Parameters.AddWithValue("@STUECK", STUECKZAHL.text);
            command.Parameters.AddWithValue("@BESCHREIBUNG", BESCHREIBUNG.text);
            command.Parameters.AddWithValue("@PREIS", PREIS.text);
            try
            {
                dbConnection.Open();
                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                startManager.LogError("Fehler beim Speichern des Items.", "Error Saving Item Data", " Inventory_Manager :: UpdateCurrentItem(); Error: " + ex);
                startManager.Error("UpdateCurrentItem(InventoryList);", "" + ex);
            }
            finally
            {
                dbConnection.Close();
                startManager.Log("Modul Inventory_Manager :: Item Upgedated.", "Modul Inventory_Manager :: Item Upgedated");
                EditMode = false;
                ARTIKELNUMMER.text = "";
                STUECKZAHL.text = "";
                BESCHREIBUNG.text = "";
                PREIS.text = "";
                SelectedID = -1;
                for (int i = 0; i < Selected.Length; i++)
                {
                    Selected[i].isOn = false;
                }
                Item.Clear();
                RefreschIndex();
            }
        }
    }

    public void Break()
    {
        EditWindow.text = "Neuer Artikel Hinzufügen";
        UpdateBTN.text = "Hinzufügen";
        ARTIKELNUMMER.text = "";
        STUECKZAHL.text = "";
        BESCHREIBUNG.text = "";
        PREIS.text = "";
        SelectedID = -1;
        EditMode = false;
        for (int i = 0; i < Selected.Length; i++)
        {
            Selected[i].isOn = false;
        }
    }
}