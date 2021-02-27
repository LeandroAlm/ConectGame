using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    [SerializeField] LevelLayout[] AllLevelInGame;
    public static int MaxlvlAvaiable; // use to show available levels, this var is saved 
    public static int CurrentLevel; // current level behing show on Game scene

    [SerializeField] GameObject Levels, BttPref;

    private void Start()
    {
        MaxlvlAvaiable = 0;
        if (PlayerPrefs.HasKey("Level"))
        {
            MaxlvlAvaiable = PlayerPrefs.GetInt("Level");
        }
        else
        {
            unlockNextLevel();
        }

        loadPossibleLevels(MaxlvlAvaiable);
    }

    public static void MenuOnButtonClickLevel(int level_id)
    {
        CurrentLevel = level_id;
        SceneManager.LoadScene("Game");
    }

    public static void unlockNextLevel()
    {
        // call when finish level
        MaxlvlAvaiable++;
        PlayerPrefs.SetInt("Level", MaxlvlAvaiable);
    }

    public static void setNextLvlasCurrentLvl()
    {
        CurrentLevel++;
    }

    private void loadPossibleLevels(int levels)
    {
        Transform buttonPrefab;
        string text = "";

        int line_break = 0;
        int spaw_position_x = -150;
        int spaw_position_y = 300;


        for (int i = 1; i <= levels; i++)
        {
            buttonPrefab = Instantiate(BttPref, Vector3.zero, new Quaternion(0, 0, 0, 0), Levels.transform).transform;
            buttonPrefab.localPosition = new Vector3(spaw_position_x, spaw_position_y, 0);
            buttonPrefab.name = ""+i;


            if (i <= 9) { text = "0" + i; }
            else { text = "" + i; }
            buttonPrefab.transform.GetChild(0).GetComponent<Text>().text = text;

            spaw_position_x += 150;
            line_break++;
            if(line_break >= 3)
            {
                line_break = 0;
                spaw_position_x = -150;
                spaw_position_y -= 150;
            }
        }

        line_break = 1;
        foreach (Transform t in Levels.transform)
        {
            if (t.gameObject.GetComponent<Button>() != null)
            {
                addListener(t.gameObject.GetComponent<Button>(), line_break);
                line_break++;
            }
        }
    }

    public static void addListener(Button leButton, int i)
    {
        leButton.onClick.AddListener(() => MenuOnButtonClickLevel(i));
    }
}
