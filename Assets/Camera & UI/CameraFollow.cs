using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField] float horizontalRotationSpeed = 75;
    [SerializeField] float verticalRotationSpeed = 10;

    public float panHorizontalStick = 0f;
    public float panVerticalStick = 0f;

    private GameObject player;
    private float maxDistanceToPlayer = 30f;
    private float minDistanceToPlayer = 5f;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void LateUpdate()
    {
        panHorizontalStick = Input.GetAxis("RS_Horizontal");
        panVerticalStick = Input.GetAxis("RS_Vertical");

        transform.position = player.transform.position;
        transform.Rotate(Vector3.up, -Time.deltaTime*horizontalRotationSpeed*panHorizontalStick);

        Vector3 vectorToPlayer = player.transform.position - transform.position;
        Vector3 abNorm = vectorToPlayer.normalized;

        // Move camera to position between max and min distance to player
        
        
    }
}
