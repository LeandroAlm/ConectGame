using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [SerializeField] GameObject globalAudio;

    private void Awake()
    {
        globalAudio = GameObject.FindGameObjectsWithTag("AudioSource")[0];
    }

    public void onActicateSettings()
    {
        Sprite sp;
        if (MenuController.soundTrigger == 1)
        {
            sp = Resources.Load<Sprite>("Images/soundOn");
        }
        else
        {
            sp = Resources.Load<Sprite>("Images/soundOff");
        }
        transform.Find("SoundControl").GetComponent<Image>().sprite = sp;

        if (MenuController.VibrationTrigger == 1)
        {
            sp = Resources.Load<Sprite>("Images/vibrationOn");
        }
        else
        {
            sp = Resources.Load<Sprite>("Images/vibrationOff");
        }
        transform.Find("VibrationControl").GetComponent<Image>().sprite = sp;
    }

    public void onSoundButtonClick()
    {
        if(MenuController.soundTrigger == 1)
        {
            globalAudio.GetComponent<Audiocontroller>().stopBackgorundMusic();
            MenuController.soundTrigger = 0;
        }
        else
        {
            if (!globalAudio.GetComponent<Audiocontroller>().BackgorundMusicisPlaying())
            {
                globalAudio.GetComponent<Audiocontroller>().playBackgorundMusicWithDelay(0.5f);
            }
            MenuController.soundTrigger = 1;
        }

        PlayerPrefs.SetInt("Sound", MenuController.soundTrigger);
        onActicateSettings();
    }

    public void onVibrationButtonClick()
    {
        if (MenuController.VibrationTrigger == 1)
        { MenuController.VibrationTrigger = 0; }
        else { MenuController.VibrationTrigger = 1; }
        PlayerPrefs.SetInt("Vibration", MenuController.VibrationTrigger);
        onActicateSettings();
    }

    public void onMenuButtonClick()
    {
        gameObject.SetActive(false);
    }

}
