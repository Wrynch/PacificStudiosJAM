using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player
{
    public string playerName;
    public int posJugador;
    public GameObject avatar;
    public int connectId;
}

public class Menu : MonoBehaviour
{

    /*VARIABLES CONEXION*/
    /*VARIABLES CONEXION*/
    /*VARIABLES CONEXION*/
    /*VARIABLES CONEXION*/

    private const int MAX_CONNECTION = 2;
    private int port = 5701;

    private int hostId;
    private int webHostId;

    private int reliableChannel;
    private int unReliableChannel;

    private float connectionTime;
    private int connectionId;
    private bool isConnected;

    public string playerName;
    private int ourClientId;

    private byte error;

    public GameObject loginCartel;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/

    public Transform jugador1, jugador2;
    public List<Player> jugadores = new List<Player>();

    public int Logeado = 0;

    public float posicionAnterior;
    public void Connect()
    {
        Debug.Log("Nombre del Jugador: " + playerName);
        string pName = GameObject.Find("DatosJugadorLocal").GetComponent<DatosJugadorLocal>().miNombre;

            if (pName == "")
            {
                Debug.Log("Debes escribir un nombre");
                return;
            }

            playerName = pName;

        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unReliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);

        hostId = NetworkTransport.AddHost(topo, 0);
        connectionId = NetworkTransport.Connect(hostId, "127.0.0.1", port, 0, out error);

        connectionTime = Time.time;
        isConnected = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isConnected)
        {
            return;
        }

        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            /*case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                break;*/

            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                string[] splitData = msg.Split('|');
                switch (splitData[0])
                {
                    case "ASKNAME":
                        OnAskName(splitData);
                        break;

                    case "CNN":
                        SpawnPlayer(splitData[1], int.Parse(splitData[2]));
                        Debug.Log("ConectionServidor: " + splitData[1]);
                        break;

                    case "DC":
                        break;

                    case "EMPEZAR":
                        break;

                    default:
                        break;

                }
                break;

                /* case NetworkEventType.DisconnectEvent:
                     break;*/
        }
    }
    private void OnAskName(string[] data)
    {
        //Id del player
        ourClientId = int.Parse(data[1]);


        //Enviar el nombre al servidor
        Send("NAMEIS|" + playerName, reliableChannel);


        //enviar datos al resto de jugadores
        for (int i = 2; i < data.Length - 1; i++)
        {
            string[] d = data[i].Split('%');
            Debug.Log("Conection: " + playerName);
        }

    }
    private void Send(string message, int channelId)
    {
        byte[] msg = Encoding.Unicode.GetBytes(message);
        NetworkTransport.Send(hostId, connectionId, channelId, msg, message.Length * sizeof(char), out error);

    }

    
    private void SpawnPlayer(string playerName, int cnnId)
    {

        Vector3 position1 = new Vector3(-1.48f, -3.99f, 0);
        Vector3 position2 = new Vector3(1.79f, -3.99f, 0);

        if (cnnId == ourClientId)
        {
            loginCartel.SetActive(false);
        }

        Player p = new Player();
        if (cnnId % 2 != 0)
        {
            p.avatar = Instantiate(playerPrefab1, position1, Quaternion.identity);
            playerPrefab2.SetActive(false);
            Logeado = 1;
        }
        else
        {
            p.avatar = Instantiate(playerPrefab2, position2, Quaternion.identity);
            playerPrefab2.SetActive(true);
        }
        
        p.playerName = playerName;
        p.connectId = cnnId;
        p.avatar.GetComponentInChildren<TextMesh>().text = playerName;
        jugadores.Add(p);
    }

    public int getClienteId()
    {

        return connectionId;
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("logeado", Logeado);
        PlayerPrefs.SetString("playerName", playerName);
        
    }
    void OnEnable()
    {
        Logeado = PlayerPrefs.GetInt("logeado");

        if (Logeado == 1)
        {
            playerName = PlayerPrefs.GetString("playerName");
            posicionAnterior = PlayerPrefs.GetFloat("ultimaPosicion");
            loginCartel.SetActive(false);
            Vector3 position3 = new Vector3(posicionAnterior, -3.99f, 0);
            Instantiate(playerPrefab1, position3, Quaternion.identity);
        }
    }

    public void salir()
    {
        Logeado = 0;
        posicionAnterior = -1.48f;
        PlayerPrefs.SetInt("logeado", Logeado);
        PlayerPrefs.SetFloat("ultimaPosicion", posicionAnterior);
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        Logeado = 0;
        posicionAnterior = -1.48f;
        playerName = "";
        PlayerPrefs.SetInt("logeado", Logeado);
        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.SetFloat("ultimaPosicion", posicionAnterior);
        Application.Quit();
    }
}
