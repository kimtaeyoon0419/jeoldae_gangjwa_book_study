using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
    // ÃÑ¾ËÀÇ ÆÄ±«·Â
    public float dmaage = 20.0f;
    // ÃÑ¾Ë ¹ß»ç Èû
    public float force = 1500.0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rb.AddForce(transform.forward * force);
    }
}
