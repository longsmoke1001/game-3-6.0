using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler SharedInstance;
    public List<Pool> pools;
    [System.Serializable]
    public class Pool
    {
        public List<GameObject> pooledObjects;
        public GameObject objectToPool;
        public int amountToPool;
    }

    void Awake()
    {
        SharedInstance = this;
    }

    //    Start is called before the first frame update
    void Start()
    {
        // Loop through list of pooled objects,deactivating them and adding them to the list 
        foreach (Pool pool in pools)
        {
            pool.pooledObjects = new List<GameObject>();
            for (int i = 0; i < pool.amountToPool; i++)
            {
                GameObject obj = (GameObject)Instantiate(pool.objectToPool);
                obj.SetActive(false);
                pool.pooledObjects.Add(obj);
                obj.transform.SetParent(this.transform); // set as children of Spawn Manager
            }
        }
    }

    public GameObject GetPooledObject(int x)
    {
        // For as many objects as are in the pooledObjects list
        List<GameObject> objects = pools[x].pooledObjects;
        for (int i = 0; i < objects.Count; i++)
        {
            // if the pooled objects is NOT active, return that object 
            if (!objects[i].activeInHierarchy)
            {
                return objects[i];
            }
        }
        // otherwise, return null   
        return null;
    }

}
