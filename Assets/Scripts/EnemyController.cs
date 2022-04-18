using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Standby,
    Patrol,
    Follow
}

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    EnemyState state;

    public EnemyState State { set => state = value; }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;      
    }

    // Update is called once per frame
    void Update()
    {
        if(state == EnemyState.Follow)
            agent.destination = player.position;
    }
}
