using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusBehaviour : MonoBehaviour
{
    public enum FocusCenter { Player, Room }
    public FocusCenter focusType;
    public enum MusicState { Exploration, Puzzle, BossDoor, Boss }
    public MusicState musicState;
    
    // Musique
    private FMODUnity.StudioEventEmitter music;

    public LightActivator activator;

    [Header("changement")]
    public float fovChange;

    [Header("Focus Distance (X axis & Y axis)")]
    public Vector2 presetCameraOffset;

    [Header("Focus Distance (Z axis)")]
    public int focusDistance;

    public float smooth;

    private Camera cam;
    private CameraMovement camMove;

    void Start()
    {
        music = GameObject.Find("MusicManager").GetComponent<FMODUnity.StudioEventEmitter>();
        Debug.Log(music);

        cam = FindObjectOfType<Camera>();
        camMove = GameObject.FindObjectOfType<CameraMovement>();
    }

    public void OnTriggerStay(Collider other)
    {
         if (other.gameObject.tag == "Player")
         {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fovChange, smooth);
            camMove.focus = focusDistance;

            if (focusType == FocusCenter.Player)
            {
                camMove.target = GameObject.FindGameObjectWithTag("Player").transform;
                camMove.offsetPosZone = presetCameraOffset;
            }

            else if (focusType == FocusCenter.Room)
            {
                camMove.target = gameObject.transform;
                camMove.offsetPosZone = presetCameraOffset;
            }

            CheckMusicState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            music.SetParameter("Boss Fight", 0);
            music.SetParameter("Boss Door", 0);
            music.SetParameter("Puzzle", 0);
            music.SetParameter("Puzzle Completed", 0);
        }
    }

    void CheckMusicState()
    {
        if (musicState == MusicState.Puzzle)
        {
            music.SetParameter("Puzzle", 1);
        }
        else if (musicState == MusicState.Boss)
        {
            music.SetParameter("Boss Fight", 1);
            music.SetParameter("Exploration", 1);
            music.SetParameter("Puzzle Completed", 1);
            music.SetParameter("Boss Door", 0);
            music.SetParameter("Puzzle", 0);
        }
        else if (musicState == MusicState.BossDoor)
        {
            music.SetParameter("Boss Door", 1);
            music.SetParameter("Exploration", 1);
            music.SetParameter("Puzzle", 0);
            music.SetParameter("Puzzle Completed", 0);
        }
        else
        {
            music.SetParameter("Boss Fight", 0);
            music.SetParameter("Boss Door", 0);
            music.SetParameter("Puzzle", 0);
        }

        if (activator != null)
        {
            if (activator.GetComponent<LightActivator>().activatorActive == true)
            {
                music.SetParameter("Puzzle Completed", 1);
            }
            else
            {
                music.SetParameter("Puzzle Completed", 0);
            }
        }
    }
}
