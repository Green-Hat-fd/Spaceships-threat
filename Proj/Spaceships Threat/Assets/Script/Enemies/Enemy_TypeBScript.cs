using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_TypeBScript : MonoBehaviour
{
    ObjectPoolingScript poolingScr;
    [SerializeField] string typeB_tag = "Enemy type B";

    [Min(0)]
    [SerializeField] float maxHealth;
    float health_now;
    bool isDead = false;


    [Space(20)]
    [SerializeField] PlayerMovemRB playerMovScript;
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



    private void Awake()
    {
        health_now = maxHealth;

        playerMovScript = FindObjectOfType<PlayerMovemRB>();
        
        shootTimer.maxTime = fireRate_Seconds;
        //shootTimer.OnTimerDone_event.AddListener(() => _______());
        /*PickRandomMaxTimeToChangePosition*/ChangePosition();
        positionTimer.OnTimerDone_event.AddListener(() => ChangePosition());
    }

    void Update()
    {
        //Checks if it's arrived in the new position
        float dist = Vector3.Distance(transform.position, positionToGo);
        isInPosition = dist <= MIN_DISTANCE;


        if (isInPosition)
        {
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


    void ChangePosition()
    {
        Vector2 playerBoundaryBox = playerMovScript.GetBoundaryBox();

        //Gets a new position to go inside
        //the player boundary box
        positionToGo = new Vector3(Random.Range(-playerBoundaryBox.x/2, playerBoundaryBox.x/2),
                                   Random.Range(-playerBoundaryBox.y/2, playerBoundaryBox.y/2),
                                   transform.position.z);

        //Pick a new random time to wait to change position
        PickRandomMaxTimeToChangePosition();

        positionTimer.Restart();
    }

    void PickRandomMaxTimeToChangePosition()
    {
        positionTimer.maxTime = Random.Range(changePos_timeRange.x, changePos_timeRange.y);
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
        if (isDead)
        {
            poolingScr.ReAddObject(typeB_tag, gameObject);    //Re-adds the enemy to the pool



            //Adds the scraps to the player
            stats_SO.AddTempScraps(scrapsDroppedWhenDead);
        }
    }


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
    }

    #endregion
}
