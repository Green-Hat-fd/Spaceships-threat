using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class BulletScript : MonoBehaviour
{
    enum ProjType_Enum
    {
        Enemy,
        Player,
        Special
    }
    
    [SerializeField] string bullet_poolTag = "Bullets";
    ObjectPoolingScript poolingScr;

    Rigidbody rb;

    [Space(20)]
    [Min(1)]
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] float projectileLifetime = 5;
    CustomTimer projectileTimer = new CustomTimer();

    [Space(20)]
    [SerializeField] ProjType_Enum whoIsShooting = ProjType_Enum.Enemy;

    IDamageable damageableObj;



    private void Awake()
    {
        poolingScr = FindObjectOfType<ObjectPoolingScript>();

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;

        projectileTimer.maxTime = projectileLifetime;
        projectileTimer.OnTimerDone_event
                       .AddListener(() => RemoveBullet());
    }

    private void Update()
    {
        projectileTimer.AddTimeToTimer();
    }

    void FixedUpdate()
    {
        //Moves the projectile in the forward direction
        rb.AddForce(transform.forward * projectileSpeed * 10f, ForceMode.Force);

        #region Speed limit

        //Takes the projectile velocity
        Vector3 velTemp = new Vector3(0, 0, rb.velocity.z);

        //Check if there's too much acceleration, i.e. if it's over the speed limit
        if (rb.velocity.z >= projectileSpeed)
        {
            //Limits the velocity to the intended one, giving it to the RigidBody
            rb.velocity = velTemp.normalized * projectileSpeed;
        }

        #endregion
    }

    private void OnTriggerEnter(Collider other)
    {
        //A variable to get & check if the
        //object collided can take damage
        damageableObj = other?.GetComponent<IDamageable>();

        if(damageableObj != null)
        {
            //Checks WHO is shooting this projectile
            switch (whoIsShooting)
            {
                #region If the enemy hits the player

                case ProjType_Enum.Enemy:
                    IPlayer player_check = other?.GetComponent<IPlayer>();

                    if (player_check != null)
                    {
                        damageableObj.TakeDamage(1);    //Damages the player

                        RemoveBullet();                 //Removes the bullet
                    }
                    break; 
                #endregion


                #region If the player hits an enemy

                case ProjType_Enum.Player:
                    IEnemy enemy_check = other.GetComponent<IEnemy>();

                    if (enemy_check != null)
                    {
                        damageableObj.TakeDamage(1);    //Damages the enemy

                        RemoveBullet();                 //Removes the bullet
                    }
                    break;
                #endregion


                #region If the player has used any power-up
                //TODO: SIstema ^^

                case ProjType_Enum.Special:
                    //TODO: aggiungere
                    break;
                #endregion
            }
        }
    }


    void RemoveBullet()
    {
        poolingScr.ReAddObject(bullet_poolTag, gameObject);
    }
}
