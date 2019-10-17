/*
 * 
 *   TrainBase Stats Manager Version 1 from 26.03.2019 written by Michael Kux
 * 
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;

public class StatsManager : MonoBehaviour {

    public ProgrammSettings loader;
    public LogWriterManager Logger;
    public string StartCounter;
    public string UserCounter;
    public string WinUser;
    public string LinuxUser;
    public string AppleUser;
    public string VersionStat;
    public string UUID;
    public string MiddleUUID;
    public string FrontUUID;
    public string LastUUID;
    public string ProgrammVersion;
    public string OS;

    void Start()
    {
        if(Application.isEditor == false)
        {
            OS = SystemInfo.operatingSystemFamily.ToString();
            ProgrammVersion = loader.Version;
            Logger.PrintLog("ENABLE Stats_Manager -> Message is Normal.");
            if (File.Exists(Application.dataPath + "/" + "Config" + "/" + "uuid.pub"))
            {
                MiddleUUID = File.ReadAllText(Application.dataPath + "/" + "Config" + "/" + "uuid.pub");
                FrontUUID = File.ReadAllText(Application.dataPath + "/" + "Config" + "/" + "FrontUUID.pub");
                LastUUID = File.ReadAllText(Application.dataPath + "/" + "Config" + "/" + "LastUUID.pub");
                UUID = FrontUUID + "-" + MiddleUUID + "-" + LastUUID;
            }

            if (UUID == (SystemInfo.processorType + "-" + MiddleUUID + "-" + SystemInfo.systemMemorySize.ToString()))
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: UserPassed.");
                }
            }
            else
            {
                MiddleUUID = System.Guid.NewGuid().ToString();
                FrontUUID = SystemInfo.processorType;
                LastUUID = SystemInfo.systemMemorySize.ToString();

                FileStream fs = new FileStream(Application.dataPath + "/" + "Config" + "/" + "uuid.pub", FileMode.Append, FileAccess.Write, FileShare.Write);
                fs.Close();
                StreamWriter sw = new StreamWriter(Application.dataPath + "/" + "Config" + "/" + "uuid.pub", true, Encoding.ASCII);
                sw.Write(MiddleUUID);
                sw.Close();

                FileStream fss = new FileStream(Application.dataPath + "/" + "Config" + "/" + "FrontUUID.pub", FileMode.Append, FileAccess.Write, FileShare.Write);
                fss.Close();
                StreamWriter sws = new StreamWriter(Application.dataPath + "/" + "Config" + "/" + "FrontUUID.pub", true, Encoding.ASCII);
                sws.Write(FrontUUID);
                sws.Close();

                FileStream fsss = new FileStream(Application.dataPath + "/" + "Config" + "/" + "LastUUID.pub", FileMode.Append, FileAccess.Write, FileShare.Write);
                fsss.Close();
                StreamWriter swss = new StreamWriter(Application.dataPath + "/" + "Config" + "/" + "LastUUID.pub", true, Encoding.ASCII);
                swss.Write(LastUUID);
                swss.Close();
                StartCoroutine(RegisterNewUser());
                UUID = FrontUUID + "-" + MiddleUUID + "-" + LastUUID;
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: New User, Thanks for Using TrainbaseV2.");
                }
            }
            setStats();
            StartCoroutine(SetProgrammVersion());
        }
        else
        {
            Debug.Log("Application is in Editor Mode, No Stats set current.!!");
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Stats_Manager :: Programm is in Editor Mode no New Stats sets Current.!!");
            }
        }
    }

    public void setStats()
    {
        if (Application.isEditor == true)
        {
            if (Logger.logIsEnabled == true)
            {
                Logger.PrintLog("MODUL Stats_Manager :: Programm is in Editor-Mode No Stats set to this Session.!");
            }
        }
        else
        {
            if (OS == "Windows")
            {
                StartCoroutine(Windows());
            }
            else if (OS == "Linux")
            {
                StartCoroutine(Linux());
            }
            else
            {
                StartCoroutine(Sonstige());
            }
        }
    }

    private IEnumerator RegisterNewUser()
    {
        {
            WWW www = new WWW("http://" + UserCounter);
            yield return www;
            if (www.error != null)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: ERROR by set New Userstart +1 ");
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: Rigister New User +1 ");
                }
            }
        }
    }

    private IEnumerator SetProgrammVersion()
    {
        {
            WWW www = new WWW("http://" + VersionStat + loader.Version.ToString() + ".php");
            yield return www;
            if (www.error != null)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: ERROR by Write Programmversion: " + loader.Version + " to Statistik");
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: Write Programmversion: " + loader.Version + " to Statistik");
                }
            }
        }
    }

    private IEnumerator Windows()
    {
        {
            WWW www = new WWW("http://" + WinUser);
            yield return www;
            if (www.error != null)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: ERROR by set Windows +1 ");
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: set Windows +1 ");
                }
            }
        }
    }

    private IEnumerator Linux()
    {
        {
            WWW www = new WWW("http://" + LinuxUser);
            yield return www;
            if (www.error != null)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: ERROR by set Linux +1 ");
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: set Linux +1 ");
                }
            }
        }
    }

    private IEnumerator Sonstige()
    {
        {
            WWW www = new WWW("http://" + AppleUser);
            yield return www;
            if (www.error != null)
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: ERROR by set unknown OS +1 ");
                }
            }
            else
            {
                if (Logger.logIsEnabled == true)
                {
                    Logger.PrintLog("MODUL Stats_Manager :: ERROR by set unknown OS +1 ");
                }
            }
        }
    }
}
