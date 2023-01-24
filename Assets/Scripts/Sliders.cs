using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class Sliders : MonoBehaviour
{
    private string sliderName;
    private Slider slider;
    private CharacterCustomisation characterCustomisation;

    void Start()
    {
        sliderName = this.name.Replace("Slider","");
        slider = GetComponent<Slider>();
        characterCustomisation = GetComponentInParent<CharacterCustomisation>();
        

        slider.onValueChanged.AddListener(value => characterCustomisation.ChangeValue(sliderName, value));
    }
}
