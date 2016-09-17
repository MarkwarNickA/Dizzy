using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using Valve.VR;
using NewtonVR;

class PlayerMovementDetector : MonoBehaviour
{

    SteamVR_Camera PlayerCamera;

    Vector3[] RecordedPositions;
    Quaternion[] RecordedRotations;

    public void Awake()
    {
        SteamVR_Utils.Event.Listen("OnNewPoseApplied", RecordPoseData);
    }

    public void OnDestroy()
    {
        SteamVR_Utils.Event.Remove("OnNewPoseApplied", RecordPoseData);
    }

    public void FixedUpdate()
    {

    }

    private void RecordPoseData(params object[] args)
    {

    }


}

