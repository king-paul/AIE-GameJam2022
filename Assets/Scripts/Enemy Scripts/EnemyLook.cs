using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLook : MonoBehaviour
{
    enum Direction { left, right, center };

    [Range(-180, 360)]
    public float leftAngle = 270;
    [Range(-180, 360)]
    public float rightAngle = 90;
    [Range(1, 100)]
    public float turnSpeed = 30f;
    [Range(0, 10)]
    public float waitTime = 1f;
    public bool invertRotation = false;

    private float startAngle;
    private Direction targetRotation;

    EnemyController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<EnemyController>();
        startAngle = transform.rotation.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.State == EnemyState.Look)
        {
            if (invertRotation)
                LookInRangeInverted();
            else
                LookInRange();
            
        }
    }

    void OldLookInRange()
    {
        float angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);

        //Debug.Log(angle);

        if (targetRotation == Direction.left)
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);

            if (angle <= leftAngle + startAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                targetRotation = Direction.right;
                controller.State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }

        }
        else if (targetRotation == Direction.right)
        {
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);

            angle = Vector3.SignedAngle(Vector3.forward, transform.forward, Vector3.up);

            if (angle >= startAngle + rightAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                targetRotation = Direction.center;
                controller.State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }
        }
        else if (targetRotation == Direction.center)
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);

            if (angle <= startAngle)
            {
                //Debug.Log("anti-clockwise Angle has been reached");
                targetRotation = Direction.left;
                controller.State = EnemyState.Standby;

                Invoke("StartLooking", waitTime);
            }
        }

    }

    void LookInRange()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        if (targetRotation == Direction.left) // left rotation
        {
            rotation.y -= 0.3f;
            if (rotation.y > 180.0f + startAngle && rotation.y < startAngle + leftAngle)
            {
                rotation.y = startAngle + leftAngle;
                targetRotation = Direction.right;

                controller.State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }

        if (targetRotation == Direction.right) // right rotation
        {
            rotation.y += 0.3f;
            if (rotation.y < startAngle + 180.0f && rotation.y > startAngle + rightAngle)
            {
                rotation.y = rightAngle;
                targetRotation = Direction.center;

                controller.State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }
        if (targetRotation == Direction.center) // back to start
        {
            rotation.y -= 0.3f;

            if (rotation.y <= startAngle)
            {
                rotation.y = startAngle;
                targetRotation = Direction.left;

                controller.State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }

        transform.rotation = Quaternion.Euler(rotation);
    }

    void LookInRangeInverted()
    {
        Vector3 rotation = transform.rotation.eulerAngles;

        if (targetRotation == Direction.left) // anti-clockwise
        {
            rotation.y -= 0.3f;

            if (rotation.y <= rightAngle)
            {
                targetRotation = Direction.right;

                controller.State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }

        if (targetRotation == Direction.right) // clockwise
        {
            rotation.y += 0.3f;

            if (rotation.y >= leftAngle)
            {
                targetRotation = Direction.center;

                controller.State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }

        if (targetRotation == Direction.center) // back to start
        {
            rotation.y -= 0.3f;

            if (rotation.y <= startAngle)
            {
                rotation.y = startAngle;
                targetRotation = Direction.left;

                controller.State = EnemyState.Standby;
                Invoke("StartLooking", waitTime);
            }
        }

        transform.rotation = Quaternion.Euler(rotation);
    }

    private void StartLooking()
    {
        controller.State = EnemyState.Look;
    }
}
