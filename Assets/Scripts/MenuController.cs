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
    public static int soundTrigger, VibrationTrigger;

    [SerializeField] GameObject Levels, BttPref, SettingsPanel;
    private static MenuController MenuControl; // avoid duplicate when change scene

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (MenuControl == null)
        {
            MenuControl = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SettingsPanel.SetActive(false);

        MaxlvlAvaiable = 0;

        #region Load saved vars
        if (PlayerPrefs.HasKey("Level"))
        {
            MaxlvlAvaiable = PlayerPrefs.GetInt("Level");
        }
        else
        {
            unlockNextLevel();
        }
        if (PlayerPrefs.HasKey("Sound"))
        {
            soundTrigger = PlayerPrefs.GetInt("Sound");
        }
        else
        {
            soundTrigger = 1;
        }
        if (PlayerPrefs.HasKey("Vibration"))
        {
            VibrationTrigger = PlayerPrefs.GetInt("Vibration");
        }
        else
        {
            VibrationTrigger = 1;
        }
        #endregion

        if(soundTrigger == 1)
        {
            if(!gameObject.GetComponent<AudioSource>().isPlaying)
            {
                gameObject.GetComponent<AudioSource>().Play();
            }
        }

        loadPossibleLevels(AllLevelInGame.Length, MaxlvlAvaiable);
    }

    public static void onButtonLevelClick(int level_id)
    {
        CurrentLevel = level_id;
        SceneManager.LoadScene("Game");
    }

    public void onButtonSettingsClick()
    {
        SettingsPanel.GetComponent<SettingsController>().onActicateSettings();
        SettingsPanel.SetActive(true);
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

    private void loadPossibleLevels(int levels, int avaiable_levels)
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
            buttonPrefab.name = "" + i;


            if (i <= 9) { text = "0" + i; }
            else { text = "" + i; }
            buttonPrefab.transform.GetChild(0).GetComponent<Text>().text = text;

            spaw_position_x += 150;
            line_break++;
            if (line_break >= 3)
            {
                line_break = 0;
                spaw_position_x = -150;
                spaw_position_y -= 150;
            }

            if (i <= avaiable_levels)
            {
                if (buttonPrefab.gameObject.GetComponent<Button>() != null)
                {
                    addListener(buttonPrefab.gameObject.GetComponent<Button>(), i);
                }
            }
            else
            {
                Color temp_c = new Color(100, 100, 100, 0.2f);
                buttonPrefab.GetComponent<Image>().color = temp_c;
            }
        }
    }

    public static void addListener(Button leButton, int i)
    {
        leButton.onClick.AddListener(() => onButtonLevelClick(i));
    }
}
