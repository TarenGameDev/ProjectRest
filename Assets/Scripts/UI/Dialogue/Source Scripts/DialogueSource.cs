using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSource : MonoBehaviour
{
    public virtual void CreateDialogue(string id = "0")
    {
        //Changes behaviour based on Scene or Prefab set up.
        //Prefab is much cleaner
    }

    [ContextMenu("Test")]
    public virtual void TestDialogue() => CreateDialogue();
}
