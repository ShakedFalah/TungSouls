using UnityEngine;
using System.Collections.Generic;

public enum ItemType
{
    None,
    Bat,
    Coin,
    Invincibility,
    Magnet,
    Multiplier,
    SfxSource,
    Lamp
}

public class TaggedObjectPooler : Singleton<TaggedObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public ItemType tag;  // The tag to identify the pool.
        public GameObject prefab;  // The prefab to pool.
        public int initialPoolSize = 10;  // Initial number of objects in the pool.
    }

    public List<Pool> pools;  // A list of different pools.

    private Dictionary<ItemType, Queue<GameObject>> pooledObjects;  // A dictionary to hold pooled objects by tags.

    override
    protected void Awake()
    {
        pooledObjects = new Dictionary<ItemType, Queue<GameObject>>();

        base.Awake();
    }
    void Start()
    {
        foreach (Pool pool in pools)
        {
            pooledObjects[pool.tag] = new Queue<GameObject>();

            for (int i = 0; i < pool.initialPoolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);  // Deactivate initially to save resources.
                pooledObjects[pool.tag].Enqueue(obj);
            }
        }
    }

    public GameObject GetPooledObject(ItemType tag)
    {
        if (pooledObjects.ContainsKey(tag) && pooledObjects[tag].Count > 0)
        {
            GameObject obj = pooledObjects[tag].Dequeue();
            obj.SetActive(true);  // Activate the object when retrieved.
            return obj;
        }

        // If no objects are available, instantiate a new one.
        Pool pool = pools.Find(p => p.tag == tag);
        if (pool != null)
        {
            GameObject newObj = Instantiate(pool.prefab);
            return newObj;
        }

        Debug.LogWarning($"No pool found for tag: {tag}");
        return null;
    }

    public void ReturnObject(GameObject obj, ItemType tag)
    {
        if (obj != null && pooledObjects.ContainsKey(tag))
        {
            obj.SetActive(false);  // Deactivate the object when returned to pool.
            pooledObjects[tag].Enqueue(obj);
        }
    }

    public GameObject GetPooledObjectWithAutoReturn(ItemType tag)
    {
        GameObject obj = GetPooledObject(tag);

        if (obj != null)
        {
            var poolableComponent = obj.GetComponent<IPoolableObject>();
            if (poolableComponent != null)
            {
                // Set up auto-return action
                poolableComponent.SetReturnAction(() => ReturnObject(obj, tag));
            }
        }

        return obj;
    }

}