using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    LevelManager levelManager;
    public bool menu = true;
    private void Start()
    {
        levelManager = FindObjectsOfType<LevelManager>()[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && menu == false)
        {
            Restart();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu)
            {
                Exit();
            } else
            {
                Menu();
            }
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StartLevel()
    {
        levelManager.levelSelect = 1;
        SceneManager.LoadScene("MainScene");
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void NextLevel()
    {
        levelManager.levelSelect = levelManager.levelSelect + 1;
        if (levelManager.levelSelect > 3)
        {
            SceneManager.LoadScene("Menu");
        } else
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
