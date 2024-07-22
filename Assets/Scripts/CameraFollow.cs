using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public float MoveStiffness;
    public float RotationStiffness;

    public Vector3 MoveOffset;
    public Vector3 RotationOffset;

    public Transform Target;

    void HandleMovement()
    {
        Vector3 targetPos = Target.TransformPoint(MoveOffset);

        transform.position = Vector3.Lerp(transform.position, targetPos, MoveStiffness * Time.deltaTime);
    }

    void HandleRotation()
    {
        Vector3 direction = Vector3.Normalize(Target.position - transform.position);

        Quaternion rotation = Quaternion.LookRotation(direction + RotationOffset, Vector3.up);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, RotationStiffness * Time.deltaTime);
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }
}
