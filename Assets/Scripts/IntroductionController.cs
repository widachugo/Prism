using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroductionController : MonoBehaviour
{
    [Header ("Activators")]
    public LightActivator intro1;
    public LightActivator intro2;
    public LightActivator intro3;

    private FMODUnity.StudioEventEmitter music;

    public bool introCompleted = false;
    private bool introChecker = false;

    void Start()
    {
        music = GetComponent<FMODUnity.StudioEventEmitter>();
    }

    void Update()
    {
        if (!intro3.activatorActive || !introCompleted)
        {
            if (intro1.activatorActive)
            {
                music.SetParameter("Intro 1", 1);
            }
            if (intro2.activatorActive)
            {
                music.SetParameter("Intro 2", 1);
            }
            if (intro3.activatorActive)
            {
                music.SetParameter("Intro 3", 1);
                introCompleted = true;
            }
        }

        if (introCompleted && !introChecker)
        {
            music.SetParameter("Exploration", 1);
            introChecker = true;
        }
    }
}
