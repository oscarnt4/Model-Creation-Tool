using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Buttons : MonoBehaviour
{
    private string buttonName;
    private Button button;
    private CharacterCustomisation characterCustomisation;

    void Start()
    {
        buttonName = this.name.Replace("Button","");
        button = GetComponent<Button>();
        characterCustomisation = GetComponentInParent<CharacterCustomisation>();
        

        button.onClick.AddListener(() => characterCustomisation.ActivateFunction(buttonName));
    }
}
