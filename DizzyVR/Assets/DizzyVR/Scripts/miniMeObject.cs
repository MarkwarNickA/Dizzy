using UnityEngine;
using System.Collections;
using System.IO;

public class miniMeObject : MonoBehaviour
{
    public Vector3 miniPosition;
    public Quaternion miniRotation;
    public Vector3 miniScale;
	public GameObject miniMe;

    public Vector3 bodyPartScale;
    public GameObject cameraRig;

    GameObject trackedObject;
    float miniModelScale;

    Vector3 miniModelOffset;
    MiniModel myMiniModel;

    GameObject trackedHeadObject;


    void Start()
    {
        string bodyPartName = gameObject.name;

        switch (bodyPartName)
        {
            case "Left_Hand":
                trackedObject = cameraRig.transform.GetChild(0).gameObject;
                break;
            case "Right_Hand":
                trackedObject = cameraRig.transform.GetChild(1).gameObject;
                break;
            case "Head":
                trackedObject = cameraRig.transform.GetChild(2).gameObject;
                break;
        }

        //We need the head to make relative positions from the hands (i.e. fix the head)
        trackedHeadObject = cameraRig.transform.GetChild(2).gameObject;

        //bodyPartScale = gameObject.transform.localScale;
        //myMiniModel = cameraRig.GetComponent<MiniModel>();
    }

    void Update()
    {
       
        string bodyPartName = gameObject.name;

        //HEAD
        if (bodyPartName == "Head"){
            //do not change the position of the head - just the rotation
            //miniPosition = Vector3.zero;
        }
        //HANDS
        else {
			var differenceBetweenHeadAndObject = trackedObject.transform.localPosition - trackedHeadObject.transform.localPosition;
            miniPosition = differenceBetweenHeadAndObject;
        }

        //We always pass on rotation to the hands and head
        miniRotation = trackedObject.transform.rotation;
       

        //Update this hand/head
        gameObject.transform.localPosition = miniPosition;
        gameObject.transform.rotation = miniRotation;
    }
}
