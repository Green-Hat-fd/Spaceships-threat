using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursorManager : MonoBehaviour
{
    enum TCursor
    {
        Default,
        Crosshair
    }

    //The array where all the cursor images will go (0 = default)
    [SerializeField] List<Texture2D> cursorArray  = new List<Texture2D>(5);

    //The vector 2D where all the cursor hotspot will go (0 = default)
    [SerializeField] List<Vector2> hotspotArray  = new List<Vector2>(5);



    
    private void Awake()
    {
        //Makes the cursor the default arrow
        ChangeCursor_Default();
    }

    #region All the functions that change the mouse cursor

    public void ChangeCursor_Default()
    {
        ChangeChooseVector((int)TCursor.Default);
    }

    public void ChangeCursor_Crosshair()
    {
        ChangeChooseVector((int)TCursor.Crosshair);
    }

    #endregion


    public void ChangeChooseVector(int c)
    {
        Cursor.SetCursor(cursorArray[c], hotspotArray[c], CursorMode.Auto);
    }
}
