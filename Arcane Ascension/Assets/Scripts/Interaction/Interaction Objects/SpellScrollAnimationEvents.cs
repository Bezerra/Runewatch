using UnityEngine;

/// <summary>
/// Class responsible for executing spell's animation events.
/// </summary>
public class SpellScrollAnimationEvents : MonoBehaviour
{
    private SpellScroll spellScroll;

    private void Awake() =>
        spellScroll = GetComponentInParent<SpellScroll>();

    public void BookOpeningStartAnimationEvent() =>
        spellScroll.BookOpeningStartAnimationEvent();

    public void BookOpenedEndAnimationEvent() =>
        spellScroll.BookOpenedEndAnimationEvent();
}
