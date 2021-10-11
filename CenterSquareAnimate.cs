using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CenterSquareAnimate : MonoBehaviour
{

    [SerializeField] private List<Transform> xLocations;
    [SerializeField] private List<Transform> yLocations;

    [SerializeField] private Transform xBar;
    [SerializeField] private Transform yBar;



    private Action completeAnimation;

    private List<Vector3> xPath;
    private List<Vector3> yPath;


    private int xStart;
    private int yStart;
    private int xFinish;
    private int yFinish;

    public void AnimateSquare(int finishPoint, Action completeAnimation)
    {
        this.completeAnimation = completeAnimation;
        DetermineFinishPoint(finishPoint);


        xPath = MakePath(xLocations, yFinish);
        yPath = MakePath(yLocations, xFinish);


        xBar.DOLocalPath(xPath.ToArray(), 2f);
        yBar.DOLocalPath(yPath.ToArray(), 2.5f).OnComplete(DoneWithAnimation);

    }

    private List<Vector3> MakePath(List<Transform> transformList, int endPoint)
    {
        int index = 0;
        List<Vector3> newPath = new List<Vector3>();
        for (int i = 0; i < 5; i++)
        {
            for (int m = 0; m < 3; m++)
            {
                index += 1;
                newPath.Add(transformList[m].localPosition);

                if (index >= 9 && m == endPoint - 1)
                    return newPath;
            }
        }
        return newPath;
    }

    private void DoneWithAnimation()
    {
        completeAnimation();
    }

    private void DetermineFinishPoint(int finishNum)
    {
        Debug.Log("Selected Cell: " + finishNum);
        switch (finishNum)
        {
            default:
                xFinish = 1;
                yFinish = 1;
                break;

            case 1:
                xFinish = 1;
                yFinish = 1;
                break;
            case 2:
                xFinish = 1;
                yFinish = 2;
                break;
            case 3:
                xFinish = 1;
                yFinish = 3;
                break;
            case 4:
                xFinish = 2;
                yFinish = 1;
                break;
            case 5:
                xFinish = 2;
                yFinish = 2;
                break;
            case 6:
                xFinish = 2;
                yFinish = 3;
                break;
            case 7:
                xFinish = 3;
                yFinish = 1;
                break;
            case 8:
                xFinish = 3;
                yFinish = 2;
                break;
            case 9:
                xFinish = 3;
                yFinish = 3;
                break;
        }
    }

    private int RandumNum(int startNum, int endNum)
    {
        return UnityEngine.Random.Range(startNum, endNum + 1);
    }
}
