using UnityEngine;
using System.Collections;
using VRTK;

public class miniMeRemoteControlEvents : MonoBehaviour {

    public GameObject miniMe;
    private miniMeRemoteControl miniMeRemoteControlScript;

    private void Start()
    {
        miniMeRemoteControlScript = miniMe.GetComponent<miniMeRemoteControl>();
        GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);
        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        miniMeRemoteControlScript.SetTouchAxis(e.touchpadAxis);
    }

    private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        miniMeRemoteControlScript.SetTriggerAxis(e.buttonPressure);
    }

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        miniMeRemoteControlScript.SetTouchAxis(Vector2.zero);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        miniMeRemoteControlScript.SetTriggerAxis(0f);
    }
}
