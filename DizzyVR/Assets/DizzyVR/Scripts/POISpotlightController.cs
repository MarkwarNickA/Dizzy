using UnityEngine;
using System.Collections;

public class POISpotlightController : MonoBehaviour {

    public Transform CameraRig;

    private Light Spotlight;

	// Use this for initialization
	void Start () {
        Spotlight = this.GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 RelativePlayerPosition = CameraRig.GetComponentInChildren<SteamVR_Camera>().transform.localPosition;
        Spotlight.transform.localPosition = RelativePlayerPosition;

	}
}
