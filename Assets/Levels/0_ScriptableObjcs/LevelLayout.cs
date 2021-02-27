using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class LevelLayout : ScriptableObject
{
    [SerializeField] Color bgColor;
    [SerializeField] int grid_size;
    [SerializeField] ConectPositions[] NodesPositions;


    public int getGridSize()
    {
        return grid_size;
    }
    public ConectPositions[] getAllNodes()
    {
        return NodesPositions;
    }

    public int getObectivesAmount()
    {
        return NodesPositions.Length;
    }

    public Color getBGColor()
    {
        return bgColor;
    }
}
