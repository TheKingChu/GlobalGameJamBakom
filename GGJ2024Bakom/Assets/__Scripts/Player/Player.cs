using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //player movement
    private CharacterController characterController;
    private float playerSpeed = 5.0f;
    public float mouseSensitivity = 2.0f;

    //sounds
    public AudioSource honkClip;

    [Header("Camera variables")]
    public Transform firstPersonCamera;
    public Transform thirdPersonCamera;
    public float transitionSpeed = 5.0f;
    private bool isFirstPerson = false;

    [Header("Pie variables")]
    public GameObject piePrefab;
    private float chargeTime;
    public float maxChargeTime = 3.0f;
    public float throwSpeed = 10.0f;
    public float chestHeightOffset = 1.3f;

    public LineRenderer lineRenderer;
    [SerializeField, Min(3)] int lineSegments = 60;
    [SerializeField, Min(1)] float timeOfFlight = 5f;

    //animation

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Honk();
        ChargePie();
        // Calculate start velocity based on the player's forward direction and charge time
        Vector3 startVelocity = transform.forward * (chargeTime / maxChargeTime) * throwSpeed;
        // Offset the starting position to chest height
        Vector3 chestHeightStartPosition = transform.position + Vector3.up * chestHeightOffset;
        VisualizeCharge(chestHeightStartPosition, startVelocity);
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
        else
        {
            playerSpeed = 5.0f;
        }

        //mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.Rotate(Vector3.up, mouseX);
        Camera.main.transform.Rotate(Vector3.left, mouseY);
    }

    private void ChargePie()
    {
        if (Input.GetMouseButton(0))
        {
            chargeTime += Time.deltaTime;
            chargeTime = Mathf.Clamp(chargeTime, 0f, maxChargeTime);
        }
    }

    private void ThrowPie()
    {
        if (Input.GetMouseButtonUp(0))
        {
            GameObject pieInstance = Instantiate(piePrefab, transform.position + transform.forward, Quaternion.identity);
            Rigidbody pieRb = pieInstance.GetComponent<Rigidbody>();

            float throwForce = Mathf.Lerp(10f, 30f, chargeTime / maxChargeTime);
            Vector3 throwDirection = transform.forward;
            Vector3 throwVelocity = throwDirection * throwForce;

            pieRb.velocity = throwVelocity;
            
            chargeTime = 0f;
        }
    }

    private void VisualizeCharge(Vector3 startPoint, Vector3 startVelocity)
    {
        float timeStep = timeOfFlight / lineSegments;
        Vector3[] lineRenderPoints = CalculateTrajectory(startPoint, startVelocity, timeStep);
        lineRenderer.positionCount = lineSegments;
        lineRenderer.SetPositions(lineRenderPoints);

        // Offset the Line Renderer so that it starts from the player's position
        lineRenderer.transform.position = startPoint;

        // Calculate the transition factor based on charge level
        float transitionFactor = Mathf.Clamp01(chargeTime / maxChargeTime);

        // Lerp between third person and first person camera positions
        Camera.main.transform.SetPositionAndRotation(Vector3.Lerp(thirdPersonCamera.position, firstPersonCamera.position, transitionFactor), 
            Quaternion.Lerp(thirdPersonCamera.rotation, firstPersonCamera.rotation, transitionFactor));

        // Use the first person camera's position and rotation for Line Renderer visualization
        lineRenderer.transform.SetPositionAndRotation(firstPersonCamera.position, firstPersonCamera.rotation);
    }

    Vector3[] CalculateTrajectory(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];
        lineRendererPoints[0] = startPoint;
        Vector3 currentVelocity = startVelocity;
        Vector3 currentPosition = startPoint;

        for (int i = 0; i < lineSegments; i++)
        {
            currentVelocity += Physics.gravity * timeStep;
            currentPosition += currentVelocity * timeStep;
            lineRendererPoints[i] = currentPosition - startPoint;
        }

        return lineRendererPoints;
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
