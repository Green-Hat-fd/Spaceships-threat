using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    ObjectPoolingScript bulletsPool;
    [SerializeField] string bullet_poolTag = "Bullets";

    [Space(15)]
    [SerializeField] Transform shootingPosition;

    [Space(15)]
    [SerializeField] float fireRate = 1;
    [SerializeField] bool autoShooting = true;
    CustomTimer shootTimer = new CustomTimer();



    private void Awake()
    {
        bulletsPool = FindObjectOfType<ObjectPoolingScript>();

        shootTimer.OnTimerDone_event.AddListener(() => Shoot());
        shootTimer.maxTime = fireRate;


        //Restarts the timer only
        //if "autoShooting" is ON
        //(then shoot when timer's done)
        shootTimer.LoopTimer(autoShooting);
    }

    void Update()
    {
        //---Timer---//
        shootTimer.AddTimeToTimer();
    }

    void Shoot()
    {
        bulletsPool.TakeObjectFromPool(bullet_poolTag,
                                       shootingPosition.position,
                                       shootingPosition.rotation);
    }

    public void SetFireRate(float sec)
    {
        fireRate = sec;
    }
}
