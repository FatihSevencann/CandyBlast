using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Definations : MonoBehaviour
{
    public TextMeshProUGUI scoreText, howLeftMove,nextPopupText;
    public static Definations instance;
    public Slider slider;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            return;

        }
    }
}
