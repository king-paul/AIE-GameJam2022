using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class TitleMenu : MonoBehaviour
{
    public Button startButton;
    public Button endButton;
   public void ClickButton(int buttonClicked)
    {
        if (buttonClicked == 0)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
        }
        else if (buttonClicked == 1)
        {

            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
