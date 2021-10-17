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

    [SerializeField] Rigidbody _rigidbody;
    HordeMoveControl _hordeMoveControl;
    List<GameObject> memberList = new List<GameObject>();

    void Start()
    {
        _hordeMoveControl.Init(this);
        SetMembersHordePosition();
    }

    void FixedUpdate()
    {
        _hordeMoveControl.FUpdate();
    }

    public void SetMembersHordePosition()
    {
        Vector3 movePosition = Vector3.zero;
        List<Vector3> targetPositionList = GetPositionListAround(movePosition, hordeRingMemberDistance, hordeRingMemberCount);

        for (int i = 0; i < memberList.Count; i++)
        {
            memberList[i].transform.DOLocalMove(targetPositionList[i], 0.5f);
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
        [SerializeField] private DynamicJoystick joystick;
        float _hordeRadius;
        Horde _horde;

        public void Init(Horde horde)
        {
            _horde = horde;
        }

        public void FUpdate()
        {
            
        }

        private void FindHordeRadius()
        {
            float maxDistace = 0f;
            for (int i = 0; i < memberList.Count; i++)
            {
                var radius = Vector3.Distance(_horde.memberList[i].transform.position, _horde.centerPoint.position);
                if (radius > maxDistace)
                {
                    maxDistace = radius;
                }
            }
            _hordeRadius = maxDistace - 0.1f;
        }

        private void ForwardMove()
        {
            _horde._rigidbody.MovePosition(_horde._rigidbody.transform.forward * Time.deltaTime * _forwardSpeed);
        }
        private void SwerveMove(Rigidbody rigidbody)
        {
            float clampedHorizontal = Mathf.Clamp(rigidbody.transform.position.x + joystick.Horizontal * _swerveSpeed * Time.deltaTime, _minHorizontalMovementBorder, _maxHorizontalMovementBorder);
            Vector3 direction = new Vector3(clampedHorizontal, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, direction, 1);
        }
    }

}
