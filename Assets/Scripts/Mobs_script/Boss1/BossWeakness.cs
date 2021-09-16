using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakness : MonoBehaviour
{
    [HideInInspector] public float damage;

    public float timeToDestroy;
    public float t;

    private bool activeOnce;

    private FMODUnity.StudioEventEmitter sound; 

    private void Start()
    {
        sound = GetComponent<FMODUnity.StudioEventEmitter>();
        t = 0f;
        activeOnce = false;
    }

    public void RaycastTouchBossWeakness()
    {
        t += Time.deltaTime;

        if (t >= timeToDestroy && !activeOnce)
        {
            activeOnce = true;
            sound.Play();
            Boss1 boss = GameObject.FindObjectOfType<Boss1>();
            boss.healthBoss -= damage;
            Destroy(gameObject);
        }
    }
}
