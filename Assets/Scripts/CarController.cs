using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float MaxAcceleration = 30.0f;
    public float BrakeAcceleration = 50.0f;

    public float SteeringSensitivity = 1.0f;
    public float MaxSteeringAngle = 30.0f;

    public List<Wheel> Wheels;

    float moveInput;
    float steerInput;

    Rigidbody carRb;

    public Vector3 CenterOfMass;

    public Quaternion WheelOffsetRotation;

    // Start is called before the first frame update
    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = CenterOfMass;

        WheelOffsetRotation = Quaternion.AngleAxis(90.0f, new Vector3(0.0f, 0.0f, 1.0f));
    }

    void GetInputs()
    {
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");
    }

    void Move()
    {
        foreach (Wheel wheel in Wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * -600.0f * MaxAcceleration * Time.deltaTime;
        }
    }

    void Steer()
    {
        foreach (Wheel wheel in Wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                float steerAngle = steerInput * SteeringSensitivity * MaxSteeringAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
            }
        }
    }

    void HandBrake()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            foreach (Wheel wheel in Wheels)
            {
                wheel.wheelCollider.brakeTorque = 300.0f * BrakeAcceleration * Time.deltaTime;
            }
        }
        else
        {
            foreach (Wheel wheel in Wheels)
            {
                wheel.wheelCollider.brakeTorque = 0.0f;
            }
        }
    }

    void AnimateWheels()
    {
        foreach (Wheel wheel in Wheels)
        {
            Quaternion rot;
            Vector3 pos;

            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot * Quaternion.Inverse(WheelOffsetRotation);
        }
    }

    void LateUpdate()
    {
        Move();
        Steer();
        HandBrake();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        AnimateWheels();
    }
}
