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

public class Image_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager UserSettings;
    public Train_List TL;
    public Wagon_List WL;
    public Inventory_Manager IM;
    [Header("Elements")]
    public GameObject ImageView;
    public Text PageIndex;
    public Text Path;
    public Button[] Slots;
    public Texture2D[] Pic;
    public RawImage TrainEditPic;
    public RawImage WagonEditPic;
    public List<string> Images = new List<string>();
    public List<string> ImageExtensions = new List<string> { ".JPG", ".jpg", ".BMP", ".bmp", ".PNG", ".png" };
    public int Type = 0;
    [Header("Workflow")]
    public string DirPath;
    public int PageOffset = 0;
    public int PageOffset2 = 12;
    public int CurrentPage = 1;
    public int SelectedID = -1;
    
    void Start ()
    {
        startManager.Log("Lade Image_Manager -> Nachricht ist Normal.", "Load Image_Manager -> message is normal");
        DirPath = (System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Replace("\\", "/") + "/");
        Path.text = DirPath.ToString();
    }

    private void Update()
    {
        SetScreen();
    }

    public void GetRows()
    {
        Images.Clear();
        string[] imports = Directory.GetFiles(DirPath);
        foreach (var f in imports)
        {
            if (ImageExtensions.Contains(System.IO.Path.GetExtension(f).ToLowerInvariant()))
            {
                Images.Add(f);
                for (int i = 0; i < Images.Count; i++)
                {
                    StartCoroutine(setImage(Images[i], i));
                    Pic = new Texture2D[Images.Count];
                }
            }
        }
        startManager.Log("Modul Image_Manager :: Bilder Gelesen " + Images.Count + " Verwendbare(s) Gefunden", "Modul Image_Manager :: Images Read " + Images.Count + " Useable Images Found");
    }

    public void SetScreen()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].gameObject.SetActive(false);
        }
        for (int i = PageOffset; i < Pic.Length && i < PageOffset2; i++)
        {
            Slots[i - PageOffset].GetComponent<RawImage>().texture = Pic[i];
            Slots[i - PageOffset].GetComponentInChildren<Text>().text = GetDataName(Images[i].ToString());
            Slots[i - PageOffset].gameObject.SetActive(true);
        }
    }

    public void CopyImage(int id)
    {
        if (Type == 0)
        {
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (TL.SelectedID + 1) + "." + "png");
            File.Copy(Images[id], System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (TL.SelectedID + 1) + "." + "png");
            TL.CacheImage[TL.SelectedID] = Pic[id];
            TrainEditPic.texture = Pic[id];
            TL.GetLokData();

        }

        if (Type == 1)
        {
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WL.SelectedID + 1) + "." + "png");
            File.Copy(Images[id], System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WL.SelectedID + 1) + "." + "png");
            WL.CacheImage[WL.SelectedID] = Pic[id];
            WagonEditPic.texture = Pic[id];
            WL.GetLokData();
        }

        if (Type == 2)
        {
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/" + (IM.SelectedID + 1) + "." + "png");
            File.Copy(Images[id], System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Inventory/" + (IM.SelectedID + 1) + "." + "png");
            IM.CacheImage[IM.SelectedID] = Pic[id];
            IM.PrintScreen();
        }
        ImageView.SetActive(false);
    }

    public void SetImageRecorder(int id)
    {
        Type = id;
        startManager.Log("Modul Image_Manager :: Start Image Loader, Typ ist: " + Type + " |0=Lok|1=Wagon|2=Inventar|", "Modul Image_Manager :: Start Image Loader, Typ is: " + Type + " |0=Train|1=Wagon|2=Inventory|");
        startManager.Notify("Zeige Verfügbare Bilder an", "List Valid Images", "blue", "blue");
    }

    public void PageVorward()
    {
        if (Pic.Length >= PageOffset)
        {
            PageOffset2 = PageOffset2 + 28;
            PageOffset = PageOffset + 28;
            CurrentPage = CurrentPage + 1;
            PageIndex.text = CurrentPage.ToString();
        }
        else
        {
            PageOffset2 = 28;
            PageOffset = 0;
            CurrentPage = 1;
            PageIndex.text = CurrentPage.ToString();
        }
    }

    public void PageBack()
    {
        if (PageOffset == 0)
        {
        }
        else
        {
            PageOffset2 = PageOffset2 - 28;
            PageOffset = PageOffset - 28;
            CurrentPage = CurrentPage - 1;
            PageIndex.text = CurrentPage.ToString();
        }
    }

    string GetDataName(string path)
    {
        string[] s = path.Split('/');
        return s[s.Length - 1];
    }

    IEnumerator setImage(string url, int number)
    {
        Texture2D tex;
        tex = new Texture2D(2, 2, TextureFormat.DXT1, false);
        using (WWW www = new WWW("file:///" + url.Replace("\\", "/")))
        {
            yield return www;
            www.LoadImageIntoTexture(tex);
            Pic[number] = tex;
        }
    }
}
