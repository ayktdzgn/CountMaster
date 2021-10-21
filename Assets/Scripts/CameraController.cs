using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    [SerializeField] float _followSpeed;

    float distanceFromTarget;

    private void Start()
    {
        distanceFromTarget = target.transform.position.z - transform.position.z;
    }
    private void LateUpdate()
    {
        var xPos = Mathf.Lerp(transform.position.x , target.transform.position.x , _followSpeed * Time.deltaTime);

        transform.position = new Vector3(xPos, transform.position.y, target.transform.position.z - distanceFromTarget);
    }
}
