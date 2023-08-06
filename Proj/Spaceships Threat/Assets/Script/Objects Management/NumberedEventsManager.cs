using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NumberedEventsManager : MonoBehaviour
{
    [System.Serializable]
    public class NumbEvents_Class
    {
        public int numberToWait;
        public UnityEvent OnNumberReached;
    }

    [SerializeField] List<NumbEvents_Class> numberedEvents;

    int count = 0;
    int controlIndex = 0;   //Index used to execute all the events in order



    /*
    private void Awake()
    {
        SortList(numberedEvents);
    }
    //*/


    /// <summary>
    /// Increases the count by one (+1)
    /// <br></br> and checks the counting
    /// </summary>
    public void IncreaseCount()
    {
        //Adds one (+1) to the internal counting
        count++;

        CheckCounting();
    }
    /// <summary>
    /// Decreases the count by one (-1)
    /// <br></br> and checks the counting
    /// </summary>
    public void DecreaseCount()
    {
        //Removes one (-1) to the internal counting
        count--;

        CheckCounting();
    }
    /// <summary>
    /// Decreases the count by a custom amount
    /// <br></br> and checks the counting
    /// </summary>
    public void CustomAddCount(int customValue)
    {
        //Adds to the internal counting
        count += customValue;

        CheckCounting();
    }
    public void ResetCount()
    {
        count = 0;
        controlIndex = 0;
    }


    /// <summary>
    /// Checks if the counting has reached a number and calls the related event
    /// </summary>
    void CheckCounting()
    {
        //Checks if the counting matches the number
        //on the next event
        if (count == numberedEvents[controlIndex].numberToWait)
        {
            //If matches, calls the event
            numberedEvents[controlIndex].OnNumberReached.Invoke();


            controlIndex++;   //Adds to the index to check
        }
    }


    //TODO: se hai tempo, fai un algoritmo di ordinamento (sorting algorithm)
    /*void SortList(List<NumbEvents_Class> allEvents)
    {
        List<NumbEvents_Class> list_Temp = new List<NumbEvents_Class>();


        for (int i = 0; i < allEvents.Count; i++)
        {

        }


        allEvents = list_Temp;
    }
    //*/
}
