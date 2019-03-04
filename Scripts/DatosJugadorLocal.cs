using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DatosJugadorLocal : MonoBehaviour {
    Scene escenaActual;

    public string miNombre;
    public int miID;
    public int miDinero;
    public int misVictorias;
    public int misDerrotas;
    private string miIcono;

    public int Logeado = 0;
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public float posicionAnterior;
    public GameObject loginCartel;
    public InputField inputNombre;
    public InputField inputPassword;

    // Use this for initialization
    void Start () {
        if (GameObject.FindGameObjectsWithTag("DatosJugador").Length>1 && miNombre=="")
        {
            Destroy(this.gameObject);
        }
        inputNombre.text = "Aitor";
        inputPassword.text = "Almi123";
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public DatosJugadorLocal(string miNombre, int miID, int miDinero, int misVictorias, int misDerrotas, string miIcono)
    {
        //DontDestroyOnLoad(this.gameObject);
        this.miNombre = miNombre;
        this.miID = miID;
        this.miDinero = miDinero;
        this.misVictorias = misVictorias;
        this.misDerrotas = misDerrotas;
        this.miIcono = miIcono;
    }

    public void cogerDatosUsuario()
    {
        DontDestroyOnLoad(this.gameObject);
        miNombre = "Dam";
        SpawnPlayer();
    }
    private void SpawnPlayer()
    {

        Vector3 position1 = new Vector3(-1.48f, -3.99f, 0);

        Player p = new Player();
        int prefab = Random.Range(1, 2);
        if (prefab == 1)
        {
            p.avatar = Instantiate(playerPrefab1, position1, Quaternion.identity);
        }
        else
        {
            p.avatar = Instantiate(playerPrefab2, position1, Quaternion.identity);
        }
        loginCartel.SetActive(false);
        Logeado = 1;
        PlayerPrefs.SetInt("logeado", Logeado);
    }

    void OnDisable()
    {
        PlayerPrefs.SetInt("logeado", Logeado);
    }
    void OnLevelWasLoaded()
    {
        escenaActual = SceneManager.GetActiveScene();
        if (escenaActual.name =="Menu")
        {
            Logeado = PlayerPrefs.GetInt("logeado");

            if (Logeado == 1)
            {
                posicionAnterior = PlayerPrefs.GetFloat("ultimaPosicion");
                Vector3 position3 = new Vector3(posicionAnterior, -3.99f, 0);
                Instantiate(playerPrefab1, position3, Quaternion.identity);
                loginCartel.SetActive(false);
            }
        }else if(escenaActual.name == "Partida")
        {
            GameObject.Find("Partida").GetComponent<Partida>().getDatos(miNombre, miID, miDinero, misVictorias, misDerrotas, miIcono);
        }
    }

    void salir()
    {
        Logeado = 0;
        posicionAnterior = -1.48f;
        PlayerPrefs.SetInt("logeado", Logeado);
        PlayerPrefs.SetFloat("ultimaPosicion", 0F);
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        Logeado = 0;
        posicionAnterior = -1.48f;
        miNombre = "";
        miID = 0;
        miDinero = 0;
        misVictorias = 0;
        misDerrotas = 0;
        PlayerPrefs.SetInt("logeado", Logeado);
        PlayerPrefs.SetFloat("ultimaPosicion", 0F);
        Application.Quit();
    }

    public void IniciarSesion()
    {
        Debug.Log(inputNombre.text+" - "+ inputPassword.text);
        StartCoroutine(LoadPost("http://192.168.6.138:8000/ws/iniciosesion/" + inputNombre.text + "-" + inputPassword.text));

    }
    IEnumerator LoadPost(string URL)
    {
        WWW www = new WWW(URL);
        
        yield return www;
        
        JSONObject json = new JSONObject(www.text);

        if (json[0][0] != null)
        {
            DontDestroyOnLoad(this.gameObject);
            miID = int.Parse(json[0][0]["id"].ToString());
            string[] array = json[0][0]["nombre"].ToString().Split('"');
            miNombre = array[1];
            misVictorias = int.Parse(json[0][0]["victorias"].ToString());
            misDerrotas = int.Parse(json[0][0]["derrotas"].ToString());
            miDinero = int.Parse(json[0][0]["dinero"].ToString());
            array = json[0][0]["icono"].ToString().Split('"');
            miIcono = array[1];
            SpawnPlayer();
            
            
        }
        else
        {
            Debug.Log("Inicio Incorrecto");
        }
    }
}
