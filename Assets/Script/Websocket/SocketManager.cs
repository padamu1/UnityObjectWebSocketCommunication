using UnityEngine;
using System.Collections;
using WebSocketSharp;
using System;

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
}

public class SocketManager : MonoBehaviour
{
    string data;
    PositionData myObject;
    PositionData positionData;
    private WebSocketSharp.WebSocket m_Socket = null;
    public GameObject playerObject;
    private GameObject player;
    private Transform playerTransform;
    private void Start()
    {
        myObject = null;
        data = "";
        player=Instantiate(playerObject,new Vector3(1f,0.5f,1f), Quaternion.identity);
        playerTransform = player.GetComponent<Transform>();
        //InvokeRepeating(함수명, 지연초, 반복초);
        positionData = new PositionData();
        positionData.type = "위치정보";
        positionData.name = "client_1";
        m_Socket = new WebSocketSharp.WebSocket("ws://localhost:3000");
        m_Socket.OnMessage += Recv;
        m_Socket.Connect();
    }
    void Update()
    {
        if(data != "")
        {
            // data가 공백이 아닌 경우 활용.
            DataProcess();
        }
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {
            positionData.x = playerTransform.position.x;
            positionData.y = playerTransform.position.y;
            positionData.z = playerTransform.position.z;
            m_Socket.Send(JsonUtility.ToJson(positionData));
        }
        */
    }

    /// <summary>
    /// 새로운 데이터를 받아오는 메서드.
    /// </summary>
    public void Recv(object sender, MessageEventArgs e)
    {
        //Debug.Log(e.Data);
        data =e.Data;
    }

    /// <summary>
    /// 플레이어의 위치정보를 표시함.
    /// </summary>
    void DataProcess()
    {
        // 데이터 처리
        Debug.Log(data);
        if (data != "")
        {
            myObject = JsonUtility.FromJson<PositionData>(data);
            myObject.Print();
        }

        data = "";
    }
}