using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SphereCollider))]

public class DizzyCameraFader : MonoBehaviour {

    public GameObject SpotlightPrefab;
    string BlockerLayer = "BlocksVision";

    public bool InBlocker;
    public bool HasBeenInBlocker;
    public bool Blocked;
    public Vector3 PositionWhenBlocked;

    public Transform Spotlight;

    public FadeController Fader;

    float ColliderRadius = 0.2f;
    
	// Use this for initialization
	void Start () {

        InBlocker = false;
        Blocked = false;

        this.GetComponent<SphereCollider>().radius = ColliderRadius;

        if (this.GetComponent<Rigidbody>() == null)
        {
            var rb = this.gameObject.AddComponent<Rigidbody>();
            rb.isKinematic = true;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {

        float Distance = Vector3.Distance(this.transform.position, PositionWhenBlocked);

        if (InBlocker)
        {
            Blocked = true;
            //Distance = this.GetComponent<SphereCollider>().radius;
        }
        else
        {
            if (Distance > ColliderRadius * 0.75)
            {
                if (HasBeenInBlocker)
                {
                    Blocked = true;
                }
                else
                {
                    Blocked = false;
                    if (Spotlight)
                    {
                        Destroy(Spotlight.gameObject);
                    }
                }
            }
            else
            {
                Blocked = false;
                HasBeenInBlocker = false;
                if (Spotlight)
                {
                    Destroy(Spotlight.gameObject);
                }
            }
        }

        if (Blocked)
        {
            float FadeAmount = Mathf.Clamp(Distance, 0.0f, GetComponent<SphereCollider>().radius) / GetComponent<SphereCollider>().radius;
            print(FadeAmount);
            Fader.Fade(FadeAmount);
        }
        else
        {
            Fader.Fade(0);
        }
	}

    void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer == LayerMask.NameToLayer(BlockerLayer))
        {
            InBlocker = true;
            HasBeenInBlocker = true;

            if (!Spotlight)
            {
                PositionWhenBlocked = transform.position; ;
                Spotlight = SpawnSpotlight(new Vector3(PositionWhenBlocked.x, PositionWhenBlocked.y + 2.5f, PositionWhenBlocked.z));
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(BlockerLayer))
        {
            InBlocker = false;
        }
    }

    private Transform SpawnSpotlight(Vector3 position)
    {
        GameObject Spotlight = GameObject.Instantiate(SpotlightPrefab);
        Spotlight.transform.position = position;
        Spotlight.transform.rotation = Quaternion.Euler(90,0,0);
        return Spotlight.transform;
    }

}
