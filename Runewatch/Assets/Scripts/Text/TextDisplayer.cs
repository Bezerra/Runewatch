using UnityEngine;
using TMPro;

/// <summary>
/// Class used by gameobjects that print text.
/// </summary>
public class TextDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private bool canSkipText = true;
    [SerializeField] private float timeToPrintCharacter = .1f;
    [SerializeField] private float timeToNextLineOfDialog = 1f;
    public bool FirstTimeDialog { get; set; }

    [SerializeField] private string[] linesOfDialog;
    [Header("Will only work if canSkipText is true")]
    [SerializeField] private string[] linesOfDialogSecondTime;

    public byte CurrentLine { get; set; }
    public int LinesOfDialogLength => linesOfDialog.Length;
    public int LinesOfDialogSecondTime => linesOfDialogSecondTime.Length;
    public float TimeToNextLineOfDialog => timeToNextLineOfDialog;
    public TextMeshProUGUI TextBox => textBox;

    private void Awake()
    {
        FirstTimeDialog = true;
    }

    private void Start()
    {
        DisplayDialog();
    }

    /// <summary>
    /// Displays dialog with typewriter effect.
    /// Displays linesOfDialog first, then it displays linesOfDialogSecondTime (after linesOfDialog reach the end).
    /// </summary>
    public void DisplayDialog()
    {
        if (textBox.enabled == false)
        {
            textBox.text = " ";
            textBox.enabled = true;
        }

        if (FirstTimeDialog)
        {
            if (canSkipText)
                Typewriter.Write(textBox, linesOfDialog[CurrentLine], timeToPrintCharacter, canSkipText, this);
            else
                Typewriter.Write(textBox, linesOfDialog[CurrentLine], timeToPrintCharacter, canSkipText);

            CurrentLine++;
        }

        else
        {
            if (linesOfDialogSecondTime.Length > 0)
                Typewriter.Write(textBox, linesOfDialogSecondTime[CurrentLine], timeToPrintCharacter, canSkipText, this);

            CurrentLine++;
        }
    }
}
