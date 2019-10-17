/*
 * 
 *   TrainBase ImageUploader Version 1 from 16.10.2019 written by Michael Kux
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
using Mono.Data.Sqlite;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;
using System.Threading;
using DiscordPresence;

public class Images_Uploader : MonoBehaviour {

    [Header("Basics")]
    public LogWriterManager Logger;
    public GameObject TrainList;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public SettingsManager Usettings;
    public GameObject Trains;
    public GameObject Wagons;
    public Images_Uploader IU;
    public LokView LV;
    public WagonView WV;
    [Header("Window Settings")]
    public Text Path;
    public string DirPath;
    public bool IsTrain = false;
    public bool IsWagon = false;
    public Button[] slots;
    public RawImage[] slotPic;
    public List<string> ImageExtensions = new List<string> { ".JPG", ".jpg", ".BMP", ".bmp", ".PNG", ".png" };
    public List<string> Images = new List<string>();
    public Texture2D[] Pic;
    public int PicID = 0;

    void Start ()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Picture Manager -> Message is Normal.");
        }
        DirPath = (System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory).Replace("\\", "/") + "/" );
        Path.text = DirPath.ToString();
        GetRows();
    }

#pragma warning disable
    public void SetInventar ()
    {
        if (Trains.active == true)
        {
            IsTrain = true;
            IsWagon = false;
            Logger.PrintLog("MODUL Picture_Manager :: Edit Entry is ah Train.");
        }
 
        if (Wagons.active == true)
        {
            IsWagon = true;
            IsTrain = false;
            Logger.PrintLog("MODUL Picture_Manager :: Edit Entry is ah Wagon.");
        }
    }

#pragma warning restore
    public void GetRows()
    {
        string[] imports = Directory.GetFiles(DirPath);
        foreach (var f in imports)
        {
            if (ImageExtensions.Contains(System.IO.Path.GetExtension(f).ToLowerInvariant()))
            {
                Images.Add(f);
                for (int i = 0; i < Images.Count; i++)
                {
                    StartCoroutine(setImage(Images[i] , i));
                    slotPic[i].texture = Pic[i];
                    slots[i].GetComponentInChildren<Text>().text = GetDataName(Images[i].ToString());
                    slots[i].gameObject.SetActive(true);
                }
            }
        }
    }

    public void Refresch()
    {
        for (int i = 0; i < Images.Count; i++)
        {
            Logger.PrintLog("MODUL Picture_Manager :: Refresch cached Pictures.! :: " + i + " Found.");
            Images.Clear();
        }
        GetRows();
    }

    string GetDataName(string path)
    {
        string[] s = path.Split('/');
        return s[s.Length - 1];
    }

    public void SelectedID(int id)
    {
        if (IsTrain == true)
        {
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (LV.SelectedID + 1) + "." + Usettings.ImageType);
            File.Copy(Images[id], System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Trains/" + (LV.SelectedID + 1) + "." + Usettings.ImageType);
            LV.GetLokData();
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Picture_Manager :: Replace Picture from TrainID:   " + LV.SelectedID + " to: " + Images[id]);
            }
        }

        if (IsWagon == true)
        {
            File.Delete(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WV.SelectedID + 1) + "." + Usettings.ImageType);
            File.Copy(Images[id], System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments) + "/TrainBaseV2/Images/Wagons/" + (WV.SelectedID + 1) + "." + Usettings.ImageType);
            WV.GetLokData();
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Picture_Manager :: Replace Picture from WagonID:   " + WV.SelectedID + " to: " + Images[id]);
            }
        }
        PicID = id;
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
