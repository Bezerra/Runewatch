/// <summary>
/// Class responsible for saving permanent character data.
/// </summary>
public class CharacterSaveData : AbstractSaveData
{
    // General public fields for JSON
    public byte[] CurrentSkillTreePassives;
    public int ArcanePower;

    public SkillTreeSaveData SkillTreeSaveData;
}
