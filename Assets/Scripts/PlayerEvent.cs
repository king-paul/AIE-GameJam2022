using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerEvent : MonoBehaviour
{
    //public Transform startPosition;

    // item variables
    [SerializeField] bool haveDogFood = false;
    [SerializeField] bool haveKey = false;
    [SerializeField] bool haveToy = false;

    public UnityEvent OnCollectDogFood, onCollectKey, onCollectToy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3) // item layer
        {
            switch (other.gameObject.tag)
            {
                case "DogFood":
                    haveDogFood = true;
                    OnCollectDogFood.Invoke();
                    break;

                case "Toy":
                    haveToy = true;
                    onCollectToy.Invoke();
                    break;

                case "Key":
                    haveKey = true;
                    onCollectKey.Invoke();                    
                    break;
            }

            GameObject.Destroy(other.gameObject);
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.layer == 6) // enemy layer
        {
            // reload the scene
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

    }
}
