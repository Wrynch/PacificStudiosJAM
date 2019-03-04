using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class carta : MonoBehaviour {
    public string nombre;
    public string categoria;
    public string rareza;
    public string imagen;
    public string descripcion;
    public string tipo;
    public int ataque = 0;
    public int vida = 0;
    public int coste = 0;

    public GameObject ataqueTxt;
    public GameObject vidaTxt;
    public GameObject costeTxt;
    public GameObject defensaTxt;



    // Use this for initialization
    void Start () {

        if (tipo == "atacante")
        {
            defensaTxt.SetActive(false);

            cambiarAtaque(ataque);
            cambiarVida(vida);
        }
        else if(tipo == "defensor")
        {
            ataqueTxt.SetActive(false);
            vidaTxt.SetActive(false);

            cambiarVida(vida);
        }
        cambiarCoste(coste);

        

    }
	
	// Update is called once per frame
	void Update () {
		
	}



    public void cambiarAtaque(int ataque)
    {
        this.ataque = ataque;
        ataqueTxt.GetComponent<Text>().text = ataque.ToString();
    }
    public void cambiarVida(int vida)
    {
        this.vida = vida;
        if (tipo == "atacante")
        {
            vidaTxt.GetComponent<Text>().text = vida.ToString();
        }
        else if (tipo == "defensor")
        {
            defensaTxt.GetComponent<Text>().text = vida.ToString();
        }
        
    }

    public void cambiarCoste(int coste)
    {
        this.coste = coste;
        costeTxt.GetComponent<Text>().text = coste.ToString();
    }

    public string getNombre()
    {
        return nombre;
    }
}
