using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Translator : MonoBehaviour
{
    public bool German;
    public bool Other;
    public LogWriterManager Logger;
    [Header("MainMenue")]
    public Text LokListe;
    public Text NewLok;
    public Text WagonList;
    public Text NewWagon;
    public Text Settings;
    public Text EinkaufsList;
    public Text Decoder;
    public Text Stats;
    public Text Importer;
    public Text Backups;
    public Text ProgrammInfo;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Translator_Manager -> Message is Normal.");
            if (Application.systemLanguage == SystemLanguage.German)
            {
                German = true;
                Logger.PrintLog("MODUL Translator_Manager :: Setze Sprache auf Deutsch." + "\n");
            }
            else
            {
                Other = true;
                Logger.PrintLog("MODUL Translator_Manager :: Set Laguane to Englisch." + "\n");

            }
        }
    }

    void SetLaguane()
    {
        //** TODO Here the Complete obj's
    
    }
}
