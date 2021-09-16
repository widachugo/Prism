using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Image whiteScreen;

    public float smoothTime;

    private float t;
    private float lerpColor;
    private float alpha;

    public bool whiteScreenActivated;
    public AudioSettings audioSettings;

    private void Start()
    {
        whiteScreenActivated = false;
    }

    private void Update()
    {
        if (whiteScreenActivated)
        {
            t += Time.deltaTime;

            lerpColor = Mathf.InverseLerp(0, smoothTime, t);
            alpha = Mathf.SmoothStep(0, 255, lerpColor);

            whiteScreen.color = new Color(255, 255, 255, alpha);

            audioSettings.volumeSlider.value = Mathf.Lerp(audioSettings.masterVolume, 0, t);
        }

        if (alpha >= 1)
        {
            Debug.Log("C tout ok man");
            SceneManager.LoadScene(0);
        }
    }
}
