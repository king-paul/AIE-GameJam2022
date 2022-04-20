using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Tooltip("The number of seconds the enemy will stop on each waypoint before moving to another")]
    [Range(0f, 10f)]
    public float waitTime = 5f;

    [Tooltip("Holds all of the waypoints in the enmies patrol route")]
    public Transform[] patrtolRoute;

    private int wayPointNum = 0;
    
    NavMeshAgent agent;
    EnemyController controller;

    public void MoveToNextWayPoint()
    {
        controller.State = EnemyState.Patrol;
        agent.destination = patrtolRoute[wayPointNum].position;
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = patrtolRoute[wayPointNum].position;

        controller = GetComponent<EnemyController>();
        controller.State = EnemyState.Patrol;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == patrtolRoute[wayPointNum])
        {
            if (wayPointNum < patrtolRoute.Length - 1)
                wayPointNum++;
            else
                wayPointNum = 0;

            controller.State = EnemyState.Standby;
            Invoke("MoveToNextWayPoint", waitTime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
    }
}
