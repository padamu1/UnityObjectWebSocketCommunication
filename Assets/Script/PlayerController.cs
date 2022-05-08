using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform transform;
    private bool inputCheck;
    public static Vector3 vector3ForRotation = new Vector3(0, 45, 0);
    public static Vector3 vector3MoveXOne = new Vector3(1, 0, 0);

    void Start()
    {
        inputCheck = false;
        transform = this.GetComponent<Transform>();
        SocketManager.instance.SendPlayerPosition(transform.position);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            inputCheck = true;
            transform.position += (new Vector3(1f, 0f, 0f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputCheck = true;
            transform.position += (new Vector3(0f, 0f, 1f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputCheck = true;
            transform.position += (new Vector3(-1f, 0f, 0f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputCheck = true;
            transform.position += (new Vector3(0f, 0f, -1f) * Time.deltaTime * 100f);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                bullet.transform.rotation = transform.rotation;
                bullet.transform.position = transform.position;
                bullet.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(SetBullerAroundPlayer());
        }
    }

    void FixedUpdate()
    {
        if (inputCheck)
        {
            SocketManager.instance.SendPlayerPosition(transform.position);
            inputCheck = false;
        }
    }

    IEnumerator SetBullerAroundPlayer()
    {
        int count = 0;

        while (8 > count++)
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
            if (bullet != null)
            {
                transform.Rotate(vector3ForRotation);
                bullet.transform.rotation = transform.rotation;
                bullet.transform.position = transform.position + vector3MoveXOne;
                bullet.SetActive(true);
            }
        }
        yield return null;
    }

}
