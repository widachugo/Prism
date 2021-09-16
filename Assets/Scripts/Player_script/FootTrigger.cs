using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootTrigger : MonoBehaviour
{
    public Rigidbody rb;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Enemy_Aberration>())
        {
            other.gameObject.GetComponent<Enemy_Aberration>().stun = true;
            rb.velocity += Vector3.up * 10;
        }

        if (other.gameObject.GetComponent<PressurePlateReset>())
        {
            other.gameObject.GetComponent<PressurePlateReset>().ballReset.ResetPos();
        }
    }
}
