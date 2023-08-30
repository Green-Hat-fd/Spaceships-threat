using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextScript : MonoBehaviour
{

    #region Tooltip()
    [Tooltip("The tag to search for in the language's Scriptable Obj \n(see below)")]
    #endregion
    [SerializeField] string tagToSearch;

    [Space(10)]
    [SerializeField] OptionsSO_Script opt_SO;

    [Header("—— Languages (Scriptable Obj.) ——")]
    [SerializeField] LanguageSO_Script english;
    [SerializeField] LanguageSO_Script italian;

    /// <summary>
    /// The TMPro Text component to change
    /// </summary>
    TMP_Text textComp;



    private void Awake()
    {
        textComp = GetComponent<TMP_Text>();

        //Arranges, for each language, from list to dictionary
        english.MoveTextsIntoDictionary();
        italian.MoveTextsIntoDictionary();
    }

    private void Update()
    {
        ChangeText();
    }

    public void ChangeText()
    {
        LanguageSO_Script chosenTextLanguageSO = default;

        //Takes the Scriptable Obj. relative to the chosen language
        switch (opt_SO.GetChosenLanguage())
        {
            #region Inglese

            case OptionsSO_Script.Language_Enum.English:
                chosenTextLanguageSO = english;
                break;
            #endregion

            #region Italiano

            case OptionsSO_Script.Language_Enum.Italian:
                chosenTextLanguageSO = italian;
                break;
                #endregion
        }

        //Takes the text from the chosen language
        //and changes it in the TMPro component
        textComp.text = chosenTextLanguageSO.GetTexts(tagToSearch);
    }
}
