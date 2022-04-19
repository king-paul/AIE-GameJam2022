using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Placeholder());
    }
    //Play the animation here
    IEnumerator Placeholder()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Main Scene");
    }

    
}
