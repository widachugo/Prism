using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBall : MonoBehaviour
{
    private Vector3 iniPos;
    private Rigidbody rb;

    private void Start()
    {
        iniPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void ResetPos()
    {
        transform.position = iniPos;
        rb.velocity = Vector3.zero;
    }
}
