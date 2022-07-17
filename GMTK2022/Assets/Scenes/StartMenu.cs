using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void startGame()
    {
        SceneManager.LoadScene("tutorial");
    }
    public void RealstartGame()
    {
        SceneManager.LoadScene("Main");
    }
    public void EndGame()
    {
#if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
 #else
         Application.Quit();
 #endif
    }
}
