using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeAScript : MonoBehaviour, IEnemy, IDamageable
{
    ObjectPoolingScript poolingScr;
    [SerializeField] string typeA_tag = "Type A Enemy";
    [SerializeField] string damagePart_tag = "Damage particles";
    [SerializeField] string deathPart_tag = "Enemy Death particles";

    [Min(0)]
    [SerializeField] int maxHealth = 5;
    int health_now;
    bool isDead = false;
    [SerializeField] int invSec = 1;
    CustomTimer invTimer = new CustomTimer();
    bool isInvincible = true;


    [Space(20)]
    [Min(0)]
    [SerializeField] float fireRate_Seconds = 5;
    [Min(0)]
    [SerializeField] float secsInScreen = 45;
    CustomTimer inScreenTimer = new CustomTimer();


    [Header("—— Scraps ——")]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] int scrapsDroppedWhenDead = 25;


    [Header("—— Feedback ——")]
    [SerializeField] ParticleSystem
                     .MinMaxGradient deathPart_colors = new ParticleSystem
                                                            .MinMaxGradient(Color.black, Color.black);
    ParticleSystem deathTypeA_part;




    private void Awake()
    {
        ResetHealth();

        poolingScr = FindObjectOfType<ObjectPoolingScript>();

        inScreenTimer.maxTime = secsInScreen;
        //inScreenTimer.OnTimerDone_event.AddListener(() => _______());
        invTimer.maxTime = invSec;
        invTimer.OnTimerDone_event.AddListener(() => {isInvincible = false; });


        //Changes the fire-rate of the enemy
        GetComponentInChildren<ShootingScript>().SetFireRate(fireRate_Seconds);
    }


    private void OnEnable()
    {
        ResetHealth();
    }


    void Update()
    {
        //---Timer---//
        inScreenTimer.AddTimeToTimer();
        
        if (isInvincible)
        {
            invTimer.AddTimeToTimer();
        }
    }


    void ResetHealth()
    {
        health_now = maxHealth;
    }


    #region Damage & Death

    public void TakeDamage(int amount)
    {
        if (!isInvincible)    //If the enemy CAN take damage...
        {
            health_now -= amount;   //Subtracts the damage amount to the current health

            #region Feedback

            //Plays the damaged particles
            //(only when it's not dead)
            if (health_now > 0)
            {
                GameObject dmgObj;
                ParticleSystem dmgPart;
                dmgObj = poolingScr.TakeObjectFromPool(damagePart_tag, transform.position, Quaternion.identity);
                dmgPart = dmgObj.GetComponent<ParticleSystem>();
                dmgPart.gameObject.transform.position = transform.position;
                dmgPart.Play();
            }

            #endregion

            CheckDeath();   //Checks if this enemy is dead


            isInvincible = true;
        }
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
            //to the one of the enemy
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
