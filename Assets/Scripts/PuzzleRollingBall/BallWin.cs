using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWin : MonoBehaviour
{
    public GameObject doorActivator;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<RollingBall>())
        {
            doorActivator.SetActive(false);
        }
    }
}
