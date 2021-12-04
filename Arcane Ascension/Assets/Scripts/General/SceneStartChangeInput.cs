using UnityEngine;

/// <summary>
/// Class that changes input on awake.
/// </summary>
public class SceneStartChangeInput : MonoBehaviour
{
    [SerializeField] private TypeOfControl typeOfControl;

    private void Start()
    {
        IInput input = FindObjectOfType<PlayerInputCustom>();

        switch(typeOfControl)
        {
            case TypeOfControl.UI:
                input.SwitchActionMapToUI();
                break;
            case TypeOfControl.Gameplay:
                input.SwitchActionMapToGameplay();
                break;
            case TypeOfControl.Abilities:
                input.SwitchActionMapToAbilitiesUI();
                break;
            case TypeOfControl.CheatConsole:
                input.SwitchActionMapToCheatConsole();
                break;
            case TypeOfControl.None:
                input.SwitchActionMapToNone();
                break;
        }
    }
}
