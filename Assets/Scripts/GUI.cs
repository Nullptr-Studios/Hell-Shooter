using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public Slider health;
    public Slider experience;
    public TextMeshProUGUI levelPoints;
    
    public void SetHealth(float value) => health.value = value;
    public void SetExperience(float value) => experience.value = value;
    public void SetLevelPoints(int value) => levelPoints.text = value.ToString();
}
