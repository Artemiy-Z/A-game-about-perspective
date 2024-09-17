using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MENULOAD : MonoBehaviour
{
    public bool pause;
    public GameObject pm;
    public float t;

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void SetTimeSpeed(float m)
    {
        Time.timeScale = m;
    }

    public void P()
    {
        pause = false;
        t = 1;
    }

    private void Update()
    {
        Time.timeScale = t;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        if (!pause)
        {
            t = 0;
            Time.timeScale = t;
            pause = true;
            pm.SetActive(true);
            GetComponent<AudioSource>().Play();
        }
        else
        {
            t = 1;
            pause = false;
            pm.SetActive(false);
            GetComponent<AudioSource>().Play();
        }
    }
}
