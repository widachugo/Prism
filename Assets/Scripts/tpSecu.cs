using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tpSecu : MonoBehaviour
{

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerBody");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PrismRotation>())
        {
            player.transform.position = new Vector3(transform.position.x, transform.position.y + 2, 0);
        }
    }
}
