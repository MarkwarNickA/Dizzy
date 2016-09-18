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
	GameObject cameraEye;


    public GameObject oneToOneModel;
    public GameObject miniModelInstance;


    //Will get added to the minimap when it is instantiated
    public GameObject MiniMePrefab; 
    private GameObject MiniPrefabInstance;
    private MiniMeController miniMeControllerInstance;

    public float ModeSwitchDurationSeconds = 3.2f;


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
		cameraEye = cameraRig.transform.GetChild(2).gameObject;
        oneToOneModelOffset = oneToOneModel.transform.position;

        //create the minimap        
        miniModelInstance = miniBuildingModelInstantiate(oneToOneModel);

        //add the avatar to the minimap
        var tempScale = MiniMePrefab.transform.localScale;
        this.MiniPrefabInstance = (GameObject)Instantiate(MiniMePrefab, miniModelInstance.transform);
        this.MiniPrefabInstance.transform.localScale = tempScale;
        this.miniMeControllerInstance = this.MiniPrefabInstance.GetComponent<MiniMeController>();

		leftHand.GetComponent<miniMeRemoteControlEvents>().miniMe = this.MiniPrefabInstance;
		rightHand.GetComponent<miniMeRemoteControlEvents>().miniMe = this.MiniPrefabInstance;
        MiniPrefabInstance.GetComponent<miniMeRemoteControl>().cameraRig = cameraRig;

        if (MiniPrefabInstance == null) Debug.Log("No prefab Instance");
        if (miniMeControllerInstance == null) Debug.Log("No controller Instance");
        if (cameraRig == null) Debug.Log("No cameraRig");
        miniMeControllerInstance.cameraRig = cameraRig;
        miniMeControllerInstance.miniModelScale = miniModelScale;

        //Bind Controller buttons for 1st/3rd Person mode switcher
        setControllerButtons();

        //disable hand controls
        MiniPrefabInstance.GetComponent<miniMeRemoteControl>().RemoteControlEnabled = false;
        Start3rdPerson(false);
    }

    void Update()
    {
        miniModelPositionUpdate(miniModelInstance);

		//sloppy keyboard event for non-vives
		if (Input.GetKeyDown(KeyCode.Space)){
            swapModeNow();
        }

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


    /// <summary>
    /// Position the Whole mini map relative to the 1st person camera's position.
    /// </summary>
    /// <param name="miniModelInstance"></param>
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

        leftHand.GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(swapMode);
		rightHand.GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(swapMode);


    }

	public void swapMode(object sender, ControllerInteractionEventArgs e){
		swapModeNow(true);
	}


    /// <summary>
    /// Overload - in most cases we want the camera+other transitions to happenn
    /// </summary>
    public void swapModeNow()
    {
        swapModeNow(true);
    }


    /// <summary>
    /// Switcvh between modes - decide whether to do transitions here
    /// </summary>
    /// <param name="transitionsOn"></param>
    public void swapModeNow(bool transitionsOn){
        firstPersonMode = !firstPersonMode;

        if (firstPersonMode == true){
            Start1stPerson(transitionsOn);
        }
        else {
            Start3rdPerson(transitionsOn);
        }

	}


    public delegate void AnimationCompleted(bool AnimationCancelled);

    /// <summary>
    /// Go from 3rd to 1st peson mode - happens on an event update, and on start.
    /// scale the minimap to be full size
    /// </summary>
    /// <param name="doTransition"></param>
    private void Start1stPerson(bool doTransition) {
        Debug.Log("1st PERSON MODE!");

        firstPersonMode = true;


        Vector3 eyeOffset = new Vector3(cameraEye.transform.localPosition.x, 0, cameraEye.transform.localPosition.z);

        Vector3 rigPos = MiniPrefabInstance.transform.localPosition + eyeOffset;
        cameraRig.transform.position = rigPos;

        //Zoom the mini-map larger to match the real world coordinates
        var scalefrom = new Vector3(miniModelScale, miniModelScale, miniModelScale);
        var scaleTo = new Vector3(1.0f, 1.0f, 1.0f);
        var positionFrom = miniModelInstance.transform.position;
        var positionTo = Vector3.zero;

        ChangePosition(miniModelInstance, positionFrom, positionTo, scalefrom, scaleTo, this.ModeSwitchDurationSeconds, doTransition,
            delegate (bool cancel)
            {
                //Stuff to Do after the animation completes
                //hide miniMe
                MiniPrefabInstance.BroadcastMessage("Hide");

                //hide miniMap
                miniModelInstance.BroadcastMessage("Hide");

                //show environment
                oneToOneModel.BroadcastMessage("Show");

                //enabled hand controls
                MiniPrefabInstance.GetComponent<miniMeRemoteControl>().RemoteControlEnabled = false;
                miniMeControllerInstance.IsInThirdPerson = false;
                Debug.Log("DONE ANIMATION to 1st PERSON mode!");
            }
            );
    }

    public Vector3 GetMiniMePosition()
    {
        return miniMeControllerInstance.transform.position;
    }


    /// <summary>
    /// Go from 1st to 3rd person mode - happens on an event update, and on start.
    /// Zoom out to see the minimap again
    /// </summary>
    /// <param name="doTransition"></param>
    private void Start3rdPerson(bool doTransition)
    {
        Debug.Log("3RD PERSON MODE!:");
        firstPersonMode = false;

        //show miniMap
        miniModelInstance.BroadcastMessage("Show");

        //hide environment
        oneToOneModel.BroadcastMessage("Hide");

        //Move tghe mini-map back to it's offset position away from the PLayspace (cameraRig)
        //Zoom the mini-map larger to match the real world coordinates
        var scalefrom = miniModelInstance.transform.localScale;
        var scaleTo = new Vector3(miniModelScale, miniModelScale, miniModelScale);
        var positionFrom = miniModelInstance.transform.position;
        var positionTo = miniModelOffset;


        ChangePosition(miniModelInstance, positionFrom, positionTo, scalefrom, scaleTo, this.ModeSwitchDurationSeconds, doTransition,
            delegate(bool cancel) {
                //Do stuff when the animation ends

                //show miniMe
                MiniPrefabInstance.BroadcastMessage("Show");

                //disable hand controls
                MiniPrefabInstance.GetComponent<miniMeRemoteControl>().RemoteControlEnabled = true;
                miniMeControllerInstance.IsInThirdPerson = true;

                Debug.Log("DONE ANIMATION to 3RD PERSON!");
            }
            );
    }


    private bool IsLerping = false;

    public void ChangePosition(
        GameObject model,
        Vector3 positionFrom,
        Vector3 positionTo,
        Vector3 scaleFrom,
        Vector3 scaleTo,
        float duration,
        bool animate,
        AnimationCompleted callback
        )
    {
        IsLerping = false; //this will stop any currently running lerp coroutine
        if (duration > 0 && animate == true)
        {
            IsLerping = true;
            //Start the lerp here
            StartCoroutine(doAnimation(model,positionFrom,positionTo,scaleFrom,scaleTo,duration, callback));
        }
        else
        {
            model.transform.position = positionTo;
            model.transform.localScale = scaleTo;
            callback(false);
        }
    }


    //Animate the scale of the minimap to 
    private IEnumerator doAnimation(
        GameObject model,
        Vector3 positionFrom,
        Vector3 positionTo,
        Vector3 scaleFrom,
        Vector3 scaleTo,
        float duration,
        AnimationCompleted callback)
    {

        for (float t = 0.0f; t < duration; t += Time.deltaTime)
        {
            model.transform.position = Vector3.Lerp(positionFrom, positionTo, t / duration);
            model.transform.localScale = Vector3.Lerp(scaleFrom, scaleTo, t / duration);
            yield return null;

            // if isLerping is set to false while this coroutine is running
            // ...stop running lerp coroutine
            if (IsLerping == false) yield break; 
        }

        //Only jump to the end position if the coroutine hasn't been interupted.
        if (IsLerping) { 
            model.transform.position = positionTo;
            model.transform.localScale = scaleTo;
            callback(false);
            IsLerping = false;
        }

    }


}

