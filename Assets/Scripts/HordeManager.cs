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
            if (value <= 0)
            {
                _hordeCount = 0;
                FindObjectOfType<GameManager>().gameState = GameState.Lose;
            }
            _hordeCount = value;          
        }
    }

    public List<Member> HordeList => hordeList;

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
                break;
            case OperatorType.Sub:
                if (member != null) 
                {
                    DeSpawnMember(member);
                }
                else
                {
                    DeSpawnMember(changeCount >= HordeCount ? HordeCount : changeCount, hordeList);
                }           
                break;
            case OperatorType.Mul:
                var mulCount = (HordeCount * changeCount) - HordeCount;
                SpawnMember(mulCount, memberSpawnTransform);
                break;
            case OperatorType.Div:
                var divCount = HordeCount - (HordeCount / changeCount);
                DeSpawnMember(divCount , hordeList);
                break;
        }
        OnHordeCountChange?.Invoke(HordeCount);
    }

    void SpawnMember(int spawnCount, Transform parent)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var spawned = poolManager.SpawnFromPool("Member", parent.position, Quaternion.identity);

            spawned.transform.SetParent(parent);
            spawned.GetComponent<Rigidbody>().AddForce(Vector3.forward * 2 , ForceMode.Impulse);
            spawned.transform.position = new Vector3(spawned.transform.position.x, parent.position.y, spawned.transform.position.z);
            spawned.transform.DOScale(0.6f, 1);

            hordeList.Add(spawned.GetComponent<Member>());
        }
        HordeCount += spawnCount;
    }

    void DeSpawnMember(int deSpawnCount, List<Member> deSpawnObjects)
    {
        for (int i = 0; i < deSpawnCount; i++)
        {
            poolManager.DestoryFromPool("Member", deSpawnObjects[0].gameObject);

            hordeList.Remove(deSpawnObjects[0]);
        }     
        HordeCount -= deSpawnCount;
    }

    void DeSpawnMember(Member deSpawnObject)
    {

        poolManager.DestoryFromPool("Member", deSpawnObject.gameObject);
        hordeList.Remove(deSpawnObject);
        HordeCount -= 1;

    }
}
