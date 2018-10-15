using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

// TODO Provide a Movement Mode Notification on the HUD.

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] int CrouchToggleDelay = 20;

    public bool isInDirectMode = false;

    ThirdPersonCharacter player;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    float walkMoveStopRadius = 0.1f;
    
    bool jump = false;
    bool crouch = false;

    int CrouchToggleCount = 0;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        player = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    private void Update()
    {
        // TODO Add to a menu
        if (Input.GetKeyDown(KeyCode.G))
        {
            isInDirectMode = !isInDirectMode;
        }

        if (isInDirectMode)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }

    }

    public bool getMovementMode()
    {
        return isInDirectMode;
    }

    private void CrouchToggle()
    {
        if (Input.GetAxis("DPAD Vertical") == -1)
        {
            CrouchToggleCount++;
            if (CrouchToggleCount > CrouchToggleDelay)
            {
                crouch = !crouch;
                CrouchToggleCount = 0;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                crouch = !crouch;
            }
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (isInDirectMode)
        {
            ProcessDirectMovement();
        }
        else
        {
            ProcessMouseMovement();
        }
    }

    private void ProcessDirectMovement()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        CrouchToggle();
        //crouch = Input.GetButtonDown("Joystick Button 8");

        if (!jump)
        {
            jump = Input.GetButtonDown("Jump");
        }


        // calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        // pass all parameters to the character control script
        player.Move(m_Move, crouch, jump);
        jump = false;
    }

    private void ProcessMouseMovement()
    {

        Quaternion facing = player.transform.rotation;

        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.layerHit)
            {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    break;

                case Layer.Enemy:
                    break;

                default:
                    break;
            }
        }

        if (!jump)
        {
            jump = Input.GetButtonDown("Jump");
        }

        //crouch = Input.GetButtonDown("Joystick Button 8");

        if (Vector3.Distance(currentClickTarget, transform.position) > walkMoveStopRadius)
        {
            player.Move(currentClickTarget - transform.position, crouch, jump);
            currentClickTarget = player.transform.position;
        }
        else
        {
            player.Move(Vector3.zero, crouch, jump);
            currentClickTarget = player.transform.position;
        }

        if(jump)
        {
            player.transform.rotation = facing;
        }
        CrouchToggle();
        jump = false;
    }
}

