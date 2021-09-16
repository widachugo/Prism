using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFall : MonoBehaviour
{
    public float frequency;

    public GameObject ball;

    private int stage;

    private void Start()
    {
        StartCoroutine(SpawnBall());
    }

    private IEnumerator SpawnBall()
    {
        Instantiate(ball,transform.position, transform.rotation);
        yield return new WaitForSeconds(frequency);
        StartCoroutine(SpawnBall());
    }
}
