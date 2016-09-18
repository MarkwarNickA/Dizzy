using UnityEngine;
using System.Collections;

public class Visibility : MonoBehaviour {

	// Use this for initialization
	Renderer rend;

	void Start () {
		rend = GetComponent<Renderer> ();
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

