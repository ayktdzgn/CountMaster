using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            HordeManager.OnHordeChange?.Invoke(1, OperatorType.Sub , this);
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HordeManager.OnHordeChange?.Invoke(1, OperatorType.Sub, this);
        }
    }

    public void PullMemberToCenter(Vector3 center, Rigidbody parentRigidbody)
    {
        //if ((center - _rigidbody.position).sqrMagnitude > 0.0001f)
        //{ // if vectors are different
            AddForce(center, parentRigidbody);
        //}
    }
    private void AddForce(Vector3 center, Rigidbody parentRigidbody)
    {
        var relativeTarget = center - _rigidbody.position;
        //var dir = relativeTarget.normalized;

        _rigidbody.velocity = new Vector3(relativeTarget.x + parentRigidbody.velocity.x , relativeTarget.y, relativeTarget.z + parentRigidbody.velocity.z);
    }
}
