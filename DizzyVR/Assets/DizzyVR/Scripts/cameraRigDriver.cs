using UnityEngine;
using System.Collections;

public class cameraRigDriver : MonoBehaviour {

    public GameObject cameraRig;
    public GameObject miniMe;
    public Vector3 cameraRigPosition;
    MiniModel myMiniModel;

    void Start()
    {
        myMiniModel = cameraRig.GetComponent<MiniModel>();
    }

    void Update()
    {
        cameraRigPosition = miniMe.transform.position;
        cameraRigPosition -= myMiniModel.miniModelOffset;
        cameraRigPosition /= myMiniModel.miniModelScale;

        gameObject.transform.position = cameraRigPosition;

    }
}
