using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MiddleRingCaptureZone : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ringSprite;
    [SerializeField] private List<Color> playerColors;

    private void Start()
    {
        ringSprite.color = playerColors[2];
    }

    public void AssignNewColor(int playerNum)
    {
        if (playerNum == 1)
        {
            ringSprite.color = playerColors[0];
        }
        else if (playerNum == 2)
        {
            ringSprite.color = playerColors[1];
        }
        else
        {
            ringSprite.color = playerColors[2];
        }
    }

}
