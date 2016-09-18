using UnityEngine;
using System.Collections;

public class MiniMeController : MonoBehaviour {

    public GameObject cameraRig;
    public GameObject Head;
    public GameObject LeftHand;
    public GameObject RightHand;

    //needed for calculating positions
    public float miniModelScale;

    //When switching to thirds person, we cache the position of the camera in the world and apply it to the mini-map.
    private bool _isInThirdPerson = true;
    public bool IsInThirdPerson;

    //Keep a reference to the Headset Gameobject (inside the camera rig)
    GameObject trackedHeadObject;

    //The head position when the app was started, or when the 3rd person mode was invoked.
    Vector3 cachedMiniMePosition;


    void Awake() {
        
    }


    // Use this for initialization
    void Start () {

        //Initialize the Head and Hands with the Camera Rig
        Head.GetComponent<miniMeObject>().cameraRig = this.cameraRig;
        LeftHand.GetComponent<miniMeObject>().cameraRig = this.cameraRig;
        RightHand.GetComponent<miniMeObject>().cameraRig = this.cameraRig;
        GetComponent<miniMeRemoteControl>().cameraRig = this.cameraRig;

        //We need the head to make relative positions from the hands (i.e. fix the head)
        trackedHeadObject = cameraRig.transform.GetChild(2).gameObject;


        //set the initial cached position of the head, and set the position
        updateMiniMePosition();

    }
	
	// Update is called once per frame
	void Update () {

        //If the value has changed since last frame, and recache the head position
        //on change of the "Allows Movement" property, recache the head position, and set this position
        if (_isInThirdPerson != this.IsInThirdPerson)
        {
			updateMiniMePosition();
        }



        
        //Debug.Log("" + trackedHeadObject.transform.position + " - " +  miniModelScale);
    }

	void updateMiniMePosition() {
		cachedMiniMePosition = trackedHeadObject.transform.position;
        _isInThirdPerson = this.IsInThirdPerson;
		this.gameObject.transform.localPosition = this.cameraRig.transform.localPosition;
		print (this.cameraRig.transform.localPosition);
    }
}
