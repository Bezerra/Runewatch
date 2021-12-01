using UnityEngine;

public class ContactPointDoor : MonoBehaviour
{
    private IPassageBlock passageBlock;

    private void Awake()
    {
        passageBlock = GetComponentInChildren<IPassageBlock>();
    }

    public void OpenPassage() =>
        passageBlock.Open();

    public void ClosePassage() =>
        passageBlock.Close();
}
