/// <summary>
/// Abstract class responsible for disabling the spell hit gameobject.
/// </summary>
public class SpellOnHitBehaviourDisable : SpellOnHitBehaviourSO
{
    public override void StartBehaviour(SpellOnHitBehaviour parent)
    {
        // Left blank on purpose
    }

    public override void ContinuousUpdateBehaviour(SpellOnHitBehaviour parent)
    {
        throw new System.NotImplementedException();
    }
}
