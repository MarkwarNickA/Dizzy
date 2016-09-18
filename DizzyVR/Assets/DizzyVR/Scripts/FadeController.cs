using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour {

    public int RenderQueue;

	// Use this for initialization
	void Start () {
        Renderer LocalRender;
        LocalRender = this.GetComponent<Renderer>();
        LocalRender.material.renderQueue = RenderQueue;
	}
	
	// Update is called once per frame
	void Update () {

    }

    public void Fade(float amount)
    {
        this.GetComponent<Renderer>().material.color = new Color(0, 0, 0, amount);
    }
}
