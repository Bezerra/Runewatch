using UnityEngine;
using TMPro;

/// <summary>
/// Logic for typewriter effect. Prints a character at a time on update.
/// </summary>
public class TypewriterLogic
{
    private TextMeshProUGUI text;
    private string textToWrite;
    private int characterIndex;
    private float timePerCharacter;
    private Typewriter parentTypewriter;
    private float timer;

    public TypewriterLogic(TextMeshProUGUI text, string textToWrite, float timePerCharacter, Typewriter parentTypewriter)
    {
        this.text = text;
        this.textToWrite = textToWrite;
        this.timePerCharacter = timePerCharacter;
        this.parentTypewriter = parentTypewriter;
        characterIndex = 0;
    }

    /// <summary>
    /// Prints a character at a time.
    /// </summary>
    /// <returns>Returns true when all characters are printed.</returns>
    public bool PrintNextCharacter()
    {
        timer -= Time.deltaTime;
        while(timer <= 0f)
        {
            timer += timePerCharacter;
            characterIndex++;

            // Inserts a character on the characterIndex index
            string textToShow = textToWrite.Substring(0, characterIndex);

            // Prints all text on front of it as transparent (so it keeps alligned while typing)
            textToShow += "<color=#00000000>" + textToWrite.Substring(characterIndex);

            // Sets text
            text.text = textToShow;

            // If last letter was printed
            if (characterIndex >= textToWrite.Length)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Gets current typewriter.
    /// </summary>
    /// <returns>Returns current typewriter TextMeshproUGUI.</returns>
    public TextMeshProUGUI GetCurrentTypewriter() => text;

    /// <summary>
    /// Checks if this typewriter is active.
    /// </summary>
    /// <returns>Returns true if the typewriter is active.</returns>
    public bool IsCurrentTypewriterActive() => characterIndex < textToWrite.Length;

    /// <summary>
    /// Types all characters.
    /// </summary>
    public void WriteAll()
    {
        text.text = textToWrite;
        characterIndex = textToWrite.Length;
        parentTypewriter.RemoveTypewriter(text);
    }
}
