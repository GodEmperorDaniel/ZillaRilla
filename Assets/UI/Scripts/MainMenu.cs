using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public void StartNewGame()
    {
        Debug.Log("Start Game!");
        GameManager.Instance.StartNewGame();
    }

    public void Options()
    {
        //GameManager.Instance.Options();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }

}
