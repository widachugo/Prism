using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrismRotation : MonoBehaviour
{
    public float maxSpeed;
    private float curSpeed;

    private float desiredRot = -90;
    public float desiredRotZ;
    public float maxSpeedTime;
    private float lerp;

    private float t;

    private Rigidbody rb;

    private bool rightDirection;
    private bool leftDirection;

    public Slider sensitivitySlider;

    private void Start()
    {
        rb = GetComponentInParent<Rigidbody>();
        sensitivitySlider.value = 75;
    }

    public void Update()
    {
        maxSpeed = sensitivitySlider.value;

        if (rb.velocity.x > 0.05f)
        {
            rightDirection = true;
            leftDirection = false;
        }
        else if (rb.velocity.x < -0.05f)
        {
            rightDirection = false;
            leftDirection = true;
        }

        if (rightDirection)
        {
            //Rotation prisme vers la gauche
            if (Input.GetKey(KeyCode.A) || Input.GetAxis("RotationLeft") > 0)
            {
                t += Time.deltaTime;

                lerp = Mathf.InverseLerp(0, maxSpeedTime, t);
                curSpeed = Mathf.SmoothStep(0, maxSpeed, lerp);

                if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot += curSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot += maxSpeed * Time.deltaTime;
                }

                var desiredRotQ = Quaternion.Euler(desiredRot, 0, desiredRotZ);
                transform.localRotation = desiredRotQ;
            }
            //Rotation prisme vers la droite
            else if (Input.GetKey(KeyCode.E) || Input.GetAxis("RotationRight") > 0)
            {
                t += Time.deltaTime;

                lerp = Mathf.InverseLerp(0, maxSpeedTime, t);
                curSpeed = Mathf.SmoothStep(0, maxSpeed, lerp);
                if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot -= curSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot -= maxSpeed * Time.deltaTime;
                }

                var desiredRotQ = Quaternion.Euler(desiredRot, 0, desiredRotZ);
                transform.localRotation = desiredRotQ;
            }
            //Reset des variables quand il n'y a pas d'input
            else
            {
                t = 0f;
                lerp = 0f;
                curSpeed = 0f;
            }
        }
        else if (leftDirection)
        {
            //Rotation prisme vers la gauche
            if (Input.GetKey(KeyCode.E) || Input.GetAxis("RotationRight") > 0)
            {
                t += Time.deltaTime;

                lerp = Mathf.InverseLerp(0, maxSpeedTime, t);
                curSpeed = Mathf.SmoothStep(0, maxSpeed, lerp);
                if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot += curSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot += maxSpeed * Time.deltaTime;
                }

                var desiredRotQ = Quaternion.Euler(desiredRot, 0, desiredRotZ);
                transform.localRotation = desiredRotQ;
            }
            //Rotation prisme vers la droite
            else if (Input.GetKey(KeyCode.A) || Input.GetAxis("RotationLeft") > 0)
            {
                t += Time.deltaTime;

                lerp = Mathf.InverseLerp(0, maxSpeedTime, t);
                curSpeed = Mathf.SmoothStep(0, maxSpeed, lerp);
                if (!Input.GetKey(KeyCode.LeftShift) || !Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot -= curSpeed * Time.deltaTime;
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button1))
                {
                    desiredRot -= maxSpeed * Time.deltaTime;
                }

                var desiredRotQ = Quaternion.Euler(desiredRot, 0, desiredRotZ);
                transform.localRotation = desiredRotQ;
            }
            //Reset des variables quand il n'y a pas d'input
            else
            {
                t = 0f;
                lerp = 0f;
                curSpeed = 0f;
            }
        }
    }
}
