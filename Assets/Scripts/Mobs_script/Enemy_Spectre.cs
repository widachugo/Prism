using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Spectre : MonoBehaviour
{
    public float health;

    public int damage;
    private float tCooldown;
    private bool cooldownActivated;
    public float cooldownAttack;

    private float tCooldownProjectile;
    public float cooldownProjectileAttack;

    public Transform[] waypoints;
    public int indexWaypoint;

    private NavMeshAgent navMeshAgent;

    private bool targetPlayer;

    private GameObject player;
    private PlayerBehaviour playerBehaviour;

    public GameObject projectileOrigin;
    public GameObject projectile;

    public GameObject particulDeath;

    public void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Positionnement au waypoint de départ
        indexWaypoint = 0;
        transform.position = waypoints[indexWaypoint].position;
    }

    public void Update()
    {
        //Update de la position du joueur s'il est ciblé par l'ennemi
        if (targetPlayer)
        {
            player = GameObject.FindGameObjectWithTag("PlayerBody");
            SetDestinationToPlayer(player.transform);
        }

        if (cooldownActivated)
        {
            tCooldown += Time.deltaTime;

            if (tCooldown >= cooldownAttack)
            {
                cooldownActivated = false;
                tCooldown = 0f;
            }
        }

        if (health <= 0)
        {
            Instantiate(particulDeath, transform);

            Destroy(gameObject,0.3f);
        }
    }

    //Trigger
    private void OnTriggerEnter(Collider other)
    {
        //Collision avec un waypoint
        if (other.gameObject.tag == "Waypoint_Enemy")
        {
            ChangeTargetWaypoint();
        }

        //Collision avec le joueur
        if (other.gameObject.tag == "PlayerBody")
        {
            targetPlayer = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PlayerBody")
        {
            targetPlayer = false;
            SetDestinationToTarget();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            tCooldownProjectile += Time.deltaTime;

            if (tCooldownProjectile >= cooldownProjectileAttack)
            {
                Instantiate(projectile, projectileOrigin.transform.position, projectileOrigin.transform.rotation);
                tCooldownProjectile = 0f;
            }
        }
    }

    //Collision
    private void OnCollisionStay(Collision collision)
    {
        //Collision avec le joueur
        if (collision.gameObject.tag == "PlayerBody")
        {
            if (!cooldownActivated)
            {
                playerBehaviour = collision.gameObject.GetComponent<PlayerBehaviour>();
                playerBehaviour.Damage(damage, this.gameObject);

                cooldownActivated = true;
            }
        }
    }

    private void ChangeTargetWaypoint()
    {
        if (!targetPlayer)
        {
            //Incrementation de l'index
            if (indexWaypoint != (waypoints.Length - 1))
            {
                indexWaypoint++;
            }
            else
            {
                indexWaypoint = 0;
            }

            SetDestinationToTarget();
        }
    }
    //Deplacement vers le waypoint selon l'index
    private void SetDestinationToTarget()
    {
        Debug.Log(gameObject.name + ": Target le waypoint " + indexWaypoint);
        navMeshAgent.SetDestination(waypoints[indexWaypoint].position);
    }

    //Player pris pour cible
    private void SetDestinationToPlayer(Transform playerPos)
    {
        Debug.Log(gameObject.name + ": Target le joueur");
        navMeshAgent.SetDestination(playerPos.position);
    }
}
