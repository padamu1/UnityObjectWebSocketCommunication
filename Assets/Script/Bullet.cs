using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform transform;
    private float bulletMaxTime;
    private float bulletUseTime;

    void Start()
    {
        transform = this.GetComponent<Transform>();
        bulletMaxTime = 2f;
        bulletUseTime = 0f;
    }

    void Update()
    {
        if(bulletUseTime >= bulletMaxTime)
        {
            bulletUseTime = 0f;
            this.gameObject.SetActive(false);
        }
        bulletUseTime += Time.deltaTime;
        transform.position += transform.forward;
    }
}
