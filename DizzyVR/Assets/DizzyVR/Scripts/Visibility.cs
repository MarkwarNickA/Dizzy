using UnityEngine;
using System.Collections;

public class Visibility : MonoBehaviour {

    // Use this for initialization

    public int RendQueue = -1;

    Renderer rend;

	void Awake () {
		rend = GetComponentInChildren<Renderer> ();

        if (RendQueue != -1)
        {
            rend.material.renderQueue = RendQueue;
        }


    }
	
	// Update is called once per frame
	void Update () {
	
	}

	void Hide(){
		rend.enabled = false;
	}

	void Show(){
		rend.enabled = true;
	}
		
}

