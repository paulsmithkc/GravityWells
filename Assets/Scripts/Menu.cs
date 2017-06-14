using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public bool loading = false;

    public void Start()
    {
        loading = false;
    }

    public void Update()
    {
        if (!loading)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                LoadScene("Paul");
                return;
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        //PlayClick();

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogErrorFormat("LoadScene({0}): scene name not specified", sceneName);
        }
        else if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogErrorFormat("LoadScene({0}): scene {0} not found", sceneName);
        }
        else
        {
            Debug.LogFormat("LoadScene({0})", sceneName);
            Time.timeScale = 1.0f;
            SceneManager.LoadScene(sceneName);
        }
    }
}
