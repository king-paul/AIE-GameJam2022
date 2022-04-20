using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstructionViewer : MonoBehaviour
{
    //public LayerMask obstructionMask;
    public float hideRadius = 0.5f;
    public float showRadius = 10f;    
    public Vector3 hideDirection = Vector3.left;
    public Vector3 showDirection = Vector3.right;

    GameManager gameManager;
    GameObject[] walls;

    // Start is called before the first frame update
    void Start()
    {
        walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        
        if(Physics.SphereCast(transform.position, hideRadius, hideDirection, out hit))
        //if (Physics.Raycast(transform.position, Vector3.left, out hit))
        {
            foreach (GameObject wall in walls)
            {
                if (wall == hit.transform.gameObject)
                {
                    wall.GetComponent<MeshRenderer>().shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
                }

            }
        }

        if(Physics.SphereCast(transform.position, showRadius, showDirection, out hit))
        //if (Physics.Raycast(transform.position, Vector3.right, out hit))
        {
            foreach (GameObject wall in walls)
            {
                if (wall == hit.transform.gameObject)
                {
                    wall.GetComponent<MeshRenderer>().shadowCastingMode =
                    UnityEngine.Rendering.ShadowCastingMode.On;
                }

            }
        }
    } 
}
