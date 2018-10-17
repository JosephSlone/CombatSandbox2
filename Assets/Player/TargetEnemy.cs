using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TargetEnemy : MonoBehaviour {

    [SerializeField] float targetingRadius = 5f;
    [SerializeField] float maxRange = 10f;

    private List<RaycastHit> potentialTargets;
    private List<RaycastHit> currentTargets;
    private List<RaycastHit> droppedTargets;
    private int layerMask = 1 << (int)Layer.Enemy;

    // Use this for initialization
    void Start () {
        int layerMask = 1 << (int)Layer.Enemy;
        currentTargets = Physics.SphereCastAll(transform.position, targetingRadius, Vector3.forward, maxRange, layerMask).ToList();
    }
	
	// Update is called once per frame
	void Update () {

        if(Input.GetButtonDown("Left Bumper"))
        {
            print("Left Bumper");
            ProcessTargets();
        }
		
	}

    private void LateUpdate()
    {
        ProcessTargets();
    }


    private void ProcessTargets()
    {
        potentialTargets = Physics.SphereCastAll(transform.position, targetingRadius, Vector3.forward, maxRange, layerMask).ToList();
        droppedTargets = currentTargets.Except(potentialTargets).ToList();
        currentTargets = potentialTargets;

        foreach (RaycastHit target in droppedTargets)
        {
            GameObject currentCollider = target.transform.gameObject;
            GameObject currentTarget = currentCollider.transform.parent.gameObject;
            EnemyTarget enemyTarget = currentTarget.GetComponentInChildren<EnemyTarget>();
            enemyTarget.ToggleTarget(false);
        }

        foreach (RaycastHit target in currentTargets)
        {
            GameObject currentCollider = target.transform.gameObject;
            GameObject currentTarget = currentCollider.transform.parent.gameObject;
            EnemyTarget enemyTarget = currentTarget.GetComponentInChildren<EnemyTarget>();
            enemyTarget.ToggleTarget(true);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        if (currentTargets != null)
        {
            foreach (RaycastHit hit in currentTargets)
            {
                Gizmos.DrawLine(transform.position, hit.transform.position);
                Gizmos.DrawSphere(hit.transform.position, 0.15f);
            }
        }
    }
}
