using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerPool : MonoBehaviour
{
    public static PlayerPool instance;
    public int maxPlayerCount;
    public GameObject PlayerObject;
    private List<GameObject> listOfPlayer;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        listOfPlayer = new List<GameObject>();
        if (maxPlayerCount <= 0) Debug.Log("오브젝트가 생성되지 않음.");
        for (int i = 0; i < maxPlayerCount; i++)
        {
            MakePlayer();
        }
    }

    /// <summary>
    /// 새로운 오브젝트를 생성하여 풀에 넣는다.
    /// </summary>
    void MakePlayer()
    {
        GameObject tmp;
        if (PlayerObject == null) Debug.Log("오브젝트가 비어있음.");
        tmp = Instantiate(PlayerObject);
        tmp.SetActive(false);
        listOfPlayer.Add(tmp);
    }

    /// <summary>
    /// 풀에서 사용중이지 않은 오브젝트를 반환한다.
    /// </summary>
    public GameObject GetPlayerInPool()
    {
        for(int i = 0; i < maxPlayerCount; i++)
        {
            if(!listOfPlayer[i].activeInHierarchy) return listOfPlayer[i];
        }
        Debug.Log("풀에 오브젝트가 부족함.");
        return null;
    }
}
