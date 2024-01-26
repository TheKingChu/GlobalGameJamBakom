using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joke : MonoBehaviour
{
    public bool correctJoke = false, criticalHonk = false;
    private JokeController jester;
    // Start is called before the first frame update
    void Start()
    {
        jester = GameObject.Find("Jester").GetComponent<JokeController>();
    }
    public void JokeValue()
    {
        if (correctJoke)
        {
            Debug.Log("Good Joke");
        }
        else
        {
            Debug.Log("Bad Joke");
            jester.maxTime -= 2f;
        }

        if (criticalHonk)
        {
            Debug.Log("Critical Honk!");
            jester.maxTime += 1f;
        }
        jester.NextJoke();
    }
}
