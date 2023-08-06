using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    #region All enemies' pools
    
    [SerializeField] ObjectPoolingScript TypeA_pool;
    [SerializeField] ObjectPoolingScript TypeB_pool;
    [SerializeField] ObjectPoolingScript TypeC_pool;
    [SerializeField] ObjectPoolingScript TypeDelta_pool;
    [SerializeField] ObjectPoolingScript TypeOmega_pool;
    #endregion

    int maxEnemiesOnScreen;
    


    void Update()
    {
        
    }


    void SpawnEnemiesAtStart()
    {

    }

    void AllEnemiesDead()
    {

    }


    #region Custom Set Functions

    public void SetMaxEnemiesOnScreen(int newMax)
    {
        maxEnemiesOnScreen = newMax;
    }

    #endregion
}
