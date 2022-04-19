using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Range(0, 10)]
    public float moveSpeed = 3f;
    public bool diagonalMovement = false;
    public bool offSetCamera = false;
    public Vector3 cameraOffset = new Vector3(0, 12.8f, -8.92f);    

    CharacterController controller;
    Transform camera;
    Vector3 direction;
    Vector3 vertical, horizontal;
    Vector3 upRight, downRight;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = transform.GetChild(0);

        if (camera.rotation.y >= 0)
        {
            upRight = new Vector3(1, 0, 1);
            downRight = new Vector3(1, 0, -1);
        }
        else
        {
            upRight = new Vector3(-1, 0, 1);
            downRight = new Vector3(1, 0, 1); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (diagonalMovement)
        {
            vertical = transform.forward * Input.GetAxis("Vertical");
            horizontal = transform.right * Input.GetAxis("Horizontal");
            direction = new Vector3(horizontal.x, 0, vertical.z);
        }
        else
        {
            vertical = upRight * Input.GetAxis("Vertical");
            horizontal = downRight * Input.GetAxis("Horizontal");
            
            direction = vertical + horizontal;
        }

        controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if(offSetCamera)
            camera.position = transform.position + cameraOffset;
    }
}