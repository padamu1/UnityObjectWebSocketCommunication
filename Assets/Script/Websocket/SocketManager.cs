using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using System;

/// <summary>
/// 데이터를 처리하기 위한 클래스.
/// </summary>
[System.Serializable]
public class RecvData
{
    public string type;
    public string data;
}

/// <summary>
/// 플레이어의 위치정보가 담겨있는 클래스.
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
/// 다른 플레이어의 오브젝트를 처리하기 위한 클래스.
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
/// 데이터 처리 주요 클래스.
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
        positionData.type = "위치정보";
        positionData.name = "user_1";
        m_Socket = new WebSocketSharp.WebSocket("ws://localhost:3000");
        m_Socket.OnMessage += Recv;
        m_Socket.Connect();
    }

    /// <summary>
    /// 새로운 데이터를 받아오는 메서드.
    /// </summary>
    public void Recv(object sender, MessageEventArgs e)
    {
        data = e.Data;
    }

    /// <summary>
    /// 위치정보에 대한 데이터를 내보냄.
    /// </summary>
    public void SendPlayerPosition()
    {
        positionData.x = playerTransform.position.x;
        positionData.y = playerTransform.position.y;
        positionData.z = playerTransform.position.z;
        m_Socket.Send(JsonUtility.ToJson(positionData));
    }

    /// <summary>
    /// 받아온 데이터를 처리함.
    /// </summary>
    void DataProcess()
    {
        // 데이터 처리
        Debug.Log(data);
        if (data != "")
        {
            RecvData newData = JsonUtility.FromJson<RecvData>(data);
            if (newData.type == "위치정보")
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
            // data가 공백이 아닌 경우 활용.
            DataProcess();
        }
    }

}