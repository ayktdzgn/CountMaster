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
    }

    public void MoveTo(Vector3 targetPosition)
    {
        _rigidbody.MovePosition(targetPosition);
    }
}
