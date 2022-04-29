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
    public static SocketManager instance;
    List<string> data;
    PositionData myObject;
    PositionData positionData;
    public GameObject playerObject;
    private List<OtherPlayerObject> OtherPlayerList;
    private bool checkUser;
    private WebSocketSharp.WebSocket m_Socket = null;

    void Awake()
    {
        if(instance == null)
            instance = this;
    }

    private void Start()
    {
        OtherPlayerList = new List<OtherPlayerObject>();
        myObject = null;
        data = new List<string>();
        Instantiate(playerObject, new Vector3(0f, 0.5f, 0f),Quaternion.identity);
        //InvokeRepeating("SendPlayerPosition", 0, 1);
        positionData = new PositionData();
        positionData.type = "��ġ����";
        positionData.name = "user_1"; // �ĺ� ������ id ���� �Է��ؾ���.
        m_Socket = new WebSocketSharp.WebSocket("ws://localhost:3000"); // ���� ip�ּ�
        m_Socket.OnMessage += Recv;
        m_Socket.Connect();
    }

    /// <summary>
    /// ���ο� �����͸� �޾ƿ��� �޼���.
    /// </summary>
    public void Recv(object sender, MessageEventArgs e)
    {
        data.Add(e.Data);
    }

    /// <summary>
    /// ��ġ������ ���� �����͸� ������.
    /// </summary>
    public void SendPlayerPosition(Vector3 playerPosition)
    {
        positionData.x = playerPosition.x;
        positionData.y = playerPosition.y;
        positionData.z = playerPosition.z;
        m_Socket.Send(JsonUtility.ToJson(positionData));
    }

    /// <summary>
    /// �޾ƿ� �����͸� ó����.
    /// </summary>
    void DataProcess()
    {
        for(int i = 0; i < data.Count; i++)
        {
            Debug.Log(data[i]);
            if (data[i] != null)
            {
                checkUser = false;
                RecvData newData = JsonUtility.FromJson<RecvData>(data[i]);
                if (newData.type == "��ġ����")
                {
                    myObject = JsonUtility.FromJson<PositionData>(data[i]);
                    if (OtherPlayerList.Count > 0)
                    {
                        for (int j = 0; j < OtherPlayerList.Count; j++)
                        {
                            if (OtherPlayerList[j].ObjectName == myObject.name)
                            {
                                OtherPlayerList[j].GameObject.transform.position = myObject.GetPosition();
                                checkUser = true;
                            }
                        }
                    }
                    if(!checkUser)
                    {
                        OtherPlayerObject tmp = new OtherPlayerObject();
                        tmp.ObjectName = myObject.name;
                        tmp.GameObject = PlayerPool.instance.GetPlayerInPool();
                        tmp.GameObject.transform.position = myObject.GetPosition();
                        OtherPlayerList.Add(tmp);
                    }
                }
            }
        }
        data = new List<string>();
    }

    void Update()
    {
        if(data.Count>0)
        {
            // data�� ������ �ƴ� ��� Ȱ��.
            DataProcess();
        }
    }

}