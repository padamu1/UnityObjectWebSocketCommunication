using UnityEngine;
using System.Collections;
using WebSocketSharp;
using System;

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
        //InvokeRepeating(�Լ���, ������, �ݺ���);
        positionData = new PositionData();
        positionData.type = "��ġ����";
        positionData.name = "client_1";
        m_Socket = new WebSocketSharp.WebSocket("ws://localhost:3000");
        m_Socket.OnMessage += Recv;
        m_Socket.Connect();
    }
    void Update()
    {
        if(data != "")
        {
            // data�� ������ �ƴ� ��� Ȱ��.
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
    /// ���ο� �����͸� �޾ƿ��� �޼���.
    /// </summary>
    public void Recv(object sender, MessageEventArgs e)
    {
        //Debug.Log(e.Data);
        data =e.Data;
    }

    /// <summary>
    /// �÷��̾��� ��ġ������ ǥ����.
    /// </summary>
    void DataProcess()
    {
        // ������ ó��
        Debug.Log(data);
        if (data != "")
        {
            myObject = JsonUtility.FromJson<PositionData>(data);
            myObject.Print();
        }

        data = "";
    }
}