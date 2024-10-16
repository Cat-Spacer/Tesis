using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue;
    [SerializeField] private DialogueManager _dialogueManager;
    [SerializeField] private Image _image;

    private void Start()
    {
        _dialogueManager = DialogueManager.Instance;
    }

    public void TriggerDialogue()
    {
        if (_dialogue == null || !FindObjectOfType<DialogueManager>()) return;
        _dialogueManager.StartDialogue(_dialogue);
    }
    public void EndDialogue()
    {
        if (_dialogue == null || !FindObjectOfType<DialogueManager>()) return;
        _dialogueManager.EndDialogue();
    }

    private void LoadDialogue()
    {
        if (_dialogueManager == null) return;
        //var CharacterNameExpression = new Regex(@"Dialogue\((?<charName>[a-zA-Z0-9_]+)\)");

        foreach (var image in _dialogueManager.saveManager.LoadData().imageNames)
            if (image == _dialogue.charName)
                _image = Resources.Load("Art/Avatars" + image) as Image;

            foreach (var dialogues in _dialogueManager.saveManager.LoadData().dialogues)
                if (dialogues[0] == _dialogue.charName)
                    for (int i = 1; i < dialogues.Length; i++)
                        _dialogue.sentences.Add(dialogues[i]);
    }
}
