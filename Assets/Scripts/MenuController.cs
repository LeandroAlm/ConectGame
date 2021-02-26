using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] LevelLayout[] AllLevelInGame;
    public static LevelLayout lvlLayout;

    public static LevelLayout GetLevelLayout()
    {
        return lvlLayout;
    }

    public void MenuOnButtonClickLevel(int level_id)
    {
        SceneManager.LoadScene("Game");
        MenuController.lvlLayout = AllLevelInGame[level_id - 1];

        //SceneManager.UnloadScene("Menu");
    }
}
