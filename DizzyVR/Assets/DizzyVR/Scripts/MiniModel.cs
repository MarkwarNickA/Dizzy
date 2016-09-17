using UnityEngine;
using System.Collections;
using Valve.VR;
using VRTK;

    public class MiniModel : MonoBehaviour
    {
        public bool miniModelIsVisible = false;

        [Tooltip("Set starting miniModel offset")]
        public Vector3 miniModelOffset;

        [Tooltip("Mini Model Scale.")]
        public float miniModelScale;

        public Vector3 oneToOneModelOffset;

        GameObject miniModelRoom;
        GameObject cameraRig;
        GameObject leftHand;
        GameObject rightHand;



        public GameObject oneToOneModel;
        public GameObject miniModelInstance;
        
        Renderer[] buildingModelRenderers;

        public int floorLayer;
        public int unTeleportableLayer;

        void Awake()
        {
            if (this.gameObject.GetComponent<SteamVR_ControllerManager>() == null)
            {
                Debug.LogError("You need to put the MiniModel script on your cameraRig.");
                return;
            }
            cameraRig = this.gameObject;

            //need to not find the hands by index
            leftHand = cameraRig.transform.GetChild(0).gameObject;
            rightHand = cameraRig.transform.GetChild(1).gameObject;

            //set up layermasks
            floorLayer = LayerMask.NameToLayer("Floor");
            unTeleportableLayer = LayerMask.NameToLayer("Unteleportable");

            oneToOneModel = GameObject.Find("Building_Model");
            oneToOneModelOffset = oneToOneModel.transform.position;

            miniModelRoom = GameObject.Find("MiniModel_Room");
            miniModelRoom.SetActive(false);

        }

        void Start()
        {
               
            miniModelInstance = miniBuildingModelInstantiate(oneToOneModel);
            miniModelInstance.SetActive(false);

            setControllerButtons();
        }

        void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
        {
            //event for a trigger press
            switchModels();
        }

        public void switchModels()
        {

            if (miniModelIsVisible)
            {
                miniModelInstance.SetActive(false);
                miniModelInstance.SetActive(false);
                miniModelRoom.SetActive(false);
                oneToOneModel.SetActive(true);
                miniModelIsVisible = false;
            }

            else
            {
                miniModelRoom.SetActive(true);
                miniModelInstance.SetActive(true);
                oneToOneModel.SetActive(false);
                miniModelIsVisible = true;
            }
        }

        public GameObject miniBuildingModelInstantiate(GameObject model)
        {
            GameObject miniBuildingModelInstance = (GameObject)Instantiate(model, miniModelOffset, Quaternion.identity);
            miniBuildingModelInstance.transform.localScale = new Vector3(miniModelScale, miniModelScale, miniModelScale);
            foreach (Collider col in miniBuildingModelInstance.GetComponentsInChildren<Collider>())
            {
                if (col.gameObject.layer != floorLayer && col.gameObject.layer != unTeleportableLayer)
                {
                    Destroy(col);
                }
            }

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

        void setControllerButtons()
        {
            //Setup controller event listeners
            if (leftHand.GetComponent<VRTK_ControllerEvents>() == null
               || rightHand.GetComponent<VRTK_ControllerEvents>() == null)
            {
                Debug.LogError("You need to put a VRTK_ControllerEvents script on your SteamVR Controllers");
                return;
            }

            leftHand.GetComponent<VRTK_ControllerEvents>().TriggerPressed +=
                new ControllerInteractionEventHandler(DoTriggerPressed);
            rightHand.GetComponent<VRTK_ControllerEvents>().TriggerPressed +=
                new ControllerInteractionEventHandler(DoTriggerPressed);
        }
    }

