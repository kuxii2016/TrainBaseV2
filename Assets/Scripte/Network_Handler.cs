/*
 * 
 *   TrainBase Network Manager Version 1 from 29.03.2019 written by Michael Kux
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
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Text.RegularExpressions;

[System.Serializable]
public class Trains : System.Object
{
    public string DbBaureihe;
    public string DbFarbe;
    public int DbLokTyp;
    public int DbHersteller;
    public string DbKatalognummer;
    public string DbSeriennummer;
    public int DbKaufTag;
    public int DbKaufMonat;
    public int DbKaufJahr;
    public int DbPreis;
    public int DbWartungTag;
    public int DbWartungMonat;
    public int DbWartungJahr;
    public string DbAdresse;
    public int DbProtokoll;
    public int DbFahrstufen;
    public string DbDecHersteller;
    public string DbDecID;
    public string DbAngelegt;
    public int DbRauch;
    public int DbSound;
    public int DbROTWEISS;
    public int DbBeleuchtung;
    public int DbPandos;
    public int DbTelex;
    public int DbElekKupplung;
    public Int32 DbSpurweite;
    public Int32 DbCV2;
    public Int32 DbCV3;
    public Int32 DbCV4;
    public Int32 DbCV5;
}

public class Network_Handler : MonoBehaviour
{
    [Header("Basic Things")]
    public LogWriterManager Logger;
    public StartUpManager StartManager;
    public ProgrammSettings Settings;
    public SettingsManager USettings;
    public LokView lv;
    public WagonView Wv;
    public InventoryManager Im;
    [Header("Netwerk Settings")]
    int m_ServerSocket;
    int m_ClientSocket;
    public int m_ConnectionID;
    byte m_ChannelID;
    HostTopology m_HostTopology;
    bool m_ClientsActive;
    string myText;
    public InputField m_InputField;
    public Button m_ClientButton, m_ServerButton;
    [Header("Connecton Elements")]
    public InputField ServerIP;
    [Header("Rückgabe Werte")]
    public string ConnectionIp;
    public bool IsServer;
    public bool IsClient;
    public bool IsReady = false;
    public bool IsTrain = false;
    public bool IsWagon = false;
    public string RecieveData;

    public string CacheData0;
    public string CacheData1;
    public string CacheData2;
    public string CacheData3;
    public string CacheData4;
    public string CacheData5;
    public string CacheData6;
    public string CacheData7;
    public string CacheData8;
    public string CacheData9;
    public string CacheData10;
    public string CacheData11;
    public string CacheData12;
    public string CacheData13;
    public string CacheData14;
    public string CacheData15;
    public string CacheData16;
    public string CacheData17;
    public string CacheData18;
    public string CacheData19;
    public string CacheData20;
    public string CacheData21;
    public string CacheData22;
    public string CacheData23;
    public string CacheData24;
    public string CacheData25;
    public string CacheData26;
    public string CacheData27;
    public string CacheData28;
    public string CacheData29;
    public string CacheData30;
    public string CacheData31;
    public string CacheData32;
    public string CacheData33;

    void Start ()
    {
        if (Logger.logIsEnabled == true)
        {
            Logger.PrintLog("ENABLE Network_Handler -> Message is Normal.");
        }
        CreateNewConnection();
        m_ClientsActive = false;
        myText = "Please Type Message Here...";
        ConnectionConfig config = new ConnectionConfig();
        m_ChannelID = config.AddChannel(QosType.Reliable);
        m_HostTopology = new HostTopology(config, 20);
        NetworkTransport.Init();
        m_ClientButton.onClick.AddListener(ClientButton);
        m_ServerButton.onClick.AddListener(ServerButton);
        m_InputField.onEndEdit.AddListener(delegate { SendMessageField(); });
    }

    void SendMyMessage(string textInput)
    {
        byte error;
        byte[] buffer = new byte[1024];
        Stream message = new MemoryStream(buffer);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(message, textInput);
        NetworkTransport.Send(m_ClientSocket, m_ConnectionID, m_ChannelID, buffer, (int)message.Position, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            Logger.PrintLog("Message send error: " + (NetworkError)error);
        }
        Logger.PrintLog("Send to = " + m_ClientSocket + ", connectionId = " + m_ConnectionID + ", channelId = " + m_ChannelID + ", size = " + (int)message.Position + ", error = " + error.ToString());
        Logger.PrintLog("Send data = " + textInput);
    }
    
    void Update ()
    {
        int outHostId;
        int outConnectionId;
        int outChannelId;
        byte[] buffer = new byte[1024];
        int receivedSize;
        byte error;
        NetworkEventType eventType = NetworkTransport.Receive(out outHostId, out outConnectionId, out outChannelId, buffer, buffer.Length, out receivedSize, out error);
        switch (eventType)
        {
            case NetworkEventType.ConnectEvent:
                {
                    OnConnect(outHostId, outConnectionId, (NetworkError)error);
                    break;
                }
            case NetworkEventType.DataEvent:
                {
                    OnData(outHostId, outConnectionId, outChannelId, buffer, receivedSize, (NetworkError)error);
                    break;
                }
            case NetworkEventType.Nothing:
                break;
            default:
                break;
        }
        m_InputField.gameObject.SetActive(m_ClientsActive);
        if (m_ClientsActive)
        {
            m_ClientButton.gameObject.SetActive(false);
            m_ServerButton.gameObject.SetActive(false);
        }
        ConnectionIp = ServerIP.text;
    }

    void OnConnect(int hostID, int connectionID, NetworkError error)
    {
        Logger.PrintLog("OnConnect(hostId = " + hostID + ", connectionId = " + connectionID + ", error = " + error.ToString() + ")");
        m_ClientsActive = true;
    }

    void OnData(int hostId, int connectionId, int channelId, byte[] data, int size, NetworkError error)
    {
        Stream serializedMessage = new MemoryStream(data);
        BinaryFormatter formatter = new BinaryFormatter();
        string message = formatter.Deserialize(serializedMessage).ToString();
        Logger.PrintLog("Receive from = " + hostId + ", connectionId = " + connectionId + ", channelId = " + channelId + ", size = " + size + ", error = " + error.ToString());
        m_InputField.text = "DATA = " + message;
        RecieveData = message;
        string strOne = message;
        string[] strArrayOne = new string[] { "" };
        strArrayOne = strOne.Split(',');
        Logger.PrintLog("Receiv: " + strArrayOne[0] + "||" + strArrayOne[1] + "||" + strArrayOne[2] + "||" + strArrayOne[3] + "||" + strArrayOne[4] + "||" + strArrayOne[5] + "||" + strArrayOne[6] + "||" + strArrayOne[7] + "||" + strArrayOne[8] + "||" + strArrayOne[9] + "||" + strArrayOne[10] + "||" + strArrayOne[11] + "||" + strArrayOne[12] + "||" + strArrayOne[13] + "||" + strArrayOne[14] + "||" + strArrayOne[15] + "||" + strArrayOne[16] + "||" + strArrayOne[17] + "||" + strArrayOne[18] + "||" + strArrayOne[19] + "||" + strArrayOne[20] + "||" + strArrayOne[21] + "||" + strArrayOne[22]);
        StartManager.SystemMeldung.color = Color.cyan;
        StartManager.SystemMeldung.text = ("Data: " + strArrayOne[0] + " in: " + strArrayOne[1] + " Erfolgreich Empfangen.!");
        CacheData0 = strArrayOne[0];
        CacheData1 = strArrayOne[1];
        CacheData2 = strArrayOne[2];
        CacheData3 = strArrayOne[3];
        CacheData4 = strArrayOne[4];
        CacheData5 = strArrayOne[5];
        CacheData6 = strArrayOne[6];
        CacheData7 = strArrayOne[7];
        CacheData8 = strArrayOne[8];
        CacheData9 = strArrayOne[9];
        CacheData10 = strArrayOne[10];
        CacheData11 = strArrayOne[11];
        CacheData12 = strArrayOne[12];
        CacheData13 = strArrayOne[13];
        CacheData14 = strArrayOne[14];
        CacheData15 = strArrayOne[15];
        CacheData16 = strArrayOne[16];
        CacheData17 = strArrayOne[17];
        CacheData18 = strArrayOne[18];
        CacheData19 = strArrayOne[19];
        CacheData20 = strArrayOne[20];
        CacheData21 = strArrayOne[21];
        CacheData22 = strArrayOne[22];
        CacheData23 = strArrayOne[23];
        CacheData24 = strArrayOne[24];
        CacheData25 = strArrayOne[25];
        CacheData26 = strArrayOne[26];
        CacheData27 = strArrayOne[27];
        CacheData28 = strArrayOne[28];
        CacheData29 = strArrayOne[29];
        CacheData30 = strArrayOne[30];
        CacheData31 = strArrayOne[31];
        CacheData32 = strArrayOne[32];
        CacheData33 = strArrayOne[33];

        if (CacheData0 == "TRAIN")
        {
            Logger.PrintLog("Ok, Empfange TRAIN daten..!");
            StartManager.SystemMeldung.color = Color.green;
            StartManager.SystemMeldung.text = ("System Empfängt Train Daten");
            lv.verifyTrainData();
        }

        if (CacheData0 == "WAGON")
        {
            Logger.PrintLog("Ok, Empfange WAGON daten..!");
            StartManager.SystemMeldung.color = Color.green;
            StartManager.SystemMeldung.text = ("System Empfängt Wagon Daten");
            Wv.verifyTrainData();
        }

        if (CacheData0 == "ITEM")
        {
            Logger.PrintLog("Ok, Empfange ITEM daten..!");
            StartManager.SystemMeldung.color = Color.green;
            StartManager.SystemMeldung.text = ("System Empfängt Item Daten");
            Im.VerifyTrainData();
        }
    }

    void ClientButton()
    {
        IsClient = true;
        byte error;
        m_ClientSocket = NetworkTransport.AddHost(m_HostTopology);
        m_ConnectionID = NetworkTransport.Connect(m_ClientSocket, ConnectionIp, 54321, 0, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            StartManager.SystemMeldung.color = Color.red;
            StartManager.SystemMeldung.text = ("TrainBaseV2 Error: " + (NetworkError)error);
        }
        StartManager.SystemMeldung.color = Color.green;
        StartManager.SystemMeldung.text = ("TrainBaseV2 Sender Connected on: " + USettings.IP);
    }

    void ServerButton()
    {
        IsServer = true;
        byte error;
        m_ServerSocket = NetworkTransport.AddHost(m_HostTopology, 54321);
        NetworkTransport.Connect(m_ServerSocket, "127.0.0.1", 54321, 0, out error);

        StartManager.SystemMeldung.color = Color.yellow;
        StartManager.SystemMeldung.text = ("TrainBaseV2 Empfänger Startet on: " + USettings.IP);
    }

    void SendMessageField()
    {
        myText = m_InputField.text;
        SendMyMessage(myText);
    }

    public void TrySendTrainData(string TrainData)
    {
        SendMyMessage(TrainData);
    }

    public void CreateNewConnection()
    {
        CacheData0 = "MOTHERFUCKERABFUCK";
        CacheData1 = "MOTHERFUCKERABFUCK";
        CacheData2 = "MOTHERFUCKERABFUCK";
        CacheData3 = "MOTHERFUCKERABFUCK";
        CacheData4 = "MOTHERFUCKERABFUCK";
        CacheData5 = "MOTHERFUCKERABFUCK";
        CacheData6 = "MOTHERFUCKERABFUCK";
        CacheData7 = "MOTHERFUCKERABFUCK";
        CacheData8 = "MOTHERFUCKERABFUCK";
        CacheData9 = "MOTHERFUCKERABFUCK";
        CacheData10 = "MOTHERFUCKERABFUCK";
        CacheData11 = "MOTHERFUCKERABFUCK";
        CacheData12 = "MOTHERFUCKERABFUCK";
        CacheData13 = "MOTHERFUCKERABFUCK";
        CacheData14 = "MOTHERFUCKERABFUCK";
        CacheData15 = "MOTHERFUCKERABFUCK";
        CacheData16 = "MOTHERFUCKERABFUCK";
        CacheData17 = "MOTHERFUCKERABFUCK";
        CacheData18 = "MOTHERFUCKERABFUCK";
        CacheData19 = "MOTHERFUCKERABFUCK";
        CacheData20 = "MOTHERFUCKERABFUCK";
        CacheData21 = "MOTHERFUCKERABFUCK";
        CacheData22 = "MOTHERFUCKERABFUCK";
        CacheData23 = "MOTHERFUCKERABFUCK";
        CacheData24 = "MOTHERFUCKERABFUCK";
        CacheData25 = "MOTHERFUCKERABFUCK";
        CacheData26 = "MOTHERFUCKERABFUCK";
        CacheData27 = "MOTHERFUCKERABFUCK";
        CacheData28 = "MOTHERFUCKERABFUCK";
        CacheData29 = "MOTHERFUCKERABFUCK";
        CacheData30 = "MOTHERFUCKERABFUCK";
        CacheData31 = "MOTHERFUCKERABFUCK";
        CacheData32 = "MOTHERFUCKERABFUCK";
        CacheData33 = "MOTHERFUCKERABFUCK";
    }
}