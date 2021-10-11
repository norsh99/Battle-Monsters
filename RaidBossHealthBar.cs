using Shapes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading.Tasks;

public class RaidBossHealthBar : MonoBehaviour
{
    [SerializeField] private List<Rectangle> rectangles;
    [SerializeField] private GameObject agitatedOutline;

    private int currentHealth;


    public async Task UpdateHealth(float newHealth, float maxHealth)
    {
        float newMath = newHealth / maxHealth;

        await rectangles[0].transform.DOScaleX(newMath, .3f).AsyncWaitForCompletion();
        await rectangles[1].transform.DOScaleX(newMath, .3f).AsyncWaitForCompletion();
    }

    public void SetAgitatedOutline(bool isAgitated)
    {
        agitatedOutline.SetActive(isAgitated);
    }





}
