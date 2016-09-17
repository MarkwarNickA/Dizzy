using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

using NewtonVR;

namespace Assets
{
    class PlayerScalingController : MonoBehaviour
    {
        NVRHand Hand;

        bool ScalingOn;

        public void Awake()
        {
            Hand = this.GetComponent<NVRHand>();
        }

        public void Update()
        {
            if (Hand && !Hand.IsInteracting)
            {
                if (Hand.UseButtonDown)
                {
                    ScalingOn = true;
                }
                else
                {
                    ScalingOn = false;
                }
            }

            if (ScalingOn)
            {
            }
        }

        public void FixedUpdate()
        {

        }

        private float ModifyScale( float step )
        {
            return 1f;
        }
    }
}
