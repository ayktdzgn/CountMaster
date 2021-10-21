using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    bool isAttacking;
    Horde _targetHorde;
    Rigidbody _rigidbody;
    Animator _animator;
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
        _animator = GetComponent<Animator>();
    }

    public void PlayRunAnimation()
    {
        _animator.SetTrigger("Run");
    }

    public void AddForce(Vector3 center)
    {
        var relativeTarget = center - _rigidbody.position;

        _rigidbody.velocity = relativeTarget;
    }
}
