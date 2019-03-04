using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


public class ServerClient
{
    public int connectionId;
    public string playerName;
    public int id;
    public int cantidadVida = 20;
    public int cantidadCementerio = 0;
    public int cantidadMazo = 0;
    public int victorias;
    public int derrotas;
    public int dinero;
    public int coste = 0;
}



public class Servidor : MonoBehaviour
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

    private bool isStarted = false;
    private byte error;

    private List<ServerClient> clients = new List<ServerClient>();

    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/

    // Use this for initialization
    void Start ()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();

        reliableChannel = cc.AddChannel(QosType.Reliable);
        unReliableChannel = cc.AddChannel(QosType.Unreliable);

        HostTopology topo = new HostTopology(cc, MAX_CONNECTION);
        hostId = NetworkTransport.AddHost(topo, port, null);
        webHostId = NetworkTransport.AddWebsocketHost(topo, port, null);

        isStarted = true;

    }
	
	// Update is called once per frame
	void Update () {

        if (!isStarted)
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
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                OnConnection(connectionId);
                break;

            case NetworkEventType.DataEvent:
                string msg = Encoding.Unicode.GetString(recBuffer, 0, dataSize);
                string[] splitData = msg.Split('|');
                switch (splitData[0])
                {
                    case "NAMEIS":
                        OnNameIs(connectionId, splitData[1]);
                        if(connectionId == 2)
                        {
                            
                        }
                        break;

                    case "CNN":
                        break;

                    case "DC":
                        break;

                    default:
                        break;

                }
                break;

            case NetworkEventType.DisconnectEvent:
                break;
        }
    }
    private void OnConnection(int cnnId)
    {
        //Añadir a la lista
        ServerClient c = new ServerClient();
        c.connectionId = cnnId;
        c.playerName = "TEMP";
        clients.Add(c);

        //Despues de añadir el cliente al servidor
        //mandamos un cliente a los clientes
        string msg = "ASKNAME|" + cnnId + "|";

        foreach (ServerClient sc in clients)
            msg += sc.playerName + "%" + sc.connectionId + '|';

        msg = msg.Trim('|');
        //ejemplo de linea de envio --> ASKNAME|1|ANDER%1|
        Send(msg, reliableChannel, cnnId);

    }
    private void Send(string message, int channelId, int cnnId)
    {
        List<ServerClient> c = new List<ServerClient>();
        c.Add(clients.Find(x => x.connectionId == cnnId));
        Send(message, channelId, c);
    }
    private void Send(string message, int channelId, List<ServerClient> c)
    {
        byte[] msg = Encoding.Unicode.GetBytes(message);
        foreach (ServerClient sc in c)
        {
            NetworkTransport.Send(hostId, sc.connectionId, channelId, msg, message.Length * sizeof(char), out error);
        }
    }
    private void OnNameIs(int cnnId, string playerName)
    {
        //Asignar el nombre al id de la conexion
        clients.Find(x => x.connectionId == cnnId).playerName = playerName;



        //Enviar a los demas clientes el jugador conectado
        Send("CNN|" + playerName + '|' + cnnId, reliableChannel, clients);
        Debug.Log(playerName + " " + cnnId);
    }
    private void EmpezarPartida()
    {
        Send("EMPEZAR|", reliableChannel, clients);
    }

    private void RecogerMazoJ1()
    {

    }
    private void RecogerMazoJ2()
    {

    }
}
