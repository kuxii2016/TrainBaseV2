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
using System.Net.NetworkInformation;

public class Network_Manager : MonoBehaviour
{
    [Header("Depents")]
    public Start_Manager startManager;
    public Settings_Manager settingsManager;
    public Inventory_Manager itemManager;
    public Decoder_Manager decoderManager;
    public Train_List trainList;
    public Wagon_List wagonList;

    [Header("Cache Data")]
    public int FirstConnection = 0;
    public string OtherIP = "0.0.0.0";
    public bool ConnectionIsReady = false;
    [Header("Cache Data")]
    public string RecieveData = null;
    public string CacheData0 = null;
    public string CacheData1 = null;
    public string CacheData2 = null;
    public string CacheData3 = null;
    public string CacheData4 = null;
    public string CacheData5 = null;
    public string CacheData6 = null;
    public string CacheData7 = null;
    public string CacheData8 = null;
    public string CacheData9 = null;
    public string CacheData10 = null;
    public string CacheData11 = null;
    public string CacheData12 = null;
    public string CacheData13 = null;
    public string CacheData14 = null;
    public string CacheData15 = null;
    public string CacheData16 = null;
    public string CacheData17 = null;
    public string CacheData18 = null;
    public string CacheData19 = null;
    public string CacheData20 = null;
    public string CacheData21 = null;
    public string CacheData22 = null;
    public string CacheData23 = null;
    public string CacheData24 = null;
    public string CacheData25 = null;
    public string CacheData26 = null;
    public string CacheData27 = null;
    public string CacheData28 = null;
    public string CacheData29 = null;
    public string CacheData30 = null;
    public string CacheData31 = null;
    public string CacheData32 = null;
    public string CacheData33 = null;
    public string CacheData34 = null;
    byte[] Image;
    [Header("")]
    [Header("Network Settings")]
    public int Beginn = 0;
    public int m_ServerSocket;
    public int m_ClientSocket;
    public int m_ConnectionID;
    public byte m_ChannelID;
    HostTopology m_HostTopology;
    public bool m_ClientsActive;
    public string myText;
    public InputField m_InputField;
    public Button m_ClientButton, m_ServerButton;
    public bool IsServer;
    public bool IsClient;
    public string ConnectionIp;
    public int TickCounts = 0;
    public int TrainTick = 0;
    public int WagonTick = 0;
    public int DecoderTick = 0;
    public int ItemTick = 0;
    public bool Semmel = false;

    void Start()
    {
        startManager.Log("Lade Network_Manager -> Nachricht ist Normal.", "Load Network_Manager -> Nachricht ist Normal.");
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

    void Update() //Init Connection
    {
        TickCounts = itemManager.Item.Count + decoderManager.dbDecoder.Count + trainList.Trains.Count + wagonList.Trains.Count;
        ConnectionIp = settingsManager.SenderIP.text;
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
                    ConnectionIsReady = true;
                    Screen.sleepTimeout = -1;
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
            //m_ClientButton.gameObject.SetActive(false);
            //m_ServerButton.gameObject.SetActive(false);
        }
        if (CacheData0 == "IP")
        {
            if (OtherIP == "0.0.0.0")
            {
                OtherIP = CacheData1;
            }
        }

        if (IsClient == true)
        {
            if (ConnectionIsReady == true)
            {
                if (FirstConnection == 0)
                {
                    SendData("IP?" + Network.player.ipAddress.ToString() + "?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null?null");
                    FirstConnection = 1;
                }
            }
        }
        if (IsClient == true && ConnectionIsReady == true && Beginn == 0)
        {
            Beginn = 1;
            StartCoroutine(PushTick());
        }
    }

    void OnConnect(int hostID, int connectionID, NetworkError error)
    {
        //startManager.Log("Modul Network_Manager :: OnConnect(hostId = " + hostID + ", connectionId = " + connectionID + ", error = " + error.ToString() + ")");
        m_ClientsActive = true;
        settingsManager.SenderIP.gameObject.SetActive(false);
    }

    void OnData(int hostId, int connectionId, int channelId, byte[] data, int size, NetworkError error)
    {
        Stream serializedMessage = new MemoryStream(data);
        BinaryFormatter formatter = new BinaryFormatter();
        string message = formatter.Deserialize(serializedMessage).ToString();
        Debug.Log("Modul Network_Manager :: Receive from = " + hostId + ", connectionId = " + connectionId + ", channelId = " + channelId + ", size = " + size + ", error = " + error.ToString());
        Debug.Log("Modul Network_Manager :: Receive data = " + message);
        m_InputField.text = "Incomming = " + message;
        RecieveData = message;
        string strOne = message;
        string[] strArrayOne = new string[] { "" };
        strArrayOne = strOne.Split('?');
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
        CacheData34 = strArrayOne[34];
        if (CacheData0 == "TRAIN")
        {
            for (int i = 0; i < trainList.Trains.Count; i++)
            {
                if (CacheData32 == trainList.Trains[i].DBIdentifyer)
                {
                    Debug.Log(CacheData32);
                    Debug.Log("Semmel Found: " + i);
                    Semmel = true;
                }
            }
            if (Semmel == true)
            {
                Debug.Log("Semmel is here");

               // startManager.Log("Modul Network_Manager :: Ok, Empfange TRAIN daten..!");
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (Application.persistentDataPath + "/" + "Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "UPDATE Trains  SET BAUREIHE = @BAUREIHE, FARBE = @FARBE , TYP = @TYP , HERSTELLER = @HERSTELLER , KATALOGNUMMER = @KATALOGNUMMER , SERIENNUMMER = @SERIENNUMMER , PREIS = @PREIS , WARTUNGDAY = @WARTUNGDAY ,WARTUNGMONAT = @WARTUNGMONAT , WARTUNGJEAR = @WARTUNGJEAR , ADRESSE = @ADRESSE , PROTOKOLL = @PROTOKOLL , FAHRSTUFEN =@FAHRSTUFEN , DECHERSTELLER = @DECHERSTELLER , KTAG = @KTAG , KMONAT = @KMONAT , KJAHR = @KJAHR, SPURWEITE = @SPURWEITE, CV2 = @CV2,  CV3 = @CV3, CV4 = @CV4, CV5 = @CV5, RAUCH = @RAUCH , SOUND = @SOUND, ROTWEISS = @ROTWEISS, PANDO = @PANDO, TELEX = @TELEX, KUPPLUNG = @KUPPLUNG, LAGERORT = @LAGERORT WHERE IDENTIFYER='" + CacheData32 + "' AND IDENTIFYER='" + CacheData32 + "'  ";
                    command.Parameters.AddWithValue("@BAUREIHE", CacheData1);
                    command.Parameters.AddWithValue("@FARBE", CacheData2);
                    command.Parameters.AddWithValue("@TYP", Int32.Parse(CacheData3));
                    command.Parameters.AddWithValue("@HERSTELLER", Int32.Parse(CacheData4));
                    command.Parameters.AddWithValue("@KATALOGNUMMER", CacheData5);
                    command.Parameters.AddWithValue("@SERIENNUMMER", CacheData6);
                    command.Parameters.AddWithValue("@PREIS", CacheData10);
                    command.Parameters.AddWithValue("@WARTUNGDAY", Int32.Parse(CacheData11));
                    command.Parameters.AddWithValue("@WARTUNGMONAT", Int32.Parse(CacheData12));
                    command.Parameters.AddWithValue("@WARTUNGJEAR", Int32.Parse(CacheData13));
                    command.Parameters.AddWithValue("@ADRESSE", CacheData14);
                    command.Parameters.AddWithValue("@PROTOKOLL", Int32.Parse(CacheData15));
                    command.Parameters.AddWithValue("@FAHRSTUFEN", Int32.Parse(CacheData16));
                    command.Parameters.AddWithValue("@DECHERSTELLER", CacheData17);
                    command.Parameters.AddWithValue("@KTAG", Int32.Parse(CacheData7));
                    command.Parameters.AddWithValue("@KMONAT", Int32.Parse(CacheData8));
                    command.Parameters.AddWithValue("@KJAHR", Int32.Parse(CacheData9));
                    command.Parameters.AddWithValue("@SPURWEITE", Int32.Parse(CacheData27));
                    command.Parameters.AddWithValue("@LAGERORT", Int32.Parse(CacheData33));
                    command.Parameters.AddWithValue("@CV2", Int32.Parse(CacheData28));
                    command.Parameters.AddWithValue("@CV3", Int32.Parse(CacheData29));
                    command.Parameters.AddWithValue("@CV4", Int32.Parse(CacheData30));
                    command.Parameters.AddWithValue("@CV5", Int32.Parse(CacheData31));
                    command.Parameters.AddWithValue("@RAUCH", Int32.Parse(CacheData20));
                    command.Parameters.AddWithValue("@SOUND", Int32.Parse(CacheData21));
                    command.Parameters.AddWithValue("@ROTWEISS", Int32.Parse(CacheData22));
                    command.Parameters.AddWithValue("@PANDO", Int32.Parse(CacheData24));
                    command.Parameters.AddWithValue("@TELEX", Int32.Parse(CacheData25));
                    command.Parameters.AddWithValue("@KUPPLUNG", Int32.Parse(CacheData26));
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        //startManager.LogError("Fehler beim Speichern des Lok Updates.", " Network_Manager :: SaveEditTrain().IsEditMode==False; Error: " + ex);
                    }
                    finally
                    {
                        //startManager.Log("Modul Network_Manager :: Lok Upgedated");
                    }
                    dbConnection.Close();
                    dbConnection = null;
                    trainList.RefreshIntervall();
                    Semmel = false;
                }
            }
            else
            {
                Debug.Log("Semmel is not here");

                //startManager.Log("Modul Network_Manager :: Ok, Empfange TRAIN daten..!");
                SqliteConnection dbConnection = new SqliteConnection("Data Source = " + (Application.persistentDataPath + "/" + "Database/" + "TrainBase.ext2db"));
                using (SqliteCommand command = new SqliteCommand())
                {
                    command.Connection = dbConnection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = "INSERT into Trains (BAUREIHE , FARBE , TYP , HERSTELLER , KATALOGNUMMER , SERIENNUMMER , PREIS , WARTUNGDAY ,WARTUNGMONAT , WARTUNGJEAR ,ADRESSE , PROTOKOLL , FAHRSTUFEN , DECHERSTELLER , RAUCH , SOUND , ROTWEISS ,PANDO , TELEX , KUPPLUNG , KTAG , KMONAT , KJAHR, SPURWEITE, IDENTIFYER, LAGERORT, CV2, CV3, CV4, CV5) VALUES" + " " +
                     "(@BAUREIHE , @FARBE , @TYP ,  @HERSTELLER ,  @KATALOGNUMMER , @SERIENNUMMER , @PREIS , @WARTUNGDAY , @WARTUNGMONAT , @WARTUNGJEAR , @ADRESSE , @PROTOKOLL , @FAHRSTUFEN , @DECHERSTELLER , @RAUCH , @SOUND , @ROTWEISS , @PANDO , @TELEX , @KUPPLUNG, @KTAG , @KMONAT , @KJAHR, @SPURWEITE, @IDENTIFYER, @LAGERORT, @CV2, @CV3, @CV4, @CV5)";
                    command.Parameters.AddWithValue("@BAUREIHE", CacheData1);
                    command.Parameters.AddWithValue("@FARBE", CacheData2);
                    command.Parameters.AddWithValue("@TYP", Int32.Parse(CacheData3));
                    command.Parameters.AddWithValue("@HERSTELLER", Int32.Parse(CacheData4));
                    command.Parameters.AddWithValue("@KATALOGNUMMER", CacheData5);
                    command.Parameters.AddWithValue("@SERIENNUMMER", CacheData6);
                    command.Parameters.AddWithValue("@PREIS", CacheData10);
                    command.Parameters.AddWithValue("@WARTUNGDAY", Int32.Parse(CacheData11));
                    command.Parameters.AddWithValue("@WARTUNGMONAT", Int32.Parse(CacheData12));
                    command.Parameters.AddWithValue("@WARTUNGJEAR", Int32.Parse(CacheData13));
                    command.Parameters.AddWithValue("@ADRESSE", CacheData14);
                    command.Parameters.AddWithValue("@PROTOKOLL", Int32.Parse(CacheData15));
                    command.Parameters.AddWithValue("@FAHRSTUFEN", Int32.Parse(CacheData16));
                    command.Parameters.AddWithValue("@DECHERSTELLER", CacheData17);
                    command.Parameters.AddWithValue("@KTAG", Int32.Parse(CacheData7));
                    command.Parameters.AddWithValue("@KMONAT", Int32.Parse(CacheData8));
                    command.Parameters.AddWithValue("@KJAHR", Int32.Parse(CacheData9));
                    command.Parameters.AddWithValue("@SPURWEITE", Int32.Parse(CacheData27));
                    command.Parameters.AddWithValue("@LAGERORT", Int32.Parse(CacheData33));
                    command.Parameters.AddWithValue("@CV2", Int32.Parse(CacheData28));
                    command.Parameters.AddWithValue("@CV3", Int32.Parse(CacheData29));
                    command.Parameters.AddWithValue("@CV4", Int32.Parse(CacheData30));
                    command.Parameters.AddWithValue("@CV5", Int32.Parse(CacheData31));
                    command.Parameters.AddWithValue("@RAUCH", Int32.Parse(CacheData20));
                    command.Parameters.AddWithValue("@SOUND", Int32.Parse(CacheData21));
                    command.Parameters.AddWithValue("@ROTWEISS", Int32.Parse(CacheData22));
                    command.Parameters.AddWithValue("@PANDO", Int32.Parse(CacheData24));
                    command.Parameters.AddWithValue("@TELEX", Int32.Parse(CacheData25));
                    command.Parameters.AddWithValue("@KUPPLUNG", Int32.Parse(CacheData26));
                    command.Parameters.AddWithValue("@IDENTIFYER", CacheData32);
                    try
                    {
                        dbConnection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (SqliteException ex)
                    {
                        //startManager.LogError("Fehler beim Speichern des Lok Updates.", " Network_Manager :: SaveEditTrain().IsEditMode==False; Error: " + ex);
                    }
                    finally
                    {
                        //startManager.Log("Modul Network_Manager :: Lok Upgedated");
                    }
                    dbConnection.Close();
                    dbConnection = null;
                    trainList.RefreshIntervall();
                }
            }
            Semmel = false;
        }
        else
        if (CacheData0 == "WAGON")
        {
            //startManager.Log("Modul Network_Manager :: Ok, Empfange WAGON daten..!");
        }
        else
        if (CacheData0 == "ITEM")
        {
            //startManager.Log("Modul Network_Manager :: Ok, Empfange ITEM daten..!");
        }
        else
        if (CacheData0 == "Decoder")
        {
            //startManager.Log("Modul Network_Manager :: Ok, Empfange Decoder daten..!");
        }
        else
        if (CacheData0 == "IP")
        {
            //startManager.Log("Modul Network_Manager :: Getting Other IP");
            OtherIP = CacheData1;
        }
        else
        if (CacheData0 == "disconnect")
        {
            startManager.Log("Modul Network_Manager :: Bekomme andere IP", "Modul Network_Manager :: Getting Other IP");
            DisconnectHost();
        }
    }

    void ClientButton() //Open Client -> Receiver
    {
        IsClient = true;
        byte error;
        m_ClientSocket = NetworkTransport.AddHost(m_HostTopology);
        m_ConnectionID = NetworkTransport.Connect(m_ClientSocket, ConnectionIp, 54321, 0, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            //startManager.Log("Modul Network_Manager :: Error: " + (NetworkError)error);
            //startManager.Notify("Verbindungs Fehler.", "red");
        }
    }

    void ServerButton() //Open the Server -> Sender
    {
        //startManager.Notify("Verbindung ist OK.", "green");
        IsServer = true;
        byte error;
        m_ServerSocket = NetworkTransport.AddHost(m_HostTopology, 54321);
        NetworkTransport.Connect(m_ServerSocket, "127.0.0.1", 54321, 0, out error);
    }

    void SendMessageField() //Zwischen Cache Für Debug Daten
    {
        myText = m_InputField.text;
        SendData(myText);
    }

    void SendData(string textInput)
    {
        byte error;
        byte[] buffer = new byte[1024];
        Stream message = new MemoryStream(buffer);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(message, textInput);
        NetworkTransport.Send(m_ClientSocket, m_ConnectionID, m_ChannelID, buffer, (int)message.Position, out error);
        if ((NetworkError)error != NetworkError.Ok)
        {
            //startManager.Log("Modul Network_Manager :: Message send error: " + (NetworkError)error);
        }
        //startManager.Log("Modul Network_Manager :: Send to = " + m_ClientSocket + ", connectionId = " + m_ConnectionID + ", channelId = " + m_ChannelID + ", size = " + (int)message.Position + ", error = " + error.ToString());
        //startManager.Log("Modul Network_Manager :: Send data = " + textInput);
        //startManager.Notify("Sende Daten..", "blue");
    }

    public void DisconnectHost()
    {
        NetworkTransport.RemoveHost(m_ServerSocket);
        ConnectionIsReady = false;
        m_ClientsActive = false;
        IsServer = false;
        Debug.Log("Trenne Verbindung");
    }

    IEnumerator PushTick()
    {
        if (settingsManager.Autosync.isOn == true)
        {
            Debug.Log("Read Data");
            yield return new WaitForSeconds(2);

            if (TrainTick != trainList.Trains.Count)
            {
                trainList.SendTrain(TrainTick);
                TrainTick = TrainTick + 1;
            }
            else
            if (WagonTick != wagonList.Trains.Count)
            {
                WagonTick = WagonTick + 1;
            }
            else
            if (ItemTick != itemManager.Item.Count)
            {
                ItemTick = ItemTick + 1;
            }
            else
            if (DecoderTick != decoderManager.dbDecoder.Count)
            {
                DecoderTick = DecoderTick + 1;
            }
            else
            if (TickCounts == (TrainTick + WagonTick + ItemTick + DecoderTick))
            {
                DisconnectHost();
            }
            StartCoroutine(PushTick());
        }
        else
        {

        }
    }

    public void TrySendTrainData(string TrainData)
    {
        SendData(TrainData);
    }
}