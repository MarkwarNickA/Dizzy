using UnityEngine;
using VRTK;
using System.Collections;

public class PlayAreaCursor : MonoBehaviour {
    [Tooltip("The material to use on the rendered version of the pointer. If no material is selected then the default `WorldPointer` material will be used.")]
    public Material pointerMaterial;
    [Tooltip("If this is enabled then the play area boundaries are displayed at the tip of the pointer beam in the current pointer colour.")]
    public bool showPlayAreaCursor = true;
    [Tooltip("Adjust the height of play area marker to adjust for floor height")]
    public float heightAdjustment = 0.5f;

    private Transform playArea;
    private GameObject playAreaCursor;
    private GameObject[] playAreaCursorBoundaries;
    private Transform headset;
    private bool playAreaShown;

    // Mini vars
    public GameObject cameraRig;
    public Vector3 miniPosition;

    float miniModelScale;
    Vector3 miniModelOffset;
    MiniModel myMiniModel;


    void Awake() {
        headset = VRTK_DeviceFinder.HeadsetTransform();
        playArea = VRTK_DeviceFinder.PlayAreaTransform();
        playAreaCursorBoundaries = new GameObject[4];
    }
	
	void Update() {
        if (myMiniModel.firstPersonMode == playAreaShown)
            TogglePointer(!playAreaShown);

        if (playAreaShown)
        {
            miniPosition = myMiniModel.GetMiniMePosition();
            miniPosition.y = heightAdjustment;
            SetPlayAreaCursorTransform(miniPosition);
        }
    }

    void Start()
    {
        if (cameraRig == null)
        {
            Debug.LogError("PlayAreaCursor requires that the cameraRig be assigned");
            return;
        }

        myMiniModel = cameraRig.GetComponent<MiniModel>();

        miniModelScale = myMiniModel.miniModelScale;
        miniModelOffset = myMiniModel.miniModelOffset;

        InitPlayAreaCursor();
        TogglePointer(true);
    }

    protected virtual void SetPointerMaterial()
    {
        foreach (GameObject playAreaCursorBoundary in playAreaCursorBoundaries)
        {
            playAreaCursorBoundary.GetComponent<Renderer>().material = pointerMaterial;
        }
    }

    protected virtual void SetPlayAreaCursorTransform(Vector3 destination)
    {
        var offset = Vector3.zero;

        var playAreaPos = new Vector3(playArea.transform.position.x, 0, playArea.transform.position.z) * miniModelScale;
        var headsetPos = new Vector3(headset.position.x, 0, headset.position.z) * miniModelScale;
        offset = playAreaPos - headsetPos;

        playAreaCursor.transform.position = destination + offset;
    }

    protected virtual void TogglePointer(bool state)
    {
        var playAreaState = (showPlayAreaCursor ? state : false);
        if (playAreaCursor)
        {
            playAreaCursor.gameObject.SetActive(playAreaState);
        }
        playAreaShown = state;
    }

    private void DrawPlayAreaCursorBoundary(int index, float left, float right, float top, float bottom, float thickness, Vector3 localPosition)
    {
        var playAreaCursorBoundary = GameObject.CreatePrimitive(PrimitiveType.Cube);
        playAreaCursorBoundary.name = string.Format("[{0}]WorldPointer_PlayAreaCursorBoundary_" + index, gameObject.name);
        Utilities.SetPlayerObject(playAreaCursorBoundary, VRTK_PlayerObject.ObjectTypes.Pointer);

        var width = ((right - left) / 1.065f) * miniModelScale;
        var length = ((top - bottom) / 1.08f) * miniModelScale;
        var height = thickness * miniModelScale;

        playAreaCursorBoundary.transform.localScale = new Vector3(width, height, length);
        playAreaCursorBoundary.layer = LayerMask.NameToLayer("Ignore Raycast");

        playAreaCursorBoundary.transform.parent = playAreaCursor.transform;
        playAreaCursorBoundary.transform.localPosition = localPosition;

        playAreaCursorBoundaries[index] = playAreaCursorBoundary;
    }

    private void InitPlayAreaCursor()
    {
        var btmRightInner = 0;
        var btmLeftInner = 1;
        var topLeftInner = 2;
        var topRightInner = 3;

        var btmRightOuter = 4;
        var btmLeftOuter = 5;
        var topLeftOuter = 6;
        var topRightOuter = 7;

        Vector3[] cursorDrawVertices = VRTK_SDK_Bridge.GetPlayAreaVertices(playArea.gameObject);

        var width = cursorDrawVertices[btmRightOuter].x - cursorDrawVertices[topLeftOuter].x;
        var length = cursorDrawVertices[topLeftOuter].z - cursorDrawVertices[btmRightOuter].z;
        var height = 0.01f;

        playAreaCursor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        playAreaCursor.name = string.Format("[{0}]WorldPointer_PlayAreaCursor", gameObject.name);
        Utilities.SetPlayerObject(playAreaCursor, VRTK_PlayerObject.ObjectTypes.Pointer);
        playAreaCursor.transform.parent = null;
        playAreaCursor.transform.localScale = new Vector3(width, height, length) * miniModelScale;
        playAreaCursor.SetActive(false);

        playAreaCursor.GetComponent<Renderer>().enabled = false;

        playAreaCursor.AddComponent<Rigidbody>().isKinematic = true;

        playAreaCursor.layer = LayerMask.NameToLayer("Ignore Raycast");
        
        var playAreaBoundaryX = playArea.transform.localScale.x / 2;
        var playAreaBoundaryZ = playArea.transform.localScale.z / 2;
        var heightOffset = 0f;

        DrawPlayAreaCursorBoundary(0, cursorDrawVertices[btmLeftOuter].x, cursorDrawVertices[btmRightOuter].x, cursorDrawVertices[btmRightInner].z, cursorDrawVertices[btmRightOuter].z, height, new Vector3(0f, heightOffset, playAreaBoundaryZ));
        DrawPlayAreaCursorBoundary(1, cursorDrawVertices[btmLeftOuter].x, cursorDrawVertices[btmLeftInner].x, cursorDrawVertices[topLeftOuter].z, cursorDrawVertices[btmLeftOuter].z, height, new Vector3(playAreaBoundaryX, heightOffset, 0f));
        DrawPlayAreaCursorBoundary(2, cursorDrawVertices[btmLeftOuter].x, cursorDrawVertices[btmRightOuter].x, cursorDrawVertices[btmRightInner].z, cursorDrawVertices[btmRightOuter].z, height, new Vector3(0f, heightOffset, -playAreaBoundaryZ));
        DrawPlayAreaCursorBoundary(3, cursorDrawVertices[btmLeftOuter].x, cursorDrawVertices[btmLeftInner].x, cursorDrawVertices[topLeftOuter].z, cursorDrawVertices[btmLeftOuter].z, height, new Vector3(-playAreaBoundaryX, heightOffset, 0f));
    }
}
