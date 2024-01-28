//Created by Sebastian
//Modified by Charlie

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JugglingHandler : MonoBehaviour
{
    public GameObject otherJesterRightHand;
    public GameObject[] spheres; // Array of Sphere GameObjects
    public GameObject rightHand; // Right hand GameObject
    public GameObject lefttHand; // Right hand GameObject
    public AudioClip[] backgroundMusic; // Array of background music clips
    public AudioSource audioSource; // AudioSource component
    public float bounceForce = 10f; // Force to apply when bouncing
    public float maxDistance = 0.1f; // Maximum distance for the sphere to be considered "close" to the hand
    private int counter = 0; // Counter to keep track of the number of bounces

    public GameObject pausePanel;
    public GameObject infoPanel;
    private SebastianPlayer playerController;

    private enum Stages
    {
        Easy,
        Medium,
        Hard
    }
    private Stages stage = Stages.Easy; // Current stage of the game
    
    void Start()
    {
        Debug.LogWarning("test start");
        
        // Play background music
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<SebastianPlayer>();
        pausePanel.SetActive(false);
        playerController.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 0;
    }
    
    // Method to play an audio clip
    public void PlayAudioClip(int index)
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        if (index >= 0 && index < backgroundMusic.Length)
        {
            audioSource.PlayOneShot(backgroundMusic[index]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleStages();
        handleMouseClick();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            infoPanel.SetActive(false);
            Time.timeScale = 1f;
            playerController.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0;
            playerController.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    void handleMouseClick()
    {
        if (Input.GetMouseButtonDown(1)) // 1 represents right mouse button
        {
            HandleJuggling(rightHand, lefttHand);
            if (audioSource.isPlaying == false)
            {
                PlayAudioClip(0);
            }
        }
        else if (Input.GetMouseButtonDown(0)) // 0 represents left mouse button
        {
            HandleJuggling(lefttHand, rightHand);
            if (audioSource.isPlaying == false)
            {
                PlayAudioClip(0);
            }
        }
    }
    
    void handleStages ()
    {
        
        if (counter == 2 && stage == Stages.Easy)
        {
            System.Console.WriteLine("test");
            PlayAudioClip(1);
            stage = Stages.Medium;
            SpawnAndThrowSphere(otherJesterRightHand, lefttHand);
        }
        else if (counter == 4 && stage == Stages.Medium)
        {
            PlayAudioClip(2);
            stage = Stages.Hard;
            SpawnAndThrowSphere(otherJesterRightHand, lefttHand);
        }
        else if (counter == 6 && stage == Stages.Hard)      
        {
            SpawnAndThrowSphere(otherJesterRightHand, lefttHand);
            stage = Stages.Easy;
            PlayAudioClip(1);
        }
    }

    
    
    void HandleJuggling(GameObject startHand, GameObject targetHand)
    {
        foreach (GameObject sphere in spheres)
        {
            float distance = Vector3.Distance(sphere.transform.position, startHand.transform.position);
            if (distance <= maxDistance)
            {
                counter++;
                StartCoroutine(MoveSphereOverCurve(sphere, startHand.transform.position, targetHand.transform.position));
            }
        }
    }
    /**
     * Coroutine to move a sphere over a curve
     * Height is the height of the curve
     * Duration is the duration of the movement
     */
    IEnumerator MoveSphereOverCurve(GameObject sphere, Vector3 startPos, Vector3 endPos, float height = 1f, float duration = 0.9f)
    {
        float t = 0;
        

        while (t < duration)
        {
            float yOffset = height * (t / duration - t * t / (duration * duration)) * 4; // Parabolic curve
            Vector3 pos = Vector3.Lerp(startPos, endPos, t / duration);
            pos.y += yOffset;

            sphere.transform.position = pos;

            t += Time.deltaTime;
            yield return null;
        }

        sphere.transform.position = endPos;
    }
    void SpawnAndThrowSphere(GameObject startHand, GameObject targetHand)
    {
        // Instantiate a new sphere at the position of the otherJesterRightHand
        GameObject newSphere = Instantiate(spheres[0], startHand.transform.position, Quaternion.identity);
        AddSphere(newSphere);

        // Start the coroutine to move the sphere over a curve towards the rightHand
        StartCoroutine(MoveSphereOverCurve(newSphere, startHand.transform.position, targetHand.transform.position,5f, 2f));
    }
    public void AddSphere(GameObject newSphere)
    {
        // Create a new array with a length one greater than the current array
        GameObject[] newSpheres = new GameObject[spheres.Length + 1];

        // Copy the elements from the old array to the new one
        for (int i = 0; i < spheres.Length; i++)
        {
            newSpheres[i] = spheres[i];
        }

        // Add the new sphere to the last index of the new array
        newSpheres[newSpheres.Length - 1] = newSphere;

        // Replace the old array with the new one
        spheres = newSpheres;
    }
    void OnCollisionEnter(Collision collision)
    {
            Debug.Log("A sphere hit something");

            // Check if the sphere collided with the ground
            if (collision.gameObject.tag == "Ground")
            {
                SpawnAndThrowSphere(lefttHand, otherJesterRightHand);
                // The sphere hit the ground
                Debug.Log("A sphere hit the ground");
            }
        }
    }