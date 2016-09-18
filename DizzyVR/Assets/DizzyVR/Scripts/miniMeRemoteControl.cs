using UnityEngine;
using System.Collections;
using VRTK;

public class miniMeRemoteControl : MonoBehaviour {

    public GameObject cameraRig;
    public float maxAcceleration = 1f;
    public float jumpPower = 10f;

    private float acceleration = 1f;
    private float movementSpeed = 0f;
    private float strafeSpeed = 0f;

    private bool isJumping = false;
    private Vector2 touchAxis;
    private float triggerAxis;
    private Rigidbody rb;

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
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            touchAxis = new Vector2(0f, 0f);
        }

        CalculateSpeed();
        Move();
        Jump();
    }

    private void CalculateSpeed()
    {
        if (touchAxis.y != 0f)
        {
            movementSpeed += (acceleration * touchAxis.y);
            movementSpeed = Mathf.Clamp(movementSpeed, -maxAcceleration, maxAcceleration);
        }

        if (touchAxis.x != 0f)
        {
            strafeSpeed += (acceleration * touchAxis.x);
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
        //var deviceDirector = VRTK_DeviceFinder.DeviceTransform(deviceForDirection);
        var forward = (gameObject.transform.position - cameraRig.transform.position).normalized;
        var right = new Vector3(forward.z, 0, -forward.x);
        Vector3 movement = forward * movementSpeed * Time.deltaTime;
        Vector3 strafe = right * strafeSpeed * Time.deltaTime;
        //print("ms " + movementSpeed);
        //print("right" + right);
        //print("movement " + movement);
        //print("strafe " + strafe);
        float fixY = transform.position.y;
        print("f" + forward);
        Vector3 mostraf = (movement + strafe).normalized;
        print("m" + mostraf);
        transform.position += (movement);
        transform.position = new Vector3(transform.position.x, fixY, transform.position.z);

        //rb.MovePosition(rb.position + movement + strafe);
    }

    private void Jump()
    {
        if (!isJumping && triggerAxis > 0)
        {
            float jumpHeight = (triggerAxis * jumpPower);
            rb.AddRelativeForce(Vector3.up * jumpHeight);
            triggerAxis = 0f;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        isJumping = false;
    }

    private void OnTriggerExit(Collider collider)
    {
        isJumping = true;
    }
}
