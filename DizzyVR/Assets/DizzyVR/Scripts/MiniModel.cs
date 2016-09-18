using UnityEngine;
using System.Collections;
using Valve.VR;
using VRTK;


/// <summary>
/// Instantiates the mini-map and links up the avatar and mode switching key bindings
/// </summary>
public class MiniModel : MonoBehaviour
{
    public Vector3 miniModelOffset;
    public Vector3 oneToOneModelOffset;
    public float miniModelScale;
	public bool firstPersonMode;

    GameObject cameraRig;
    GameObject leftHand;
    GameObject rightHand;


    public GameObject oneToOneModel;
    public GameObject miniModelInstance;


    //Will get added to the minimap when it is instantiated
    public GameObject MiniMePrefab; 
    private GameObject MiniPrefabInstance;

        
    Renderer[] buildingModelRenderers;

    void Awake()
    {
        if (this.gameObject.GetComponent<SteamVR_ControllerManager>() == null)
        {
            Debug.LogError("You need to put the MiniModel script on your cameraRig.");
            return;
        }  
    }


    void Start()
    {
        cameraRig = this.gameObject;

        //Get Refeences to VR Rig Components
        leftHand = cameraRig.transform.GetChild(0).gameObject;
        rightHand = cameraRig.transform.GetChild(1).gameObject;
        oneToOneModelOffset = oneToOneModel.transform.position;

        //create the minimap        
        miniModelInstance = miniBuildingModelInstantiate(oneToOneModel);

        //add the avatar to the minimap
        var tempScale = MiniMePrefab.transform.localScale;
        this.MiniPrefabInstance = (GameObject)Instantiate(MiniMePrefab, miniModelInstance.transform);
        this.MiniPrefabInstance.transform.localScale = tempScale;
        var miniMeControllerInstance = this.MiniPrefabInstance.GetComponent<MiniMeController>();

        if (MiniPrefabInstance == null) Debug.Log("No prefab Instance");
        if (miniMeControllerInstance == null) Debug.Log("No controller Instance");
        if (cameraRig == null) Debug.Log("No cameraRig");
        miniMeControllerInstance.cameraRig = cameraRig;
        miniMeControllerInstance.miniModelScale = miniModelScale;

        //Bind Controller buttons for 1st/3rd Person mode switcher
        setControllerButtons();
		firstPersonMode = false;
		//disable hand controls
		GetComponent<VRTK_TouchpadWalking>().LeftController = false;
		GetComponent<VRTK_TouchpadWalking>().RightController = false;

    }

    void Update()
    {
        miniModelPositionUpdate(miniModelInstance);

		//sloppy keyboard event for non-vives
		if (Input.GetKeyDown(KeyCode.Space)){
			if(firstPersonMode == true){
				firstPersonMode = false;
			}else {
				firstPersonMode = true;		
			}	
		}

    }

    public GameObject miniBuildingModelInstantiate(GameObject model)
    {
        GameObject miniBuildingModelInstance = (GameObject)Instantiate(model, miniModelOffset, Quaternion.identity);
        miniBuildingModelInstance.transform.localScale = new Vector3(miniModelScale, miniModelScale, miniModelScale);


        //Copy the baked lighting maps to the minimap
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
        if (leftHand.GetComponent<VRTK_ControllerEvents>() == null
            || rightHand.GetComponent<VRTK_ControllerEvents>() == null)
        {
            Debug.LogError("You need to put a VRTK_ControllerEvents script on your SteamVR Controllers");
            return;
        }

        leftHand.GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(swapMode);
		rightHand.GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(swapMode);


    }

	public void swapMode(object sender, ControllerInteractionEventArgs e){
		if(firstPersonMode == true){

			Debug.Log ("3RD PERSON MODE!:");
			firstPersonMode = false;
			//show miniMe
			MiniPrefabInstance.BroadcastMessage("Show");

			//show miniMap
			miniModelInstance.BroadcastMessage("Show");

			//hide environment
			oneToOneModel.BroadcastMessage("Hide");

			//disable hand controls
			GetComponent<VRTK_TouchpadWalking>().LeftController = true;
			GetComponent<VRTK_TouchpadWalking>().RightController = true;

		}else {
			
			Debug.Log ("1st PERSON MODE!");

			firstPersonMode = true;

			//hide miniMe
			MiniPrefabInstance.BroadcastMessage("Hide");

			//hide miniMap
			miniModelInstance.BroadcastMessage("Hide");

			//show environment
			oneToOneModel.BroadcastMessage("Show");

			//enabled hand controls
			GetComponent<VRTK_TouchpadWalking>().LeftController = false;
			GetComponent<VRTK_TouchpadWalking>().RightController = false;

		}
	}

	/*
	public void swapMode(){
		if(firstPersonMode == true){
			firstPersonMode = false;
			//show miniMe
			miniMe.BroadcastMessage("Show");

			//show miniMap
			miniModelInstance.BroadcastMessage("Show");

			//hide environment
			oneToOneModel.BroadcastMessage("Hide");


		}else {
			firstPersonMode = true;

			//hide miniMe
			miniMe.BroadcastMessage("Hide");

			//hide miniMap
			miniModelInstance.BroadcastMessage("Hide");

			//show environment
			oneToOneModel.BroadcastMessage("Show");

		}
		Debug.Log("Mode switched to: " + firstPersonMode);
	}
	*/


		

}

