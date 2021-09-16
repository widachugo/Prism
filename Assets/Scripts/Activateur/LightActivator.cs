using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(FMODUnity.StudioEventEmitter))]
public class LightActivator : MonoBehaviour
{
    public enum ActivatorType { Debug_Ray, Power2, ColorRed, PlatformRotationLeft, PlatformRotationRight, PingPong, BossLock };
    public ActivatorType type;
    public int subActivated;

    [Header ("GameObjects linked to Activators")]
    public GameObject[] requiredActivators;
    public GameObject[] goEffect;
    public GameObject[] pathActivator;

    [Header("Audio")]
    public FMODUnity.StudioEventEmitter waveSound;
    public FMODUnity.StudioEventEmitter lightSound;

    private Renderer rend;
    private Animator[] anim;
    private bool playAnimOnce;

    [Header("Activator Values")]
    public float timeToActive;
    private float t;

    public bool rayTouchActivator;

    public bool activatorActive;

    public Material matActivate;
    private Material colorIni;
    private Material colorActivate;

    private float lerp;

    public GameObject[] platformRot;
    private bool bossDoorOpen = false;

    [Header ("Particle")]
    public GameObject effetParticule;
    public GameObject effetParticule2;
    private bool boolParticule;

    [Header("Speed Rotation ")]
    //expo pour platerform rotate 
    private float t2;
    private float curSpeed;
    public float maxSpeedTime;
    public float maxSpeed;


    private void Start()
    {
        rend = GetComponent<Renderer>();

        anim = new Animator[goEffect.Length];
        for (int i = 0; i < goEffect.Length; i++)
        {
            anim[i] = goEffect[i].GetComponent<Animator>();
        }
        playAnimOnce = false;

        boolParticule = true;

        colorIni = rend.material;
        colorActivate = matActivate;
    }

    public void Update()
    {
        lerp = Mathf.InverseLerp(0, timeToActive, t);

        if (!rayTouchActivator)
        {
            playAnimOnce = false;
        }

        // Si la porte du boss est fermée, les interrupteurs intermédiaires sont checkés constamment
        // Pas besoin d'illuminer un interrupteur supplémentaire pour valider l'ouverture
        if (type == ActivatorType.BossLock)
        {
                if (!bossDoorOpen)
                {
                    CheckActivator();
                }
                else
                {
                    // Si la porte est ouverte, le check est arrêté
                    return;
                }
        }

        else
        {
            if (rayTouchActivator && !activatorActive)
            {

                if (t < timeToActive)
                {
                    Instantiate(effetParticule2, transform);
                    t += Time.deltaTime;
                    lightSound.SetParameter("IsReflecting", 1);
                    Debug.Log("Light Sound playing");
                }

                if (t >= timeToActive)
                {
                    if (type == ActivatorType.PingPong || type == ActivatorType.PlatformRotationLeft || type == ActivatorType.PlatformRotationRight)
                    {
                        activatorActive = false;
                    }
                    else if (type == ActivatorType.Power2)
                    {
                        if (subActivated == requiredActivators.Length)
                        {
                            activatorActive = true;
                        }
                        else
                        {
                            activatorActive = false;
                        }
                    }
                    else
                    {
                        activatorActive = true;
                    }
                    lightSound.SetParameter("IsReflecting", 0);
                    Debug.Log("Light sound stopping");

                    if (type == ActivatorType.PingPong && !playAnimOnce)
                    {
                        playAnimOnce = true;
                        CheckActivator();
                    }
                    else if (type != ActivatorType.PingPong)
                    {
                        CheckActivator();
                    }
                }
            }

            else
            {
                if (t > 0)
                {
                    lightSound.SetParameter("IsReflecting", 0);
                    Debug.Log("Light sound stopping");
                    t -= Time.deltaTime;
                }
            }
        }
        

        if (rayTouchActivator && type == ActivatorType.PlatformRotationLeft)
        {/*
            t2 += Time.deltaTime;

            lerp = Mathf.InverseLerp(0, maxSpeedTime, t2);
            curSpeed = Mathf.SmoothStep(0, maxSpeed, lerp);
            */

            //Quand l'interrupteur est sur "PlatformRotationLeft"
            for (int i = 0; i < platformRot.Length; i++)
            {
                PlatformRotationStat platform = platformRot[i].GetComponent<PlatformRotationStat>();
                platformRot[i].transform.Rotate(0, platform.speed * 1 * Time.deltaTime , 0, Space.Self);
            }
        }
        else if (rayTouchActivator && type == ActivatorType.PlatformRotationRight)
        {/*
            t2 += Time.deltaTime;

            lerp = Mathf.InverseLerp(0, maxSpeedTime, t2);
            curSpeed = Mathf.SmoothStep(0, maxSpeed, lerp);
            */
            //Quand l'interrupteur est sur "PlatformRotationRight"
            for (int i = 0; i < platformRot.Length; i++)
            {
                PlatformRotationStat platform = platformRot[i].GetComponent<PlatformRotationStat>();
                platformRot[i].transform.Rotate(0, platform.speed * -1 * Time.deltaTime, 0, Space.Self);
            }
        }/*
        else
        {
            t = 0f;
            //lerp = 0f;
            curSpeed = 0f;
        }*/

        if (!activatorActive)
        {
            if (type == ActivatorType.Power2)
            {
                if (requiredActivators.Length != 0)
                {
                    rend.material.SetColor("_EmissiveColor", Color.Lerp(Color.red, Color.green, lerp));
                    rend.material.SetFloat("_EmissiveIntensity", Mathf.Lerp(0, 50, lerp));
                }
            }
            else
            {
                rend.material.SetColor("_EmissiveColor", Color.Lerp(Color.red, Color.green, lerp));
                rend.material.SetFloat("_EmissiveIntensity", Mathf.Lerp(0, 50, lerp));
            }
           
            if (goEffect.Length > 0)
            {
                if (goEffect[0].GetComponent<Renderer>() != null)
                {
                    goEffect[0].GetComponent<Renderer>().material.SetFloat("Vector1_21DBD330", lerp - 0.2f);
                    goEffect[0].GetComponent<Renderer>().material.SetColor("Color_C13FA49A", Color.Lerp(new Color(45, 0, 255), new Color(176, 0, 255), lerp));
                    goEffect[0].GetComponent<Renderer>().material.SetFloat("_EmissiveIntensity", 300);
                }

                if (goEffect[0].GetComponent<Renderer>() != null && goEffect[1].GetComponent<Renderer>() != null)
                {
                    goEffect[0].GetComponent<Renderer>().material.SetFloat("Vector1_21DBD330", lerp - 0.2f);
                    goEffect[0].GetComponent<Renderer>().material.SetColor("Color_C13FA49A", Color.Lerp(new Color(45, 0, 255), new Color(176, 0, 255), lerp));
                    goEffect[0].GetComponent<Renderer>().material.SetFloat("_EmissiveIntensity", 300);

                    goEffect[1].GetComponent<Renderer>().material.SetFloat("Vector1_21DBD330", 1 - lerp);
                    goEffect[1].GetComponent<Renderer>().material.SetColor("Color_C13FA49A", Color.Lerp(new Color(45, 0, 255), new Color(176, 0, 255), lerp));
                    goEffect[1].GetComponent<Renderer>().material.SetFloat("_EmissiveIntensity", 300);
                }
            }
            
        }

        if (pathActivator != null)
        {
            for (int i = 0; i < pathActivator.Length; i++)
            {
                if (!activatorActive)
                {
                    pathActivator[i].GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.Lerp(Color.red, Color.green, lerp));
                }
                else
                {
                    pathActivator[i].GetComponent<Renderer>().material.SetColor("_EmissiveColor", Color.green);
                }
            }
        }

    }

    public void CheckActivator()
    {
        // Détruit 
        for (int i = 0; i <= goEffect.Length; i++)
        {
            if (type == ActivatorType.Debug_Ray)
            {
                rend.material = matActivate;

                if (boolParticule)
                {
                    boolParticule = false;
                    Instantiate(effetParticule, transform);
                }

                if (goEffect[i].tag == "Door")
                {
                    // Destroy(goEffect);
                    goEffect[i].SetActive(false);
                    waveSound.Play();
                }
                else if (goEffect[i].tag == "Bridge")
                {
                    if (!anim[i].GetBool("Open"))
                    {
                        anim[i].SetBool("Open", true);
                        waveSound.Play();
                    }
                    else if (anim[i].GetBool("Open"))
                    {
                        anim[i].SetBool("Open", false);
                    }                   
                }
                else if (goEffect[i].tag == "Ascenseur")
                {

                    if (!anim[i].GetBool("Up"))
                    {
                        anim[i].SetBool("Up", true);
                        waveSound.Play();
                    }
                    else
                    {
                        anim[i].SetBool("Up", false);
                    }
                }
                else if (goEffect[i].tag == "Puzzle3Door1")
                {
                    anim[i].SetBool("Ouvert", true);
                    waveSound.Play();
                }
                else if (goEffect[i].tag == "Puzzle3Door2")
                {
                    anim[i].SetBool("Ouvert", true);
                    waveSound.Play();
                }
                else if (goEffect[i].tag == "LightGenerator")
                {
                    goEffect[i].SetActive(true);
                    waveSound.Play();
                }
                else if (goEffect[i].tag == "Yes")
                {
                    goEffect[i].SetActive(false);
                    waveSound.Play();
                }
                else if (goEffect[i].tag == "ObjectAffectLight")
                {
                    anim[i].SetBool("Open", true);
                    waveSound.Play();
                }
                else if (goEffect[i].tag == "Door2")
                {
                    goEffect[0].SetActive(false);
                    goEffect[1].GetComponent<BoxCollider>().enabled = true;
                    waveSound.Play();
                }

            }
            // Allumé
            else if (type == ActivatorType.ColorRed)
            {
                Debug.Log("Sub-activator: Loop entered");
                rend.material = matActivate;

                if (boolParticule)
                {
                    boolParticule = false;
                    Instantiate(effetParticule, transform);
                }
                Instantiate(effetParticule2, transform);

                gameObject.GetComponent<SubActivator>().activated = true;
                waveSound.Play();
                Debug.Log("Sub-activator: Good");
            }
            // Demande deux objets activés avant de pouvoir être allumé
            else if (type == ActivatorType.Power2)
            {
                if (requiredActivators.Length != 0)
                {
                    for (int y = 0; y < requiredActivators.Length; y++)
                    {
                        if (requiredActivators[y].GetComponent<SubActivator>().activated && !requiredActivators[y].GetComponent<SubActivator>().hasBeenActivated)
                        {
                            subActivated++;
                            requiredActivators[y].gameObject.GetComponent<SubActivator>().hasBeenActivated = true;
                            Debug.Log("Active Sub Activators: " + subActivated);
                        }
                    }

                    if (subActivated == requiredActivators.Length)
                    {
                        rend.material = matActivate;

                        if (boolParticule)
                        {
                            boolParticule = false;
                            Instantiate(effetParticule, transform);
                        }

                        Instantiate(effetParticule2, transform);

                        if (goEffect[i].tag == "Door")
                        {
                            goEffect[i].SetActive(false);
                            waveSound.Play();
                        }
                        else if (goEffect[i].tag == "Bridge")
                        {
                            anim[i].SetBool("Open", true);
                            waveSound.Play();
                        }
                    }
                }
                else if (requiredActivators.Length == 0)
                {
                    Debug.Log("Sub Activators list is empty !");
                    rend.material.SetColor("_EmissiveColor", Color.blue);
                }

            }
            //Jouer l'animation de l'objectif en ping pong
            else if (type == ActivatorType.PingPong)
            {
                if (boolParticule)
                {
                    boolParticule = false;
                    Instantiate(effetParticule, transform);
                }

                if (anim[i].GetBool("Open"))
                {
                    anim[i].SetBool("Open", false);

                }

                else if (!anim[i].GetBool("Open"))
                {
                    anim[i].SetBool("Open", true);
                }
            }

            // Porte de boss - S'ouvre quand tous les interrupteurs nécessaires sont activés
            else if (type == ActivatorType.BossLock)
            {
                if (requiredActivators.Length != 0)
                {
                    for (int y = 0; y < requiredActivators.Length; y++)
                    {
                        if (requiredActivators[y].GetComponent<SubActivator>().activated && !requiredActivators[y].GetComponent<SubActivator>().hasBeenActivated)
                        {
                            subActivated++;
                            requiredActivators[y].gameObject.GetComponent<SubActivator>().hasBeenActivated = true;
                            Debug.Log("Active Sub Activators: " + subActivated);
                        }
                    }

                    if (subActivated >= requiredActivators.Length)
                    {
                        FindObjectOfType<BossTrigger>().boss1IsActive = true;
                    }

                    if (subActivated == requiredActivators.Length)
                    {
                        bossDoorOpen = true;
                    }
                }

                else if (requiredActivators.Length == 0)
                {
                    Debug.Log("Sub Activators list is empty !");
                    rend.material.SetColor("_EmissiveColor", Color.blue);
                }
            }
        }
    }
}
