using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool SharedInstance;
    public GameObject poolObject;
    public int poolCapacity;
    private List<GameObject> obejctList;

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        obejctList = new List<GameObject>();
        for(int i = 0; i < poolCapacity; i++)
        {
            AddObjectToPool();
        }
    }

    void AddObjectToPool()
    {
        GameObject tmp;
        tmp = Instantiate(poolObject);
        tmp.SetActive(false);
        obejctList.Add(tmp);
    }

    public GameObject GetPooledObject()
    {
        for(int i = 0;i < poolCapacity;i++)
        {
            if(!obejctList[i].activeInHierarchy)
            {
                Debug.Log("출력된 오브젝트 index : "+i);
                return obejctList[i];
            }
        }
        AddObjectToPool();
        poolCapacity += 1;
        Debug.Log("출력된 오브젝트 index : " + (poolCapacity-1));
        return obejctList[poolCapacity-1];
    }
}
