using UnityEngine;
using System.Collections;

public class GravityFreeZone : MonoBehaviour {

    string MiniMeLayerName = "MiniMe";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(MiniMeLayerName))
        {
            var rb = collider.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer(MiniMeLayerName))
        {
            var rb = collider.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }

        }
    }
}
