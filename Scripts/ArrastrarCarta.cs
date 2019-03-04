using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastrarCarta : MonoBehaviour {

    /*Mover Raton*/
    private bool draggingItem = false;
    public GameObject draggedObject;
    private Vector2 touchOffset;
    private Transform miTransform;
    

    /*Cursor*/
    public Vector2 hotSpot = Vector2.zero;
    public Texture2D cursorGeneral;
    public Texture2D cursorMano;
    public Texture2D CursorEspada;
    public CursorMode cursorMode = CursorMode.ForceSoftware;

    /*Order Layer al mover las cartas*/
    public int sortingOrderDrag = 9;
    public int sortingOrderNormal = 1;
    private SpriteRenderer sprite;

    private GameObject partida;
    public GameObject HUD_info;

    void Start()
    {
        miTransform = this.transform;
        sprite = GetComponent<SpriteRenderer>();
        Cursor.SetCursor(cursorGeneral, hotSpot, cursorMode);
        partida = GameObject.Find("Partida");
    }

    void Update()
    {
        if (HasInput)
        {
            DragOrPickUp();
        }
        else
        {
            if (draggingItem)
                DropItem();
        }
    }

    Vector2 CurrentTouchPosition
    {
        get
        {
            Vector2 inputPos;
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return inputPos;
        }
    }

    private void DragOrPickUp()
    {
        var inputPosition = CurrentTouchPosition;

        if (draggingItem)
        {
            draggedObject.transform.position = inputPosition + touchOffset;
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null && hit.transform == this.gameObject.transform && hit.transform.gameObject.transform.tag == "cManoPropia")
                {
                    draggingItem = true;
                    draggedObject = hit.transform.gameObject;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    cambiarOrder(11);
                    draggedObject.transform.parent = null;
                    draggedObject.transform.localScale = new Vector3(0.23f, 0.23f, 0.23f);


                }
            }
        }
    }
    private bool HasInput
    {
        get
        {
            return Input.GetMouseButton(0);
        }
    }

    void DropItem()
    {
        draggingItem = false;
        draggedObject.transform.localScale = new Vector3(0.1801447f, 0.1801447f, 0.1801447f);
        if (miTransform.position.x < GameObject.Find("CuadCampoArribaDch").gameObject.transform.position.x && miTransform.position.x > GameObject.Find("CuadCampoAbajoIzq").gameObject.transform.position.x && miTransform.position.y < GameObject.Find("CuadCampoArribaDch").gameObject.transform.position.y && miTransform.position.y > GameObject.Find("CuadCampoAbajoIzq").gameObject.transform.position.y)
        {
            Debug.Log("dentro");
            partida.GetComponentInChildren<Partida>().InvocarCarta(draggedObject.GetComponent<carta>().getNombre());
        }
        else
        {
            Debug.Log("fuera");
            partida.GetComponentInChildren<Partida>().volverCartaMano(draggedObject.GetComponent<carta>().getNombre());
        }
        cambiarOrder(10);
    }
    /*Hover de raton para animaciones y cursor*/
    void OnMouseOver()
    {
        Cursor.SetCursor(cursorMano, hotSpot, cursorMode);

        string nombre = this.gameObject.GetComponent<carta>().nombre;
        string tipo = this.gameObject.GetComponent<carta>().tipo;
        string descripcion = this.gameObject.GetComponent<carta>().descripcion;
        string categoria = this.gameObject.GetComponent<carta>().categoria;
        int ataque = this.gameObject.GetComponent<carta>().ataque;
        int vida = this.gameObject.GetComponent<carta>().vida;
        int coste = this.gameObject.GetComponent<carta>().coste;
        string imagen = this.gameObject.GetComponent<carta>().imagen;
        string rareza = this.gameObject.GetComponent<carta>().rareza;

        partida.GetComponentInChildren<Partida>().cargarCartaInfo(nombre, tipo, categoria, rareza, descripcion, ataque, vida, coste, imagen);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(cursorGeneral, hotSpot, cursorMode);
        GameObject.Find("CartaInfo").GetComponent<Animator>().SetBool(this.gameObject.GetComponent<carta>().imagen, false);
        HUD_info.SetActive(false);
    }

    private void cambiarOrder(int order)
    {
        draggedObject.GetComponent<SpriteRenderer>().sortingOrder = order;
        draggedObject.transform.GetChild(0).GetComponent<Canvas>().sortingOrder = order;
        draggedObject.transform.GetChild(1).GetComponent<Canvas>().sortingOrder = order;
        draggedObject.transform.GetChild(2).GetComponent<Canvas>().sortingOrder = order;
        draggedObject.transform.GetChild(3).GetComponent<Canvas>().sortingOrder = order;

    }
}
