using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Transform transform;
    void Start()
    {
        transform = this.GetComponent<Transform>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.position += (new Vector3(1f, 0f, 0f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += (new Vector3(0f, 0f, 1f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += (new Vector3(-1f, 0f, 0f) * Time.deltaTime * 100f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += (new Vector3(0f, 0f, -1f) * Time.deltaTime * 100f);
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            GameObject bullet = ObjectPool.SharedInstance.GetPooledObject();
            if(bullet != null)
            {
                bullet.transform.rotation = transform.rotation;
                bullet.transform.position = transform.position;
                bullet.SetActive(true);
            }
        }
    }
}
