using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossTrigger : MonoBehaviour
{
    [Header("Boss1 :")]
    public GameObject boss1Stuck;
    public GameObject bossEnemy;
    private Animation boss1Anim;
    private bool boss1AnimOnce;
    private float boss1AnimStuckDuration;

    public bool boss1IsActive;
    public bool cameraToBoss1;
    public float delayAfterAnim;

    public GameObject[] bossDoors;
    public GameObject[] lightsBossRoom;

    public bool playerDetected;

    private PlayerBehaviour playerBehaviour;

    public FMODUnity.StudioEventEmitter musicManager;

    private void Start()
    {
        playerBehaviour = FindObjectOfType<PlayerBehaviour>();

        //Boss1
        boss1Anim = boss1Stuck.GetComponent<Animation>();
        boss1Anim.playAutomatically = false;
        boss1AnimOnce = true;
        boss1AnimStuckDuration = boss1Anim.clip.length;

        for (int i = 0; i < lightsBossRoom.Length; i++)
        {
            lightsBossRoom[i].SetActive(false);
        }
    }

    private void Update()
    {
        //Boss1
        if (boss1IsActive && boss1AnimOnce)
        {
            boss1Anim.Play("Boss1Stuck");
            StartCoroutine(CameraSetToBoss1());
            boss1AnimOnce = false;
        }

        if (boss1IsActive && playerDetected)
        {
            bossEnemy.GetComponentInChildren<Boss1>().enabled = true;

            for (int i = 0; i < bossDoors.Length; i++)
            {
                bossDoors[i].SetActive(true);
            }
        }

        if (!playerDetected)
        {
            bossEnemy.GetComponentInChildren<Boss1>().enabled = false;

            for (int i = 0; i < bossDoors.Length; i++)
            {
                bossDoors[i].SetActive(false);
                ResetMusic();
            }
        }
        else if (playerDetected && boss1IsActive)
        {
            BossMusic();
        }

        if (boss1IsActive)
        {
            for (int i = 0; i < lightsBossRoom.Length; i++)
            {
                lightsBossRoom[i].SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerDetected = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerDetected = false;
        }
    }

    private IEnumerator CameraSetToBoss1()
    {
        cameraToBoss1 = true;
        yield return new WaitForSeconds(boss1AnimStuckDuration);
        yield return new WaitForSeconds(delayAfterAnim);
        cameraToBoss1 = false;
        yield return new WaitForSeconds(2);
        boss1Stuck.SetActive(false);
        bossEnemy.SetActive(true);
    }

    void BossMusic()
    {
        musicManager.SetParameter("Boss Fight", 1);
        musicManager.SetParameter("Exploration", 1);
        musicManager.SetParameter("Puzzle Completed", 1);
        musicManager.SetParameter("Boss Door", 0);
        musicManager.SetParameter("Puzzle", 0);
        Debug.Log("BOSS INTENSIFIES");
    }

    void ResetMusic()
    {
        musicManager.SetParameter("Boss Fight", 0);
        musicManager.SetParameter("Boss Door", 1);
        Debug.Log("BOSS NOT INTENSIFIES");
    }
}
