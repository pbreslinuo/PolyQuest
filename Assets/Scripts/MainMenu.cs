using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void selectScene()
    {
        switch (this.gameObject.name)
        {
            case "Tutorial":
                SceneManager.LoadScene("TutorialLevel");
                break;
            case "Beginner":
                SceneManager.LoadScene("MediumLevel");
                break;
            case "Difficult":
                SceneManager.LoadScene("testbed");
                break;
        }
    }

    public void quit()
    {
        Application.Quit();
    }
}
