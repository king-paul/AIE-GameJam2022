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

enum Direction { left, right, center };

public class EnemyController : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    [SerializeField] EnemyState state;

    [Header("Look State")]
    //[Range(-180, 180)]
    float startAngle = 0;
    [Range(-180, 360)]
    public float leftAngle = 270;
    [Range(-180, 360)]
    public float rightAngle = 90;
    [Range(1, 100)]
    public float turnSpeed = 30f;
    [Range(0, 10)]
    public float waitTime = 1f;

    //bool clockwise;
    Direction targetRotation;

    public EnemyState State { get => state; set => state = value; }    

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;

        startAngle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Follow)
        {
            agent.speed = 60;
            agent.destination = player.position;
        }

        if (state == EnemyState.Look)
            LookInRange();
    }

    void LookInRange()
    {
        float angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);

        //Debug.Log(angle);
        
        if(targetRotation == Direction.left)
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);

            if (angle <= leftAngle + startAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                targetRotation = Direction.right;
                State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }

        }
        else if(targetRotation == Direction.right)
        {
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);

            angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);

            if (angle >= startAngle + rightAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                targetRotation = Direction.center;
                State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }
        }
        else if(targetRotation == Direction.center)
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);

            if (angle <= startAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                targetRotation = Direction.left;
                State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }
        }

    }

    void NewLookInRange()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        if (targetRotation == Direction.left) // left rotation
        {
            rotation.y -= 0.3f;
            if (rotation.y > 180.0f + startAngle && rotation.y < leftAngle + startAngle)
            {
                rotation.y = 270.0f;
                targetRotation = Direction.right;

                State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }

        if (targetRotation == Direction.right) // right rotation
        {
            rotation.y += 0.3f;
            if (rotation.y < 180.0f + startAngle && rotation.y > rightAngle + startAngle)
            {
                rotation.y = 90.0f;
                targetRotation = Direction.center;

                State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }
        if(targetRotation == Direction.center) // back to start
        {
            rotation.y -= 0.3f;

            if (rotation.y <= startAngle)
            {
                rotation.y = startAngle;
                targetRotation = Direction.left;

                State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }        

        transform.rotation = Quaternion.Euler(rotation);
    }

    private void StartLooking()
    {
        State = EnemyState.Look;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.other.tag == "Player")
        {            
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

    }
}
