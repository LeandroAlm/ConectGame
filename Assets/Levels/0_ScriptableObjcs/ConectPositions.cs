using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ConenctPoint")]
public class ConectPositions : ScriptableObject
{
    [SerializeField] Color NodeColor, glowColor;
    [SerializeField] Vector2 position_1, position_2;
    private bool is_finished = false;

    public Color getColorNode()
    {
        return NodeColor;
    }
    public Color getGlowColor()
    {
        return glowColor;
    }


    public Vector2 getPosition1()
    {
        return position_1;
    }

    public Vector2 getPosition2()
    {
        return position_2;
    }

    public bool isFinished()
    {
        return is_finished;
    }

    public void setAsUnfinished()
    {
        is_finished = false;
    }

    public void setAsFinished()
    {
        is_finished = true;
    }
}
