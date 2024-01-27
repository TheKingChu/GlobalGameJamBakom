using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 represents left mouse button
        {
            animator.SetTrigger("LeftHand"); // Replace "Animation1" with the name of your first animation
        }
        else if (Input.GetMouseButtonDown(1)) // 1 represents right mouse button
        {
            animator.SetTrigger("RightHand"); // Replace "Animation2" with the name of your second animation
        }
    }
}
