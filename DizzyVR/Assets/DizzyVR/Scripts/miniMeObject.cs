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


    private bool _allowAvatarTranslation = true;
    public bool AllowAvatarTranslation;

    GameObject trackedObject;
    float miniModelScale;

    Vector3 miniModelOffset;
    MiniModel myMiniModel;

    GameObject trackedHeadObject;

    //The head position when the app was started, or when the 3rd person mode was invoked.
    Vector3 cachedHeadPosition;


    void Start()
    {
        _allowAvatarTranslation = this.AllowAvatarTranslation;

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


        //set the initial cached position of the head
        cachedHeadPosition = trackedHeadObject.transform.position;
    }

    void Update()
    {
        miniModelScale = myMiniModel.miniModelScale;
        miniModelOffset = myMiniModel.miniModelOffset;

        //track when the value has changed, and recache the head position
        if (_allowAvatarTranslation != this.AllowAvatarTranslation) {
            cachedHeadPosition = trackedHeadObject.transform.position;
        }

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
                miniPosition += cameraRig.transform.position;
            }
            else
            {
                // Don't update the position of the head - this sdhould only be updated by the vive wands.  
                miniPosition = new Vector3(cachedHeadPosition.x, trackedObject.transform.position.y, cachedHeadPosition.z);
                miniPosition *= miniModelScale;
                miniPosition += miniModelOffset;
                miniPosition += cameraRig.transform.position;
            }
        }
        else {
            miniPosition = trackedObject.transform.position;
            miniPosition *= miniModelScale;
            miniPosition += miniModelOffset;
            miniPosition += cameraRig.transform.position;
        }

        miniRotation = trackedObject.transform.rotation;
        miniScale = bodyPartScale * miniModelScale;


        //Update this hand/head
        gameObject.transform.localScale = miniScale;
        gameObject.transform.position = miniPosition;
        gameObject.transform.rotation = miniRotation;
    }
}
