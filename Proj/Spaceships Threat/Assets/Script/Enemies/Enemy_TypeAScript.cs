using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeAScript : MonoBehaviour, IEnemy, IDamageable
{
    ObjectPoolingScript poolingScr;
    [SerializeField] string typeA_tag = "Enemy type A";

    [Min(0)]
    [SerializeField] float maxHealth;
    float health_now;
    bool isDead = false;


    [Space(20)]
    [Min(0)]
    [SerializeField] float fireRate_Seconds = 5;
    CustomTimer shootTimer = new CustomTimer();
    [Min(0)]
    [SerializeField] float secsInScreen = 45;
    CustomTimer inScreenTimer = new CustomTimer();


    [Header("—— Scraps ——")]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] int scrapsDroppedWhenDead = 25;



    private void Awake()
    {
        health_now = maxHealth;

        shootTimer.maxTime = fireRate_Seconds;
        //shootTimer.OnTimerDone_event.AddListener(() => _______());
        inScreenTimer.maxTime = secsInScreen;
        //inScreenTimer.OnTimerDone_event.AddListener(() => _______());
    }

    void Update()
    {
        //---Timer---//
        shootTimer.AddTimeToTimer();
        inScreenTimer.AddTimeToTimer();
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
            poolingScr.ReAddObject(typeA_tag, gameObject);    //Re-adds the enemy to the pool


            //Adds the scraps to the player
            stats_SO.AddTempScraps(scrapsDroppedWhenDead);



            #region Feedback

            #endregion
        }
    }
}
