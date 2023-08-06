using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeAScript : MonoBehaviour, IEnemy, IDamageable
{
    [Min(0)]
    [SerializeField] float maxHealth;
    float health_now;
    bool isDead = false;

    [Space(20)]
    [Min(0)]
    [SerializeField] float fireRate_Seconds = 5;



    private void Awake()
    {
        health_now = maxHealth;
    }

    void Update()
    {

    }


    public void TakeDamage(float amount)
    {
        health_now -= amount;   //Subtracts the damage amount to the current health

        CheckDeath();   //Checks if this enemy is dead
    }

    public void CheckDeath()
    {
        isDead = health_now <= 0;

        //What to do when the enemy dies
        if(isDead)
        {
            ;
        }
    }
}
