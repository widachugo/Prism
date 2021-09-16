using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Esprit : MonoBehaviour
{
    private float tCooldown;
    public float cooldownLightray;

    public Transform[] waypoints;
    public int indexWaypoint;

    private NavMeshAgent navMeshAgent;

    public GameObject lightrayOrigin;

    public void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        //Positionnement au waypoint de départ
        indexWaypoint = 0;
        transform.position = waypoints[indexWaypoint].position;
    }

    public void ObjectTouchWithLight()
    {
        StartCoroutine("LightrayAttack");
    }

    public IEnumerator LightrayAttack()
    {
        for (int i = 0; i < 3; i++)
        {
            lightrayOrigin.SetActive(true);
            yield return new WaitForSeconds(cooldownLightray);
            lightrayOrigin.SetActive(false);
            yield return new WaitForSeconds(cooldownLightray);
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
    }

    private void ChangeTargetWaypoint()
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
    //Deplacement vers le waypoint selon l'index
    private void SetDestinationToTarget()
    {
        Debug.Log(gameObject.name + ": Target le waypoint " + indexWaypoint);
        navMeshAgent.SetDestination(waypoints[indexWaypoint].position);
    }
}
