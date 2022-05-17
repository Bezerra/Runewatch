using System.Collections;
using UnityEngine;
using System;

/// <summary>
/// Class responsible for executing logic when the player presses and releases
/// attack key.
/// </summary>
public class PlayerCastSpell : MonoBehaviour, IFindInput
{
    // Components
    private IInput input;
    private PlayerStats playerStats;
    private PlayerSpells playerSpells;
    private Player player;
    private Animator anim;

    // Current cast spell behaviour
    private SpellBehaviourAbstract spellBehaviour;
    private GameObject currentlyCastSpell;
    public bool CurrentlyCasting => currentlyCastSpell == null ? false : true;

    private float lastTimeSpellWasCast;

    [SerializeField] private RunStatsLogicSO runStatsLogic;

    /// <summary>
    /// Property to know if the animation is over or still running.
    /// </summary>
    public bool AnimationOver { get; private set; }

    private void Awake()
    {
        input = FindObjectOfType<PlayerInputCustom>();
        playerStats = GetComponentInParent<PlayerStats>();
        playerSpells = GetComponentInParent<PlayerSpells>();
        player = GetComponentInParent<Player>();
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Coroutine that runs every X seconds.
    /// If the player has not enough mana to cast the active spell, 
    /// it cancels shake, cancels attack and resets casting spells variables.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Start()
    {
        AnimationOver = true;
        YieldInstruction wfs = new WaitForSeconds(0.2f);

        while (playerSpells.ActiveSpell == null)
        {
            yield return wfs;
        }

        // Will only run this after the player obtains a spell
        while (true)
        {
            yield return wfs;

            if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost < 0)
            {
                currentlyCastSpell = null;
                currentlyCastSpell = null;
            }
        }
    }

    private void OnEnable()
    {
        FindInput();
    }

    private void OnDisable()
    {
        LostInput();
    }

    /// <summary>
    /// This is used only for spells with release. If the player is pressing the cast
    /// button, it will start casting the spell as soon as possible.
    /// </summary>
    private void Update()
    {
        if (input != null)
        {
            if (playerSpells.ActiveSpell != null)
            {
                if (input.HoldingCastSpell &&
                    playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCastWithRelease)
                {
                    if (AnimationOver)
                        AttackKeyPress();
                }
            }
        }
    }

    /// <summary>
    /// A fixed spell cast for every spells.
    /// </summary>
    /// <returns></returns>
    public bool SpellCastOnFixedDelay()
    {
        if (Time.time - lastTimeSpellWasCast > 0.15f)
            return false;
        return true;
    }

    /// <summary>
    /// Casts secondary basic spell.
    /// </summary>
    private void SecondaryAttackKeyPress()
    {
        if (AnimationOver == false)
            return;

        // If main spell is not in cooldown
        if (playerSpells.CooldownOver(playerSpells.SecondarySpell) &&
            currentlyCastSpell == null)
        {
            if (SpellCastOnFixedDelay())
                return;
            lastTimeSpellWasCast = Time.time;

            anim.SetTrigger("CastSecondarySpell");
            AnimationOver = false;
        }
    }

    /// <summary>
    /// Triggered on animation for secondary spells. Casts spell.
    /// </summary>
    public void AttackKeyPressSecondarySpellAnimationEvent()
    {
        playerSpells.SecondarySpell.AttackBehaviour.AttackKeyPress(
            ref currentlyCastSpell, playerSpells.SecondarySpell, player, playerStats, 
            ref spellBehaviour);

        runStatsLogic.ShotsFired++;

        OnEventAttack(playerSpells.SecondarySpell.CastType);
        OnEventStartCooldown(playerSpells.SecondarySpell);
    }

    /// <summary>
    /// Triggered after releasing key on secondary spell.
    /// </summary>
    public void AttackKeyReleaseSecondarySpellAnimationEvent()
    {
        OnSpellcastAny();

        playerSpells.SecondarySpell.AttackBehaviour.AttackKeyRelease(
             ref currentlyCastSpell, playerSpells.SecondarySpell, player, playerStats, 
             ref spellBehaviour);

        spellBehaviour = null;
        currentlyCastSpell = null;
        anim.ResetTrigger("CastSecondarySpell");
        anim.ResetTrigger("CastSpell");
    }

    /// <summary>
    /// Happens when the player presses attack key. Triggers attack animations.
    /// </summary>
    private void AttackKeyPress()
    {
        if (AnimationOver == false)
            return;

        if (playerSpells.ActiveSpell == null)
            return;

        // If spell is not in cooldown
        // Important for OneShot spells (continuous don't have cooldown)
        if (playerSpells.CooldownOver(playerSpells.ActiveSpell) &&
            playerSpells.CooldownOver(playerSpells.SecondarySpell))
        {
            if (SpellCastOnFixedDelay())
                return;

            // If player has enough mana to cast the active spell
            if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost > 0)
            {
                if (SpellCastOnFixedDelay())
                    return;

                lastTimeSpellWasCast = Time.time;

                currentlyCastSpell = null;

                // Triggers animations that will cast spells.
                if (playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCast)
                    anim.SetTrigger("CastSpell");
                else
                    anim.SetBool("Channeling", true);

                // Bool to know that animation us running
                AnimationOver = false;
            }
        }
    }

    /// <summary>
    /// Triggered on animation after pressing primary attack. Casts spell.
    /// </summary>
    public void AttackKeyPressAnimationEvent()
    {
        if (currentlyCastSpell != null)
            return;

        // If it's a one shot cast, this logic will cast the spell
        // If it's a one shot with release, this logic will channel it first
        playerSpells.ActiveSpell.AttackBehaviour.AttackKeyPress(
            ref currentlyCastSpell, playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);

        // Mana and cooldown on oneshot is done here
        if (playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCast)
        {
            OnEventStartCooldown(playerSpells.ActiveSpell);
            OnEventSpendMana(playerSpells.ActiveSpell.ManaCost);
            OnEventAttack(playerSpells.ActiveSpell.CastType);
            OnEventStartScreenShake(playerSpells.ActiveSpell.CastType);
        }
    }

    /// <summary>
    /// Triggered when attack key is released.
    /// </summary>
    public void AttackKeyRelease()
    {
        OnReleasedAttackButton();

        // If it's NOT a one shot with release it will ignore the rest
        if (anim.GetBool("Channeling") == false)
        {
            return;
        }
        else
        {
            if (playerSpells.ActiveSpell == null)
                return;

            // If player has a spell being prepared in hand and CDs are over
            if (playerSpells.CooldownOver(playerSpells.ActiveSpell) &&
                playerSpells.CooldownOver(playerSpells.SecondarySpell) &&
                currentlyCastSpell != null)
            {
                // If the player has enough mana, triggers release animation.
                if (playerStats.Mana - playerSpells.ActiveSpell.ManaCost > 0)
                {
                    // Ends channeling animation. This will start release animation.
                    anim.SetBool("Channeling", false);
                }
            }
            else
            {
                // If player has no mana, ignores this method
                currentlyCastSpell = null;
                return;
            }
        }
    }

    /// <summary>
    /// Triggered on animation. One shot release spells main logic happen here.
    /// </summary>
    public void AttackKeyReleaseAnimationEvent()
    {
        // Mana and cooldown logic for one shot with release happens here
        if (playerSpells.ActiveSpell.CastType == SpellCastType.OneShotCastWithRelease)
        {
            OnEventStartCooldown(playerSpells.ActiveSpell);
            OnEventSpendMana(playerSpells.ActiveSpell.ManaCost);
            OnEventAttack(playerSpells.ActiveSpell.CastType);
        }

        OnSpellcastAny();

        // Spell release logic
        playerSpells.ActiveSpell.AttackBehaviour.AttackKeyRelease(
             ref currentlyCastSpell, playerSpells.ActiveSpell, player, playerStats, ref spellBehaviour);

        runStatsLogic.ShotsFired++;

        spellBehaviour = null;
        currentlyCastSpell = null;

        // Resets one shot spells animation triggers
        anim.ResetTrigger("CastSecondarySpell");
        anim.ResetTrigger("CastSpell");
    }

    /// <summary>
    /// Method used to update a bool to know that the animations are over.
    /// </summary>
    public void AnimationOverAnimationEvent() => AnimationOver = true;

    protected virtual void OnEventStartCooldown(ISpell spell) => EventStartCooldown?.Invoke(spell);
    public event Action<ISpell> EventStartCooldown;

    protected virtual void OnEventSpendMana(float amount) => EventSpendMana?.Invoke(amount);
    public event Action<float> EventSpendMana;

    protected virtual void OnEventAttack(SpellCastType castType) => EventAttack?.Invoke(castType);
    public event Action<SpellCastType> EventAttack;

    /// <summary>
    /// Called through animation events.
    /// </summary>
    public virtual void OnAttackAnimationStart() => AttackAnimationStart?.Invoke();
    public event Action AttackAnimationStart;

    /// <summary>
    /// Called through animation events.
    /// </summary>
    public virtual void OnAttackAnimationEnd() => AttackAnimationEnd?.Invoke();
    public event Action AttackAnimationEnd;

    // Registered on PlayerGenerateCinemachineImpulse
    protected virtual void OnEventStartScreenShake(SpellCastType castType) => 
        EventStartScreenShake?.Invoke(castType);
    public event Action<SpellCastType> EventStartScreenShake;

    protected virtual void OnSpellcastAny() =>
        SpellcastAny?.Invoke();
    public event Action SpellcastAny;

    protected virtual void OnReleasedAttackButton() =>
        ReleasedAttackButton?.Invoke();
    public event Action ReleasedAttackButton;


    public void FindInput(PlayerInputCustom input = null)
    {
        LostInput();
        if (this.input == null)
        {
            this.input = FindObjectOfType<PlayerInputCustom>();
            this.input.CastSpell += AttackKeyPress;
            this.input.StopCastSpell += AttackKeyRelease;
            this.input.CastBasicSpell += SecondaryAttackKeyPress;
        }
    }

    public void LostInput(PlayerInputCustom input = null)
    {
        if (this.input != null)
        {
            this.input.CastSpell -= AttackKeyPress;
            this.input.StopCastSpell -= AttackKeyRelease;
            this.input.CastBasicSpell -= SecondaryAttackKeyPress;
            this.input = null;
        }
    }
}
