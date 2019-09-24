/*
 * 
 *   TrainBase Settings Manager Version 1 from 25.03.2018 written by Michael Kux
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

public class ProgrammSettings : MonoBehaviour {

    public LogWriterManager Logger;
    [Header("Programm StartLoader")]
    public string Version = "2.0.2";
    [Tooltip("DatenBank Name")]
    public string DatabasesName = "TrainBase.ext2db";
    [Tooltip("Update Check URL")]
    public string UpdateURL = "";
    public bool IsProgrammInEditorMode = false;
    public int LokLimit;
    public int WagonLimit;
    public int InventoryLimit;
    public Text ProgrammVersion;

    private void Start()
    {
        ProgrammVersion.text = "Build:  " + Version.ToString();
    }
}