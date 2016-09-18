using UnityEngine;
using System.Collections;

public class SetRenderQueue : MonoBehaviour {

    public int RenderQueue;

    // Use this for initialization
    void Start () {

        this.GetComponent<Renderer>().material.renderQueue = RenderQueue;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
