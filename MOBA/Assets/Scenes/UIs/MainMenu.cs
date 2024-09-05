using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button start;

    private static void StartGame()
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

    private void Start()
    {
        start.onClick.AddListener(StartGame);
    }
}
