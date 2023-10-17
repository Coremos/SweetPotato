using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : SingleTone<ObjectPoolManager>
{
    private GameObject poolRootObject;
    private Dictionary<int, int> prefabs; // gameobject 에서 , prefab 찾기용
    private Dictionary<int, ObjectPool<GameObject>> pools;

    public ObjectPoolManager()
    {
        pools = new Dictionary<int, ObjectPool<GameObject>>();
        prefabs = new Dictionary<int, int>();
    }


    public void Initialize()
    {
        Clear();

        if (poolRootObject != null)
            Destroy(poolRootObject);
        poolRootObject = new GameObject("poolRootObject");

        poolRootObject.transform.SetParent(this.transform);
        poolRootObject.SetActive(false);
    }
    private void Clear()
    {
        foreach (var pool in pools)
        {
            pool.Value.Dispose();
        }
        pools.Clear();
    }

    public GameObject doInstantiate(GameObject prefab, Vector3 position)
    {
        //if (pools.TryGetValue(prefab.GetInstanceID(), out ObjectPool<GameObject> op) == false)
        //{
        //    op = new ObjectPool<GameObject>(() => ActionOnCreate(prefab, position), ActionOnGet, ActionOnRelease, ActionOnDestroy);
        //    pools.Add(prefab.GetInstanceID(), op);
        //}

        //var go = op.Get();

        //if (prefabs.ContainsKey(go.GetInstanceID()) == false)
        //    prefabs.Add(go.GetInstanceID(), prefab.GetInstanceID());
        
        //return go;
        return Instantiate(prefab, position, Quaternion.identity);
    }

    public void doDestroy(GameObject go)
    {
        //if (prefabs.TryGetValue(go.GetInstanceID(), out int prefab) && pools.TryGetValue(prefab, out ObjectPool<GameObject> op))
        //{
        //    prefabs.Remove(go.GetInstanceID());
        //    op.Release(go);
        //}
        //else
        //{
        //    Destroy(go);
        //}
        Destroy(go);
    }

    private GameObject ActionOnCreate(GameObject prefab, Vector3 position)
    {
        return GameObject.Instantiate<GameObject>(prefab, position, Quaternion.identity);
    }

    private void ActionOnGet(GameObject go)
    {
        if (go == null) return;
        go.transform.SetParent(null);
        go.SetActive(true);
    }

    private void ActionOnRelease(GameObject go)
    {
        go.transform.SetParent(poolRootObject.transform);
        go.SetActive(false);
    }

    private void ActionOnDestroy(GameObject go)
    {
        Destroy(go);
    }
}
