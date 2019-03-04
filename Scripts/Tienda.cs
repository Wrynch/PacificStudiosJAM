using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tienda : MonoBehaviour {

    public GameObject botonVolver;

    float ultimaPosicion;
    int logeado;
    string playerName;

    

    // Use this for initialization
    void Start () {
        Debug.Log(ultimaPosicion);
        Debug.Log(logeado);
        Debug.Log(playerName);

    }
	
	// Update is called once per frame
	void Update () {
       
    }

    public void volverMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    void OnEnable()
    {
        ultimaPosicion = PlayerPrefs.GetFloat("posicion");
        logeado = PlayerPrefs.GetInt("logeado");
        playerName = PlayerPrefs.GetString("playerName");
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat("posicion", ultimaPosicion);
        PlayerPrefs.SetInt("logeado", logeado);
        PlayerPrefs.SetString("playerName", playerName);
    }

    public GameObject cartaMostrar;
    public GameObject animacion1;
    public GameObject animacion2;
    float tiempoCarta = 1.2f;
    public void comprarSobre()
    {
        Invoke("newVoid", tiempoCarta); //2 is the time
        GameObject.Find("animacionDetras").GetComponent<Animator>().Play("aparecer", 0);
        GameObject.Find("animacionDelante").GetComponent<Animator>().Play("aparecer", 0);
   
    }

    void newVoid()
    {
        cartaMostrar.SetActive(true);
    }
}
