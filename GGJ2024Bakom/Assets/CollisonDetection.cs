using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CollisonDetection : MonoBehaviour
{
    public bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("A sphere hit something");

        // Check if the sphere collided with the ground
        if (collision.gameObject.tag == "Ground")
        {
            // The sphere hit the ground
            Debug.Log("A sphere hit the ground");
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
