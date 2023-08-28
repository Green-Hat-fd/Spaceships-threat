using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    #region All enemies' pool tags
    
    [SerializeField] string TypeA_pool = "Type A Enemy";
    [SerializeField] string TypeB_pool = "Type B Enemy";
    [SerializeField] string TypeC_pool = "Type C Enemy";
    [SerializeField] string TypeDelta_pool = "Type Delta Enemy";
    [SerializeField] string TypeOmega_pool = "Type Omega Enemy";
    #endregion

    int maxEnemiesOnScreen;

    int typeA_killed,
        typeB_killed,
        typeDelta_killed,
        typeOmega_killed,
        maxKilled;
    


    void Update()
    {



        //Updates the "max enemy kill" counter
        maxKilled = typeA_killed
                    + typeB_killed
                    + typeDelta_killed
                    + typeOmega_killed;
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

    public void AddKillTypeA()
    {
        typeA_killed++;
    }
    public void AddKillTypeB()
    {
        typeB_killed++;
    }
    public void AddKillTypeDelta()
    {
        typeDelta_killed++;
    }
    public void AddKillTypeOmega()
    {
        typeOmega_killed++;
    }

    #endregion
}
