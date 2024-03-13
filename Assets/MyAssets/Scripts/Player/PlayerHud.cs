using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] public float currentHealth;
    [SerializeField] public float maxHealth;

    [SerializeField] public Transform health;

    public void SetHP(float maxHP)
    {
        maxHealth = maxHP;
        currentHealth = maxHP;
    }

    public void TakeDamage(float damageHP)
    {
        currentHealth -= damageHP;
    }

    public void UpdateHP()
    {
        health.localScale = new Vector3((currentHealth/maxHealth), 1f);
    }
}
