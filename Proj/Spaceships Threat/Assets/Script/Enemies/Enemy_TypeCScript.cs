using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeCScript : MonoBehaviour
{
    ObjectPoolingScript poolingScr;
    [SerializeField] string typeB_tag = "Enemy type C";
    [SerializeField] string deathPart_tag = "Enemy Death particles";

    [Min(0)]
    [SerializeField] int maxHealth = 10;
    int health_now;
    bool isDead = false;


    [Space(20)]
    PlayerStatsManager playerStatsManag;

    [Min(1)]
    [SerializeField] float movingVelocity = 1.5f;
    Vector3 positionToGo;


    [Space(20)]
    [Min(0)]
    [SerializeField] float fireRate_Seconds = 5;
    CustomTimer shootTimer = new CustomTimer();


    [Header("—— Scraps ——")]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] int scrapsDroppedWhenDead = 75;


    [Header("—— Feedback ——")]
    [SerializeField]
    ParticleSystem.MinMaxGradient deathPart_colors = new ParticleSystem
                                                         .MinMaxGradient(Color.black, Color.black);
    ParticleSystem deathTypeC_part;




    private void Awake()
    {
        health_now = maxHealth;

        poolingScr = FindObjectOfType<ObjectPoolingScript>();
        playerStatsManag = FindObjectOfType<PlayerStatsManager>();


        shootTimer.maxTime = fireRate_Seconds;
        //shootTimer.OnTimerDone_event.AddListener(() => _______());
    }


    void Update()
    {
        //Gets the player XY position
        //and moves towards the player position
        positionToGo = new Vector3(playerStatsManag.transform.position.x,
                                   playerStatsManag.transform.position.y,
                                   transform.position.z);

        transform.position = Vector3.MoveTowards(transform.position,
                                                    positionToGo,
                                                    Time.deltaTime * movingVelocity);
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
            poolingScr.ReAddObject(typeB_tag, gameObject);    //Re-adds the enemy to the pool


            //Adds the scraps to the player
            stats_SO.AddTempScraps(scrapsDroppedWhenDead);



            #region Feedback

            //Gets the death particle randomly
            //from the enemies' death part. pool
            GameObject poolPart = poolingScr.TakeObjectFromPool(deathPart_tag,
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
            deathTypeC_part.Play();

            #endregion
        }
    }

    #endregion



    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75f);
        Gizmos.DrawCube(positionToGo, Vector3.one * 0.5f);
        Gizmos.color = new Color(0, 0, 1f, 0.75f);
        Gizmos.DrawSphere(transform.position, 0.2f);

        //Checks if it's arrived in the new position
        float dist = Vector3.Distance(transform.position, positionToGo);
        bool isInPos = dist <= 0.05f;

        //Line from position to the new one (when goes to the player position)
        //or a simple vertical line (when it's arrived)
        Gizmos.color = isInPos ? Color.blue : Color.red;
        Gizmos.DrawLine(transform.position,
                        isInPos ? transform.position + Vector3.up : positionToGo);
    }

    #endregion


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        deathPart_colors.mode = ParticleSystemGradientMode.TwoColors;
    }

    #endregion
}
