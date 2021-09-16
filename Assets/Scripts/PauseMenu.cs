using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    private PlayerController playerController;
    private PrismRotation prismRotation;

    public static bool gameIsPaused = false;

    public GameObject pauseMenuUI;

    public GameObject panelControl;
    public GameObject ControlMove;
    public GameObject ControlPrism;
    public GameObject ControlJump;
    public GameObject ControlPause;

    public int cptBoucleControl;

    public Image sliderImage;
    private float timer;
    private bool introCompleted = false;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        prismRotation = FindObjectOfType<PrismRotation>();
        cptBoucleControl = 0;
        timer = 0f;
        sliderImage.fillAmount = 0;
    }

    void Update()
    {
        if (playerController.enabled)
        {
            if (!prismRotation.enabled)
            {
                introCompleted = true;
                prismRotation.enabled = true;
            }
        }

        if (introCompleted)
        {
            Control();
            sliderImage.fillAmount = timer;
        }

        if (Input.GetButtonDown("Pause"))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

    }

    public void Resume()
    {
        playerController.enabled = true;
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void Pause()
    {
        if (!panelControl.activeInHierarchy)
        {
            playerController.enabled = false;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;
        }
    }

    public void Controls()
    {
        Resume();
        cptBoucleControl = 0;
        timer = 0f;
        Control();
    }

    public void QuitGame()
    {
        Debug.Log("Exit...");
        Application.Quit();
    }

    public void Control()
    {
        if (cptBoucleControl == 0)
        {
            //activation du panel
            panelControl.SetActive(true);
            ControlMove.SetActive(true);
            cptBoucleControl++;
        }
        else if (cptBoucleControl == 1) //1er panelMove
        {
            //MOVE
            if (Input.GetAxis("Horizontal") != 0)
            {
                //augmentation du timer
                timer += 0.025f;

                //si le timer est ok
                if (timer >= 1)
                {
                    //activation du panel d'apres
                    ControlMove.SetActive(false);
                    ControlPrism.SetActive(true);
                    cptBoucleControl++;
                    timer = 0f;
                    sliderImage.fillAmount = 0;
                }
            }
            else if (timer > 0)
            {
                //si on arrive pas au bout du timer
                timer -= Time.deltaTime;
            }
        }
        else if (cptBoucleControl == 2)
        {
            //PRISM
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.E) || Input.GetAxis("RotationLeft") > 0 || Input.GetAxis("RotationRight") > 0)
            {
                timer += 0.01f;

                if (timer >= 1)
                {
                    ControlPrism.SetActive(false);
                    ControlJump.SetActive(true);
                    cptBoucleControl++;
                    sliderImage.fillAmount = 0;
                    timer = 0f;
                }
            }
            else if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }
        else if (cptBoucleControl == 3)
        {
            //JUMP
            if (Input.GetButton("Jump"))
            {
                timer += 0.1f;
                Debug.Log(timer);

                if (timer >= 1)
                {
                    ControlJump.SetActive(false);
                    ControlPause.SetActive(true);
                    cptBoucleControl++;
                    sliderImage.fillAmount = 0;
                    timer = 0f;
                }
            }
            else if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
        }
        else if (cptBoucleControl == 4)
        {
            //PAUSE
            if (Input.GetButtonDown("Pause"))
            {
                timer += 1f;

                if (timer >= 1)
                {
                    Debug.Log("1");
                    ControlPause.SetActive(false);
                    panelControl.SetActive(false);
                    cptBoucleControl++;
                }
            }
        }
    }
}
