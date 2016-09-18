using UnityEngine;
using System.Collections;
using Valve.VR;
using VRTK;

public class MiniModel : MonoBehaviour
{
    public Vector3 miniModelOffset;
    public Vector3 oneToOneModelOffset;
    public float miniModelScale;

    GameObject cameraRig;
    GameObject leftHand;
    GameObject rightHand;

    public GameObject oneToOneModel;
    public GameObject miniModelInstance;
        
    Renderer[] buildingModelRenderers;

    void Awake()
    {
        if (this.gameObject.GetComponent<SteamVR_ControllerManager>() == null)
        {
            Debug.LogError("You need to put the MiniModel script on your cameraRig.");
            return;
        }

        cameraRig = this.gameObject;

        leftHand = cameraRig.transform.GetChild(0).gameObject;
        rightHand = cameraRig.transform.GetChild(1).gameObject;
        oneToOneModelOffset = oneToOneModel.transform.position;
    }

    void Start()
    {             
        miniModelInstance = miniBuildingModelInstantiate(oneToOneModel);
        setControllerButtons();
    }

    void Update()
    {
        miniModelPositionUpdate(miniModelInstance);
    }

    public GameObject miniBuildingModelInstantiate(GameObject model)
    {
        GameObject miniBuildingModelInstance = (GameObject)Instantiate(model, miniModelOffset, Quaternion.identity);
        miniBuildingModelInstance.layer = LayerMask.NameToLayer("MiniModel");
        for(int i = 0; i < miniBuildingModelInstance.transform.childCount; i++)
        {
            miniBuildingModelInstance.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("MiniModel");
        }
        miniBuildingModelInstance.transform.localScale = new Vector3(miniModelScale, miniModelScale, miniModelScale);

        buildingModelRenderers = oneToOneModel.GetComponentsInChildren<Renderer>();
        Renderer[] miniBuildingModelInstanceRenderers = miniBuildingModelInstance.GetComponentsInChildren<Renderer>();

        for (int i = 0; i < buildingModelRenderers.Length; i++)
        {
            int lightmapIndex = buildingModelRenderers[i].lightmapIndex;
            Vector4 lightmapScaleOffset = buildingModelRenderers[i].lightmapScaleOffset;              
            miniBuildingModelInstanceRenderers[i].lightmapIndex = lightmapIndex;
            miniBuildingModelInstanceRenderers[i].lightmapScaleOffset = lightmapScaleOffset;
        }

        return miniBuildingModelInstance;
    }

    void miniModelPositionUpdate(GameObject miniModelInstance)
    {
        miniModelInstance.transform.position = cameraRig.transform.position + miniModelOffset; 
    }

    void setControllerButtons()
    {
        //Setup controller event listeners
        //if (leftHand.GetComponent<VRTK_ControllerEvents>() == null
        //    || rightHand.GetComponent<VRTK_ControllerEvents>() == null)
        //{
        //    Debug.LogError("You need to put a VRTK_ControllerEvents script on your SteamVR Controllers");
        //    return;
        //}

        //leftHand.GetComponent<VRTK_ControllerEvents>().TriggerPressed +=
        //    new ControllerInteractionEventHandler(DoTriggerPressed);
        //rightHand.GetComponent<VRTK_ControllerEvents>().TriggerPressed +=
        //    new ControllerInteractionEventHandler(DoTriggerPressed);
    }
}

