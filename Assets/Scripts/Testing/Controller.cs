using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float moveSpeed = 6;

    CharacterController controller;
    Camera viewCamera;
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        viewCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 mousePos = Input.mousePosition;
            //viewCamera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        //transform.LookAt(mousePos, Vector3.up);
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;

        //Debug.Log(mousePos);
        //Debug.DrawLine(transform.position, mousePos);
    }

    void FixedUpdate()
    {
        controller.Move(velocity * Time.fixedDeltaTime);
        
    }
}
