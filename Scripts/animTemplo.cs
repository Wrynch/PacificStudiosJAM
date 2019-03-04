using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animTemplo : MonoBehaviour {
    private Transform miTransform;

    private Animator miAnimator;
    public GameObject jugador;
    // Use this for initialization
    void Start () {
        /*miTransform = jugador.transform;*/
        miAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        /*if (miTransform.position.x < GameObject.Find("rightPointJ").gameObject.transform.position.x && miTransform.position.x > GameObject.Find("leftPointJ").gameObject.transform.position.x)
        {
            miAnimator.SetBool("dentro", true);
        }
        else
        {
            miAnimator.SetBool("dentro", false);
        }*/
    }
}
