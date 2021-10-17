using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HordeManager : MonoBehaviour
{
    [SerializeField] Transform memberSpawnTransform;

    List<Member> hordeList = new List<Member>();
    PoolManager poolManager;
    int _hordeCount;

    public int HordeCount
    {
        get => _hordeCount;
        set
        {
            if (_hordeCount - value <= 0)
            {
                _hordeCount = 0;
                //GameStateManager.state = GameState.Lose;
            }
        }
    }

    public delegate void HordeHandler(int numOfCharacter, OperatorType operatorType , Member member = null);
    public static HordeHandler OnHordeChange;
    public static event Action<int> OnHordeCountChange;

    private void Awake()
    {
        OnHordeChange += HordeCountChange;
    }

    private void OnDestroy()
    {
        OnHordeChange -= HordeCountChange;
    }

    private void Start()
    {
        poolManager = PoolManager.Instance;
    }

    void HordeCountChange(int changeCount , OperatorType operatorType , Member member = null)
    {
        switch (operatorType)
        {
            case OperatorType.Add:
                SpawnMember(changeCount, memberSpawnTransform);
                HordeCount += changeCount;
                break;
            case OperatorType.Sub:
                var tempMember = member != null ? member.gameObject : hordeList[0].gameObject;
                DeSpawnMember(changeCount , tempMember);
                HordeCount -= changeCount;
                break;
            case OperatorType.Mul:
                var mulCount = (HordeCount * changeCount) - HordeCount;
                SpawnMember(mulCount, memberSpawnTransform);
                HordeCount += mulCount;
                break;
            case OperatorType.Div:
                var divCount = HordeCount - (HordeCount / changeCount);
                DeSpawnMember(divCount , hordeList[0].gameObject);
                HordeCount -= divCount;
                break;
        }
        OnHordeCountChange?.Invoke(_hordeCount);
    }

    void SpawnMember(int spawnCount, Transform parent)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var spawned = poolManager.SpawnFromPool("Member", parent.position, Quaternion.identity);

            spawned.transform.SetParent(parent);
            spawned.GetComponent<Rigidbody>().velocity = transform.TransformDirection(Vector3.forward * 3);
            spawned.transform.position = new Vector3(spawned.transform.position.x, 0.2f, spawned.transform.position.z);
            spawned.transform.DOScale(0.6f, 1);

            hordeList.Add(spawned.GetComponent<Member>());
        }
    }

    void DeSpawnMember(int deSpawnCount, GameObject deSpawnObject)
    {
        for (int i = 0; i < deSpawnCount; i++)
        {
            poolManager.DestoryFromPool("Member", deSpawnObject);

            hordeList.Remove(deSpawnObject.GetComponent<Member>());
        }       
    }
}
