using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectreProjectile : MonoBehaviour
{
    public float thrust = 1.0f;
    public Rigidbody rb;

    private GameObject player;

    private Vector3 iniPos;
    private Vector3 dir;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        iniPos = transform.position;
        dir = ((iniPos - player.transform.position).normalized) * -1;
    }

    void FixedUpdate()
    {
        rb.AddForce(dir * thrust);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
