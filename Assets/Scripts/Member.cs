using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Member : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public Rigidbody Rigidbody => _rigidbody;

    Animator _animator;
    Enemy hitEnemy;

    float enemyHitHoldTime = 3f;
    float time;

    public bool IsTriggerByEndSequence { get => isTriggerByEndSequence; set => isTriggerByEndSequence = value; }
    public bool IsHitByEnemy { get => isHitByEnemy; set => isHitByEnemy = value; }

    bool isHitByEnemy;
    bool isTriggerByEndSequence;

    private void OnEnable()
    {
        isHitByEnemy = false;
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && (!isHitByEnemy||(hitEnemy!=null && !hitEnemy.gameObject.activeSelf)))
        {
            Enemy enemy;
            if (hitEnemy == null) 
            {
                enemy = collision.gameObject.GetComponent<Enemy>();
            }
            else
            {
                enemy = hitEnemy;
            }

            isHitByEnemy = true;
            hitEnemy = enemy;

            gameObject.layer = 8;
            enemy.gameObject.layer = 8;

            if (!enemy.IsHitByMember)
            {
               HordeManager.OnHordeChange?.Invoke(1, OperatorType.Sub, this);
               EnemySpawner.OnEnemyDestory?.Invoke(enemy.ParentSpawnerID, enemy);
            }

            enemy.IsHitByMember = true;
        }
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            HordeManager.OnHordeChange?.Invoke(1, OperatorType.Sub, this);
        }
    }

    private void Update()
    {
        if (isHitByEnemy)
        {
            if (time > enemyHitHoldTime)
            {
                isHitByEnemy = false;
                gameObject.layer = 6;
                hitEnemy = null;
                time = 0;
            }
            time += Time.deltaTime;
        }      
    }

    public void SetMemberYPos()
    {
        transform.localPosition = new Vector3(transform.localPosition.x,0, transform.localPosition.z);
    }

    public void PlayIdleAnimation()
    {
        _animator.SetTrigger("Idle");
    }

    public void PullMemberToCenter(Vector3 center, Rigidbody parentRigidbody)
    {
        AddForce(center, parentRigidbody);
    }
    private void AddForce(Vector3 center, Rigidbody parentRigidbody)
    {
        var relativeTarget = center - _rigidbody.position;

        _rigidbody.velocity = new Vector3(relativeTarget.x + parentRigidbody.velocity.x , relativeTarget.y, relativeTarget.z + parentRigidbody.velocity.z);
    }
}
