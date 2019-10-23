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

}

public class Inventory_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;

    [Header("Elements")]
    public GameObject[] Slot;
    public RawImage[] SlotImage;
    public Text[] Created;
    public Text[] Description;
    public Text[] Stock;
    public Text[] Number;
    public Text[] Price;
    public Toggle[] Selected;

    // Use this for initialization
    void Start ()
    {
        startManager.Log("Lade Inventory_Manager -> Nachricht ist Normal.", "Load Inventory_Manager -> message is normal");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
