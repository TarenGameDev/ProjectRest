using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueChoiceComponent : MonoBehaviour
{
    [HideInInspector] public DialogueChoice _choice;
    TextMeshProUGUI _text;

    public void Initialise(int i, DialogueChoice choice)
    {
        TryGetComponent(out _text);
        _text.text = (i+1).ToString() + ". " + choice._text;
        _choice = choice;
        TryGetComponent(out Button button);
        button.onClick.AddListener(_choice.onChoose.Invoke);
    }
}
