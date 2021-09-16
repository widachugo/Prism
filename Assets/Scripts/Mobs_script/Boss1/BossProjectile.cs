using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 dir;
    private Vector3 iniPos;
    private PlayerBehaviour player;
    private FMODUnity.StudioEventEmitter sound;

    public float force;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindObjectOfType<PlayerBehaviour>();
        sound = GetComponent<FMODUnity.StudioEventEmitter>();

        iniPos = transform.position;

        CalculateDirection();
    }

    public void CalculateDirection()
    {
        dir = (iniPos - player.transform.position).normalized * -1;

        rb.AddForce(new Vector3(dir.x, 0.2f, 0) * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerBehaviour>().Damage(50, gameObject);
            sound.Play();
            Destroy(gameObject);
        }
        else if (other.gameObject.tag != "PlayerBody" && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            sound.Play();
            Destroy(gameObject);
        }
    }
}
