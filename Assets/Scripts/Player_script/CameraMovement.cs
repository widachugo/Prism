using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    private Vector3 offssetPos;
    [HideInInspector] public Vector2 offsetPosZone;

    public float smoothPos;

    public int focus;

    public GameObject boss1;
    private BossTrigger bossTrigger;

    [HideInInspector] public bool activatorFocusBoss;

    public void Start()
    {
        offssetPos = transform.position - target.position;
        bossTrigger = FindObjectOfType<BossTrigger>();
        activatorFocusBoss = false;
    }

    private void FixedUpdate()
    {
        if (!activatorFocusBoss)
        {
            if (!bossTrigger.cameraToBoss1)
            {
                transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x + offsetPosZone.x, target.position.y + offsetPosZone.y, target.position.z - focus) + offssetPos, Time.deltaTime * smoothPos);
            }
            else
            {
                //Focus sur boss
                transform.position = Vector3.Lerp(transform.position, new Vector3(boss1.transform.position.x, boss1.transform.position.y, boss1.transform.position.z - 20), Time.deltaTime);
            }
        }
        else
        {
            //Focus sur les activateurs du boss
            transform.position = Vector3.Lerp(transform.position, new Vector3(bossTrigger.transform.position.x, bossTrigger.transform.position.y, bossTrigger.transform.position.z - 40), Time.deltaTime);
        }
    }
}
