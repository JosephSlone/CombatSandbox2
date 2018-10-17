using UnityEngine;
using Cinemachine;

// Add a UI Socket transform to your enemy
// Attack this script to the socket
// Link to a canvas prefab that contains NPC UI
public class EnemyUI : MonoBehaviour {

    // Works around Unity 5.5's lack of nested prefabs
    [Tooltip("The UI canvas prefab")]
    [SerializeField]
    GameObject enemyCanvasPrefab = null;


    Camera cameraToLookAt;
    CinemachineFreeLook freeLook;

    // Use this for initialization 
    void Start()
    {
        Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity, transform);
        cameraToLookAt = Camera.main;
        freeLook = FindObjectOfType<CinemachineFreeLook>();
    }

    // Update is called once per frame 
    void LateUpdate()
    {
        transform.LookAt(freeLook.transform);
        transform.rotation = Quaternion.LookRotation(freeLook.transform.forward);
      
    }
}