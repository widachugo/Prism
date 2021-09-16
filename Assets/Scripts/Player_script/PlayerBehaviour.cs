using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    [Header ("Player Health")]
    public int health;
    public int maxHealth = 100;
    public Slider healthSlider;

    public Transform respawnPoint;

    public float recoilDamage;

    private Rigidbody rb;
    private PlayerController controller;
    private Animator anim;
    private Animator animPrism;

    private bool introChecker = false;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
        anim = GetComponentInChildren<Animator>();
        animPrism = GameObject.Find("Prism").GetComponent<Animator>();

        health = maxHealth;
        transform.position = respawnPoint.position;
    }

    public void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Intro") && !introChecker)
        {
            controller.enabled = false;
        }
        else if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Intro") && !introChecker)
        {
            controller.enabled = true;
            animPrism.enabled = false;
            introChecker = true;
            Debug.Log("Check intro animation loop: Ended");
        }

        healthSlider.value = health;

        if (health <= 0)
        {
            Death();
        }

        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
            Death();
        }
    }

    public void Damage(int amountDamage, GameObject enemy)
    {
        health -= amountDamage;
        Debug.Log("Vie joueur : " + health);

        var dir = ((enemy.transform.position - transform.position).normalized) * -1;
        rb.AddForce(new Vector3(dir.x, 5, 0) * recoilDamage);
    }

    public void Death()
    {
        transform.position = respawnPoint.position;
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Respawn")
        {
            respawnPoint = other.gameObject.transform;
        }
    }
}
