using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrastrarCartaTEST : MonoBehaviour {

    private bool draggingItem = false;
    public GameObject draggedObject;
    public GameObject atacar;
    private Vector2 touchOffset;
    private Transform miTransform;
    private List<Vector3> TiendaPos = new List<Vector3>();
    private Animator miAnimator;
  

    public Vector2 hotSpot = Vector2.zero;
    public Texture2D cursorTexture;
    public Texture2D cursorMano;
    public Texture2D CursorEspada;

    public CursorMode cursorMode = CursorMode.ForceSoftware;

    public int CartasDentro = 0;

    void Start()
    {
        CargarPosiciones();
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        miTransform = this.transform;
        miAnimator = GetComponent<Animator>();
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
            if (this.gameObject.GetComponent<Collider2D>().isTrigger == false)
            {
                this.gameObject.AddComponent<Rigidbody2D>();
                Rigidbody2D cuerp = this.GetComponent<Rigidbody2D>();
                cuerp.mass = 1;
                cuerp.gravityScale = 0;
                this.GetComponent<Collider2D>().isTrigger = true;
            }
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null)
                {
                    draggingItem = true;
                    draggedObject = hit.transform.gameObject;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    cambiarOrder(9);
                    /*draggedObject.transform.localScale = new Vector3(0.46f, 0.46f, 0.46f);*/
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
        draggedObject.transform.localScale = new Vector3(0.41f, 0.41f, 0.41f);
        if (miTransform.position.x < GameObject.Find("Punto2").gameObject.transform.position.x && miTransform.position.x > GameObject.Find("Punto1").gameObject.transform.position.x && miTransform.position.y < GameObject.Find("Punto2").gameObject.transform.position.y && miTransform.position.y > GameObject.Find("Punto1").gameObject.transform.position.y)
        {
            
            if (CartasDentro == 0)
            {
                miTransform.transform.position = TiendaPos[0];
            }
            if (CartasDentro == 1)
            {
                miTransform.transform.position = TiendaPos[1];
            }
            if (CartasDentro == 2)
            {
                miTransform.transform.position = TiendaPos[2];
            }
            CartasDentro = CartasDentro + 1;
        }
        
        /*draggedObject.transform.position = TiendaPos[3];*/
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

        cambiarOrder(2);
        Destroy(this.GetComponent<Rigidbody2D>());
        this.GetComponent<Collider2D>().isTrigger = false;

    }

    private void CargarPosiciones()
    {
        
        TiendaPos.Add(new Vector3(-5.9f, 1.35f, 0));
        TiendaPos.Add(new Vector3(-3.15f, 1.35f, 0));

        TiendaPos.Add(new Vector3(1.72f, 1.35f, 0));
        TiendaPos.Add(new Vector3(4.47f, 1.35f, 0));
        TiendaPos.Add(new Vector3(7.26f, 1.35f, 0));

    }

    void OnMouseOver()
    {
        Cursor.SetCursor(cursorMano, hotSpot, cursorMode);
        miAnimator.SetBool("hover", true);
    }
    void OnMouseExit()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        miAnimator.SetBool("hover", false);
    }
    private void cambiarOrder(int order)
    {
        draggedObject.GetComponent<SpriteRenderer>().sortingOrder = order;
    }

    public int orden1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (atacar != null)
        {
            return;
        }
        atacar = collision.gameObject;
        orden1 = collision.GetComponent<SpriteRenderer>().sortingOrder;
        if (orden1 < 5)
        {
        collision.GetComponent<SpriteRenderer>().color = Color.red;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.name== atacar.name)
        {
            atacar = null;
        }
        collision.GetComponent<SpriteRenderer>().color = Color.white;
    }

}
