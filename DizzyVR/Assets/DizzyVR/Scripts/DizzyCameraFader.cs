using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]

public class DizzyCameraFader : MonoBehaviour {

    public GameObject SpotlightPrefab;
    
    string BlockerLayer = "BlocksVision";

    public bool InBlocker;
    public bool InSpotlight;

    public bool Blocked;

    public SpotlightController Spotlight;

    public FadeController Fader;

    float ColliderRadius;
    Vector3? PositionWhenBlocked;
    
	// Use this for initialization
	void Start () {

        InBlocker = false;
        InSpotlight = false;

        Blocked = false;

        PositionWhenBlocked = null;

        this.GetComponent<SphereCollider>().isTrigger = true;
        ColliderRadius = this.GetComponent<SphereCollider>().radius;

        if (this.GetComponent<Rigidbody>() == null)
        {
            var rb = this.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (InBlocker)
        {
            Blocked = true;
        }
        else
        {
            if (InSpotlight)
            {
                Blocked = false;
            }
            else
            {
                Blocked = true;
            }
        }

        if (Blocked)
        {
            if (PositionWhenBlocked.HasValue)
            {
                float Distance = XZDistance(this.transform.position, PositionWhenBlocked.Value);
                float FadeAmount = Mathf.Clamp(Distance, 0.0f, ColliderRadius) / ColliderRadius;
                print(FadeAmount);
                Fader.Fade(FadeAmount);
            }

        }
        else
        {
            Fader.FadeOut();
            PositionWhenBlocked = null;
            Spotlight.FadeOut();
        }
	}

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer(BlockerLayer))
        {
            InBlocker = true;

            if (PositionWhenBlocked.HasValue == false)
            {
                PositionWhenBlocked = transform.position;
                Spotlight = SpawnSpotlight(new Vector3(PositionWhenBlocked.Value.x, PositionWhenBlocked.Value.y + 2.5f, PositionWhenBlocked.Value.z)).GetComponent<SpotlightController>();
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(BlockerLayer))
        {
            InBlocker = true;
        }
        else
        {
            InBlocker = false;
        }

        if (collider.gameObject.GetComponent<SpotlightController>() == true)
        {
            InSpotlight = true;
        }
        else
        {
            InSpotlight = false;
        }
    }

    private Transform SpawnSpotlight(Vector3 position)
    {
        GameObject Spotlight = GameObject.Instantiate(SpotlightPrefab);
        Spotlight.transform.position = position;
        Spotlight.transform.rotation = Quaternion.Euler(90,0,0);
        return Spotlight.transform;
    }

    private float XZDistance(Vector3 Pos1, Vector3 Pos2)
    {
        Vector3 ModPos1 = new Vector3(Pos1.x, 0, Pos1.z);
        Vector3 ModPos2 = new Vector3(Pos2.x, 0, Pos2.z);

        return Vector3.Distance(ModPos1, ModPos2);
    }

}
