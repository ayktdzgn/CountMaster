using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isAttacking;
    Horde _targetHorde;
    Rigidbody _rigidbody;
    bool isHitByMember;
    int parentSpawnerID;

    public Horde TargetHorde { get => _targetHorde; set => _targetHorde = value; }
    public bool IsAttacking { get => isAttacking; set => isAttacking = value; }
    public Rigidbody Rigidbody  => _rigidbody;

    public bool IsHitByMember { get => isHitByMember; set => isHitByMember = value; }
    public int ParentSpawnerID { get => parentSpawnerID; set => parentSpawnerID = value; }

    private void OnEnable()
    {
        isHitByMember = false;
    }
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isAttacking && _targetHorde != null)
        {
            Attack(_targetHorde);
        }
    }

    public void AddForce(Vector3 center)
    {
        var relativeTarget = center - _rigidbody.position;

        _rigidbody.velocity = relativeTarget;
    }

    public void AttackStatus(bool status, Horde targetHorde)
    {
            if (status != isAttacking)
            {
                isAttacking = status;
            }
            _targetHorde = targetHorde;  
    }

    void Attack(Horde targetHorde)
    {
        this.transform.DOMove(targetHorde.transform.position, 1.5f);
    }
}
