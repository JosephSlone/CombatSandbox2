using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookat : MonoBehaviour {

    [SerializeField] float rotationSpeed = 5f;
    private GameObject player;
    

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void LateUpdate () {
        Vector3 relativePosition = player.transform.position - transform.position;

        Quaternion rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * rotationSpeed);
        transform.rotation = rotation;

        transform.LookAt(player.transform);

	}


}
