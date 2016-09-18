using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using VRTK;

namespace Assets
{

    enum ScalingState { NotScaling, ScalingUp, ScalingDown }

    class PlayerScalingController : MonoBehaviour
    {
        public Transform CameraRigRoot;
        public ScalingState CurrentScalingState;

        public float ScalingStep;

        public Vector3 CurrentPosition;
        public Vector3 StartPosition;
        public Vector3 MidPosition;

        VRTK_ControllerEvents Controller;

        public void Awake()
        {
            Controller = GetComponent<VRTK_ControllerEvents>();
            CurrentScalingState = ScalingState.NotScaling;
        }

        public void Update()
        {
            if (Controller.gripPressed)
            {
                if (CurrentScalingState == ScalingState.NotScaling)
                {
                    StartPosition = transform.position;
                }

                CurrentScalingState = ScalingState.ScalingUp;
                CurrentPosition = transform.position;
            }
            else
            {
                if (CurrentScalingState == ScalingState.ScalingUp)
                {
                    MidPosition = transform.position;
                }

                CurrentPosition = transform.position;
                if (Vector3.Distance(StartPosition, MidPosition) > Vector3.Distance(MidPosition, CurrentPosition))
                {
                    CurrentScalingState = ScalingState.ScalingDown;
                }
                else
                {
                    CurrentScalingState = ScalingState.NotScaling;
                }
            }

            if (CurrentScalingState == ScalingState.ScalingUp | CurrentScalingState == ScalingState.ScalingDown)
            {
                float PreClampedPercent = CalculatePercent(CurrentPosition, StartPosition, MidPosition, CurrentScalingState);
                SetScale(Mathf.Clamp(PreClampedPercent, 0, 1) + 1);
            }


        }

        public void FixedUpdate()
        {

        }

        private float CalculatePercent(Vector3 CurrentPoint, Vector3 StartPoint, Vector3 MidPoint, ScalingState State)
        {
            float PreClampedPercent = 0;
            float Distance = new Vector2((StartPoint - CurrentPoint).x, (StartPoint - CurrentPoint).z).magnitude;
            if (State == ScalingState.ScalingUp)
            {
                Distance = Vector3.Distance(StartPoint, CurrentPoint);
                PreClampedPercent = Distance * ScalingStep;
            }
            else if (State == ScalingState.ScalingDown)
            {
                float MidDistance = Vector3.Distance(StartPoint, MidPoint);
                Distance = Vector3.Distance(MidPoint, CurrentPoint);
                Distance = MidDistance - Distance;
                PreClampedPercent = Distance * ScalingStep;
            }
            return PreClampedPercent;
        }

        private void SetScale(float PercentScaled)
        {
            CameraRigRoot.localScale.Set(CameraRigRoot.localScale.x * PercentScaled, CameraRigRoot.localScale.y * PercentScaled, CameraRigRoot.localScale.z * PercentScaled);
        }
    }
}
