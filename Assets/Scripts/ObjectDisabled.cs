using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDisabled : MonoBehaviour
{
    public LightRay[] lightRays;
    public LineRenderer[] lineRenderers;
    public Light[] lightRenderer;

    private float scaleSphere;
    private CameraMovement cameraM;

    public Vector3 sphereOffset;

    private void Start()
    {
        lightRays = GameObject.FindObjectsOfType<LightRay>();
        lineRenderers = GameObject.FindObjectsOfType<LineRenderer>();
        lightRenderer = GameObject.FindObjectsOfType<Light>();

        cameraM = GameObject.FindObjectOfType<CameraMovement>();

        for (int i = 0; i < lightRays.Length; i++)
        {
            lightRays[i].enabled = false;
        }
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].enabled = false;
        }
        for (int i = 0; i < lightRenderer.Length; i++)
        {
            lightRenderer[i].enabled = false;
        }
    }

    private void Update()
    {
        scaleSphere = cameraM.transform.position.z * 2;
        transform.localScale = new Vector3(scaleSphere - sphereOffset.x, scaleSphere - sphereOffset.y, scaleSphere - sphereOffset.z);
        transform.position = new Vector3(cameraM.transform.position.x, cameraM.transform.position.y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<LightRay>())
        {
            other.gameObject.GetComponent<LightRay>().enabled = true;
            other.gameObject.GetComponent<LineRenderer>().enabled = true;
        }
        if (other.gameObject.GetComponent<Light>())
        {
            other.gameObject.GetComponent<Light>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<LightRay>())
        {
            other.gameObject.GetComponent<LightRay>().enabled = false;
            other.gameObject.GetComponent<LineRenderer>().enabled = false;
        }
        if (other.gameObject.GetComponent<Light>())
        {
            other.gameObject.GetComponent<Light>().enabled = false;
        }
    }
}
