using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MENU : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider progressbar;
    private AsyncOperation l;
    private int index;

    public void StartGame(int LevelIndex)
    {
        index = LevelIndex;
        StartCoroutine("AsyncLoad");
    }

    public void QuitGame()
    {
        print("QUIT!");
        Application.Quit();
    }

    public Slider slider;
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
    }
    public void SetLevel()
    {
        float sliderValue = slider.value;
        mixer.SetFloat("GlobalVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MusicVolume", sliderValue);
    }

    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(index);

        while (!operation.isDone)
        {
            float progress = operation.progress;
            progressbar.value = progress;
            yield return null;
        }
    }
}
