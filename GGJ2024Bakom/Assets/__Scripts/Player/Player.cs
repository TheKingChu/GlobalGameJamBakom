using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private float playerSpeed = 5.0f;
    private Camera mainCamera;

    public AudioSource honkClip;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Honk();
        ThrowPie();
    }

    private void PlayerMovement()
    {
        //movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * Time.deltaTime * playerSpeed);

        //running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 10f;
        }
    }

    private void ThrowPie()
    {
        if (Input.GetMouseButton(0))
        {

        }
    }

    public void Honk()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("HONK");
            honkClip.Play();
        }
    }
}
