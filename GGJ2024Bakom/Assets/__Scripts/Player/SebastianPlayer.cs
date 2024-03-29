//original by Charlie
//modified by Sebastian

using UnityEngine;
public class SebastianPlayer : MonoBehaviour
{
    //player movement
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private float playerSpeed = 5.0f;
    //animation
    private Animator animator; // Add this line
    //
    private Camera mainCamera;

    //sounds
    public AudioSource honkClip;

    //pie
    public GameObject piePrefab;

        //pie
        private bool hasPie;

        //animation

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
        }
        



    private void PlayerMovement()
    {
        //movement
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        characterController.Move(move * (Time.deltaTime * playerSpeed));

        //running
        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerSpeed = 10f;
        }
            


        //rotation
        float turn = Input.GetAxis("Mouse X");
        transform.Rotate(0, turn, 0);
            
        //vertical movement
        float verticalMove = Input.GetAxis("Mouse Y");
        transform.Rotate(-verticalMove, 0, 0);
    }
    
}