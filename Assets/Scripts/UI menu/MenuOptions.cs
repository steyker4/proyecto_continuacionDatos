using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuOptions : MonoBehaviour
{
    // Start is called before the first frame update
    public void NewGame()
    {
        SceneManager.LoadScene(1);
    }
    public void Play()
    {
        SceneManager.LoadScene(2);
    }
   public void Exit()
    {
#if UNITY_EDITOR
       EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
