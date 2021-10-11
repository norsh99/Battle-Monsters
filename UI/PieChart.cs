using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Threading.Tasks;
using DG.Tweening;

public class PieChart : MonoBehaviour
{

    //[SerializeField] private List<float> values;
    //[SerializeField] private List<Color> colors;
    //[SerializeField] private Image slicePrefab;
    [SerializeField] private List<PieSlice> pieSlices;
    [SerializeField] private List<Color> wedgeColors;
    [SerializeField] private Image outerCircle;
    [SerializeField] private Image innerCircle;



    public List<AttackMove> currentAttackMoveList;

    [Button("Creat Chart")]
    private void MakeChart()
    {
        float total = 0f;
        float zRotation = 0f;


        for (int i = 0; i < pieSlices.Count; i++)
        {
            total += pieSlices[i].value;
        }


        for (int i = 0; i < pieSlices.Count; i++)
        {
            pieSlices[i].backgroundImage.color = pieSlices[i].colorPick;
            pieSlices[i].backgroundImage.fillAmount = pieSlices[i].value / total;
            pieSlices[i].transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, zRotation));

            float textRotation = (pieSlices[i].backgroundImage.fillAmount * -180f);
            pieSlices[i].textTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, textRotation));
            pieSlices[i].numTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, textRotation));


            zRotation -= pieSlices[i].backgroundImage.fillAmount * 360f;


            string pieSliceNum = pieSlices[i].num.ToString();
            if (pieSlices[i].attackMove != null)
            {
                if (pieSlices[i].attackMove.attackColor == AttackColor.Red)
                {
                    pieSliceNum = "";
                }
            }

            pieSlices[i].GetComponent<PieSlice>().UpdateText(pieSlices[i].backgroundImage.fillAmount, pieSlices[i].title, pieSliceNum);
        }
    }

    private List<AttackMove> CreateNewAttackMoveList(List<AttackMove> theList)
    {
        List<AttackMove> newList = new List<AttackMove>();
        for (int i = 0; i < theList.Count; i++)
        {
            AttackMove newMove = new AttackMove(theList[i]);
            newList.Add(newMove);
        }
        return newList;
    }

    public void LoadIn(Monster monster, List<AttackMove> attackMoves)
    {

        innerCircle.color = GetSpiritColor(monster.spirit);
        outerCircle.color = GetSpiritColor(monster.spirit);


        currentAttackMoveList = CreateNewAttackMoveList(attackMoves);


        for (int i = 0; i < pieSlices.Count; i++)
        {
            if (i < attackMoves.Count)
            {
                pieSlices[i].attackMove = attackMoves[i];
                pieSlices[i].title = attackMoves[i].title;
                pieSlices[i].value = attackMoves[i].wedgeSize;
                pieSlices[i].num = attackMoves[i].damage;

                pieSlices[i].colorPick = GetWedgeColor(attackMoves[i].attackColor);
            }
            else
            {
                pieSlices[i].value = 0;
            }
        }

        MakeChart();
    }

    private Color GetSpiritColor(Spirit spirit)
    {
        if (spirit == Spirit.Dark)
        {
            return wedgeColors[5];
        }
        else if (spirit == Spirit.Fairy)
        {
            return wedgeColors[6];
        }
        return wedgeColors[7];
    }


    private Color GetWedgeColor(AttackColor attackColor)
    {
        if (attackColor == AttackColor.White)
        {
            return wedgeColors[(int)ColorSelect.white];
        }
        else if (attackColor == AttackColor.Gold)
        {
            return wedgeColors[(int)ColorSelect.gold];
        }
        else if (attackColor == AttackColor.Purple)
        {
            return wedgeColors[(int)ColorSelect.purple];
        }
        else if (attackColor == AttackColor.Red)
        {
            return wedgeColors[(int)ColorSelect.red];
        }
        else if (attackColor == AttackColor.Blue)
        {
            return wedgeColors[(int)ColorSelect.blue];
        }
        else if (attackColor == AttackColor.Green)
        {
            return wedgeColors[(int)ColorSelect.green];
        }
        else if (attackColor == AttackColor.Pink)
        {
            return wedgeColors[(int)ColorSelect.pink];
        }
        return wedgeColors[(int)ColorSelect.white];
    }



    public AttackMove GetAttackMove(int pieNum) 
    {
        int upperNum = 0;
        foreach (AttackMove attackMove in currentAttackMoveList)
        {  
            upperNum += attackMove.wedgeSize;
            int lowerNum = upperNum - attackMove.wedgeSize;
            if (pieNum < upperNum && pieNum >= lowerNum)
            {
                return attackMove;
            }
        }
        return null;
    }



    public void MakeEntireWheelMiss()
    {
        foreach (AttackMove attackMove in currentAttackMoveList)
        {
            attackMove.attackColor = AttackColor.Red;
            attackMove.damage = 0;
        }
    }


    private async Task ChangePieSliceToColor(PieSlice slice, AttackColor attackColor)
    {
        float animationTime = .2f;
        Color currentColor = slice.colorPick;
        Color newColor = GetWedgeColor(attackColor);

        slice.colorPick = GetWedgeColor(attackColor);

        await DOTween.To(() => slice.backgroundImage.color, x => slice.backgroundImage.color = x, newColor, animationTime).AsyncWaitForCompletion();
        Debug.Log("1");
        await DOTween.To(() => slice.backgroundImage.color, x => slice.backgroundImage.color = x, currentColor, animationTime).AsyncWaitForCompletion();
        Debug.Log("2");
        await DOTween.To(() => slice.backgroundImage.color, x => slice.backgroundImage.color = x, newColor, animationTime).AsyncWaitForCompletion();
        Debug.Log("3");
        await DOTween.To(() => slice.backgroundImage.color, x => slice.backgroundImage.color = x, currentColor, animationTime).AsyncWaitForCompletion();
        Debug.Log("4");
        await DOTween.To(() => slice.backgroundImage.color, x => slice.backgroundImage.color = x, newColor, animationTime).AsyncWaitForCompletion();
        Debug.Log("5");

        await Task.Delay(500);
    }

    public async Task EnergyBoostUpgrade(Spirit spirit)
    {
        switch (spirit)
        {
            case Spirit.None:
                break;
            case Spirit.Normal:
                break;
            case Spirit.Dark:
                await ChangeAllWhiteToColor(AttackColor.Green);
                break;
            case Spirit.Fairy:
                await ChangeAllWhiteToColor(AttackColor.Pink);
                break;
            default:
                break;
        }
    }

    private async Task ChangeAllWhiteToColor(AttackColor attackColor)
    {
        for (int i = 0; i < pieSlices.Count; i++)
        {
            if (pieSlices[i].attackMove.attackColor == AttackColor.White)
            {
                await ChangePieSliceToColor(pieSlices[i], attackColor);
                pieSlices[i].attackMove.attackColor = AttackColor.Pink;
            }
        }
    }






    private enum ColorSelect { white = 0, gold = 1, purple = 2, red = 3, blue = 4, green = 5, pink = 6 }

}
