using UnityEngine;
using System.Collections;
using VRTK;

public class miniMeRemoteControl : MonoBehaviour {

    public GameObject cameraRig;
    public float maxAcceleration = 1f;
    public float jumpPower = 10f;

    public bool RemoteControlEnabled;

    private float acceleration = 1f;
    private float movementSpeed = 0f;
    private float strafeSpeed = 0f;

    private bool isJumping = false;
    private Vector2 touchAxis;
    private float triggerAxis;
    private Rigidbody rb;

    //float miniModelScale;

    public VRTK_DeviceFinder.Devices deviceForDirection = VRTK_DeviceFinder.Devices.Headset;

    public void SetTouchAxis(Vector2 data)
    {
        touchAxis = data;
    }

    public void SetTriggerAxis(float data)
    {
        triggerAxis = data;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //miniModelScale = cameraRig.GetComponent<MiniModel>().miniModelScale;
        RemoteControlEnabled = false;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            //touchAxis = new Vector2(0f, 0f);
        }

        CalculateSpeed();
        Move();
        Jump();
    }

    private void CalculateSpeed()
    {

		//print (touchAxis);
		if (touchAxis.y != 0f) {
			//print("mmS" + miniModelScale);
			movementSpeed = (acceleration * touchAxis.y / 5);
			//print ("movementSpeed");
			movementSpeed = Mathf.Clamp (movementSpeed, -maxAcceleration, maxAcceleration);
		}

        if (touchAxis.x != 0f)
        {
			strafeSpeed = (acceleration * touchAxis.x / 5);
            strafeSpeed = Mathf.Clamp(strafeSpeed, -maxAcceleration, maxAcceleration);
        }

        if (touchAxis.y == 0f && touchAxis.x == 0f)
        {
            Decelerate();

        }
    }

    private void Decelerate()
    {
        //if (movementSpeed > 0)
        //{
        //    movementSpeed -= Mathf.Lerp(acceleration, maxAcceleration, 0f);
        //}
        //else if (movementSpeed < 0)
        //{
        //    movementSpeed += Mathf.Lerp(acceleration, -maxAcceleration, 0f);
        //}
        //else
        //{
        movementSpeed = 0;
        strafeSpeed = 0;
        //}
    }


    private void Move()
    {
        if (RemoteControlEnabled)
        {
            var deviceDirector = VRTK_DeviceFinder.DeviceTransform(deviceForDirection);
            //var forward = (gameObject.transform.position - cameraRig.transform.position).normalized;
            //var right = new Vector3(forward.z, 0, -forward.x);
            Vector3 movement = deviceDirector.forward * movementSpeed * Time.deltaTime;
            Vector3 strafe = deviceDirector.right * strafeSpeed * Time.deltaTime;
            float fixY = transform.position.y;
            transform.position += (movement + strafe);
            transform.position = new Vector3(transform.position.x, fixY, transform.position.z);
        }
    }

    private void Jump()
    {
		Debug.Log ("trig" + triggerAxis);
		if (!isJumping && triggerAxis > 0.5 )
        {
            
			float jumpHeight = ( jumpPower);
			print ("JUMPING!");
            rb.AddRelativeForce(Vector3.up * jumpHeight);
            triggerAxis = 0f;
			isJumping = true;
        }
		if(rb.velocity.y==0){
			isJumping = false;	
		}
	
    }

    private void OnTriggerStay(Collider collider)
    {
        //isJumping = false;
    }

    private void OnTriggerExit(Collider collider)
    {
       // isJumping = true;
    }
}
