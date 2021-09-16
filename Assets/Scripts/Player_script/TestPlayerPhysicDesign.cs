using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerPhysicDesign : MonoBehaviour
{
    private Rigidbody rb;

    public float speed;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = Vector3.left * speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            rb.velocity = Vector3.left * 0;
        }
    }
}
