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

    [SerializeField] Transform enemyHolder;
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
                Horde.OnPlayerAttackHandler?.Invoke(false,null);
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
            enemyHolder.DOMove(horde.transform.position , 3f);

            Horde.OnPlayerAttackHandler?.Invoke(true,transform);
        }
    }

    private void Start()
    {
        poolManager = PoolManager.Instance;

        SpawnEnemy(enemySpawnCount , enemyHolder);
        SetEnemyHordePosition();
    }

    private void FixedUpdate()
    {
            for (int i = 0; i < enemyList.Count; i++)
            {
                enemyList[i].AddForce(enemyHolder.position);
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
        List<Vector3> targetPositionList = PositionSetter.GetPositionListAround(movePosition, new float[]{0.6f,1.2f,1.8f,2.4f,3f },new int[] {5,10,20,30,40 });

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].transform.DOLocalMove(targetPositionList[i], 0.5f);
        }
    }
}
