using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool_Class
{
    public string tag;
    public GameObject obj;
    public int size;
}

public class ObjectPoolingScript : MonoBehaviour
{
    Dictionary<string, Queue<GameObject>> poolDict;

    [SerializeField] List<Pool_Class> allOfPools;


    void Awake()
    {
        //Creates a new empty Dictionary
        poolDict = new Dictionary<string, Queue<GameObject>>();


        //Takes every pool in the list
        foreach (Pool_Class p in allOfPools)
        {
            Queue<GameObject> obj_queue = new Queue<GameObject>();  //Creates a new empty Queue
            GameObject empty = new GameObject(p.tag);  //Createa a new empty obj with the same tag as the name

            empty.transform.SetParent(transform);

            //Creates as many objects as the size and put them in the pool
            for (int i = 0; i < p.size; i++)
            {
                GameObject pref = Instantiate(p.obj);

                pref.SetActive(false);                      //Deactivates it
                pref.transform.SetParent(empty.transform);  //Makes it the child of the new empty obj

                obj_queue.Enqueue(pref);
            }

            //Adds the newly instantiated queue to the dictionary
            poolDict.Add(p.tag, obj_queue);
        }
    }


    /// <summary>
    /// Takes any object from the pool specified by the <i><b>tag</b></i> given position and rotation
    /// </summary>
    /// <param name="poolTag">Name/Tag of the pool which the object will be taken</param>
    /// <returns></returns>
    public GameObject TakeObjectFromPool(string poolTag, Vector3 pos, Quaternion rot)
    {
        //Checks if there is an object in the dictionary
        if (poolDict.ContainsKey(poolTag))
        {
            //Takes the object with the tag and dequeues it
            GameObject obj = poolDict[poolTag].Dequeue();


            //Replaces, rotates, resets the Rigidbody and activates the object
            obj.transform.position = pos;
            obj.transform.rotation = rot;
            ResetAllRigidBodies(obj);
            obj.SetActive(true);


            //Enqueues it
            poolDict[poolTag].Enqueue(obj);


            return obj;
        }
        else
            return null;
    }


    /// <summary>
    /// Put back the object <i><b>obj</b></i> in pool specified by the <i><b>tag</b></i>
    /// </summary>
    public void ReAddObject(string poolTag, GameObject obj)
    {
        //Checks if there is an object in the dictionary
        if (poolDict.ContainsKey(poolTag))
        {
            obj.SetActive(false);   //Deactivates it

            poolDict[poolTag].Enqueue(obj);   //Enqueues it
        }
    }

    /// <summary>
    /// Deactivates all objects inside the pool specified by the <i><b>tag</b></i>
    /// </summary>
    public void NascondiOgniOggettoDiUnaPool(string poolTag)
    {
        //Checks if there is an object in the dictionary
        if (poolDict.ContainsKey(poolTag))
        {
            //For each objects in the pool...
            foreach(GameObject obj in poolDict[poolTag].ToArray())
            {
                obj.SetActive(false);   //Deactivates it

                poolDict[poolTag].Enqueue(obj);   //Enqueues it
            }
        }
    }

    /// <summary>
    /// Resets the Rigidbody's velocity of the object & all of its children
    /// </summary>
    public static void ResetAllRigidBodies(GameObject objToReset)
    {
        //Checks if the obj has the RigidBody and resets its velocity
        Rigidbody rb_obj = objToReset.GetComponent<Rigidbody>();

        if (rb_obj)
        {
            rb_obj.velocity = Vector3.zero;
            rb_obj.angularVelocity = Vector3.zero;
        }


        //Checks if the children have the RigidBody and resets their velocity
        Rigidbody[] rb_child = objToReset.GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in rb_child)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}
