using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;

public class JokeController : MonoBehaviour
{
    public List<string> buildUps, punchlineList1, punchlineList2, punchlineList3, punchlineList4;
    public List<int> critHonkIndex;
    public Joke joke1, joke2, joke3, joke4, tellingJoke;
    public GameObject buildUp, punchline1, punchline2, punchline3, punchline4, jesterHead, king, headRoll, mainCam, jesting;
    
    public float maxTime = 60f, timePunish = 1f, pauseTime = 3f;
    public AudioSource backMusic, honk, laugh, boo, win, victory, lose, loseBoo1, loseBoo2, loseBoo3;
    public bool isHonk = false, dramaPause;


    public Material faceMat;
    public ParticleSystem confetti;

    private int jokeIndex = 0;
    private string punch1, punch2, punch3, punch4;
    private TMP_Text buildTmp, punchTmp1, punchTmp2, punchTmp3, punchTmp4;
    private GameObject b1, b2, b3, b4;
    private Color faceColor;
    private Color colorSave;
    private float colorChange = 0f, currentTime, pausing;
    private bool critHonk = false, won = false, lost = false;
    public Animator jestingAnim, headRollAnim, kingAnim;
    
    

    // Start is called before the first frame update
    void Start()
    {
        dramaPause = false;
        pausing = pauseTime;
        backMusic.Play();
        b1 = joke1.gameObject;
        b2 = joke2.gameObject;
        b3 = joke3.gameObject;
        b4 = joke4.gameObject;
        int materialIndex = 0;
        foreach(Material mat in jesterHead.GetComponent<SkinnedMeshRenderer>().materials)
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
        tellingJoke = joke1;
        jestingAnim = jesting.GetComponent<Animator>();
        headRollAnim = headRoll.GetComponent<Animator>();
        kingAnim = king.GetComponent<Animator>();
        jestingAnim.SetFloat("Nervousness", maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(won)
        {
            if (kingAnim.GetCurrentAnimatorStateInfo(0).IsName("Laugh"))
            {
                if (kingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                }
            }
            if (!victory.isPlaying)
            {
                
            kingAnim.SetBool("Laugh", true);
                win.Play();
                laugh.pitch = 1f;
                laugh.Play();
            }
            if (!win.isPlaying && !victory.isPlaying)
            {
                victory.Play();
            }
            backMusic.Stop();
        }
        else if(lost)
        {

            if (!lose.isPlaying)
            {
                lose.Play();
                loseBoo1.Play();
                loseBoo2.Play();
                loseBoo3.Play();
                kingAnim.SetBool("Kill", true);
                headRoll.SetActive(true);
                mainCam.SetActive(false);
                jesting.SetActive(false);
            }
            backMusic.Stop();
            /*if (kingAnim.GetCurrentAnimatorStateInfo(0).IsName("Kill"))
            {
                    if (kingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                    {
                    }
                }
            */
            

        }
        faceMat.SetColor("_Color", faceColor);
        //timeIndicator.color = timeColor;
        if(currentTime > 0 && !won && !lost)
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
                buildTmp.text = "Please spare me!";
                b1.SetActive(false);
                b2.SetActive(false);
                b3.SetActive(false);
                b4.SetActive(false);
                lost = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Honk();
        }
        if (dramaPause)
        {
            currentTime = maxTime;
            if (jestingAnim.GetCurrentAnimatorStateInfo(0).IsName("Punchline") && jestingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f && dramaPause)
            {
                backMusic.Pause();
            }
            else if (jestingAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && dramaPause)
            { 
                dramaPause = false;

                jestingAnim.SetBool("Punchline", false);

                if (tellingJoke.correctJoke)
                    {
                        Debug.Log("Good Joke");
                        laugh.Play();
                        NextJoke();
                    }
                    else if (tellingJoke.criticalHonk)
                    {
                        if (isHonk)
                        {
                            Debug.Log("Critical Honk!");
                            maxTime += timePunish / 2;
                            backMusic.pitch -= timePunish / maxTime / 2;
                            boo.pitch -= timePunish / maxTime / 2;
                            laugh.pitch -= timePunish / maxTime / 2;
                            laugh.Play();
                            NextJoke();
                        }
                        else
                        {
                            Debug.Log("Bad Joke");
                            TimeReduction();
                            NextJoke();
                        }
                    }
                    else
                    {
                        Debug.Log("Bad Joke");
                        TimeReduction();
                        NextJoke();
                    }
                    backMusic.volume *= 3;
                    pausing = pauseTime;
                
            }
        }
    }

    public void NextJoke()
    {
        isHonk = false;
        if (buildUps.Count > 0)
        {
            jokeIndex = Random.Range(0, buildUps.Count);
            string build = buildUps[jokeIndex];
            if (critHonkIndex.Contains(jokeIndex))
            {
                critHonk = true;
            }
            else
            {
                critHonk = false;
            }
            buildTmp.text = build;
            buildUps.Remove(build);
            int i = Random.Range(0, 4);
            if(i == 0)
            {
                joke1.correctJoke = true;
                joke2.correctJoke = false;
                joke3.correctJoke = false;
                joke4.correctJoke = false;
                joke4.criticalHonk = critHonk;
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
                joke4.criticalHonk = critHonk;
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
                joke4.criticalHonk = critHonk;
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
                joke1.criticalHonk = critHonk;
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
            confetti.Play();
            won = true;
        }
        currentTime = maxTime;
        faceColor = colorSave;
    }

    public void TimeReduction()
    {
        maxTime -= timePunish;
        jestingAnim.SetFloat("Nervousness", maxTime);
        backMusic.pitch += timePunish / 10;
        boo.pitch += timePunish / 10;
        laugh.pitch += timePunish / 10;
        boo.Play();
    }

    private void Honk()
    {
        honk.Play();
        isHonk = true;
    }

    IEnumerator PunchlineCoroutine()
    {
        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 3 seconds.
        yield return new WaitForSeconds(5);

        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }

    public void Joking(Joke told)
    {
        tellingJoke = told;
        isHonk = false;
        backMusic.volume /= 3;
        dramaPause = true;
        jestingAnim.SetBool("Punchline", true);
    }
}
