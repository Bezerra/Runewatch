using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for creating typewritter effect.
/// </summary>
public class Typewriter : MonoBehaviour
{
    private static Typewriter instance;

    // Type writers
    private IList<TypewriterLogic> typeWriters;
    private TypewriterLogic canSkipTypewriter;

    // Object that is displaying text
    private TextDisplayer textDisplayer;

    private IEnumerator skipToNextDialogAfterSeconds;

    private PlayerInputCustom input;

    private void Awake()
    {
        instance = this;
        typeWriters = new List<TypewriterLogic>();
        input = FindObjectOfType<PlayerInputCustom>();
        skipToNextDialogAfterSeconds = SkipToNextDialogAfterSeconds();
    }

    private void OnEnable() =>
        input.Click += SkipText;

    private void OnDisable() =>
        input.Click -= SkipText;

    /// <summary>
    /// Skils text or changes text to next line of dialog.
    /// </summary>
    /// <param name="dir">Left or right click.</param>
    private void SkipText()
    {
        // If text is still showing up
        if (canSkipTypewriter != null)
        {
            // Skips text until the end (if it's still printing)
            if (canSkipTypewriter.IsCurrentTypewriterActive())
            {
                canSkipTypewriter.WriteAll();
                canSkipTypewriter = null;

                StopCoroutine(skipToNextDialogAfterSeconds);
                skipToNextDialogAfterSeconds = SkipToNextDialogAfterSeconds();
                StartCoroutine(skipToNextDialogAfterSeconds);
            }
            else
            {
                canSkipTypewriter = null;
            }
        }
        // If text already reached the end, starts printing next message
        else
        {
            StopCoroutine(skipToNextDialogAfterSeconds);
            DisableTextOrPrintNextLine();
        }
    }

    /// <summary>
    /// Static write new typewriter.
    /// </summary>
    /// <param name="text">Text component.</param>
    /// <param name="textToWrite">Text to write.</param>
    /// <param name="timePerCharacter">Time after each character.</param>
    /// <param name="canSkip">If this typewriter is skipable when clicked.</param>
    /// <param name="textDisplayer">TextDisplayer that is using a typewriter.</param>
    public static void Write(TextMeshProUGUI text, string textToWrite, float timePerCharacter, bool canSkip,
        TextDisplayer textDisplayer = null)
    {
        instance.RemoveTypewriter(text);
        instance.AddTypewriter(text, textToWrite, timePerCharacter, canSkip);

        if (canSkip)
            instance.textDisplayer = textDisplayer;
    }

    /// <summary>
    /// Adds new typewriter to typewriters list.
    /// </summary>
    /// <param name="text">TextBox to add typewritter to.</param>
    /// <param name="textToWrite">Text to write.</param>
    /// <param name="timePerCharacter">Time per each character.</param>
    /// <param name="canSkip">If this typewriter is skipable when clicked.</param>
    private void AddTypewriter(TextMeshProUGUI text, string textToWrite, float timePerCharacter, bool canSkip)
    {
        TypewriterLogic typewriterToAdd = new TypewriterLogic(text, textToWrite, timePerCharacter, this);
        typeWriters.Add(typewriterToAdd);

        if (canSkip)
            canSkipTypewriter = typewriterToAdd;
    }

    /// <summary>
    /// Removes current typewriter from typewriters list.
    /// </summary>
    /// <param name="text">Typewriter to remove.</param>
    public void RemoveTypewriter(TextMeshProUGUI text)
    {
        for (int i = 0; i < typeWriters.Count; i++)
        {
            if (typeWriters[i].GetCurrentTypewriter() == text)
            {
                typeWriters.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// Updates characters. If it's the last character it will start a coroutine to skip to next line.
    /// </summary>
    private void Update()
    {
        for (int i = 0; i < typeWriters.Count; i++)
        {
            bool textIsFinished = typeWriters[i].PrintNextCharacter();

            // If typewriter is finished, it removes it from the typewrites list.
            if (textIsFinished)
            {
                typeWriters.RemoveAt(i);
                i--;

                canSkipTypewriter = null;

                StopCoroutine(skipToNextDialogAfterSeconds);
                skipToNextDialogAfterSeconds = SkipToNextDialogAfterSeconds();
                StartCoroutine(skipToNextDialogAfterSeconds);
            }
        }
    }

    /// <summary>
    /// Waits for X seconds and skips to next line or disables text box.
    /// </summary>
    /// <returns>Null.</returns>
    private IEnumerator SkipToNextDialogAfterSeconds()
    {
        yield return new WaitForSeconds(textDisplayer.TimeToNextLineOfDialog);

        DisableTextOrPrintNextLine();
    }

    /// <summary>
    /// Displays next line of text or disables text box if it's the last line.
    /// </summary>
    private void DisableTextOrPrintNextLine()
    {
        if (textDisplayer != null)
        {
            if (textDisplayer.FirstTimeDialog)
            {
                if (textDisplayer.CurrentLine >= textDisplayer.LinesOfDialogLength)
                {
                    if (textDisplayer.CurrentLine >= textDisplayer.LinesOfDialogLength)
                        textDisplayer.FirstTimeDialog = false;

                    textDisplayer.CurrentLine = 0;
                    textDisplayer.TextBox.enabled = false;
                    textDisplayer = null;
                }
                else
                {
                    textDisplayer.DisplayDialog();
                }
            }
            else
            {
                if (textDisplayer.CurrentLine >= textDisplayer.LinesOfDialogSecondTime)
                {
                    textDisplayer.CurrentLine = 0;
                    textDisplayer.TextBox.enabled = false;
                    textDisplayer = null;
                }
                else
                {
                    textDisplayer.DisplayDialog();
                }
            }
        }
    }
}
