using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidScript : MonoBehaviour, IDamageable
{
    ObjectPoolingScript poolingScr;
    [SerializeField] string asteroid_tag = "Asteroids";

    [Min(0)]
    [SerializeField] int maxHealth = 10;
    int health_now;
    bool isDestroyed = false;

    [Header("—— Scraps ——")]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] int scrapsDroppedWhenDead = 40;



    void Update()
    {

    }


    #region Damage & Death

    public void TakeDamage(int amount)
    {
        health_now -= amount;   //Subtracts the damage amount to the current health

        CheckDeath();   //Checks if this enemy is dead
    }

    public void CheckDeath()
    {
        isDestroyed = health_now <= 0;

        //What to do when the asteroid is destroyed
        if (isDestroyed)
        {
            poolingScr.ReAddObject(asteroid_tag, gameObject);    //Re-adds the asteroid to the pool


            //Adds the scraps to the player
            stats_SO.AddTempScraps(scrapsDroppedWhenDead);



            //TODO: Feedback
            #region Feedback

            /*
            //Gets the death particle randomly
            //from the enemies' death part. pool
            GameObject poolPart = poolingScr.TakeObjectFromPool(asteroid_tag,
                                                                transform.position,
                                                                Quaternion.identity);
            deathTypeC_part = poolPart.GetComponent<ParticleSystem>();


            //Changes the death particle's color
            //to the one of the enemy
            //and plays it
            ParticleSystem.MainModule main_death = deathTypeC_part.main;

            main_death.startColor = new ParticleSystem.MinMaxGradient(deathPart_colors.colorMin,
                                                                      deathPart_colors.colorMax);

            deathTypeC_part.gameObject.SetActive(true);
            deathTypeC_part.Play();*/

            #endregion
        }
    }

    #endregion
}
