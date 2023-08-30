using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeNumberTextScript : MonoBehaviour
{
    [SerializeField] OptionsSO_Script options_SO;

    TMP_Text textToChange;

    [Space(10)]
    [SerializeField] string toAddAfter;


    private void Awake()
    {
        textToChange = GetComponent<TMP_Text>();
    }


    #region Functions that changes the text

    /// <summary>
    /// Changes the text with the assigned value,
    /// <br></br> adding the variable <i><b>toAddAfter</b></i> after the number
    /// </summary>
    /// <param name="t">The text to change</param>
    public void ChangeText_WithString(string t)
    {
        textToChange.text = t + toAddAfter;
    }


    /// <summary>
    /// Makes the <i>float</i> to a <i>int</i> and writes it as a text
    /// </summary>
    /// <param name="t">The text to change</param>
    public void ChangeNumText_FloatToInt(float t)
    {
        textToChange.text = Mathf.RoundToInt(t).ToString();
    }
    /// <summary>
    /// Makes the <i>float</i> to a <i>int</i> and writes it as a text,
    /// <br></br> adding the variable <i><b>toAddAfter</b></i> after the number
    /// </summary>
    public void ChangeNumText_FloatToInt_WithString(float t)
    {
        textToChange.text = Mathf.RoundToInt(t) + toAddAfter;
    }


    /// <summary>
    /// Approximates the <i>float</i> to 2 decimal and writes it as a text
    /// </summary>
    /// <param name="t">The text to change</param>
    public void ChangeNumText_Approx(float t)
    {
        textToChange.text = ((float)(Mathf.RoundToInt(t * 100f) / 100f)).ToString();
    }
    /// <summary>
    /// Approximates the <i>float</i> to 2 decimal and writes it as a text,
    /// <br></br> adding the variable <i><b>toAddAfter</b></i> after the number
    /// </summary>
    public void ChangeNumText_Approx_WithString(float t)
    {
        textToChange.text = (float)(Mathf.RoundToInt(t * 100f) / 100f) + toAddAfter;
    }


    /// <summary>
    /// Takes the number in the stile of the AudioMixerGroup (in the range <b>[-80; 5]</b>) and makes it a percentage <i>(form 0 to 100)</i>
    /// </summary>
    public void ChangeVolumeText(float t)
    {
        textToChange.text = VolumeToPercent(t).ToString();
    }
    /// <summary>
    /// Takes the number in the stile of the AudioMixerGroup (in the range <b>[-80; 5]</b>) and makes it a percentage <i>(form 0 to 100)</i>,
    /// <br></br> adding the variable <i><b>toAddAfter</b></i> after the number
    /// </summary>
    public void ChangeVolumeText_WithString(float t)
    {
        textToChange.text = VolumeToPercent(t) + toAddAfter;
    }

    #endregion


    #region Funct. that converts from slider value to percentage

    /// <summary>
    /// Takes the number from AudioMixerGroup and converts it as a percentage
    /// </summary>
    /// <param name="num">Number (range <b>[-80; 5]</b>) to be converted as a percentage</param>
    float VolumeToPercent(float num)
    {
        return Mathf.RoundToInt(num * 100);
    }

    #endregion
}
