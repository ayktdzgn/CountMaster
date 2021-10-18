using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Rigidbody _rigidbody;
    public Transform target;
    public float speed = 1f;
    void FixedUpdate()
    {
        var dir = (target.position - _rigidbody.position).normalized;
        var distance =Mathf.Clamp( Vector3.Distance(target.position, _rigidbody.position),0,int.MaxValue);
        Debug.Log(dir);

        _rigidbody.velocity = (target.position - (_rigidbody.transform.position + _rigidbody.centerOfMass)) * speed * Time.deltaTime;
    }
}
