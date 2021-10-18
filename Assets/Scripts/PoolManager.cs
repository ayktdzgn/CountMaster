using System.Collections.Generic;
using UnityEngine;

public class PoolManager: MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    #region Singleton
    public static PoolManager Instance;
    public void Awake()
    {
        Instance = this;
   
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab,transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    #endregion 
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "does not exist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void DestoryFromPool(string tag, GameObject gameObject)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "does not exist");
        }
        if (poolDictionary[tag].Contains(gameObject))
        {
            gameObject.SetActive(false);
        }
    }

    public void DeActivateAllObjectFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + "does not exist");
        }
        for (int i = 0; i < poolDictionary[tag].Count; i++)
        {
            GameObject tempObject = poolDictionary[tag].Dequeue();
            if (tempObject.activeInHierarchy)
            {
                tempObject.SetActive(false);
            }
            poolDictionary[tag].Enqueue(tempObject);
        }
            
    }

}
