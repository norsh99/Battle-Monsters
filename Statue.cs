using EPOOutline;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Statue : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRender;
    [SerializeField] private Transform model;
    [SerializeField] private Outlinable outline;

    [SerializeField] private List<GameObject> effects; //1: Frozen
    [SerializeField] private GameObject useEnergyEffect;






    public Outlinable GetOutline() { return outline; }

    public void RotateModel(bool frontFacing)
    {
        if (frontFacing)
        {
            model.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            model.eulerAngles = new Vector3(0, 180, 0);

        }
    }


    public void AssignPlayerColor(Material material)
    {
        meshRender.material = material;
    }




    //EFFECTS
    public void TurnOffAllEffects()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            effects[i].SetActive(false);
        }
    }


    private void EnableEffect(int index)
    {
        for (int i = 0; i < effects.Count; i++)
        {
            if (i == index-1)
            {
                effects[i].SetActive(true);
            }
            else
            {
                effects[i].SetActive(false);
            }
        }
    }
    public void EnableFrozen()
    {
        EnableEffect(1);
    }


    public async void EnableEnergyEffect(bool enableEffect)
    {
        useEnergyEffect.SetActive(enableEffect);
        await Task.Delay(6000);
        useEnergyEffect.SetActive(false);

    }
}
