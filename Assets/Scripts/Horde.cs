using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Horde : MonoBehaviour
{
    public int startMemberCount = 30;
    public Transform centerPoint;

    public int[] hordeRingMemberCount;
    public float[] hordeRingMemberDistance;

    [SerializeField] float pullCenterTime;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] HordeManager _hordeManager;
    [SerializeField] HordeMoveControl _hordeMoveControl;

    public delegate void HordeAttackHandler(bool isPlayerMovementFreezed,Transform enemySpawner);
    public static HordeAttackHandler OnPlayerAttackHandler;

    bool isHordeAttacking;
    bool isEndSequence;
    float tempTime = Mathf.Infinity;
    private float _smoothTime = 0.1f;
    Transform _enemyTarget;

    public HordeManager HordeManager => _hordeManager;
    public bool IsEndSequence => isEndSequence;

    private void Awake()
    {
        OnPlayerAttackHandler += HordeAttackStatus;
        HordeManager.OnHordeChange += PullCenter;
        EndSequence.OnHordeEndSequenceHandler += EndSequenceStatus;
    }

    private void OnDestroy()
    {
        OnPlayerAttackHandler -= HordeAttackStatus;
        HordeManager.OnHordeChange -= PullCenter;
        EndSequence.OnHordeEndSequenceHandler -= EndSequenceStatus;
    }

    void Start()
    {
        _hordeMoveControl.Init(this);

        HordeManager.OnHordeChange?.Invoke(startMemberCount , OperatorType.Add);
        SetMembersHordePosition();
    }

    void FixedUpdate()
    {       
        if (!isHordeAttacking)
        {
            if (DOTween.Play(transform) == _hordeMoveControl.AttackTweenID) DOTween.Kill(transform);
            _hordeMoveControl.FUpdate();
        }
        else
        {
            if(DOTween.Play(transform) != _hordeMoveControl.AttackTweenID)
            _hordeMoveControl.HordeAttackMovement(_enemyTarget);
        }

        if (tempTime < pullCenterTime)
        {
            tempTime += Time.fixedDeltaTime;

            for (int i = 0; i < _hordeManager.HordeList.Count; i++)
            {
                _hordeManager.HordeList[i].Rigidbody.isKinematic = false;
                _hordeManager.HordeList[i].PullMemberToCenter(centerPoint.position,_rigidbody);
                if(tempTime>= pullCenterTime)_hordeManager.HordeList[i].Rigidbody.isKinematic = true;
                if (tempTime <= pullCenterTime - 0.1f) _hordeMoveControl.FindHordeRadius();
            }
        }
    }

    private void HordeAttackStatus(bool val,Transform target)
    {
        if (val != isHordeAttacking)
        {
            isHordeAttacking = val;
            _enemyTarget = target;
        }
    }

    private void EndSequenceStatus(bool status)
    {
        if (status != isEndSequence)
        {
            isEndSequence = status;
        }
    }

    private void PullCenter(int numOfCharacter, OperatorType operatorType, Member member)
    {
        if(operatorType == OperatorType.Add || operatorType == OperatorType.Mul)SetMembersHordePosition();
        tempTime = 0;
    }

    public void SetMembersHordePosition()
    {
        Vector3 movePosition = Vector3.zero;
        List<Vector3> targetPositionList = PositionSetter.GetPositionListAround(movePosition, hordeRingMemberDistance, hordeRingMemberCount);

        for (int i = 0; i < _hordeManager.HordeList.Count; i++)
        {
            _hordeManager.HordeList[i].transform.DOLocalMove(targetPositionList[i], 0.5f);
        }
    }

    [Serializable]
    public struct HordeMoveControl
    {
        [SerializeField] private float _forwardSpeed;
        [SerializeField] private float _sideMoveSpeed;
        [SerializeField] float _hordeWalkableLimits;
        [SerializeField] private DynamicJoystick joystick;
        float _hordeRadius;
        Horde _horde;
        int _attackTweenID;

        public float HordeWalkableLimits => _hordeWalkableLimits;

        public int AttackTweenID => _attackTweenID;

        public void Init(Horde horde)
        {
            _horde = horde;
        }

        public void FUpdate()
        {
            if (!_horde.isEndSequence)
            {
                HordeMove(_horde._rigidbody);
            }
            else
            {
                HordeEndSequenceMove(_horde._rigidbody);
            }           
        }

        public void HordeAttackMovement(Transform target)
        {
            if(target != null)
                _attackTweenID =  _horde.transform.DOMove(target.position, 5f).intId;
        }

        public void FindHordeRadius()
        {
            float maxDistace = 0f;
            for (int i = 0; i < _horde._hordeManager.HordeList.Count; i++)
            {
                var radius = Vector3.Distance(_horde._hordeManager.HordeList[i].transform.position, _horde.centerPoint.position);
                if (radius > maxDistace)
                {
                    maxDistace = radius;
                }
            }
            _hordeRadius = maxDistace - 0.1f;
        }

        private void HordeEndSequenceMove(Rigidbody rigidbody)
        {
            var dir = 0 - Mathf.Clamp01(rigidbody.transform.position.x);
            rigidbody.MovePosition(new Vector3(dir * Time.deltaTime * 0.075f, rigidbody.transform.position.y, ForwardMove(rigidbody)));
        }

        private void HordeMove(Rigidbody rigidbody)
        {
            rigidbody.MovePosition(new Vector3( SwerveMove(rigidbody), rigidbody.transform.position.y, ForwardMove(rigidbody))); 
        }

        float ForwardMove(Rigidbody rigidbody)
        {
            return rigidbody.position.z + Time.deltaTime * _forwardSpeed;
        }
        float SwerveMove(Rigidbody rigidbody)
        {
            return Mathf.Clamp(rigidbody.transform.position.x + joystick.Horizontal * _sideMoveSpeed * Time.deltaTime, (-_hordeWalkableLimits/2)+_hordeRadius, (_hordeWalkableLimits/2)-_hordeRadius);
        }
    }


#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, 0.25f);
        Gizmos.DrawCube(transform.position, new Vector3(_hordeMoveControl.HordeWalkableLimits, 1, 1));
    }
#endif
}
