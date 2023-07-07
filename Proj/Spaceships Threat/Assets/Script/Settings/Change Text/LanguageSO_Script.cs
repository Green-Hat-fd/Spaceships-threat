using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextWithTag_Class
{
    public string tag;
    [TextArea(2, 7)]
    public string text;
}

[CreateAssetMenu(menuName = "Scriptable Objects/Language (S.O.)", fileName = "NewLanguage (Language)")]
public class LanguageSO_Script : ScriptableObject
{
    Dictionary<string, string> textsDict = new Dictionary<string, string>();

    #region Tooltip()
    [Tooltip("Write the text in the language specified by the Scriptable Obj. name \n\n/!\\   Associate each text with its own tag")]
    #endregion
    [SerializeField] List<TextWithTag_Class> textsToChange;



    public void MoveTextsIntoDictionary()
    {
        //Clears the entire dictionary
        textsDict.Clear();

        //Transfers all the information from the List to the Dictionary
        foreach (TextWithTag_Class t in textsToChange)
        {
            textsDict.Add(t.tag, t.text);
        }
    }

    /// <summary>
    /// Returns the text to the given <i><b>tag</b></i> (if it exits)
    /// </summary>
    public string GetTexts(string tagToCheck)
    {
        //If there's the tag in the Dictionary, then returns the text in the language
        return textsDict.ContainsKey(tagToCheck)
                ?
                textsDict[tagToCheck]
                :
                null;
    }
}
