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
        if (maxPlayerCount <= 0) Debug.Log("������Ʈ�� �������� ����.");
        for (int i = 0; i < maxPlayerCount; i++)
        {
            MakePlayer();
        }
    }

    /// <summary>
    /// ���ο� ������Ʈ�� �����Ͽ� Ǯ�� �ִ´�.
    /// </summary>
    void MakePlayer()
    {
        GameObject tmp;
        if (PlayerObject == null) Debug.Log("������Ʈ�� �������.");
        tmp = Instantiate(PlayerObject);
        tmp.SetActive(false);
        listOfPlayer.Add(tmp);
    }

    /// <summary>
    /// Ǯ���� ��������� ���� ������Ʈ�� ��ȯ�Ѵ�.
    /// </summary>
    public GameObject GetPlayerInPool()
    {
        for(int i = 0; i < maxPlayerCount; i++)
        {
            if(!listOfPlayer[i].activeInHierarchy) return listOfPlayer[i];
        }
        Debug.Log("Ǯ�� ������Ʈ�� ������.");
        return null;
    }
}
