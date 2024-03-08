using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "New_Dialogue", menuName = "List/Dialogues")]
public class SO_DialoguePrefab : ScriptableObject
{
    public List<Dialogue> dialogues = new();
    
    public Dialogue ReturnDialogue(string s)
    {
        foreach(var entry in dialogues)
        {
            if(entry._id == s)
                return entry;
        }
        Debug.LogWarning("No dialogue option found. " + this.name);
        return new Dialogue();
    }

    public void CloseWindow()
    {
        var window = FindObjectOfType<DialogueWindow>();
        window?.CloseWindow();
    }

    public void NextDialogue(string s)
    {
        var dialogue = ReturnDialogue(s);
        var window = FindObjectOfType<DialogueWindow>();
        window?.ChangeDialogue(dialogue);        
    }
}
