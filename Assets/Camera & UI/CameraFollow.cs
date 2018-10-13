using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] float horizontalRotationSpeed = 75;
    [SerializeField] Camera theCamera = null;
    [SerializeField] GameObject theBoom = null;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] float homeSpeed = 5f;

    public Vector3 target;    // We try to keep the camera pointed at this.
    public Vector3 currentRotation;
    Vector3 mouseCameraTarget;

    public float mouseScrollWheel = 0f;

    private float panHorizontalStick = 0f;
    private float panVerticalStick = 0f;

    private GameObject player;
    private bool isUsingController;

    private bool isMouseScrolling = false;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // This is the midpoint between the Boom and the Player's base.

        theCamera.transform.position = Vector3.Lerp(player.transform.position, theBoom.transform.position, 0.5f);    
        isUsingController = playerMovement.getMovementMode();


        target = transform.position;
        target.y += 2;
    }

    // Update is called once per frame
    private void Update()
    {
        isUsingController = playerMovement.getMovementMode();

        if (Input.GetMouseButtonDown(1))
        {
            isMouseScrolling = !isMouseScrolling;
        }


    }


    void LateUpdate()
    {                
        if (isUsingController)
        {
            // Using Controller, move to a seperate function later.
            panHorizontalStick = Input.GetAxis("RS_Horizontal");
            panVerticalStick = Input.GetAxis("RS_Vertical");

            // Rotate camera around player

            transform.position = player.transform.position;
            transform.Rotate(Vector3.up, -Time.deltaTime * horizontalRotationSpeed * panHorizontalStick);           

            // Clamp the camera's target verticaly but keep the x and z coordinates
            // this lets the camera pan vertically.

            target.y += panVerticalStick/5f;
            target.y = Mathf.Clamp(target.y, 0f, 5f);
            target.x = transform.position.x;
            target.z = transform.position.z;

            theCamera.transform.LookAt(target);

            // Rotate camera to home position.
            if (Input.GetKey("joystick button 5"))
            {                
                transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * homeSpeed);
                target = transform.position;
                target.y += 2;
                theCamera.transform.LookAt(target);
            }

        }
        else
        {

            // Using mouse (Move to a seperate function later.)
            
            if (isMouseScrolling)
            {
                // Pan Around with scroll wheel clicked
                print("Trying to pan with mouse");

                Vector3 newMouseTarget = mouseCameraTarget;

                float mouseXRotationSpeed = Input.GetAxis("Mouse X");
                float mouseYRotationSpeed = Input.GetAxis("Mouse Y");

                newMouseTarget.x = newMouseTarget.x + mouseXRotationSpeed * 100f;
                newMouseTarget.y = newMouseTarget.y + mouseYRotationSpeed * 100f;

                mouseCameraTarget = Vector3.MoveTowards(mouseCameraTarget, newMouseTarget, Time.deltaTime * 5f);

                theCamera.transform.LookAt(mouseCameraTarget);
            }
            else
            {
                // Moves base to player position
                transform.position = player.transform.position;
                // Rotates base to player's rotation.
                transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * rotationSpeed);

                // Moves the camera to the Boom's position
                theCamera.transform.position = Vector3.Lerp(theBoom.transform.position, theCamera.transform.position, Time.deltaTime * rotationSpeed);

                mouseCameraTarget = transform.position;
                mouseCameraTarget.y += 2f;

                theCamera.transform.LookAt(mouseCameraTarget);

            }

        }
    }
}
