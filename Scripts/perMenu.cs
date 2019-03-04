using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class perMenu : MonoBehaviour {
    private Transform miTransform;
    public int velocidad;
    private int _velocidad;
    public float ultimaPosicion;

    private Animator miAnimator;
    private SpriteRenderer spriteRenderer;

    // Use this for initialization
    void Start ()
    {
        miTransform = this.transform;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        miAnimator = GetComponent<Animator>();

        _velocidad = 0;

       
    }
	
	// Update is called once per frame
	public void Update ()
    {
        cambioEscena();
        movimiento();	
        miTransform.Translate(Vector3.left * velocidad * Time.deltaTime);

        if (velocidad < 0 || velocidad > 0)
            miAnimator.SetInteger("velocidad", velocidad);
        else
            miAnimator.SetInteger("velocidad", velocidad);

        ultimaPosicion = miTransform.position.x;
    }
    void movimiento()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            velocidad = -3;
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            velocidad = 3;
            spriteRenderer.flipX = true;
        }
        else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            velocidad = 0;
        }



        if (miTransform.position.x <= GameObject.Find("leftPointLimite").gameObject.transform.position.x && velocidad > 0)
        {
            velocidad = 0;
        }
        else if (miTransform.position.x >= GameObject.Find("rightPointLimite").gameObject.transform.position.x && velocidad < 0)
        {
            velocidad = 0;
        }
    }
    public void cambioEscena()
    {
        if (miTransform.position.x < GameObject.Find("rightPointJ").gameObject.transform.position.x && miTransform.position.x > GameObject.Find("leftPointJ").gameObject.transform.position.x && Input.GetKeyDown(KeyCode.Return))
        {

            PlayerPrefs.SetFloat("ultimaPosicion", ultimaPosicion);
            SceneManager.LoadScene("Partida");
        }
        else if (miTransform.position.x < GameObject.Find("rightPointT").gameObject.transform.position.x && miTransform.position.x > GameObject.Find("leftPointT").gameObject.transform.position.x && Input.GetKeyDown(KeyCode.Return))
        {
            ultimaPosicion = miTransform.position.x;
            PlayerPrefs.SetFloat("ultimaPosicion", ultimaPosicion);
            SceneManager.LoadScene("Tienda");
        }
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat("posicion", ultimaPosicion);
    }
}
