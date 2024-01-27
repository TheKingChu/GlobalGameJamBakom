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

    [Header("Camera variables")]
    public Transform firstPersonCamera;
    public Transform thirdPersonCamera;
    public float transitionSpeed = 5.0f;
    private bool isFirstPerson = false;
    private MouseRotation mouseRotation;

    [Header("Pie variables")]
    public GameObject piePrefab;
    private float chargeTime;
    public float maxChargeTime = 3.0f;
    public float throwSpeed = 10.0f;
    public float chestHeightOffset = 1.3f;
    private bool isThrowing = false;

    //animation
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        mouseRotation = GetComponentInChildren<MouseRotation>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Honk();
        ChargePie();
        ThrowPie();
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

    private void ChargePie()
    {
        if (Input.GetMouseButton(0))
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
            SetCameraState(true);
            isFirstPerson = true;

            //move the trajectory random from side to side
            mouseRotation.MouseMovement();
        }
    }

    private void ThrowPie()
    {
        if (Input.GetMouseButtonUp(0))
        {
            mouseRotation.MouseMovement();
            isThrowing = true;
            GameObject pieInstance = Instantiate(piePrefab, transform.position + transform.forward, Quaternion.identity);
            Rigidbody pieRb = pieInstance.GetComponent<Rigidbody>();

            float throwForce = Mathf.Lerp(10f, 30f, chargeTime / maxChargeTime);
            Vector3 throwDirection = transform.forward;
            Vector3 throwVelocity = throwDirection * throwForce;

            throwVelocity += Vector3.up * 1f; //throwing the pie upwards

            pieRb.velocity = throwVelocity;

            pieInstance.transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);

            chargeTime = 0f;
            SetCameraState(false);
            isFirstPerson = false;
        }
        if (isThrowing && !Input.GetMouseButton(0))
        {
            isThrowing = false;
            SetCameraState(false);
        }
    }

    private void SetCameraState(bool isFirstPerson)
    {
        this.isFirstPerson = isFirstPerson;

        if (isFirstPerson)
        {
            // Set first-person camera position and rotation
            Camera.main.transform.position = firstPersonCamera.position;
            Camera.main.transform.rotation = firstPersonCamera.rotation;
        }
        else if (!isFirstPerson)
        {
            // Set third-person camera position and rotation
            Camera.main.transform.position = thirdPersonCamera.position;
            Camera.main.transform.rotation = thirdPersonCamera.rotation;
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

