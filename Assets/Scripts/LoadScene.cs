using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] GameObject LevelBackgound, Objectives, CP_Pref, GameControlObject, SpawObj;
    [SerializeField] Text LevelTile;
    private LevelLayout levelLayout;

    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        foreach (Transform child in SpawObj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
        levelLayout = gameObject.GetComponent<GamerController>().getLevelById(MenuController.CurrentLevel-1);
        LevelTile.text = "LEVEL " + levelLayout.name;

        foreach (ConectPositions CP in levelLayout.getAllNodes())
        {
            CP.setAsUnfinished();
        }

        Objectives.GetComponent<Text>().text = "objectives left\n" + levelLayout.getObectivesAmount();
        Objectives.GetComponent<Text>().font.material.mainTexture.filterMode = FilterMode.Point;

        Color temp_color = levelLayout.getBGColor();
        LevelBackgound.GetComponent<Image>().color = temp_color;

        InstanciateStarterPoints();
        //Destroy(gameObject);
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

            Transform clone = Instantiate(CP_Pref, Vector3.zero, Quaternion.identity, SpawObj.transform).transform;
            clone.localScale = new Vector3(2, 2, 0);
            clone.name = "CP_" + counter + "_0";
            clone.SetParent(SpawObj.transform);
            clone.localPosition = new Vector3(-159 + (CP.getPosition1().x * 79.5f), 159 - (CP.getPosition1().y * 79.5f), 0);

            clone = Instantiate(CP_Pref, Vector3.zero, Quaternion.identity, SpawObj.transform).transform;
            clone.localScale = new Vector3(2, 2, 0);
            clone.name = "CP_" + counter + "_1";
            clone.SetParent(SpawObj.transform);
            clone.localPosition = new Vector3(-159 + (CP.getPosition2().x * 79.5f), 159 - (CP.getPosition2().y * 79.5f), 0);
            counter++;
        }
    }
}