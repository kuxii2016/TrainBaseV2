using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Translator : MonoBehaviour
{
    public bool German;
    public bool Other;
    public LogWriterManager Logger;

    void Start()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Translator_Manager -> Message is Normal.");
            Logger.PrintLogEnde();
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
