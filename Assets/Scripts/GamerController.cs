using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamerController : MonoBehaviour
{
    // 1st grid pos = 101, 387
    // others grid pos = 101 + (array_x*37.5f), 387 - (array_x*37.7f)

    [SerializeField] LevelLayout[] AllLevelInGame;
    [SerializeField] int colliderGap; // use to simulate a box collider
    [SerializeField] GameObject CP_Pref, Objectives, SpawObj;

    private LevelLayout curerntLevelLayout;
    private int [,] grid;
    private bool is_holding, is_diagonal; // Control if is dragging, control if is a diagonal to avoid visual errors
    private Vector2 CP_nod, current_node;
    private int levelObjectives;

    void Start()
    {
        is_holding = false;
        is_diagonal = false;
        current_node = Vector2.zero;
        
        curerntLevelLayout = AllLevelInGame[MenuController.CurrentLevel-1];
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
        for (int i = 0; i < curerntLevelLayout.getGridSize(); i++)
        {
            for (int j = 0; j < curerntLevelLayout.getGridSize(); j++)
            {
                grid[i,j] = 0;
            }
        }

        foreach (ConectPositions cp in curerntLevelLayout.getAllNodes())
        {
            grid[(int)cp.getPosition1().x, (int)cp.getPosition1().y] = counter;
            grid[(int)cp.getPosition2().x, (int)cp.getPosition2().y] = counter;
            counter++;
        }

    }

    private void onHandler()
    {
        // Converte mouse position to local pos of SpawObj
        if (Input.GetMouseButtonDown(0) && !is_holding)
        {
            if (levelObjectives > 0)
            {
                if (isCPPosition(new Vector2(SpawObj.transform.InverseTransformPoint(Input.mousePosition).x, SpawObj.transform.InverseTransformPoint(Input.mousePosition).y)))
                {
                    setStartCP(new Vector2(SpawObj.transform.InverseTransformPoint(Input.mousePosition).x, SpawObj.transform.InverseTransformPoint(Input.mousePosition).y));
                    is_holding = true;
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && is_holding)
        {
            UndoGeneratedPath();
            is_holding = false;
        }
    }
    
    private void VerifyNewNode()
    {
        // if dragging chek if have new slot to insert or if resarch the other point
        if (is_holding)
        {
            Vector2 gridPosition = convertGamePostionInArrayID(new Vector2(SpawObj.transform.InverseTransformPoint(Input.mousePosition).x, SpawObj.transform.InverseTransformPoint(Input.mousePosition).y));
            if (isNextNode(gridPosition))
            {
                if (isEmpetyNode(gridPosition))
                {
                    is_diagonal = false;
                    ConectPositions CP = getCPbyPosition(CP_nod);
                    if (CP != null)
                    {
                        GameObject temp_obj = CP_Pref;
                        Color temp_color = CP.getColorNode();
                        temp_color.a = 1.0f;
                        temp_obj.GetComponent<Image>().color = temp_color;

                        Transform clone = Instantiate(CP_Pref, Vector3.zero, Quaternion.identity, SpawObj.transform).transform;
                        clone.localScale = new Vector3(2, 2, 0);
                        clone.localPosition = new Vector3(-159 + (gridPosition.x * 79.5f), 159 - (gridPosition.y * 79.5f), 0);

                        clone.name = CP.name + "_0";
                        clone.SetParent(SpawObj.transform.Find("Temp_paste").transform);
                        current_node = gridPosition;
                        grid[(int)current_node.x, (int)current_node.y] = 1;
                    }
                }
                else
                {
                    if (isMatchCP(gridPosition) && !is_diagonal)
                    {
                        is_diagonal = false;
                        is_holding = false;
                        FinishLineAndPlayAnim();
                    }
                    else
                    {
                        if ((int)gridPosition.x >= 0 && (int)gridPosition.x <= curerntLevelLayout.getGridSize() - 1 && (int)gridPosition.y >= 0 && (int)gridPosition.y <= curerntLevelLayout.getGridSize() - 1)
                        {
                            if (grid[(int)gridPosition.x, (int)gridPosition.y] == 1)
                            {
                                if((int)gridPosition.x != (int)current_node.x && (int)gridPosition.y != (int)current_node.y)
                                {
                                    //&& gridPosition diff current_node
                                    is_diagonal = true;
                                }
                            }
                            else
                            {
                                is_diagonal = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void FinishLineAndPlayAnim()
    {
        Transform temp_go = SpawObj.transform.Find("Temp_paste");
        int total_childs = temp_go.childCount;
        for (int i = 0; i < total_childs; i++)
        {
            temp_go.GetChild(0).SetParent(SpawObj.transform);
        }

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
        
        ConectPositions CP_to_Finish = getCPbyPosition(CP_nod);
        CP_to_Finish.setAsFinished();
        levelObjectives--;
        Objectives.GetComponent<Text>().text = "objectives left\n" + levelObjectives;

        #region Finish level verify
        if (levelObjectives <= 0)
        {
            if(MenuController.VibrationTrigger == 1)
            {
                Handheld.Vibrate();
            }                
            gameObject.GetComponent<WinControl>().playDelayBeforeShow();

            if(MenuController.MaxlvlAvaiable+1 <= AllLevelInGame.Length)
            {
                if(MenuController.MaxlvlAvaiable <= MenuController.CurrentLevel)
                {
                    MenuController.unlockNextLevel();
                }
            }
            else
            {
                if (MenuController.CurrentLevel >= MenuController.MaxlvlAvaiable)
                {
                    gameObject.GetComponent<WinControl>().RemoveNextLvlPossibility();
                }
                gameObject.GetComponent<WinControl>().MaxLvlResearchAlert();
            }
        }
        #endregion

        int cp_id = getCPid(CP_to_Finish);
        gameObject.GetComponent<LineAnimationController>().playLineAnimation(cp_id, curerntLevelLayout.getAllNodes()[cp_id].getGlowColor());
        Destroy(temp_go.gameObject);
    }

    private void UndoGeneratedPath()
    {
        Transform temp_go = SpawObj.transform.Find("Temp_paste");
        Destroy(temp_go.gameObject);

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
        clone.SetParent(SpawObj.transform);
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
                if (node.x >= (-159 + (i * 79.5f)) - colliderGap && node.x <= (-159 + (i * 79.5f)) + colliderGap)
                {
                    if (node.y <= (159 - (j * 79.5f)) + colliderGap && node.y >= 159 - (j * 79.5f) - colliderGap)
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

    private int getCPid(ConectPositions _CP)
    {
        int position_id = -1;
        int counter = 0;
        foreach (ConectPositions CP in curerntLevelLayout.getAllNodes())
        {
            if(CP == _CP)
            {
                position_id = counter;
                break;
            }
            counter++;
        }
        return position_id;
    }

    public LevelLayout getLevelById(int lvl_id)
    {
        return AllLevelInGame[lvl_id];
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ReloadLevel()
    {
        Start();
        gameObject.GetComponent<WinControl>().hideWinPanel();
        gameObject.GetComponent<LoadScene>().LoadLevel();
    }

    public void PlayNextLvl()
    {
        MenuController.setNextLvlasCurrentLvl();
        ReloadLevel();
    }
}
