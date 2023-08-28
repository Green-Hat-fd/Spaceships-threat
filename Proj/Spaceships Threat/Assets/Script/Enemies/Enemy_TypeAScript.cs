using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeAScript : MonoBehaviour, IEnemy, IDamageable
{
    ObjectPoolingScript poolingScr;
    [SerializeField] string typeA_tag = "Enemy type A";
    [SerializeField] string deathPart_tag = "Enemy Death particles";

    [Min(0)]
    [SerializeField] int maxHealth;
    int health_now;
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


    [Header("—— Feedback ——")]
    [SerializeField] ParticleSystem.MinMaxGradient deathPart_colors = new ParticleSystem
                                                                          .MinMaxGradient(Color.black, Color.black);
    ParticleSystem deathTypeA_part;




    private void Awake()
    {
        health_now = maxHealth;

        poolingScr = FindObjectOfType<ObjectPoolingScript>();


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


    #region Damage & Death

    public void TakeDamage(int amount)
    {
        health_now -= amount;   //Subtracts the damage amount to the current health

        CheckDeath();   //Checks if this enemy is dead
    }

    public void CheckDeath()
    {
        isDead = health_now <= 0;

        //What to do when the enemy dies
        if (isDead)
        {
            poolingScr.ReAddObject(typeA_tag, gameObject);    //Re-adds the enemy to the pool


            //Adds the scraps to the player
            stats_SO.AddTempScraps(scrapsDroppedWhenDead);



            #region Feedback

            //Gets the death particle randomly
            //from the enemies' death part. pool
            GameObject poolPart = poolingScr.TakeObjectFromPool(deathPart_tag,
                                                                transform.position,
                                                                Quaternion.identity);
            deathTypeA_part = poolPart.GetComponent<ParticleSystem>();


            //Changes the death particle's color
            //to the one of the enemy A
            //and plays it
            ParticleSystem.MainModule mainMod = deathTypeA_part.main;

            mainMod.startColor = new ParticleSystem.MinMaxGradient(deathPart_colors.colorMin,
                                                                   deathPart_colors.colorMax);

            deathTypeA_part.gameObject.SetActive(true);
            deathTypeA_part.Play();

            #endregion
        }
    }

    #endregion



    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        deathPart_colors.mode = ParticleSystemGradientMode.TwoColors;
    }

    #endregion
}
