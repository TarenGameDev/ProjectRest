using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueWindow : MonoBehaviour
{
    [SerializeField] PrefabList _prefab;
    [SerializeField] TextMeshProUGUI _speakerText, _dialogueText;
    [SerializeField] Transform _optionsPanel;
    Transform _object;

    private void Start()
    {
        _object = transform.GetChild(0);
    }

    public void ShowWindow(Dialogue dialogue)
    {
        _object.gameObject.SetActive(true);
        ChangeDialogue(dialogue);
    }

    public void ChangeDialogue(Dialogue dialogue)
    {
        if(_optionsPanel.transform.childCount > 0)
            foreach(Transform child in _optionsPanel.transform)
                Destroy(child.gameObject);

        _speakerText.text = dialogue._title;
        _dialogueText.text = dialogue._text;
        for (int i = 0; i < dialogue._choices.Count; i++)
        {
            var obj = Instantiate(_prefab._dialogueOptionPrefab, _optionsPanel).GetComponent<DialogueChoiceComponent>();
            obj.Initialise(i, dialogue._choices[i]);

        }
    }

    public void CloseWindow()
    {
        _object.gameObject.SetActive(false);
    }

    
}

[System.Serializable]
public struct Dialogue
{
    public string _title;
    public string _text;
    public string _id;
    public List<DialogueChoice> _choices;
    public UnityEvent onClose;
}

[System.Serializable]
public struct DialogueChoice
{
    public string _text;
    public UnityEvent onChoose;
}
