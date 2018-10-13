using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

// TODO Provide a Movement Mode Notification on the HUD.

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField] int CrouchToggleDelay = 20;

    public bool isInDirectMode = false;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    float walkMoveStopRadius = 0.1f;
    
    bool m_Jump = false;
    bool m_crouch = false;

    int CrouchToggleCount = 0;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
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

