//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using DG.Tweening;
//using UnityEngine.UI;
//using Sirenix.OdinInspector;



//OBSOLETE


//public class EnergyBar : MonoBehaviour
//{

//    [SerializeField] private Slider slider;
//    [SerializeField] private TextMeshProUGUI sliderText;
//    [SerializeField] private Image sliderImage;
//    [SerializeField] private Image backgroundImage;

//    [SerializeField] private List<Color> backgroundColors;
//    [SerializeField] private List<Color> sliderColors;




//    [Button("Update Slider")]
//    public void UpdateEnergy(int energyNum)
//    {
//        slider.value = energyNum;

//        if (energyNum >= 0)
//        {
//            backgroundImage.color = backgroundColors[0];
//            sliderImage.color = sliderColors[0];
//            sliderText.text = energyNum.ToString();
//        }
//        if (energyNum < 0)
//        {
//            backgroundImage.color = backgroundColors[1];
//            sliderImage.color = sliderColors[1];
//            sliderText.text = (energyNum * -1).ToString();
//        }
//    }
//}
