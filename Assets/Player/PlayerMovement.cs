using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using Cinemachine;

// TODO Provide a Movement Mode Notification on the HUD.

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] int CrouchToggleDelay = 20;
    [SerializeField] float attackMoveStopRadius = 5f;

    public bool isInDirectMode = true;   // Start using game controller!

    CinemachineFreeLook freeLook;
    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;    
    bool m_Jump = false;
    bool m_crouch = false;

    int CrouchToggleCount = 0;
    string cameraYAxis;
    string cameraXAxis;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        freeLook = FindObjectOfType<CinemachineFreeLook>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();

        currentDestination = transform.position;
        cameraYAxis = freeLook.m_YAxis.m_InputAxisName;
        cameraXAxis = freeLook.m_XAxis.m_InputAxisName;
    }

    private void Update()
    {
        MouseLook();

    }

    private void MouseLook()
    {
        if (!getMovementMode())
        {
            freeLook.m_YAxisRecentering.m_enabled = false;
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                freeLook.m_YAxis.Value += 0.05f;
            }
            else
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    freeLook.m_YAxis.Value -= 0.05f;
                }
            }

            if (Input.GetMouseButton(2))
            {
                print("Center Button Down");
                freeLook.m_YAxis.m_InputAxisName = "Mouse Y";
                freeLook.m_XAxis.m_InputAxisName = "Mouse X";
            }
            else
            {
                freeLook.m_YAxis.m_InputAxisName = cameraYAxis;
                freeLook.m_XAxis.m_InputAxisName = cameraXAxis;
            }
        }
        else
        {
            freeLook.m_YAxisRecentering.m_enabled = true;
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
                m_crouch = !m_crouch;
                CrouchToggleCount = 0;
            }
        }
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.G)) // G for gamepad. TODO add to menu
        {
            isInDirectMode = !isInDirectMode; // toggle mode
            currentDestination = transform.position; // clear the click target
        }

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
        // Needs to be moved later.

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);


        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        CrouchToggle();

        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }


        // calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        // pass all parameters to the character control script
        thirdPersonCharacter.Move(m_Move, m_crouch, m_Jump);
        m_Jump = false;
    }

    private void ProcessMouseMovement()
    {        
        if (Input.GetMouseButton(0))
        {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;

                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
                    break;

                default:
                    print("Unexpected Layer Found!");
                    break;
            }
        }
        WalkToDestination();
    }


    private void WalkToDestination()
    {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0.1)
        {
            thirdPersonCharacter.Move(playerToClickPoint, false, false);
        }
        else
        {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
        }
    }

    Vector3 ShortDestination(Vector3 destination, float shortening)
    {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    //private void OnDrawGizmos()
    //{
    //    // Draw movement gizmos

    //    Gizmos.color = Color.black;
    //    Gizmos.DrawLine(transform.position, clickPoint);
    //    Gizmos.DrawSphere(currentDestination, 0.15f);
    //    Gizmos.DrawSphere(clickPoint, 0.1f);

    //    // Draw attack sphere
    //    Gizmos.color = new Color(255f, 0f, 0, .5f);
    //    Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
    //}
}

