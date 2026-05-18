using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    float currHealth;
    float maxHealth;
    [SerializeField] Character character;
    [SerializeField] Slider healthBar;
    [SerializeField] Vector3 worldOffset = new Vector3(0f, 1f, 0f);
    void Update()
    {
        if ( healthBar == null || Camera.main == null)
            return;

        transform.position = Camera.main.WorldToScreenPoint(character.transform.position + worldOffset);
        if (character.maxHealth > 0f)
            healthBar.value = character.currHealth / character.maxHealth;
    }

    public void Init(Character c)
    {
        character = c;
    }
}
