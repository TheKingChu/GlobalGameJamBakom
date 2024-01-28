//Created by Charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    //Joke mini game
    public void JokeMaster()
    {
        SceneManager.LoadScene(2);
    }

    //Juggling mini game
    public void Juggler()
    {
        SceneManager.LoadScene(3);
    }

    //Pie throwing mini game
    public void PieThrowing()
    {
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }


    //Quit game button
    public void QuitGame()
    {
        Application.Quit();
    }
}
