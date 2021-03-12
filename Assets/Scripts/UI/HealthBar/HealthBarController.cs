using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    public PlayerAttributes playerAttributes;
    private Slider _slider;
    
    // Start is called before the first frame update
    void Start()
    {
        _slider = GetComponent<Slider>();
        _slider.maxValue = playerAttributes.initialHealth;
        _slider.value = _slider.maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnHealthEvent()
    {
        print("HEALTH BAR: SOMETHISN");
        _slider.value = playerAttributes.GetHealth();
    }
}
