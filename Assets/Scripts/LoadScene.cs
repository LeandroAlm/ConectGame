using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [SerializeField] GameObject LevelBackgound, Objectives, CP_Pref, GameArea, SpawObj, slot_Pref;
    [SerializeField] Text LevelTile;
    [SerializeField] int block_size;

    private GameObject GridObj;
    private LevelLayout levelLayout;

    void Start()
    {
        LoadLevel();
    }

    public void LoadLevel()
    {
        levelLayout = gameObject.GetComponent<GamerController>().getLevelById(MenuController.CurrentLevel-1);

        if (GridObj != null) Destroy(GridObj);
        GameObject temp_go = new GameObject();
        GridObj = Instantiate(temp_go, Vector3.zero, Quaternion.identity, GameArea.transform);
        GridObj.name = "Grid";
        GridObj.transform.localPosition = new Vector3(0, 0, 0);
        GridObj.transform.SetSiblingIndex(3);

        Destroy(temp_go);

        /*int temp_counter = GridObj.transform.childCount;
        if(temp_counter > 0)
        {
            for (int i = 0; i < temp_counter; i++)
            {
                Destroy(GridObj.transform.GetChild(0));
            }
        }*/

        LoadGridbyDimenssion();

        foreach (Transform child in SpawObj.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        
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
        int start_x, start_y;
        Color temp_color;
        int counter = 0;

        gridGetfirtSlotLocation(out start_x, out start_y);

        foreach (ConectPositions CP in levelLayout.getAllNodes())
        {
            GameObject temp_obj = CP_Pref;
            temp_color = CP.getColorNode();
            temp_color.a = 1.0f;
            temp_obj.GetComponent<Image>().color = temp_color;

            Transform clone = Instantiate(CP_Pref, Vector3.zero, Quaternion.identity, SpawObj.transform).transform;
            clone.localScale = new Vector3(2, 2, 0);
            clone.name = "CP_" + counter + "_0";
            clone.localPosition = new Vector3(start_x + (CP.getPosition1().x * block_size), start_y - (CP.getPosition1().y * block_size), 0);

            clone = Instantiate(CP_Pref, Vector3.zero, Quaternion.identity, SpawObj.transform).transform;
            clone.localScale = new Vector3(2, 2, 0);
            clone.name = "CP_" + counter + "_1";
            clone.localPosition = new Vector3(start_x + (CP.getPosition2().x * block_size), start_y - (CP.getPosition2().y * block_size), 0);
            counter++;
        }
    }

    private void LoadGridbyDimenssion()
    {
        int start_x, start_y;

        gridGetfirtSlotLocation(out start_x, out start_y);

        for (int i = 0; i < levelLayout.getGridSize(); i++)
        {
            for (int j = 0; j < levelLayout.getGridSize(); j++)
            {
                Transform clone = Instantiate(slot_Pref, Vector3.zero, Quaternion.identity, GridObj.transform).transform;
                clone.localPosition = new Vector3(start_x + (i * block_size), start_y - (j * block_size), 0);
                clone.name = "slot_" + i + "_" + j;
            }
        }
    }

    public void gridGetfirtSlotLocation(out int slot_x, out int slot_y)
    {
        int grid_size = levelLayout.getGridSize();

        if (grid_size % 2 != 0)
        {
            // impar
            int temp_int = grid_size / 2;
            slot_x = 0 - (temp_int * block_size);
            slot_y = temp_int * block_size;
        }
        else
        {
            // par
            int temp_int = (grid_size-1) / 2;
            slot_x = 0 - (temp_int * block_size) - (block_size / 2);
            slot_y = ((grid_size/2) * block_size) - (block_size / 2);
        }
    }

    public int getBlockSize()
    {
        return block_size;
    }
}