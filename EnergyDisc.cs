using Shapes;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

public class EnergyDisc : MonoBehaviour
{
    [SerializeField] private List<Disc> discs;
    [SerializeField] private List<Color> colors;


    private float currentAmount;




    public async Task LoadIn(float newAmount, float maxAmount)
    {
        float newDivide = newAmount / maxAmount;
        float newDegrees = 360f * newDivide;
        float convertToRadians = newDegrees * 3.14f / 180f;

        //if (transform.localScale.x == 1)
        //{
            await ScaleUp(true);
        //}

        if (newAmount > currentAmount)
        {
            discs[1].Color = colors[2];

            await DOTween.To(() => discs[1].AngRadiansEnd, x => discs[1].AngRadiansEnd = x, convertToRadians, .5f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
            await DOTween.To(() => discs[0].AngRadiansEnd, x => discs[0].AngRadiansEnd = x, convertToRadians, .5f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
        }
        else
        {
            discs[1].Color = colors[1];
            await DOTween.To(() => discs[0].AngRadiansEnd, x => discs[0].AngRadiansEnd = x, convertToRadians, .5f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
            await DOTween.To(() => discs[1].AngRadiansEnd, x => discs[1].AngRadiansEnd = x, convertToRadians, .5f).SetEase(Ease.InQuad).AsyncWaitForCompletion();
        }
        currentAmount = newAmount;
    }

    public async Task ScaleUp(bool scaleUp)
    {
        if (scaleUp)
        {
            await transform.DOScale(1f, .3f).AsyncWaitForCompletion();
        }
        else
        {
            await transform.DOScale(0f, .3f).AsyncWaitForCompletion();
        }
    }



    [Button("Activate Load In")]
    public void TestActivate(float newAmount, float maxAmount)
    {
        _ = LoadIn(newAmount, maxAmount);
    }




}
