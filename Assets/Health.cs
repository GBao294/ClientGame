using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] public float health = 0f;
    [SerializeField] public float maxHealth = 100f;
    [SerializeField] public Slider healthSlider;
    [SerializeField] public GameObject FloatingText;
    // Start is called before the first frame update
    void Start()
    {
        //healthSlider.value = health;
        healthSlider.maxValue = maxHealth;
    }

    // Update is called once per frame
    public void UpdateHealth(float mod)
    {
        health += mod;
       
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        else if (health <= 0f)
        {
            health = 0f;
            healthSlider.value = health;
          
        }
        Debug.Log(health);
    }
  
    private void OnGUI()
    {
        float t = Time.deltaTime / 1f;
        healthSlider.value = Mathf.Lerp(healthSlider.value, health, t);
    }
}
