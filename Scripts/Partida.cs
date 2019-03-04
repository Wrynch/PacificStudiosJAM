using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Carta
{
    public string nombre;
    public string tipo;
    public string categoria;
    public string descripcion;
    public string nombreImagen;
    public string rareza;
    public int coste;
    public int ataque;
    public int vida;
    public GameObject carta;
}

public class Partida : MonoBehaviour {

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


    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/
    /*VARIABLES PARTIDA/JUEGO*/

    public string miNombre;
    public int miID;
    public int miDinero;
    public int misVictorias;
    public int misDerrotas;
    private string miIcono;

    public GameObject CanvasLoading;
    public GameObject CanvasJuego;

    public GameObject iconoPropio;
    public GameObject iconoRival;

    public GameObject Carta;
    private List<Carta> Cartas = new List<Carta>();
    private List<string> CartasUsuario = new List<string>();
    private List<Carta> MazoPropio = new List<Carta>();
    private List<Carta> MazoRival = new List<Carta>();

    private string nomPos;

    public GameObject HUD_Info;

    public GameObject audio;
    Object[] miMusica;



    public void Connect()
    {
        
        playerName = "Prueba";

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

    // Use this for initialization
    void Start () {
        Connect();
        HUD_Info.SetActive(false);
        cogerCartasUsuario();

        miMusica = Resources.LoadAll("music", typeof(AudioClip));
        playRandomMusic();
        
        

        

    }
	
	// Update is called once per frame
	void Update ()
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
                        Debug.Log("ConectionServidor: " + splitData[1]);
                        break;

                    case "DC":
                        break;

                    case "EMPEZAR":
                        CanvasLoading.SetActive(false);
                        CanvasJuego.SetActive(true);
                        break;

                    default:
                        break;

                }
                break;

                /* case NetworkEventType.DisconnectEvent:
                     break;*/
        }




        /*CODIGO PARA POSCIONAR CARTA EN POSICION VACIA*/
        /*if (Input.GetKey(KeyCode.D))
        {
            for (int i = 1; i < 6; i++)
            {
                pruebaPos = "PosDefPropia" + i;
                Debug.Log(pruebaPos);
                if(GameObject.Find(pruebaPos).transform.childCount < 1)
                {
                    GameObject.Find("carta (1)").transform.position = GameObject.Find(pruebaPos).transform.position;
                    GameObject.Find(pruebaPos).transform.parent = GameObject.Find("carta (2)").transform;
                    break;
                }
                
            }
             
        }*/
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
    /*private void CrearCarta(int cliente)
    {
        if (cliente == ourClientId)
        {
            Carta c = new Carta();
            c.carta = Instantiate(Carta);

            MazoPropio.Add(c);
        }
    }*/

    public void InvocarCarta(string nombre)
    {
        for (int i = 0 ; i < MazoPropio.Count; i++)
        {
            if (nombre == MazoPropio[i].nombre)
            {
                Debug.Log(nombre);
                Debug.Log(MazoPropio[i].nombre);
                if (MazoPropio[i].tipo == "atacante")
                {
                    for (int j = 1; j < 6; j++)
                    {
                        nomPos = "PosAtaPropia" + j;
                        if (GameObject.Find(nomPos).transform.childCount < 1)
                        {
                            Debug.Log(nomPos);
                            MazoPropio[i].carta.GetComponentInChildren<carta>().transform.position = GameObject.Find(nomPos).transform.position;
                            MazoPropio[i].carta.gameObject.transform.parent = GameObject.Find(nomPos).transform;
                            MazoPropio[i].carta.GetComponentInChildren<carta>().transform.gameObject.tag = "cAtaquePropia";
                            Debug.Log(GameObject.Find(nomPos).transform.childCount);
                            break;
                        }

                    }
                }
                else if(MazoPropio[i].tipo == "defensor")
                {
                    for (int j = 1; j < 6; j++)
                    {
                        nomPos = "PosDefPropia" + j;
                        if (GameObject.Find(nomPos).transform.childCount < 1)
                        {
                            MazoPropio[i].carta.GetComponentInChildren<carta>().transform.position = GameObject.Find(nomPos).transform.position;
                            MazoPropio[i].carta.gameObject.transform.parent = GameObject.Find(nomPos).transform;
                            MazoPropio[i].carta.GetComponentInChildren<carta>().transform.gameObject.tag = "cDefensaPropia";
                            break;
                        }

                    }
                }
            }
        }
    }

    public void volverCartaMano(string nombre)
    {
        for (int i = 0; i < MazoPropio.Count; i++)
        {
            if (nombre == MazoPropio[i].nombre)
            {
                for (int j = 1; j < 8; j++)
                {
                    nomPos = "PosManoPropia" + j;
                    if (GameObject.Find(nomPos).transform.childCount < 1)
                    {
                        MazoPropio[i].carta.GetComponentInChildren<carta>().transform.position = GameObject.Find(nomPos).transform.position;
                        MazoPropio[i].carta.gameObject.transform.parent = GameObject.Find(nomPos).transform;
                        MazoPropio[i].carta.GetComponentInChildren<carta>().transform.gameObject.tag = "cManoPropia";
                        break;
                    }

                }
                
            }
        }
    }

    public void cargarCartaInfo(string nombre, string tipo, string categoria, string rareza, string descripcion, int ataque, int vida, int coste, string imagen)
    {
        HUD_Info.SetActive(true);

        if (rareza == "legendaria")
        {
            GameObject.Find("CartaInfo").GetComponent<Animator>().enabled = true;
            GameObject.Find("CartaInfo").GetComponent<Animator>().Play(imagen, 0);
        }
        else
        {
            GameObject.Find("CartaInfo").GetComponent<Animator>().enabled = false;
            Sprite imagenSprite = Resources.Load<Sprite>("Cartas/"+imagen);
            GameObject.Find("CartaInfo").transform.GetComponent<SpriteRenderer>().sprite = imagenSprite;
        }

        GameObject.Find("Nombre").GetComponent<Text>().text = nombre;
        GameObject.Find("TipoyCategoria").GetComponent<Text>().text = tipo + " - " + categoria;
        GameObject.Find("Descripcion").GetComponent<Text>().text = descripcion;
        GameObject.Find("CartaInfo").transform.GetChild(3).GetComponent<Text>().text = coste.ToString();
        if (tipo == "atacante")
        {
            GameObject.Find("CartaInfo").transform.GetChild(1).gameObject.SetActive(false);
            GameObject.Find("CartaInfo").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("CartaInfo").transform.GetChild(2).gameObject.SetActive(true);
            GameObject.Find("CartaInfo").transform.GetChild(0).GetComponent<Text>().text = ataque.ToString();
            GameObject.Find("CartaInfo").transform.GetChild(2).GetComponent<Text>().text = vida.ToString();
        }
        else
        {
            GameObject.Find("CartaInfo").transform.GetChild(1).gameObject.SetActive(true);
            GameObject.Find("CartaInfo").transform.GetChild(0).gameObject.SetActive(false);
            GameObject.Find("CartaInfo").transform.GetChild(2).gameObject.SetActive(false);
            GameObject.Find("CartaInfo").transform.GetChild(1).GetComponent<Text>().text = vida.ToString();
        }
        


    }
    private void playRandomMusic()
    {
        audio.GetComponent<AudioSource>().clip = miMusica[Random.Range(0, miMusica.Length)] as AudioClip;
        audio.GetComponent<AudioSource>().Play();
    }

    private Sprite BaseToSprite(string base64)
    {
        byte[] b = System.Convert.FromBase64String(base64);

        Texture2D texture = new Texture2D(500, 500);
        texture.LoadImage(b);
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Sprite sprite = Sprite.Create(texture, rect, new Vector2(), 100f);
        return sprite;
    }

    public void getDatos(string miNombre, int miID, int miDinero, int misVictorias, int misDerrotas, string miIcono)
    {
        this.miNombre = miNombre;
        this.miID = miID;
        this.miDinero = miDinero;
        this.misVictorias = misVictorias;
        this.misDerrotas = misDerrotas;
        this.miIcono = miIcono;
    }

    public void cogerCatalogoCartas()
    {
        StartCoroutine(LoadPost("catalogo","http://192.168.6.138:8000/ws/cartas"));

    }

    public void cogerCartasUsuario()
    {
        StartCoroutine(LoadPost("cogerCartasUsuario","http://192.168.6.138:8000/ws/cartasusuario/"+miID));

    }
    IEnumerator LoadPost(string tipo, string URL)
    {
        WWW www = new WWW(URL);

        yield return www;

        switch (tipo)
        {
            case "catalogo":

                JSONObject json = new JSONObject(www.text);
                for (int i = 0; i < json[0].Count; i++)
                {
                    Carta c = new Carta();
                    string[] array = json[0][i]["name"].ToString().Split('"');
                    c.nombre = array[1];
                    array = json[0][i]["alias"].ToString().Split('"');
                    c.nombreImagen = array[1];
                    array = json[0][i]["descripcion"].ToString().Split('"');
                    c.descripcion = System.Text.RegularExpressions.Regex.Unescape(array[1]);
                    array = json[0][i]["categoria"].ToString().Split('"');
                    c.categoria = array[1];
                    array = json[0][i]["rareza"].ToString().Split('"');
                    c.rareza = array[1];
                    array = json[0][i]["tipo"].ToString().Split('"');
                    c.tipo = array[1];
                    c.coste = int.Parse(json[0][i]["coste"].ToString());
                    c.ataque = int.Parse(json[0][i]["ataque"].ToString());
                    c.vida = int.Parse(json[0][i]["defensa"].ToString());
                    Cartas.Add(c);
                }

                break;
            case "cogerCartasUsuario":

                JSONObject json2 = new JSONObject(www.text);
                List<string> CartasUsuario = new List<string>();
                for (int i = 0; i < json2[0].Count; i++)
                {
                    string[] array;
                    array = json2[0][i]["name"].ToString().Split('"');
                    CartasUsuario.Add(array[1]);
                }
                elegirMazo();

                break;
        }
        
        
    }
    private void elegirMazo()
    {
        while(MazoPropio.Count<2)
        {
            int num = Random.Range(0, Cartas.Count-1);
            
            MazoPropio.Add(Cartas[num]);
            Cartas.RemoveAt(num);
            
        }
        Debug.Log(MazoPropio[0].nombre+" - "+MazoPropio[1].nombre);

    }


}
