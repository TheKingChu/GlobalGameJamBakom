using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JokeController : MonoBehaviour
{
    public List<string> buildUps, punchlineList1, punchlineList2, punchlineList3, punchlineList4;
    public Joke joke1, joke2, joke3, joke4;
    public GameObject buildUp, punchline1, punchline2, punchline3, punchline4, jesterHead;
    public float maxTime = 60f, timePunish = 1f;
    private int jokeIndex = 0;
    private string punch1, punch2, punch3, punch4;
    private TMP_Text buildTmp, punchTmp1, punchTmp2, punchTmp3, punchTmp4;
    private GameObject b1, b2, b3, b4;
    public Material faceMat;
    private Color faceColor;
    private Color colorSave;
    private float colorChange = 0f, currentTime;
    

    // Start is called before the first frame update
    void Start()
    {
        b1 = joke1.gameObject;
        b2 = joke2.gameObject;
        b3 = joke3.gameObject;
        b4 = joke4.gameObject;
        int materialIndex = 0;
        foreach(Material mat in jesterHead.GetComponent<MeshRenderer>().materials)
        {
            if(materialIndex == 1)
            {
                faceMat = mat;
                break;
            }
            materialIndex++;
        }
        faceColor = faceMat.color;
        colorSave = faceColor;
        //timeColor = timeIndicator.color;
        currentTime = maxTime;
        buildTmp = buildUp.GetComponent<TMP_Text>();
        punchTmp1 = punchline1.GetComponent<TMP_Text>();
        punchTmp2 = punchline2.GetComponent<TMP_Text>();
        punchTmp3 = punchline3.GetComponent<TMP_Text>();
        punchTmp4 = punchline4.GetComponent<TMP_Text>();
        NextJoke();
    }

    // Update is called once per frame
    void Update()
    {
        
        faceMat.SetColor("_Color", faceColor);
        //timeIndicator.color = timeColor;
        if(currentTime > 0)
        {
            colorChange = ((maxTime / currentTime) * Time.deltaTime) / maxTime;
            currentTime -= Time.deltaTime;
            faceColor.r += colorChange;
            faceColor.g -= colorChange;
            faceColor.b -= colorChange;
        }
        else
        {
            faceColor = colorSave;
            //timeColor.a = 0;
            if (maxTime > 0) 
            {
                Debug.Log("Booo!");
                if (buildUps.Count > 0)
                {
                    TimeReduction();
                    NextJoke();
                }
                else
                {
                    maxTime = 0f;
                    currentTime = 0f;
                }
            }
            else
            {
                buildTmp.text = "That's all folks!";
                b1.SetActive(false);
                b2.SetActive(false);
                b3.SetActive(false);
                b4.SetActive(false);
            }
            
        }
    }

    public void NextJoke()
    {
        if (buildUps.Count > 0)
        {
            jokeIndex = Random.Range(0, buildUps.Count);
            string build = buildUps[jokeIndex];
            buildTmp.text = build;
            buildUps.Remove(build);
            int i = Random.Range(0, 4);
            if(i == 0)
            {
                joke1.correctJoke = true;
                joke2.correctJoke = false;
                joke3.correctJoke = false;
                joke4.correctJoke = false;
                punch1 = punchlineList1[jokeIndex];
                punch2 = punchlineList2[jokeIndex];
                punch3 = punchlineList3[jokeIndex];
                punch4 = punchlineList4[jokeIndex];
                punchlineList1.Remove(punch1);
                punchlineList2.Remove(punch2);
                punchlineList3.Remove(punch3);
                punchlineList4.Remove(punch4);
            }
            else if (i == 1)
            {
                joke1.correctJoke = false;
                joke2.correctJoke = true;
                joke3.correctJoke = false;
                joke4.correctJoke = false;
                punch1 = punchlineList2[jokeIndex];
                punch2 = punchlineList1[jokeIndex];
                punch3 = punchlineList3[jokeIndex];
                punch4 = punchlineList4[jokeIndex];
                punchlineList1.Remove(punch2);
                punchlineList2.Remove(punch1);
                punchlineList3.Remove(punch3);
                punchlineList4.Remove(punch4);
            }
            else if (i == 2)
            {
                joke1.correctJoke = false;
                joke2.correctJoke = false;
                joke3.correctJoke = true;
                joke4.correctJoke = false;
                punch1 = punchlineList3[jokeIndex];
                punch2 = punchlineList2[jokeIndex];
                punch3 = punchlineList1[jokeIndex];
                punch4 = punchlineList4[jokeIndex];
                punchlineList1.Remove(punch3);
                punchlineList2.Remove(punch2);
                punchlineList3.Remove(punch1);
                punchlineList4.Remove(punch4);
            }
            else if(i == 3)
            {
                joke1.correctJoke = false;
                joke2.correctJoke = false;
                joke3.correctJoke = false;
                joke4.correctJoke = true;
                punch1 = punchlineList4[jokeIndex];
                punch2 = punchlineList2[jokeIndex];
                punch3 = punchlineList3[jokeIndex];
                punch4 = punchlineList1[jokeIndex];
                punchlineList1.Remove(punch4);
                punchlineList2.Remove(punch2);
                punchlineList3.Remove(punch3);
                punchlineList4.Remove(punch1);
            }

            punchTmp1.text = punch1;
            punchTmp2.text = punch2;
            punchTmp3.text = punch3;
            punchTmp4.text = punch4;
        }
        else
        {
            buildTmp.text = "That's all folks!";
            b1.SetActive(false);
            b2.SetActive(false);
            b3.SetActive(false);
            b4.SetActive(false);
        }
        currentTime = maxTime;
        faceColor = colorSave;
    }

    public void TimeReduction()
    {
        maxTime -= timePunish;
    }
}
