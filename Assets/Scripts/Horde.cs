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

    float tempTime = Mathf.Infinity;
    private float _smoothTime = 0.1f;

    private void Awake()
    {
        HordeManager.OnHordeChange += PullCenter;
    }

    private void OnDestroy()
    {
        HordeManager.OnHordeChange -= PullCenter;
    }

    void Start()
    {
        _hordeMoveControl.Init(this);

        HordeManager.OnHordeChange?.Invoke(startMemberCount , OperatorType.Add);
        SetMembersHordePosition();
    }

    void FixedUpdate()
    {
        _hordeMoveControl.FUpdate();
        if (tempTime < pullCenterTime)
        {
            tempTime += Time.fixedDeltaTime;

            for (int i = 0; i < _hordeManager.HordeList.Count; i++)
            {
                _hordeManager.HordeList[i].Rigidbody.isKinematic = false;
                _hordeManager.HordeList[i].PullMemberToCenter(centerPoint.position,_rigidbody);
                if(tempTime>= pullCenterTime)_hordeManager.HordeList[i].Rigidbody.isKinematic = true;
                if (tempTime <= 0.6f) _hordeMoveControl.FindHordeRadius();
            }
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
        List<Vector3> targetPositionList = GetPositionListAround(movePosition, hordeRingMemberDistance, hordeRingMemberCount);

        for (int i = 0; i < _hordeManager.HordeList.Count; i++)
        {
            _hordeManager.HordeList[i].transform.DOLocalMove(targetPositionList[i], 0.5f);
        }
    }

    
    List<Vector3> GetPositionListAround(Vector3 startPos, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPos);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPos , ringDistanceArray[i] , ringPositionCountArray[i]));
        }
        return positionList;
    }

    List<Vector3> GetPositionListAround(Vector3 startPos, float distance , int positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < positionCount; i++)
        {
            float angle = i * (360 / positionCount);
            Vector3 dir = ApplyRotationVector(new Vector3(1, 0), angle);
            Vector3 pos = startPos + dir * distance;
            positionList.Add(pos);
        }
        return positionList;
    }

    Vector3 ApplyRotationVector(Vector3 vec , float angle)
    {
        return Quaternion.Euler(0,angle,0) * vec;
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

        public float HordeWalkableLimits => _hordeWalkableLimits; 

        public void Init(Horde horde)
        {
            _horde = horde;
        }

        public void FUpdate()
        {
            HordeMove(_horde._rigidbody);
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
