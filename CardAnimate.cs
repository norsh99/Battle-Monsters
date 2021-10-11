using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CardAnimate : MonoBehaviour
{
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Vector3 normalScale;

    [SerializeField] private float animateSpeed = .5f;

    private Action onCompleteAction;

    public void ActivateCard(Action onComplete = null)
    {
        onCompleteAction = onComplete;

        AnimateToLocation(startPosition, endPosition, animateSpeed);
        gameObject.SetActive(true);
    }


    public void ReverseCardOut()
    {
        onCompleteAction = null;

        AnimateToLocation(endPosition, startPosition, animateSpeed);
    }


    public void GoCenter()
    {
        onCompleteAction = null;

        AnimateToLocation(endPosition, new Vector3(0, 0, 5f), 1.5f);
        transform.DOScale(transform.localScale.x * 1.5f, 1.5f);
    }

    public void ReduceScale()
    {
        onCompleteAction = null;


        transform.DOScale(0, 1.0f);
    }










    private void AnimateToLocation(Vector3 startPos, Vector3 endPos, float animateSpeed)
    {

        transform.localPosition = startPos;
        transform.localScale = normalScale;
        transform.DOLocalMove(endPos, animateSpeed).SetEase(Ease.InOutSine).OnComplete(OnComplete);
    }


    private void OnComplete()
    {
        if (onCompleteAction != null)
        {
            onCompleteAction();
        }
    }
}
