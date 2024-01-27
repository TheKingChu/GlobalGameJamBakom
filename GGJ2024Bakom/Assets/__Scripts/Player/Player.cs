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
    private bool isThrowing = false;

    public LineRenderer lineRenderer;
    [SerializeField, Min(3)] int lineSegments = 10;
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
            SetCameraState(true);
        }
    }

    private void ThrowPie()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isThrowing = true;
            GameObject pieInstance = Instantiate(piePrefab, transform.position + transform.forward, Quaternion.identity);
            Rigidbody pieRb = pieInstance.GetComponent<Rigidbody>();

            float throwForce = Mathf.Lerp(10f, 30f, chargeTime / maxChargeTime);
            Vector3 throwDirection = transform.forward;
            Vector3 throwVelocity = throwDirection * throwForce;

            throwVelocity += Vector3.up * 1f;

            pieRb.velocity = throwVelocity;

            pieInstance.transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);
            
            chargeTime = 0f;
            SetCameraState(false);
        }
        if(isThrowing && !Input.GetMouseButton(0))
        {
            isThrowing = false;
            SetCameraState(false);
        }
    }

    private void VisualizeCharge(Vector3 startPoint, Vector3 startVelocity)
    {
        // Disable LineRenderer by default
        lineRenderer.enabled = false;

        // Check if the camera position is close to the first-person camera
        if (Vector3.Distance(Camera.main.transform.position, firstPersonCamera.position) < 0.01f)
        {
            // Enable LineRenderer only when in first-person
            lineRenderer.enabled = true;

            float timeStep = timeOfFlight / lineSegments;
            Vector3[] lineRenderPoints = CalculateTrajectory(startPoint, startVelocity, timeStep);
            lineRenderer.positionCount = lineSegments;
            lineRenderer.SetPositions(lineRenderPoints);

            // Offset the LineRenderer so that it starts from the player's position
            lineRenderer.transform.position = startPoint;
            lineRenderer.transform.rotation = transform.rotation;
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

            // Clamp the vertical rotation to limit looking down
            float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity;
            float currentXRotation = Camera.main.transform.rotation.eulerAngles.x;
            float clampedXRotation = Mathf.Clamp(currentXRotation + mouseY, -45f, 45f);
            Camera.main.transform.rotation = Quaternion.Euler(clampedXRotation, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);
        }
        else if(!isFirstPerson)
        {
            // Set third-person camera position and rotation
            Camera.main.transform.position = thirdPersonCamera.position;
            Camera.main.transform.rotation = thirdPersonCamera.rotation;
        }
    }

    Vector3[] CalculateTrajectory(Vector3 startPoint, Vector3 startVelocity, float timeStep)
    {
        Vector3[] lineRendererPoints = new Vector3[lineSegments];

        for (int i = 0; i < lineSegments; i++)
        {
            float t = i / (float)(lineSegments - 1); // Interpolation parameter between 0 and 1

            // Calculate the position based on time and gravity
            Vector3 currentPosition = startPoint + startVelocity * t * timeOfFlight + 0.5f * Physics.gravity * t * t;

            // Convert the position to local space relative to the startPoint
            lineRendererPoints[i] = transform.InverseTransformPoint(currentPosition);
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
