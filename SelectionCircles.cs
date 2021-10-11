using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Shapes;

public class SelectionCircles : MonoBehaviour
{
    [SerializeField] private List<Transform> circlesList;

    public void TurnOffCircles()
    {
        for (int i = 0; i < circlesList.Count; i++)
        {
            circlesList[i].DOScale(0, .4f);
        }
    }

    public void TurnOnSelectedCircles(List<Node> nodeList, Color colorSelection, bool isDuelMap)
    {
        if (isDuelMap)
        {   
            for (int i = 0; i < nodeList.Count; i++)
            {
                circlesList[i].GetComponent<SpriteRenderer>().color = colorSelection;
                circlesList[i].transform.position = nodeList[i].transform.position;
                circlesList[i].DOScale(0.5401938f, .3f).SetEase(Ease.InOutBounce);
            }
        }
        else
        {
            for (int i = 0; i < nodeList.Count; i++)
            {
                circlesList[i].GetComponent<Disc>().Color = colorSelection;
                circlesList[i].transform.position = nodeList[i].transform.position;
                circlesList[i].DOScale(1, .3f).SetEase(Ease.InOutBounce);
            }
        }
       
    }


}
