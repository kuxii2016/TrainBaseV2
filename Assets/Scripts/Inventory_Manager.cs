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
    public Texture2D Image;
}

public class Inventory_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public List<InventoryData> Item;
    [Header("Elements")]
    public GameObject[] Slot;
    public RawImage[] SlotImage;
    public Text[] Created;
    public Text[] Description;
    public Text[] Stock;
    public Text[] Number;
    public Text[] Price;
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
    [Header("Workflow")]
    public bool[] SelectedBool;
    public int PageOffset = 0;
    public int PageOffset2 = 10;
    public int CurrentPage = 1;
    public int SelectedID = -1;
    public int CompleteSumm = 0;
    public int SiteSumm = 0;
    public Color Normal;
    public Texture2D[] CacheImage;

    void Start()
    {
        startManager.Log("Lade Inventory_Manager -> Nachricht ist Normal.", "Load Inventory_Manager -> message is normal");
        ReadAllItems();
        PrintScreen();
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

    void SetDefaultScreen()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            Slot[i].SetActive(false);
        }
    }

    public void ReadAllItems()
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
                    Item.Add(inVD);
                }
            }
            reader.Close();
            reader = null;
        }
        catch (SqliteException ex)
        {
            startManager.LogError("Fehler beim Laden der Items.", "Error Loading Item Data", " Inventory_Manager :: ReadAllItems(); Error: " + ex);
        }
        SelectedBool = new bool[Item.Count];
        CacheImage = new Texture2D[Item.Count];
        if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Inventory"))
        {
            Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Images/" + "Inventory");
        }

        for (int i = 0; i < Item.Count; i++)
        {
            if (!Directory.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/"))
            {
                Directory.CreateDirectory(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/");
            }
            if (!File.Exists(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/" + (i + 1) + "." + UserSettings.ImageType))
            {
                File.Copy(Application.streamingAssetsPath + "/Resources/Item.png", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/" + (i + 1) + "." + UserSettings.ImageType);
                startManager.Log("Modul Inventory_Manager :: Lok ID: " + i + " Kein Bild vorhanden, Erstelle standart Bild.", "Modul Inventory_Manager :: Lok ID: " + i + " No picture available, Create standard Picture");
            }
            else
            {
                StartCoroutine(LoadImage((System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/" + (i + 1) + "." + UserSettings.ImageType), i));
            }
        }
        dbConnection.Close();
        dbConnection = null;
        startManager.Log("Modul Inventory_Manager :: Alle Items Gelesen.", "Modul Inventory_Manager :: All Items are Read");
        startManager.Log("Modul Inventory_Manager :: " +Item.Count + " Items Gefunden", "Modul Inventory_Manager :: " + Item.Count + " Items Found");
    }

    public void PrintScreen()
    {
        SetDefaultScreen();
        for (int i = PageOffset; i < Item.Count && i < PageOffset2; i++)
        {
            Slot[i - PageOffset].SetActive(true);
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

    public void PageVorward()
    {
        if (Item.Count >= PageOffset)
        {
            PageOffset2 = PageOffset2 + 10;
            PageOffset = PageOffset + 10;
            CurrentPage = CurrentPage + 1;
            PageIndex.text = CurrentPage.ToString();
            PrintScreen();
        }
        else
        {
            PageOffset2 = 12;
            PageOffset = 0;
            CurrentPage = 1;
            PageIndex.text = CurrentPage.ToString();
            PrintScreen();
        }
    }

    public void PageBack()
    {
        if (PageOffset == 0)
        {
        }
        else
        {
            PageOffset2 = PageOffset2 - 10;
            PageOffset = PageOffset - 10;
            CurrentPage = CurrentPage - 1;
            PageIndex.text = CurrentPage.ToString();
            PrintScreen();
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
                string sql = "DELETE FROM Inventory WHERE ARTKNR='" + Item[SelectedID].dbArtkNR + "' AND STUECK='" + Item[SelectedID].dbStueck + "' AND  BESCHREIBUNG='" + Item[SelectedID].dbBeschreibung + "' AND PREIS='" + Item[SelectedID].dbPreis + "'  ";
                SqliteCommand Command = new SqliteCommand(sql, dbConnection);
                Command.ExecuteNonQuery();
                dbConnection.Close();
                SelectedID = -1;
            }
            catch (Exception ex)
            {
                startManager.LogError("Fehler beim Löschen des Items.", "Error Delete Item Data", " Inventory_Manager :: DeleteItem(); Error: " + ex);
            }
            finally
            {
                startManager.Log("Modul Inventory_Manager :: Item Geloescht.", "Modul Inventory_Manager :: Item Removed");
            }
        }
        SelectedID = -1;
        Selected[0].isOn = false;
        Selected[1].isOn = false;
        Selected[2].isOn = false;
        Selected[3].isOn = false;
        Selected[4].isOn = false;
        Selected[5].isOn = false;
        Selected[6].isOn = false;
        Selected[7].isOn = false;
        Selected[8].isOn = false;
        Selected[9].isOn = false;
    }

    public void SaveItem()
    {
        SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2" + "/Database/" + "TrainBase.ext2db"));
        using (SqliteCommand command = new SqliteCommand())
        {
            command.Connection = dbConnection;
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT into Inventory (ARTKNR , STUECK , BESCHREIBUNG , PREIS) VALUES" + " (@ARTKNR , @STUECK , @BESCHREIBUNG ,  @PREIS)";
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
                startManager.LogError("Fehler beim Speichern des Items.", "Error Saving Item Data", " Inventory_Manager :: SaveItem(); Error: " + ex);
            }
            finally
            {
                dbConnection.Close();
                startManager.Log("Modul Inventory_Manager :: Item Gespeichert.", "Modul Inventory_Manager :: Item Saved");
            }
        }
        SelectedID = -1;
        Selected[0].isOn = false;
        Selected[1].isOn = false;
        Selected[2].isOn = false;
        Selected[3].isOn = false;
        Selected[4].isOn = false;
        Selected[5].isOn = false;
        Selected[6].isOn = false;
        Selected[7].isOn = false;
        Selected[8].isOn = false;
        Selected[9].isOn = false;
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
}