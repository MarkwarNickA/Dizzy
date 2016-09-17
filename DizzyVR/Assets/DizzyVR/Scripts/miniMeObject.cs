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
    GameObject trackedObject;
    float miniModelScale;

    Vector3 miniModelOffset;
    MiniModel myMiniModel;


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

        bodyPartScale = gameObject.transform.localScale;
        myMiniModel = cameraRig.GetComponent<MiniModel>();
    }

    void Update()
    {
        miniModelScale = myMiniModel.miniModelScale;
        miniModelOffset = myMiniModel.miniModelOffset;

        miniPosition = trackedObject.transform.position;
        miniPosition *= miniModelScale;
        miniPosition += miniModelOffset;
        miniPosition += cameraRig.transform.position;

        miniRotation = trackedObject.transform.rotation;

        miniScale = bodyPartScale * miniModelScale;
        gameObject.transform.localScale = miniScale;
        gameObject.transform.position = miniPosition;
        gameObject.transform.rotation = miniRotation;
    }
}
