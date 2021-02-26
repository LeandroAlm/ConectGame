using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] GameObject LevelBackgound, Objectives, CP_Pref, GameGrid, GameControlObject;
    [SerializeField] Text LevelTile;
    private LevelLayout levelLayout;

    void Start()
    {
        levelLayout = MenuController.GetLevelLayout();
        LevelTile.text = "LEVEL " + levelLayout.name;

        foreach (ConectPositions CP in levelLayout.getAllNodes())
        {
            CP.setAsUnfinished();
        }

        Objectives.GetComponent<Text>().text = "objectives left\n" + levelLayout.getObectivesAmount();

        Color temp_color = levelLayout.getBGColor();
        temp_color.a = 1.0f;
        LevelBackgound.GetComponent<Image>().color = temp_color;

        InstanciateStarterPoints();
        Destroy(gameObject);
    }

    private void InstanciateStarterPoints()
    {
        Color temp_color;
        int counter = 0;
        foreach (ConectPositions CP in levelLayout.getAllNodes())
        {
            GameObject temp_obj = CP_Pref;
            temp_color = CP.getColorNode();
            temp_color.a = 1.0f;
            temp_obj.GetComponent<Image>().color = temp_color;

            Transform clone = Instantiate(CP_Pref, new Vector3(101 + (CP.getPosition1().x * 37.5f), 387 - (CP.getPosition1().y * 37.7f), 0), Quaternion.identity).transform;
            clone.name = "CP_" + counter + "_0";
            clone.SetParent(Objectives.transform);

            clone = Instantiate(CP_Pref, new Vector3(101 + (CP.getPosition2().x * 37.5f), 387 - (CP.getPosition2().y * 37.7f), 0), Quaternion.identity).transform;
            clone.name = "CP_" + counter + "_1";
            clone.SetParent(Objectives.transform);
        }
    }
}