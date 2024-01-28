using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : MonoBehaviour
{
    //player movement
    private CharacterController characterController;
    private float playerSpeed = 5.0f;

    //sounds
    public AudioSource honkClip;

    private LaughMeter laughMeter;

    [Header("Camera variables")]
    public float transitionSpeed = 5.0f;
    private bool isFirstPerson = false;
    private MouseRotation mouseRotation;

    //animation
    public Animator animator;

    //start info
    public GameObject infoPanel;
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mouseRotation = GetComponentInChildren<MouseRotation>();
        laughMeter = GameObject.FindGameObjectWithTag("King").GetComponent<LaughMeter>();
        mouseRotation.ResumeMovement();
        infoPanel.SetActive(true);
        pausePanel.SetActive(false);
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Honk();
        if(Input.GetKeyDown(KeyCode.Space))
        {
            infoPanel.SetActive(false);
            Time.timeScale = 1f;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pausePanel.SetActive(true);
            mouseRotation.StopMouseMovement();
            Time.timeScale = 0;
        }
    }

    private void PlayerMovement()
    {
        //movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        characterController.Move(move * Time.deltaTime * playerSpeed);
        if(!isFirstPerson && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isWalking", true);
        }
        else if(Input.GetMouseButton(0))
        {
            animator.SetBool("isWalking", false);
        }

        //running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 10f;
        }
        else
        {
            playerSpeed = 5.0f;
        }
    }

    public void Honk()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            honkClip.Play();
            laughMeter.HonkEvent();
        }
    }
}

