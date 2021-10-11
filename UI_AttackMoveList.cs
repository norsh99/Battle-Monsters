using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UI_AttackMoveList : MonoBehaviour
{
    [SerializeField] private List<UI_AttackMoveDisplayItem> items;




    public void LoadInfo(List<AttackMove> attackMoveList)
    {
        
        if (attackMoveList != null)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (i < attackMoveList.Count)
                {
                    items[i].LoadInfo(attackMoveList[i]);
                    items[i].gameObject.SetActive(true);
                }
                else
                {
                    items[i].gameObject.SetActive(false);
                }
            }
        }
        gameObject.SetActive(false);
        transform.DOScale(Vector3.one, .04f).OnComplete(SetToTrue);
        transform.DOScale(Vector3.one, .08f).OnComplete(SetToFalse);
        transform.DOScale(Vector3.one, .12f).OnComplete(SetToTrue);


    }
    private void SetToFalse()
    {
        gameObject.SetActive(false);
    }
    private void SetToTrue()
    {
        gameObject.SetActive(true);
    }
}
