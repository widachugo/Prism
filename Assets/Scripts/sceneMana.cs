using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneMana : MonoBehaviour
{
    public Object scene;

    public void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("scene manager");
            SceneManager.LoadScene(1);
        }

        for (int i = 0; i < 20; i++)
        {
            if (Input.GetKey("joystick 1 button " + i))
            {
                Debug.Log("scene manager Manette "); 
                SceneManager.LoadScene(1);
            }
        }

    }
}
