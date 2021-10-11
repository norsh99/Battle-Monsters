using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteIconTrackUnit : MonoBehaviour
{
    //public GameObject unit;
    public Unit unit;
    public Camera cameraToLookAt;
    public RectTransform transformRect;

    void Start()
    {
        //cameraToLookAt = Camera.main;
    }

    private void Update()
    {


        Vector3 screenPoint = cameraToLookAt.WorldToScreenPoint(unit.transform.position);
        Vector2 result;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), screenPoint, cameraToLookAt, out result);
        transformRect.anchoredPosition = result + new Vector2(0,30f);


    }

}
