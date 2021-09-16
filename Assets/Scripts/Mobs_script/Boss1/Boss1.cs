using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1 : MonoBehaviour
{
    #region Variables
    [Range(0, 100)] public float healthBoss;

    [Range(1, 3)] public int stage;

    //Changement de stage avec la vie du boss
    [Header("Range life stage")]
    public float stage1ToStage2;
    public float stage2ToStage3;

    //Phase 1//
    [Header("Stage 1")]
    //Delay attaque phase 1
    public float delayAttack1Stage1;

    //Phase 2//
    [Header("Stage 2")]
    [Range(1, 2)] public int attackIndexStage2;
    //Delay attaque 1 phase 2
    public float delayAttack1Stage2;
    //Delay attaque 2 phase 2
    public float delayAttack2Stage2;

    //Phase 3//
    [Header("Stage 3")]
    [Range(1, 2)] public int attackIndexStage3;
    //Delay attaque 1 phase 3
    public float delayAttack1Stage3;
    //Delay attaque 2 phase 3
    public float delayAttack2Stage3;

    public enum AttackPlayed { RockAttack, JumpAttack };
    [Space(10)]
    public AttackPlayed attackPlayed;
    [Space(10)]

    //Attaque lancée de rochers
    [Header("Var rock attack (attack 1)")]
    public Transform originSpawnRock;
    public GameObject projectileRockPrefab;

    //Attaque saut sur joueur
    [Header("Var jump attack (attack 2)")]
    public float jumpVelocity;
    public float heightJump;
    private Vector3 iniPos;
    private Vector3 dir;
    private GameObject player;

    private float t;
    private float tDelayAttack;

    private Rigidbody rb;

    //Point faible
    public BossWeakness[] bossWeaknessObject;

    //Animator et Emitter FMOD
    private Animator anim;
    private FMODUnity.StudioEventEmitter sound;

    //Boolean si le boss est sur le sol (Detection faite avec "Boss1GroundDetection")
    public bool grounded;

    //Particules
    [Header("Particules")]
    public GameObject chocWaveParticle;
    #endregion

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        bossWeaknessObject = GetComponentsInChildren<BossWeakness>();
        anim = GetComponentInChildren<Animator>();
        sound = GetComponent<FMODUnity.StudioEventEmitter>();

        //Calcul et set des dégâts sur les points faibles
        for (int i = 0; i < bossWeaknessObject.Length; i++)
        {
            bossWeaknessObject[i].damage = 100 / bossWeaknessObject.Length;
        }
    }

    public void Update()
    {
        //Direction boss
        if (attackPlayed != AttackPlayed.JumpAttack)
        {
            if (player.transform.position.x <= transform.position.x)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            }
            else if (player.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
            }
        }

        //Check des stages + Attribution des delay d'attaques
        if (stage == 1)
        {
            tDelayAttack = delayAttack1Stage1;
        }
        else if (stage == 2)
        {
            if (attackIndexStage2 == 1)
            {
                tDelayAttack = delayAttack1Stage2;
            }
            else if (attackIndexStage2 == 2)
            {
                tDelayAttack = delayAttack2Stage2;
            }
        }
        else if (stage == 3)
        {
            if (attackIndexStage3 == 1)
            {
                tDelayAttack = delayAttack1Stage3;
            }
            else if (attackIndexStage3 == 2)
            {
                tDelayAttack = delayAttack2Stage3;
            }
        }

        //Switch stages par rapport à la vie du boss
        if (healthBoss > stage1ToStage2)
        {
            stage = 1;
        }
        if (healthBoss <= stage1ToStage2)
        {
            stage = 2;
        }
        if (healthBoss <= stage2ToStage3)
        {
            stage = 3;
        }

        t += Time.deltaTime;

        if (t >= tDelayAttack)
        {
            t = 0;
            Attack();
        }

        //Anim saut
        if (!grounded)
        {
            anim.SetBool("Jump", true);
        }
        else
        {
            anim.SetBool("Jump", false);

            if (attackPlayed != AttackPlayed.JumpAttack)
            {
                rb.velocity = Vector3.zero;
            }
        }

        //Mort du boss
        if (healthBoss <= 0)
        {
            gameObject.GetComponent<Boss1>().enabled = false;

            GameObject.FindObjectOfType<EndGame>().whiteScreenActivated = true;
        }
    }

    public void Attack()
    {
        if (stage == 1)
        {
            anim.SetBool("RockAttack", true);
        }
        else if (stage == 2)
        {
            if (attackIndexStage2 == 1)
            {
                anim.SetBool("RockAttack", true);
                attackIndexStage2 = 2;
            }
            else if (attackIndexStage2 == 2)
            {
                JumpAttack();
                attackIndexStage2 = 1;
            }
        }
        else if (stage == 3)
        {
            if (attackIndexStage3 == 1)
            {
                anim.SetBool("RockAttack", true);
                attackIndexStage3 = 2;
            }
            else if (attackIndexStage3 == 2)
            {
                JumpAttack();
                attackIndexStage3 = 1;
            }
        }

    }

    public void RockAttack()
    {
        Debug.Log("Rock attack");
        attackPlayed = AttackPlayed.RockAttack;

        Instantiate(projectileRockPrefab, originSpawnRock.transform.position, originSpawnRock.transform.rotation);
    }

    public void StopRockAttack()
    {
        anim.SetBool("RockAttack", false);
    }

    public void JumpAttack()
    {
        Debug.Log("Jump attack");
        attackPlayed = AttackPlayed.JumpAttack;

        iniPos = transform.position;
        dir.x = (iniPos.x - player.transform.position.x) * -1f;

        rb.AddForce(new Vector3(dir.x, heightJump, 0) * jumpVelocity); 
    }

    public void ChocWaveParticle()
    {
        Instantiate(chocWaveParticle, GetComponentInChildren<Boss1ChocWave>().transform.position, GetComponentInChildren<Boss1ChocWave>().transform.rotation);
        sound.Play();
    }
}
