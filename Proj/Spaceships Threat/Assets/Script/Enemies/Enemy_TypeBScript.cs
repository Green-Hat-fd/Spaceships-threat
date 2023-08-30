using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeBScript : MonoBehaviour, IEnemy, IDamageable
{
    ObjectPoolingScript poolingScr;
    [SerializeField] string typeB_tag = "Enemy type B";
    [SerializeField] string deathPart_tag = "Enemy Death particles";

    [Min(0)]
    [SerializeField] int maxHealth = 10;
    int health_now;
    bool isDead = false;


    [Space(20)]
    PlayerMovemRB playerMovScript;
    [Min(0)]
    [SerializeField] Vector2 changePos_timeRange = new Vector2(10, 15);
    CustomTimer positionTimer = new CustomTimer();
    
    [Min(1)]
    [SerializeField] float movingVelocity = 10;
    bool isInPosition = false;
    Vector3 positionToGo;

    const float MIN_DISTANCE = 0.05f;


    [Space(20)]
    [Min(0)]
    [SerializeField] float fireRate_Seconds = 5;
    CustomTimer shootTimer = new CustomTimer();


    [Header("—— Scraps ——")]
    [SerializeField] PlayerStatsSO_Script stats_SO;
    [SerializeField] int scrapsDroppedWhenDead = 50;


    [Header("—— Feedback ——")]
    [SerializeField] ParticleSystem
                     .MinMaxGradient deathPart_colors = new ParticleSystem
                                                            .MinMaxGradient(Color.black, Color.black);
    ParticleSystem deathTypeB_part;



    private void Awake()
    {
        health_now = maxHealth;

        poolingScr = FindObjectOfType<ObjectPoolingScript>();
        playerMovScript = FindObjectOfType<PlayerMovemRB>();
        

        shootTimer.maxTime = fireRate_Seconds;
        //shootTimer.OnTimerDone_event.AddListener(() => _______());
        ChangePosition();
        positionTimer.OnTimerDone_event.AddListener(() => ChangePosition());
    }


    void Update()
    {
        //Checks if it's arrived in the new position
        float dist = Vector3.Distance(transform.position, positionToGo);
        isInPosition = dist <= MIN_DISTANCE;


        if (isInPosition)
        {
            //---Timer---//
            positionTimer.AddTimeToTimer();

            if (positionTimer.CheckIsOver())
                positionTimer.Restart();    //Restarts the timer when arrived to the position
        }
        else
        {
            //Moves to the new chosen position
            transform.position = Vector3.MoveTowards(transform.position,
                                                     positionToGo,
                                                     Time.deltaTime * movingVelocity);
        }
    }

    void SetActiveGuns(bool value)
    {

    }


    #region Change random position

    void ChangePosition()
    {
        Vector2 playerBoundaryBox = playerMovScript.GetBoundaryBox();

        //Gets a new position to go inside
        //the player boundary box
        positionToGo = new Vector3(Random.Range(-playerBoundaryBox.x / 2, playerBoundaryBox.x / 2),
                                   Random.Range(-playerBoundaryBox.y / 2, playerBoundaryBox.y / 2),
                                   transform.position.z);

        //Pick a new random time to wait to change position
        PickRandomMaxTimeToChangePosition();

        positionTimer.Restart();
    }

    void PickRandomMaxTimeToChangePosition()
    {
        positionTimer.maxTime = Random.Range(changePos_timeRange.x, changePos_timeRange.y);
    }

    #endregion


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
            deathTypeB_part = poolPart.GetComponent<ParticleSystem>();


            //Changes the death particle's color
            //to the one of the enemy
            //and plays it
            ParticleSystem.MainModule main_death = deathTypeB_part.main;

            main_death.startColor = new ParticleSystem.MinMaxGradient(deathPart_colors.colorMin,
                                                                      deathPart_colors.colorMax);

            deathTypeB_part.gameObject.SetActive(true);
            deathTypeB_part.Play();

            #endregion
        }
    }

    #endregion



    #region EXTRA - Gizmos

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75f);
        Gizmos.DrawCube(positionToGo, Vector3.one * 0.5f);
        Gizmos.color = new Color(0, 1, 0.5f, 0.75f);
        Gizmos.DrawSphere(transform.position, 0.2f);

        //Line from position to the new one (when goes to the new position)
        //or a simple vertical line (when it's arrived and stationary)
        Gizmos.color = isInPosition ? Color.cyan : Color.red;
        Gizmos.DrawLine(transform.position,
                        isInPosition ? transform.position + Vector3.up : positionToGo);
    }

    #endregion


    #region EXTRA - Changing the Inspector

    private void OnValidate()
    {
        //Makes the the X always the min and Y the max
        changePos_timeRange.x = Mathf.Min(changePos_timeRange.x, changePos_timeRange.y);
        #region UNUSED
        //changePos_timeRange.x = Mathf.Clamp(changePos_timeRange.x, 0, changePos_timeRange.y); 
        #endregion

        deathPart_colors.mode = ParticleSystemGradientMode.TwoColors;
    }

    #endregion
}
