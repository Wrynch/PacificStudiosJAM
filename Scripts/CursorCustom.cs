using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorCustom : MonoBehaviour {

    public Vector2 hotSpot = Vector2.zero;
    public Texture2D cursorTexture;
    public Texture2D cursorMano;
    public Texture2D CursorEspada;

    public CursorMode cursorMode = CursorMode.ForceSoftware;
    // Use this for initialization
    void Start () {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
