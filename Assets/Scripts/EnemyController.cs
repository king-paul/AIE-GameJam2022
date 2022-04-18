using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerState
{
    standby,
    patrol,
    follow
}

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    PlayerState state;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        agent.destination = player.position;
    }
}
