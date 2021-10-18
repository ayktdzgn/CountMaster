using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    public Vector3 rotateAngles;
    public float rotateSpeed;

    void FixedUpdate()
    {
        transform.Rotate(rotateAngles * rotateSpeed * Time.fixedDeltaTime);
    }
}
