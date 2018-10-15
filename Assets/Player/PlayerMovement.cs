using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using Cinemachine;

// TODO Provide a Movement Mode Notification on the HUD.

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] int CrouchToggleDelay = 20;
    public bool isInDirectMode = false;

    CinemachineFreeLook freeLook;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    float walkMoveStopRadius = 0.1f;
    
    bool m_Jump = false;
    bool m_crouch = false;

    int CrouchToggleCount = 0;
    string cameraYAxis;
    string cameraXAxis;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        freeLook = FindObjectOfType<CinemachineFreeLook>();

        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;

        cameraYAxis = freeLook.m_YAxis.m_InputAxisName;
        cameraXAxis = freeLook.m_XAxis.m_InputAxisName;

        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        MouseLook();

        // TODO Add to a menu
        if (Input.GetKeyDown(KeyCode.G))
        {
            isInDirectMode = !isInDirectMode;
        }


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

        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }


        // calculate camera relative direction to move:
        Vector3 m_CamForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * Camera.main.transform.right;

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_crouch, m_Jump);
        m_Jump = false;
    }

    private void ProcessMouseMovement()
    {
        if (Input.GetMouseButton(0))
        {
            switch (cameraRaycaster.currentLayerHit)
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

        if (Vector3.Distance(currentClickTarget, transform.position) > walkMoveStopRadius)
        {
            m_Character.Move(currentClickTarget - transform.position, false, false);
        }
        else
        {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
}

