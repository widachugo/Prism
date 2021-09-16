using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorFocusBoss : MonoBehaviour
{
    private LightActivator lightActivator;
    private BossTrigger bossTrigger;
    private CameraMovement cameraMovement;

    public bool cameraToBoss;

    public float timeFocus;

    private void Start()
    {
        lightActivator = GetComponent<LightActivator>();
        bossTrigger = FindObjectOfType<BossTrigger>();
        cameraMovement = FindObjectOfType<CameraMovement>();

        cameraToBoss = false;
    }

    private void Update()
    {
        if (lightActivator.activatorActive && !cameraToBoss)
        {
            cameraToBoss = true;
            StartCoroutine(ActivatorFocus());
        }
    }

    private IEnumerator ActivatorFocus()
    {
        cameraMovement.activatorFocusBoss = true;
        yield return new WaitForSeconds(timeFocus);
        cameraMovement.activatorFocusBoss = false;
    }


}
