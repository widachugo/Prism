using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRay : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private Ray ray;
    private RaycastHit hit;

    private Vector3 reflectionDirection;

    //Nombre de fois que le rayon rebondit
    public int nbMaxBound;

    public int nbReflections = 5;

    //Nombre de point de la linerenderer 
    private int nbPoints;

    //Script de l'activateur touché par le raycast
    private LightActivator activator;

    //Dégât sur les ennemies
    public float damagePower;

    //point ligth redirection
    public GameObject lightredir;

    //La dernière activateur de touché
    private LightActivator lastActivator;

    //Affichage du raycast d'origine dans l'éditeur
    private void OnDrawGizmosSelected()
    {
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, ray.direction * 100, Color.red);
    }

    void Awake()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        //Ray origin
        ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, ray.direction * 100, Color.red);

        //Première linerenderer
        nbPoints = nbReflections;

        //Aspect graphique
        lineRenderer.SetVertexCount(nbPoints);
        lineRenderer.SetPosition(0, transform.position);

        for (int i = 0; i <= nbMaxBound; i++)
        {
            //Quand le rayon n'a pas encore rebondit
            if (i == 0)
            { 
                //Code pour le premier rebond
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, LayerMask.GetMask("ObjectActingOnLight", "Ground")))
                {
                    if (hit.collider.tag == "ObjectAffectLight")
                    {
                        ObjectLightEffect objectLightEffect = hit.collider.GetComponent<ObjectLightEffect>();
                        if (objectLightEffect.switchMod == ObjectLightEffect.SwitchMod.Reflexion)
                        {
                            //Calcul de la reflexion
                            reflectionDirection = Refract(1, 1, -hit.normal, ray.direction);
                        }
                        else if (objectLightEffect.switchMod == ObjectLightEffect.SwitchMod.Refraction)
                        {
                            //Calcul de la refraction
                            reflectionDirection = Refract(1, 2f, hit.normal, ray.direction);
                        }

                        //Nouveau ray
                        ray = new Ray(hit.point, reflectionDirection);

                        //Debug ray
                        Debug.DrawRay(hit.point, hit.normal * 3, Color.blue);
                        Debug.DrawRay(hit.point, reflectionDirection * 100, Color.yellow);

                        //Debug
                        Debug.Log("Object name: " + hit.transform.name);

                        //Aspect graphique
                        if (nbReflections == 1)
                        {
                            //Ajout d'un point sur la linerenderer 
                            lineRenderer.SetVertexCount(++nbPoints);
                        }

                        //Initialisation du linerenderer 
                        lineRenderer.SetPosition(i + 1, hit.point);
                    }
                    else
                    {
                        //Aspect graphique
                        if (nbReflections == 1)
                        {
                            //Ajout d'un point sur la linerenderer 
                            lineRenderer.SetVertexCount(++nbPoints);
                        }

                        //Initialisation du linerenderer 
                        lineRenderer.SetPosition(i + 1, hit.point);
                    }

                    ColliderDetection();
                }
            }
            else
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, LayerMask.GetMask("ObjectActingOnLight", "Ground")))
                {
                    if (hit.collider.tag == "ObjectAffectLight")
                    {
                        ObjectLightEffect objectLightEffect = hit.collider.GetComponent<ObjectLightEffect>();
                        if (objectLightEffect.switchMod == ObjectLightEffect.SwitchMod.Reflexion)
                        {
                            //Calcul de la reflexion
                            reflectionDirection = Refract(1, 1, -hit.normal, ray.direction);
                        }
                        else if (objectLightEffect.switchMod == ObjectLightEffect.SwitchMod.Refraction)
                        {
                            //Calcul de la refraction
                            reflectionDirection = Refract(1, 2f, hit.normal, ray.direction);
                        }

                        //Nouveau ray
                        ray = new Ray(hit.point, reflectionDirection);

                        //Debug ray
                        Debug.DrawRay(hit.point, hit.normal * 3, Color.blue);
                        Debug.DrawRay(hit.point, reflectionDirection * 100, Color.yellow);

                        //Debug
                        Debug.Log("Object name: " + hit.transform.name);

                        //Aspect graphique
                        //Ajout d'un point sur la linerenderer 
                        lineRenderer.SetVertexCount(++nbPoints);
                        //Initialisation du linerenderer  
                        lineRenderer.SetPosition(i + 1, hit.point);

                        if(lightredir == null)
                        {
                            return;
                        }
                        else {
                            lightredir.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        }

                    }
                    else
                    {
                        //Aspect graphique
                        //Ajout d'un point sur la linerenderer 
                        lineRenderer.SetVertexCount(++nbPoints);
                        //Initialisation du linerenderer  
                        lineRenderer.SetPosition(i + 1, hit.point);

                        if (lightredir == null)
                        {
                            return;
                        }
                        else
                        {
                            lightredir.transform.position = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        }
                    }

                    ColliderDetection();
                }
            }
        }
    }

    public void ColliderDetection()
    {
        //Check si le raycast touche un point faible de boss
        if (hit.collider.GetComponent<BossWeakness>())
        {
            hit.collider.GetComponent<BossWeakness>().RaycastTouchBossWeakness();
        }
        // Check si le raycast rentre en collision avec un activateur (Debug Tag)
        if (hit.collider.tag == "Activator_Testing")
        {
            activator = hit.collider.GetComponent<LightActivator>();
            lastActivator = activator;
            lastActivator.rayTouchActivator = true;
        }
        else if (lastActivator != null)
        {
            lastActivator.rayTouchActivator = false;
            lastActivator = null;
        }
        // Check si le raycast rentre en collision avec un ennemi
        if (hit.collider.tag == "Enemy")
        {
            //Si le gameobject est un spectre
            if (hit.collider.GetComponent<Enemy_Spectre>())
            {
                Enemy_Spectre enemy_Spectre = hit.collider.GetComponent<Enemy_Spectre>();
                enemy_Spectre.health -= damagePower;
                Debug.Log(name + "touch Spectre");
            }
            //Si le gameobject est une aberration
            if (hit.collider.GetComponent<Enemy_Aberration>())
            {
                Enemy_Aberration enemy_Aberration = hit.collider.GetComponent<Enemy_Aberration>();
                Debug.Log(name + "touch Aberration");
            }
            //Si le gameobject est un esprit
            if (hit.collider.GetComponent<Enemy_Esprit>())
            {
                Enemy_Esprit enemy_Esprit = hit.collider.GetComponent<Enemy_Esprit>();
                enemy_Esprit.ObjectTouchWithLight();
                Debug.Log(name + "touch Esprit");
            }
        }
    }

    //Calcul réflexion et réfraction
    public static Vector3 Refract(float RI1, float RI2, Vector3 surfNorm, Vector3 incident)
    {
        surfNorm.Normalize();
        incident.Normalize();

        return (RI1 / RI2 * Vector3.Cross(surfNorm, Vector3.Cross(-surfNorm, incident)) - surfNorm * Mathf.Sqrt(1 - Vector3.Dot(Vector3.Cross(surfNorm, incident) * (RI1 / RI2 * RI1 / RI2), Vector3.Cross(surfNorm, incident)))).normalized;
    }
}