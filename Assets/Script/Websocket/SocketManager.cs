using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using System;

/// <summary>
/// �����͸� ó���ϱ� ���� Ŭ����.
/// </summary>
[System.Serializable]
public class RecvData
{
    public string type;
    public string data;
}

/// <summary>
/// �÷��̾��� ��ġ������ ����ִ� Ŭ����.
/// </summary>
[System.Serializable]
public class PositionData
{
    public string type;
    public string name;
    public float x;
    public float y;
    public float z;

    public void Print()
    {
        Debug.Log("x : " + x + "y : " + y + "z : " + z);
    }

    public Vector3 GetPosition()
    {
        return new Vector3(x, y, z);
    }
}

/// <summary>
/// �ٸ� �÷��̾��� ������Ʈ�� ó���ϱ� ���� Ŭ����.
/// </summary>
public class OtherPlayerObject
{
    private GameObject gameObject;
    private string objectName;
    public GameObject GameObject
    {
        get { return gameObject; }
        set { 
            gameObject = value;
            gameObject.transform.position = new Vector3(0f, 0.5f, 0f);
            gameObject.SetActive(true);
        }
    }
    public string ObjectName
    { 
        get { return objectName; }
        set { objectName = value; } 
    }
}

/// <summary>
/// ������ ó�� �ֿ� Ŭ����.
/// </summary>
public class SocketManager : MonoBehaviour
{
    string data;
    PositionData myObject;
    PositionData positionData;
    public GameObject playerObject;
    private List<OtherPlayerObject> OtherPlayerList;
    private WebSocketSharp.WebSocket m_Socket = null;
    private GameObject player;
    private Transform playerTransform;
    private void Start()
    {
        OtherPlayerList = new List<OtherPlayerObject>();
        myObject = null;
        data = "";
        player = Instantiate(playerObject, new Vector3(0f, 0.5f, 0f),Quaternion.identity);
        playerTransform = player.GetComponent<Transform>();
        InvokeRepeating("SendPlayerPosition", 0, 1);
        positionData = new PositionData();
        positionData.type = "��ġ����";
        positionData.name = "user_1";
        m_Socket = new WebSocketSharp.WebSocket("ws://localhost:3000");
        m_Socket.OnMessage += Recv;
        m_Socket.Connect();
    }

    /// <summary>
    /// ���ο� �����͸� �޾ƿ��� �޼���.
    /// </summary>
    public void Recv(object sender, MessageEventArgs e)
    {
        data = e.Data;
    }

    /// <summary>
    /// ��ġ������ ���� �����͸� ������.
    /// </summary>
    public void SendPlayerPosition()
    {
        positionData.x = playerTransform.position.x;
        positionData.y = playerTransform.position.y;
        positionData.z = playerTransform.position.z;
        m_Socket.Send(JsonUtility.ToJson(positionData));
    }

    /// <summary>
    /// �޾ƿ� �����͸� ó����.
    /// </summary>
    void DataProcess()
    {
        // ������ ó��
        Debug.Log(data);
        if (data != "")
        {
            RecvData newData = JsonUtility.FromJson<RecvData>(data);
            if (newData.type == "��ġ����")
            {
                myObject = JsonUtility.FromJson<PositionData>(data);
                if (OtherPlayerList.Count > 0)
                {
                    for (int i = 0; i < OtherPlayerList.Count; i++)
                    {
                        if (OtherPlayerList[i].ObjectName == myObject.name)
                        {
                            OtherPlayerList[i].GameObject.transform.position = myObject.GetPosition();
                        }
                    }
                }
                else
                {
                    OtherPlayerObject tmp = new OtherPlayerObject();
                    tmp.ObjectName = myObject.name;
                    tmp.GameObject = PlayerPool.instance.GetPlayerInPool();
                    tmp.GameObject.transform.position = myObject.GetPosition();
                    OtherPlayerList.Add(tmp);
                }
            }
        }

        data = "";
    }

    void Update()
    {
        if(data != "")
        {
            // data�� ������ �ƴ� ��� Ȱ��.
            DataProcess();
        }
    }

}