using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePrefabComponent : DialogueSource
{
    [SerializeField] SO_DialoguePrefab dialoguePrefab;

    public override void CreateDialogue(string id = "0")
    {
        base.CreateDialogue(id);

        var window = FindObjectOfType<DialogueWindow>();
        var dialogue = dialoguePrefab.ReturnDialogue(id);
        window.ShowWindow(dialogue);
    }
}
