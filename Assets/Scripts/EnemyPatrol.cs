using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrtolRoute;

    private int wayPointNum = 0;
    NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.destination = patrtolRoute[wayPointNum].position;

        GetComponent<EnemyController>().State = EnemyState.Patrol;
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

            agent.destination = patrtolRoute[wayPointNum].position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
    }
}
