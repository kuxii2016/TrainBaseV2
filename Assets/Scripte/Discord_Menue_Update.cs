using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiscordPresence;
using System;

public class Discord_Menue_Update : MonoBehaviour
{
    public long Starttime;

    void Start()
    {
        Starttime = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        PresenceManager.UpdatePresence("Startup", "IDLE", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DLokList()
    {
        PresenceManager.UpdatePresence("View Trainlist", "Page Site: 1", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DNewLok()
    {
        PresenceManager.UpdatePresence("Add ", "New Train", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DWagonList()
    {
        PresenceManager.UpdatePresence("View Wagonlist", "Page Site: 1", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DNewWagon()
    {
        PresenceManager.UpdatePresence("Add", "New Wagon", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DSettings()
    {
        PresenceManager.UpdatePresence("View:", "Settings", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DInventory()
    {
        PresenceManager.UpdatePresence("View Inventorylist", "Page Site: 1", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DDecoder()
    {
        PresenceManager.UpdatePresence("View Decoderlist", "Page Site: 1", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DDecoderADD()
    {
        PresenceManager.UpdatePresence("Add", "New Decoder", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DStats()
    {
        PresenceManager.UpdatePresence("View Stats", "Index", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }

    public void DImporter()
    {
        PresenceManager.UpdatePresence("View Importer", "Index", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");

    }

    public void DBackup()
    {
        PresenceManager.UpdatePresence("View Backup", "Index", Starttime, -1, "icon", "", "", "", "", -1, -1, "", "", "");
    }
}
