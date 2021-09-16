using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    FMOD.Studio.Bus Audio;
    public float masterVolume = 1f;
    public Slider volumeSlider;

    void Awake()
    {
        Audio = FMODUnity.RuntimeManager.GetBus("bus:/Master");
    }

    void Update()
    {
        masterVolume = volumeSlider.value;
        Audio.setVolume(masterVolume);
    }
}
