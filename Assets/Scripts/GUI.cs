using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public Slider health;
    public Slider experience;
    
    public void SetHealth(float value) => health.value = value;
    public void SetExperience(float value) => experience.value = value;
}
