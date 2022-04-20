using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public enum EnemyState
{
    Standby,
    Patrol,
    Look,
    Follow
}

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    [SerializeField] EnemyState state;
    

    public EnemyState State { get => state; set => state = value; }    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Follow)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
            agent.speed = 60;
            agent.destination = player.position;
        }
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.other.tag == "Player")
        {            
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }

    }

    
}
