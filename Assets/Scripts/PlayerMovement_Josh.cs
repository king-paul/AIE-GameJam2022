using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement_Josh : MonoBehaviour
{
    [Range(0, 10)]
    public float moveSpeed = 1f;
    public Vector3 cameraOffset = new Vector3(0, 12.8f, -8.92f);

    CharacterController controller;
    Transform camera;
    Vector3 direction;
    Vector3 vertical, horizontal;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("w"))
        {
            vertical = transform.forward * Input.GetAxis("Vertical");
            horizontal = transform.right * Input.GetAxis("Vertical") * -1;

            direction = new Vector3(horizontal.x, 0, vertical.z);

            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey("s"))
        {
            vertical = transform.forward * Input.GetAxis("Vertical");
            horizontal = transform.right * Input.GetAxis("Vertical") * -1;

            direction = new Vector3(horizontal.x, 0, vertical.z);

            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);

        }
        if (Input.GetKey("a"))
        {
            vertical = transform.forward * Input.GetAxis("Horizontal");
            horizontal = transform.right * Input.GetAxis("Horizontal");

            direction = new Vector3(horizontal.x, 0, vertical.z);

            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);


        }
        if (Input.GetKey("d"))
        {
            vertical = transform.forward * Input.GetAxis("Horizontal");
            horizontal = transform.right * Input.GetAxis("Horizontal");

            direction = new Vector3(horizontal.x, 0, vertical.z);

            controller.Move(direction.normalized * moveSpeed * Time.deltaTime);


        }
    }
    private void LateUpdate()
    {
        camera.position = transform.position + cameraOffset;
    }
}

