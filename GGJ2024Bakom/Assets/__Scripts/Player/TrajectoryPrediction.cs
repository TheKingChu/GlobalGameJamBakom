//Created by Charlie

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrajectoryPrediction : MonoBehaviour
{
    [Header("Trajectory and arc variables")]
    public float arcHeight = 2f;
    public int resolution = 10;
    public float maxArcLength = 10f;
    public float minChargeTime = 1f;
    public float maxChargeTime = 5f;
    public float minThrowVelocity = 10f;
    public float maxThrowVelocity = 20f;

    private LineRenderer lineRenderer;
    private float holdStartTime;

    public GameObject piePrefab;
    public AudioSource pieAudioSource;

    [Header("Camera variables")]
    public Transform firstPersonCamera;
    public Transform thirdPersonCamera;
    public float transitionSpeed = 5.0f;
    bool isFirstPerson;
    private MouseRotation mouseRotation;


    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            holdStartTime = Time.time;
            lineRenderer.enabled = true;
            SetCameraState(true);
        }

        if (Input.GetMouseButton(0))
        {
            DrawDynamicArc();
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false; // Hide the visual line when mouse button is released
            ThrowObject();
            SetCameraState(false);
        }
    }

    void DrawDynamicArc()
    {
        float holdDuration = Time.time - holdStartTime;
        float clampedArcLength = Mathf.Clamp(holdDuration, 0f, maxArcLength);
        float gravity = Mathf.Abs(Physics.gravity.y);

        Vector3 playerPos = transform.position;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y));
        Vector3 direction = (mousePosition - playerPos).normalized;

        int visiblePoints = 10; // Adjust this value based on the desired smoothness
        float stretchingSpeed = 1f; // Adjust this value to control the stretching speed

        lineRenderer.positionCount = visiblePoints;

        for (int i = 0; i < visiblePoints; i++)
        {
            float t = i / (float)(visiblePoints - 1);
            float x = playerPos.x + direction.x * t * clampedArcLength;
            float y = playerPos.y + arcHeight + arcHeight * (2 * t - 1) * (1.0f - t * stretchingSpeed);
            float z = playerPos.z + direction.z * t * clampedArcLength;

            Vector3 point = new Vector3(x, y, z);
            lineRenderer.SetPosition(i, point);
        }
    }

    float CalculateThrowVelocity(float chargeTime)
    {
        // Linearly interpolate throw velocity based on charge time
        float t = Mathf.InverseLerp(minChargeTime, maxChargeTime, chargeTime);
        return Mathf.Lerp(minThrowVelocity, maxThrowVelocity, t);
    }

    private void ThrowObject()
    {
        float holdDuration = Time.time - holdStartTime;
        float throwVelocity = CalculateThrowVelocity(holdDuration);
        
        GameObject pieInstance = Instantiate(piePrefab, transform.position + transform.forward, Quaternion.identity);
        Rigidbody pieRb = pieInstance.GetComponent<Rigidbody>();
        
        Vector3 throwDirection = (Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y)) - transform.position).normalized;

        // Adjust this factor to control the arc shape and throw velocity
        float arcFactor = 1.0f;

        // Calculate the throw force using the adjusted arcFactor
        Vector3 throwForce = throwDirection * throwVelocity * arcFactor;
        pieRb.AddForce(throwForce, ForceMode.VelocityChange);

        pieInstance.transform.rotation = Quaternion.Euler(90f, transform.eulerAngles.y, 0f);
        pieAudioSource.Play();
    }

    private void SetCameraState(bool isFirstPerson)
    {
        this.isFirstPerson = isFirstPerson;

        if (isFirstPerson)
        {
            // Set first-person camera position and rotation
            Camera.main.transform.SetPositionAndRotation(firstPersonCamera.position, firstPersonCamera.rotation);
        }
        else if (!isFirstPerson)
        {
            // Set third-person camera position and rotation
            Camera.main.transform.SetPositionAndRotation(thirdPersonCamera.position, thirdPersonCamera.rotation);
        }
    }
}
