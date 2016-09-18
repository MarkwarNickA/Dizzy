using UnityEngine;
using System.Collections;

public class FadeController : MonoBehaviour {

    public int RenderQueue;

    public float FadeStep;

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

    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeOut()
    {
        while (this.GetComponent<Renderer>().material.color.a > 0)
        {
            var Alpha = this.GetComponent<Renderer>().material.color.a;
            var NextAlpha = Alpha - FadeStep;
            if (NextAlpha < 0)
            {
                NextAlpha = 0f;
            }
            this.GetComponent<Renderer>().material.color = new Color(0, 0, 0, NextAlpha);
            yield return null;
        }

    }
}
