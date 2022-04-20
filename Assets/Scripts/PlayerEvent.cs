using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerEvent : MonoBehaviour
{
    //public Transform startPosition;

    // item variables
    bool haveDogFood = false;
    bool haveKey = false;
    bool haveToy = false;

    public UnityEvent OnCollectDogFood, onCollectKey, onCollectToy, onCollectionComplete, 
                      onDie, onWinGame;

    public void Start()
    {
        
    }

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

            if (haveDogFood && haveKey && haveToy)
                onCollectionComplete.Invoke();
        }

        if(other.gameObject.tag == "Exit")
        {
            if (haveDogFood && haveKey && haveToy)
                onWinGame.Invoke();
            else
                Debug.Log("You do not have all of the items");
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.layer == 6) // enemy layer
        {
            onDie.Invoke();
            // reload the scene
            SceneManager.LoadScene(1, LoadSceneMode.Single);
        }

    }
}
