using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [Header("Look State")]
    [Range(-180, 180)]
    public float leftAngle = -90;
    [Range(-180, 180)]
    public float rightAngle = 90;
    [Range(1, 100)]
    public float turnSpeed = 30f;
    [Range(0, 10)]
    public float waitTime = 1f;

    bool clockwise;

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

        if (state == EnemyState.Look)
            LookInRange();
    }

    void LookInRange()
    {
        float angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);

        //Debug.Log(angle);
        
        if(!clockwise)
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);

            if (angle <= leftAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                clockwise = true;
                State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }

        }
        else
        {
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);

            angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);
            if (angle >= rightAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                clockwise = false;
                State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }
        }

    }

    private void StartLooking()
    {
        State = EnemyState.Look;
    }
}
