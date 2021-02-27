using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineAnimationController : MonoBehaviour
{
    [SerializeField] GameObject SpawObj;
    private List<GameObject> LineNode;
    private int animation_id, blink_count;
    private float animation_tick;
    private Color glowColor, originalColor;

    private void Start()
    {
        LineNode = new List<GameObject>();
        animation_id = 0;
    }

    void Update()
    {
        AnimationUpdate();
    }

    public void playLineAnimation(int CP_id, Color _glowColor)
    {
        int counter = 0;

        if(animation_id > 0)
        {
            for (int i = 0; i < LineNode.Count; i++)
            {
                LineNode[i].GetComponent<Image>().color = originalColor;
            }
        }

        ForcePlayAnimation();


        for (int i = 0; i < SpawObj.transform.childCount; i++)
        {
            if(SpawObj.transform.GetChild(i).name.Contains("CP_" + CP_id))
            {
                SpawObj.transform.GetChild(i).gameObject.transform.name = "CP_" + CP_id + "_" + counter;
                counter++;

                LineNode.Add(SpawObj.transform.GetChild(i).gameObject);
            }
        }

        originalColor = LineNode[0].GetComponent<Image>().color;
        glowColor = _glowColor;
        
        for (int i = 0; i < LineNode.Count; i++)
        {
            LineNode[i].GetComponent<Image>().color = glowColor;
        }

        animation_id = 1;
    }

    public void ForcePlayAnimation()
    {
        LineNode.Clear();
        blink_count = 0;
        animation_tick = 0.0f;
        animation_id = 0;
    }

    private void AnimationUpdate()
    {
        if (animation_id >= 1)
        {
            animation_tick += Time.deltaTime;
            if (animation_tick >= 0.2f)
            {
                animation_tick = 0.0f;

                if (animation_id == 1)
                {
                    for (int i = 0; i < LineNode.Count; i++)
                    {
                        LineNode[i].GetComponent<Image>().color = originalColor;
                    }
                    animation_id = 2;
                }
                else
                {
                    blink_count++;
                    if (blink_count >= 3)
                    {
                        animation_id = 0;
                    }
                    else
                    {
                        for (int i = 0; i < LineNode.Count; i++)
                        {
                            LineNode[i].GetComponent<Image>().color = glowColor;
                        }
                        animation_id = 1;
                    }
                }
            }
        }
    }
}
