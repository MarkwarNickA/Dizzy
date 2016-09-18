using UnityEngine;
using System.Collections;

public class SpotlightController : MonoBehaviour {

    public float FadeStep;

    float ColliderRadius;

	// Use this for initialization
	void Start () {
        ColliderRadius = this.GetComponent<CapsuleCollider>().radius;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<SteamVR_Camera>() != null)
        {
            float Distance = XZDistance(this.transform.position, collider.transform.position);
            float Intensity = (Mathf.Clamp(Distance, 0.0f, ColliderRadius) / ColliderRadius) * 4;
            this.GetComponent<Light>().intensity = Intensity;
        }

    }

    public void FadeOut()
    {
        StartCoroutine(DoFadeOut());
    }

    IEnumerator DoFadeOut()
    {
        while (this.GetComponent<Light>().intensity > 0)
        {
            var Alpha = this.GetComponent<Light>().intensity;
            var NextAlpha = Alpha - FadeStep;
            if (NextAlpha < 0)
            {
                NextAlpha = 0f;
            }
            this.GetComponent<Light>().intensity = NextAlpha;
            yield return null;
        }

    }

    private float XZDistance(Vector3 Pos1, Vector3 Pos2)
    {
        Vector3 ModPos1 = new Vector3(Pos1.x, 0, Pos1.z);
        Vector3 ModPos2 = new Vector3(Pos2.x, 0, Pos2.z);

        return Vector3.Distance(ModPos1, ModPos2);
    }
}
