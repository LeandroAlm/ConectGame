using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    static int LevelToLoad;
    

    public static int GetLevelToLoad()
    {
        return MenuController.LevelToLoad;
    }

    public void MenuOnButtonClickLevel(int level_id)
    {
        MenuController.LevelToLoad = level_id;
        SceneManager.LoadScene("Game");
        //SceneManager.UnloadScene("Menu");
    }
}
