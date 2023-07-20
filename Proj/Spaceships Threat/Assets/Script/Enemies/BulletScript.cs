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

    Rigidbody rb;
    
    [Min(1)]
    [SerializeField] float projectileSpeed = 10f;

    [Space(20)]
    [SerializeField] ProjType_Enum whoIsShooting = ProjType_Enum.Enemy;



    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
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
        //Checks who is shooting this projectile
        switch (whoIsShooting)
        {
            case ProjType_Enum.Enemy:
                break;
            
            case ProjType_Enum.Player:
                break;
            
            case ProjType_Enum.Special:
                break;
        }
    }
}
