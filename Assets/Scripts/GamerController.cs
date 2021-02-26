using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamerController : MonoBehaviour
{
    // 1st grid pos = 101, 387
    // others grid pos = 101 + (array_x*37.5f), 387 - (array_x*37.7f)

    private LevelLayout curerntLevelLayout;
    private int [,] grid;
    [SerializeField] int colliderGap; // use to simulate a box collider
    [SerializeField] GameObject CP_Pref, Objectives;

    private bool is_holding;
    private Vector2 CP_nod, current_node;
    private int levelObjectives;

    void Start()
    {
        is_holding = false;
        current_node = Vector2.zero;
        curerntLevelLayout = MenuController.GetLevelLayout();
        levelObjectives = curerntLevelLayout.getObectivesAmount();

        grid = new int[curerntLevelLayout.getGridSize(), curerntLevelLayout.getGridSize()];
        instanciateGameGrid();
    }

    void Update()
    {
        onHandler();
        VerifyNewNode();
    }

    private void instanciateGameGrid()
    {
        int counter = 2;
        // Insert all nodes id as a number starting in 2, empeties as 0
        foreach (ConectPositions cp in curerntLevelLayout.getAllNodes())
        {
            grid[(int)cp.getPosition1().x, (int)cp.getPosition1().y] = counter;
            grid[(int)cp.getPosition2().x, (int)cp.getPosition2().y] = counter;
            counter++;
        }

        for (int i = 0; i < curerntLevelLayout.getGridSize(); i++)
        {
            for (int j = 0; j < curerntLevelLayout.getGridSize(); j++)
            {
                if (grid[i,j] < 2)
                {
                    grid[i,j] = 0;
                }
            }
        }
    }

    private void onHandler()
    {
        if (Input.GetMouseButtonDown(0) && !is_holding)
        {
            if (isCPPosition(new Vector2(Input.mousePosition.x, Input.mousePosition.y)))
            {
                Debug.Log("Start drag");
                setStartCP(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                is_holding = true;
            }
        }
        else if (Input.GetMouseButtonUp(0) && is_holding)
        {
            Debug.Log("Stop drag");
            UndoGeneratedPath();
            is_holding = false;
        }
    }
    
    private void VerifyNewNode()
    {
        // if dragging chek if have new slot to insert or if resarch the other point
        if (is_holding)
        {
            Vector2 gridPosition = convertGamePostionInArrayID(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            if (isEmpetyNode(gridPosition))
            {
                if (isNextNode(gridPosition))
                {
                    ConectPositions CP = getCPbyPosition(CP_nod);
                    if (CP != null)
                    {
                        if (((int)CP.getPosition1().x == (int)CP_nod.x && (int)CP.getPosition1().y == (int)CP_nod.y) || ((int)CP.getPosition2().x == (int)CP_nod.x && (int)CP.getPosition2().y == (int)CP_nod.y))
                        {
                            Debug.Log("Instanciate");
                            GameObject temp_obj = CP_Pref;
                            Color temp_color = CP.getColorNode();
                            temp_color.a = 1.0f;
                            temp_obj.GetComponent<Image>().color = temp_color;

                            Transform clone = Instantiate(CP_Pref, new Vector3(101 + (gridPosition.x * 37.5f), 387 - (gridPosition.y * 37.7f), 0), Quaternion.identity).transform;
                            clone.name = CP.name + "_0";
                            clone.SetParent(Objectives.transform.Find("Temp_paste").transform);
                            current_node = gridPosition;
                            grid[(int)current_node.x, (int)current_node.y] = 1;
                        }
                    }
                }
            }
            else if(isMatchCP(gridPosition))
            {
                is_holding = false;
                levelObjectives--;
                Objectives.GetComponent<Text>().text = "objectives left\n" + levelObjectives;

                Transform go = Objectives.transform.Find("Temp_paste");
                int total_childs = go.childCount;

                for (int i = 0; i < total_childs; i++)
                {
                    go.GetChild(0).SetParent(Objectives.transform);
                }
                Destroy(go.gameObject);

                for (int i = 0; i < curerntLevelLayout.getGridSize(); i++)
                {
                    for (int j = 0; j < curerntLevelLayout.getGridSize(); j++)
                    {
                        if (grid[i, j] == 1)
                        {
                            grid[i, j] = grid[(int)CP_nod.x, (int)CP_nod.y];
                        }
                    }
                }
                getCPbyPosition(CP_nod).setAsFinished();
                Debug.Log("Finish 1 connect");
            }
        }
    }

    private void UndoGeneratedPath()
    {
        Transform go = Objectives.transform.Find("Temp_paste");
        Destroy(go.gameObject);

        for (int i = 0; i < curerntLevelLayout.getGridSize(); i++)
        {
            for (int j = 0; j < curerntLevelLayout.getGridSize(); j++)
            {
                if(grid[i, j] == 1)
                {
                    grid[i, j] = 0;
                }
            }
        }
    }

    private void setStartCP(Vector2 node)
    {
        // Set position of starting point and create an empety obj to be add new squares
        CP_nod = convertGamePostionInArrayID(node);
        current_node = CP_nod;
        GameObject temp_obj = new GameObject();
        Transform clone = Instantiate(temp_obj, Vector3.zero, Quaternion.identity).transform;
        clone.name = "Temp_paste";
        clone.SetParent(Objectives.transform);
    }

    private bool isNextNode(Vector2 node)
    {
        if (((int)node.x >= 0 && (int)node.x <= curerntLevelLayout.getGridSize() - 1) && ((int)node.y >= 0 && (int)node.y <= curerntLevelLayout.getGridSize() - 1))
        {
            if ((int)node.x == (int)current_node.x - 1 || (int)node.x == (int)current_node.x + 1)
            {
                if ((int)node.y == (int)current_node.y)
                {
                    return true;
                }
            }
            else if ((int)node.y == (int)current_node.y - 1 || (int)node.y == (int)current_node.y + 1)
            {
                if ((int)node.x == (int)current_node.x)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool isEmpetyNode(Vector2 node)
    {
        if(((int)node.x >= 0 && (int)node.x <= curerntLevelLayout.getGridSize()-1) && ((int)node.y >= 0 && (int)node.y <= curerntLevelLayout.getGridSize() - 1))
        {
            if (grid[(int)node.x, (int)node.y] == 0)
            {
               return true;
            }
        }
        return false;
    }

    private bool isMatchCP(Vector2 node)
    {
        if (((int)CP_nod.x != (int)node.x || (int)CP_nod.y != (int)node.y) && (node.x >= 0 && node.x <= curerntLevelLayout.getGridSize() - 1 && node.y >= 0 && node.y <= curerntLevelLayout.getGridSize() - 1))
        {
            if (grid[(int)CP_nod.x, (int)CP_nod.y] == grid[(int)node.x, (int)node.y])
            {
                return true;
            }
        }
        return false;
    }

    private bool isCPPosition(Vector2 node)
    {
        Vector2 temCPposition = convertGamePostionInArrayID(node);
        ConectPositions tempCP = getCPbyPosition(temCPposition);
        if (tempCP != null)
        {
            if (!tempCP.isFinished())
            {
                if (((int)temCPposition.x >= 0 && (int)temCPposition.x <= curerntLevelLayout.getGridSize() - 1) && ((int)temCPposition.y >= 0 && (int)temCPposition.y <= curerntLevelLayout.getGridSize() - 1))
                {
                    if (grid[(int)temCPposition.x, (int)temCPposition.y] >= 2)
                    {
                        return true;
                    }
                }
            }
            else
            {
                Debug.Log("Este Já está");
            }
        }
        else
        {
            Debug.Log("nop");
        }
        return false;
    }
    
    private Vector2 convertGamePostionInArrayID(Vector2 node)
    {
        // Convert
        Vector2 finalVector = new Vector2(-1, -1);
        for (int i = 0; i < curerntLevelLayout.getGridSize(); i++)
        {
            for (int j = 0; j < curerntLevelLayout.getGridSize(); j++)
            {
                if (node.x >= (101 + (i * 37.5f)) - colliderGap && node.x <= (101 + (i * 37.5f)) + colliderGap)
                {
                    if (node.y <= (387 - (j * 37.7f)) + colliderGap && node.y >= 387 - (j * 37.7f) - colliderGap)
                    {
                        finalVector = new Vector2(i, j);
                    }
                }
            }
        }

        return finalVector;
    }

    private ConectPositions getCPbyPosition(Vector2 node)
    {
        foreach (ConectPositions CP in curerntLevelLayout.getAllNodes())
        {
            if (((int)CP.getPosition1().x == (int)node.x && (int)CP.getPosition1().y == (int)node.y) || ((int)CP.getPosition2().x == (int)node.x && (int)CP.getPosition2().y == (int)node.y))
            {
                return CP;
            }
        }
        return null;
    }
}
