using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void EnemySpawnerHandler(int value);
    public delegate void EnemyDestoryHandler(int spawnerID, GameObject obj);
    public static EnemySpawnerHandler OnEnemyCountChange;
    public static EnemyDestoryHandler OnEnemyDestory;

    [SerializeField] int enemySpawnerID;
    [SerializeField] Collider enemyZoneTrigger;
    [SerializeField] int enemySpawnCount;
    [SerializeField] EnemyUI enemyUI;

    int currentEnemyCount;
    PoolManager poolManager;
    List<Enemy> enemyList = new List<Enemy>();
    bool isTriggered;

    public int CurrentEnemyCount { 
        get { return currentEnemyCount; } 
        set {
            if (value > 0)
            {
                currentEnemyCount = value;
                enemyUI.StackCountChange(value);
            }
            else
            {
                enemyZoneTrigger.enabled = false;
                Horde.OnPlayerAttackHandler?.Invoke(false);
                enemyUI.CloseText();
            }        
        } 
    }

    private void Awake()
    {
        OnEnemyDestory += DestroyEnemy;
    }

    private void OnDestroy()
    {
        OnEnemyDestory -= DestroyEnemy;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            var horde = other.GetComponentInParent<Horde>();
            for (int i = 0; i < enemyList.Count; i++)
            {
                var enemy = enemyList[i];
                enemy.AttackStatus(true , horde);
            }

            Horde.OnPlayerAttackHandler?.Invoke(true);
        }
    }

    private void Start()
    {
        poolManager = PoolManager.Instance;

        SpawnEnemy(enemySpawnCount , transform);
        SetEnemyHordePosition();
    }

    private void FixedUpdate()
    {
        if (!isTriggered)
        {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].AddForce(transform.position);
            }
        }
    }

    void SpawnEnemy(int spawnCount, Transform parent)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            var spawned = poolManager.SpawnFromPool("Enemy", parent.position, Quaternion.identity);

            spawned.transform.SetParent(parent);
            spawned.GetComponent<Rigidbody>().AddForce(Vector3.forward * 2, ForceMode.Impulse);
            spawned.transform.position = new Vector3(spawned.transform.position.x, parent.position.y, spawned.transform.position.z);
            spawned.transform.DOScale(0.6f, 1);
            
            var enemy = spawned.GetComponent<Enemy>();
            enemy.ParentSpawnerID = enemySpawnerID;
            enemyList.Add(enemy);
        }
        CurrentEnemyCount += spawnCount;
    }

    private void DestroyEnemy(int id, GameObject obj)
    {
        if (id == enemySpawnerID)
        {
            poolManager.DestoryFromPool("Enemy", obj);
            enemyList.Remove(obj.GetComponent<Enemy>());

            CurrentEnemyCount--;
        }       
    }

    public void SetEnemyHordePosition()
    {
        Vector3 movePosition = Vector3.zero;
        List<Vector3> targetPositionList = GetPositionListAround(movePosition, new float[]{0.6f,1.2f,1.8f,2.4f,3f },new int[] {5,10,20,30,40 });

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].transform.DOLocalMove(targetPositionList[i], 0.5f);
        }
    }


    List<Vector3> GetPositionListAround(Vector3 startPos, float[] ringDistanceArray, int[] ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(startPos);
        for (int i = 0; i < ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(startPos, ringDistanceArray[i], ringPositionCountArray[i]));
        }
        return positionList;
    }

    List<Vector3> GetPositionListAround(Vector3 startPos, float distance, int positionCount)
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

    Vector3 ApplyRotationVector(Vector3 vec, float angle)
    {
        return Quaternion.Euler(0, angle, 0) * vec;
    }
}
