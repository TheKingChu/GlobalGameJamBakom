using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player movement
    private CharacterController characterController;
    private float playerSpeed = 5.0f;

    //sounds
    public AudioSource honkClip;

    //pie
    public GameObject piePrefab;
    private bool hasPie;

    //animation

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
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
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        characterController.Move(move * Time.deltaTime * playerSpeed);

        //running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 10f;
        }
    }

    private void ThrowPie()
    {
        if (Input.GetMouseButton(0) && !hasPie)
        {
            Instantiate(piePrefab);
            hasPie = true;
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
