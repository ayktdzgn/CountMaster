using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    float distanceFromTarget;
    public float xMargin;

    private void Start()
    {
        distanceFromTarget = target.transform.position.z - transform.position.z;
    }
    private void LateUpdate()
    {
        transform.position = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z - distanceFromTarget);
        
        //var newPos = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z - distanceFromTarget);
    }
}
