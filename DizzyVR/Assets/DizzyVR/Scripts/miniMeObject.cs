using UnityEngine;
using System.Collections;

    public enum bodyPart
    {
        Left_Hand,
        Right_Hand,
        Head
    }

public class miniMeObject : MonoBehaviour
{
    public bodyPart myBodyPart;

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
        myMiniModel = cameraRig.GetComponent<MiniModel>();

        switch (myBodyPart)
        {
            case bodyPart.Left_Hand:
                trackedObject = cameraRig.transform.GetChild(0).gameObject;
                break;

            case bodyPart.Right_Hand:
                trackedObject = cameraRig.transform.GetChild(1).gameObject;
                break;

            case bodyPart.Head:
                trackedObject = cameraRig.transform.GetChild(2).gameObject;
                break;
        }
    }

    void Update()
    {
        miniModelScale = myMiniModel.miniModelScale;
        miniModelOffset = myMiniModel.miniModelOffset;

        miniPosition = trackedObject.transform.position;
        miniPosition *= miniModelScale;
        miniPosition += miniModelOffset;

        miniRotation = trackedObject.transform.rotation;

        miniScale = bodyPartScale * miniModelScale;
        gameObject.transform.localScale = miniScale;
    }
}
