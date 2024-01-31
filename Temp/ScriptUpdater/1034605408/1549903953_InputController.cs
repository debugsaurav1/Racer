using UnityEngine;

public class InputController : MonoBehaviour
{
    public WheelCollider[] wheelColliders;
    public float maxTorque = 200f;
    public float maxSteerAngle = 30f;
    public float brakeTorque = 500f;
    public float boostMultiplier = 5f;
    public float boostDuration = 10f;
    public int xRotationLimit = 20;
    public int yRotationLimit = 20;
    public int zRotationLimit = 20;

    private bool isBoosting;
    private float boostEndTime;

    private void Start()
    {
        GetComponent<Rigidbody>().centerOfMass += new Vector3(0, -1f, 0);
    }
    void Update()
    {
        
        float torque = isBoosting ? maxTorque * boostMultiplier : maxTorque;
        torque *= Input.GetAxis("Vertical");

        float steerAngle = maxSteerAngle * Input.GetAxis("Horizontal");

        bool brake = Input.GetKey(KeyCode.Space);
        bool boost = Input.GetKey(KeyCode.LeftShift) && !isBoosting && Time.time > boostEndTime;

        if (boost)
        {
            isBoosting = true;
            boostEndTime = Time.time + boostDuration;
        }
        else if (Time.time > boostEndTime)
        {
            isBoosting = false;
        }

        for (int i = 0; i < wheelColliders.Length; i++)
        {
            var wheel = wheelColliders[i];

            if (brake)
            {
                wheel.brakeTorque = brakeTorque;
            }
            else
            {
                wheel.brakeTorque = 0;
            }

            // Apply motor torque only to rear wheels
            if (i >= 2) // Assuming [2] and [3] are the rear wheels
            {
                wheel.motorTorque = torque;
            }

            // Apply steering to front wheels
            if (i < 2) // Assuming [0] and [1] are the front wheels
            {
                wheel.steerAngle = steerAngle;
            }
        }
       // LimitRotation();
    }

    void LimitRotation()
    {
        if (transform.rotation.eulerAngles.x > xRotationLimit)
            transform.rotation = Quaternion.identity;
        if (transform.rotation.eulerAngles.y > yRotationLimit)
            transform.rotation = Quaternion.identity;
        if (transform.rotation.eulerAngles.z > zRotationLimit)
            transform.rotation = Quaternion.identity;

    }

}

