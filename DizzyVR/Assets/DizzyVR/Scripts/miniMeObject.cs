using UnityEngine;
using System.Collections;
using System.IO;

public class miniMeObject : MonoBehaviour
{
    public Vector3 miniPosition;
    public Quaternion miniRotation;
    public Vector3 miniScale;

    public Vector3 bodyPartScale;

    public GameObject cameraRig;

    public bool AllowAvatarTranslation = true;

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

        bodyPartScale = gameObject.transform.localScale;
        myMiniModel = cameraRig.GetComponent<MiniModel>();
    }

    void Update()
    {
        miniModelScale = myMiniModel.miniModelScale;
        miniModelOffset = myMiniModel.miniModelOffset;



        //miniPosition = trackedObject.transform.position;

        if (AllowAvatarTranslation == false)
        {
            string bodyPartName = gameObject.name;
            if (bodyPartName != "Head")
            {
                //Get the position relative to the head
                var differenceBetweenHeadAndObject = trackedObject.transform.position - trackedHeadObject.transform.position;
                miniPosition = differenceBetweenHeadAndObject;
                miniPosition *= miniModelScale;
                miniPosition += miniModelOffset;
                miniPosition = cameraRig.transform.position;
            }
            else
            {
                // Don't update the position of the head - this sdhould only be updated by the vive wands.  
                miniPosition = new Vector3(0, trackedObject.transform.position.y, 0);
                miniPosition *= miniModelScale;
                miniPosition += miniModelOffset;
                miniPosition = cameraRig.transform.position;
            }
        }
        else {
            miniPosition = trackedObject.transform.position;
            miniPosition *= miniModelScale;
            miniPosition += miniModelOffset;
            miniPosition += cameraRig.transform.position;
            Debug.Log("hi");

        }

        miniRotation = trackedObject.transform.rotation;
        miniScale = bodyPartScale * miniModelScale;


        //Update this hand/head
        gameObject.transform.localScale = miniScale;
        gameObject.transform.position = miniPosition;
        gameObject.transform.rotation = miniRotation;
    }
}
