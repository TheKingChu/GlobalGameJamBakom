using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JokeController : MonoBehaviour
{
    public List<string> buildUps, punchlineList1, punchlineList2, punchlineList3, punchlineList4;
    private int jokeIndex = 0;
    public GameObject buildUp, punchline1, punchline2, punchline3, punchline4;
    private TextMeshPro buildTmp, punchTmp1, punchTmp2, punchTmp3, punchTmp4;
    // Start is called before the first frame update
    void Start()
    {
        buildTmp = buildUp.GetComponent<TextMeshPro>();
        punchTmp1 = punchline1.GetComponent<TextMeshPro>();
        punchTmp2 = punchline2.GetComponent<TextMeshPro>();
        punchTmp3 = punchline3.GetComponent<TextMeshPro>();
        punchTmp4 = punchline4.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextJoke()
    {
        if (buildUps.Count > 0)
        {
            jokeIndex = Random.Range(0, buildUps.Count);
            string build = buildUps[jokeIndex];
            string punch1 = punchlineList1[jokeIndex];
            string punch2 = punchlineList2[jokeIndex];
            string punch3 = punchlineList3[jokeIndex];
            string punch4 = punchlineList4[jokeIndex];
            buildTmp.text = build;
            punchTmp1.text = punch1;
            punchTmp2.text = punch2;
            punchTmp3.text = punch3;
            punchTmp4.text = punch4;
            buildUps.Remove(build);
            punchlineList1.Remove(punch1);
            punchlineList2.Remove(punch2);
            punchlineList3.Remove(punch3);
            punchlineList4.Remove(punch4);
        }
        else
        {
            buildTmp.text = "That's all folks!";
            punchTmp1.text = "";
            punchTmp2.text = "";
            punchTmp3.text = "";
            punchTmp4.text = "";
        }
    }
}
