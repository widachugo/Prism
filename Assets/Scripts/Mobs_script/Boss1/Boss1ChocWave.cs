using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ChocWave : MonoBehaviour
{
    public int damageChocWave;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerBehaviour player = GameObject.FindObjectOfType<PlayerBehaviour>();
            player.Damage(damageChocWave, this.gameObject);
        }
    }
}
