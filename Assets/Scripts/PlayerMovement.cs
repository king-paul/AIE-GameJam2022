using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Transform mesh;

    [Header("Movement")]
    [Range(0, 10)]
    public float moveSpeed = 3f;
    public bool diagonalMovement = false;

    //[Header("Camera")]
    //public bool offSetCamera = false;
    //public Vector3 cameraOffset = new Vector3(0, 12.8f, -8.92f);    

    CharacterController controller;
    Transform camera;
    Vector3 direction;
    Vector3 vertical, horizontal;
    Vector3 upRight, downRight;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = Camera.main.transform;

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
        // player will move up down, left and right when camera is rotated
        if (diagonalMovement) 
        {
            vertical = transform.forward * Input.GetAxis("Vertical");
            horizontal = transform.right * Input.GetAxis("Horizontal");
            direction = new Vector3(horizontal.x, 0, vertical.z);
        }
        else // player will move with isometric grid
        {
            vertical = upRight * Input.GetAxis("Vertical");
            horizontal = downRight * Input.GetAxis("Horizontal");
            
            direction = vertical + horizontal;
        }

        if (direction != Vector3.zero)
        {
            direction.Normalize();
            if(mesh)
                mesh.forward = direction; // rotate character mesh to face direction it is moving
        }

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    /*
    private void LateUpdate()
    {
        if(offSetCamera)
            camera.position = transform.position + cameraOffset;
    }*/
}