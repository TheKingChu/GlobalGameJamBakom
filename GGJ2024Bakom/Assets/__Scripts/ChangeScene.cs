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

    public void PieThrowing()
    {
        SceneManager.LoadScene(1);
    }

    //Pie throwing mini game

    //Quit game button
    public void QuitGame()
    {
        Application.Quit();
    }
}
