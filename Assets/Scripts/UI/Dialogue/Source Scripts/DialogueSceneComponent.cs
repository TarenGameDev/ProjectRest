using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSceneComponent : DialogueSource
{
    [SerializeField] List<Dialogue> _dialogues = new();

    public override void CreateDialogue(string id = "0")
    {
        base.CreateDialogue(id);
        var dialogueManager = FindObjectOfType<DialogueWindow>();

        foreach (var entry in _dialogues)
        {
            if (entry._id == id)
                dialogueManager.ShowWindow(entry);
        }
    }
}
