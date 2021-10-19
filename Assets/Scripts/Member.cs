using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    bool isHitByEnemy;

    private void OnEnable()
    {
        isHitByEnemy = false;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isHitByEnemy)
        {
            isHitByEnemy = true;
            var enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.IsHitByMember = true;
            HordeManager.OnHordeChange?.Invoke(1, OperatorType.Sub , this);
            EnemySpawner.OnEnemyDestory?.Invoke(enemy.ParentSpawnerID,collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HordeManager.OnHordeChange?.Invoke(1, OperatorType.Sub, this);
        }
    }

    public void PullMemberToCenter(Vector3 center, Rigidbody parentRigidbody)
    {
        AddForce(center, parentRigidbody);
    }
    private void AddForce(Vector3 center, Rigidbody parentRigidbody)
    {
        var relativeTarget = center - _rigidbody.position;
        //var dir = relativeTarget.normalized;

        _rigidbody.velocity = new Vector3(relativeTarget.x + parentRigidbody.velocity.x , relativeTarget.y, relativeTarget.z + parentRigidbody.velocity.z);
    }
}
