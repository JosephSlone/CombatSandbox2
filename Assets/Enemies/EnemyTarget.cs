using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour {

    [Tooltip("The targeting prefab")]
    [SerializeField]
    GameObject targetPrefab = null;

    private GameObject instatiatedTarget = null;

    // Use this for initialization
    void Start () {

        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleTarget(bool flag)
    {    
        if (flag)
        {
            if(instatiatedTarget == null)
            {
                instatiatedTarget = Instantiate(targetPrefab, transform.position, Quaternion.identity, transform);
            }
        }
        else
        {
            if(instatiatedTarget != null)
            {
                Destroy(instatiatedTarget);
                instatiatedTarget = null;   // Shouldn't be neccessary.
            }            
        }
    }
}
