using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1GroundDetection : MonoBehaviour
{
    private Boss1 boss1;

    private void Start()
    {
        boss1 = GetComponentInParent<Boss1>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.tag == "Ground")
        {
            boss1.grounded = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.tag == "Ground")
        {
            boss1.grounded = false;
        }
    }
}
