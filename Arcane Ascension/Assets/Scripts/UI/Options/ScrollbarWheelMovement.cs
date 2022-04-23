using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class responsible for handling scrollbar wheel movement on interface.
/// </summary>
public class ScrollbarWheelMovement : MonoBehaviour
{
    [Range(0.05f, 0.2f)] [SerializeField] private float barSpeed = 0.15f;
    [SerializeField] private Scrollbar scrollbar;

    private PlayerInputCustom input;

    private void OnEnable()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        if (input != null)
        {
            input.PreviousNextSpell += Scroll;
        }
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.PreviousNextSpell -= Scroll;
        }
    }

    private void Scroll(float val, bool emptyVar = false)
    {
        if (scrollbar.value < 0.98f)
        {
            if (val > 0)
            {
                scrollbar.value += barSpeed;
            }
        }

        if (scrollbar.value > 0.01)
        {
            if (val < 0)
            {
                scrollbar.value -= barSpeed;
            }
        }
    }
}
