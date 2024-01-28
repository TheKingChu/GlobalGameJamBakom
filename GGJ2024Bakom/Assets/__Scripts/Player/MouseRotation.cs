using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    private Vector2 turn;
    public float mouseSensitivity = 1.0f;
    private bool allowMovement = true;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        if (allowMovement)
        {
            MouseMovement();
        }
        else
        {
            StopMouseMovement();
        }
    }

    public void MouseMovement()
    {
        //mouse
        turn.x += Input.GetAxis("Mouse X") * mouseSensitivity;
        turn.y += Input.GetAxis("Mouse Y") * mouseSensitivity;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }

    public void StopMouseMovement()
    {
        allowMovement = false;
    }
}
