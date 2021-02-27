using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinControl : MonoBehaviour
{
    // use to make a delay till show win panel

    [SerializeField] GameObject WinPanel;
    private int animation_id;
    private float animation_tick;

    private void Start()
    {
        WinPanel.transform.Find("FinishedMsg").gameObject.SetActive(false);
        hideWinPanel();
        animation_id = 0;
    }

    void Update()
    {
        playDelay();
    }

    public void playDelayBeforeShow()
    {
        animation_tick = 0.0f;
        animation_id = 1;
    }

    public void hideWinPanel()
    {
        WinPanel.SetActive(false);
    }

    public void RemoveNextLvlPossibility()
    {
        WinPanel.transform.Find("AvanceNextLevel").gameObject.SetActive(false);
    }

    public void MaxLvlResearchAlert()
    {
        WinPanel.transform.Find("FinishedMsg").gameObject.SetActive(true);
    }
    private void playDelay()
    {
        if(animation_id == 1)
        {
            animation_tick += Time.deltaTime;
            if(animation_tick >= 0.5f)
            {
                animation_id = 0;
                WinPanel.SetActive(true);
            }
        }
    }
}
