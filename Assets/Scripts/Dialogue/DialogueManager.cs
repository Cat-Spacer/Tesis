using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText, dialogueText;
    [SerializeField, Range(0f, 60f)] private float textSpeed = 0.0f;
    [SerializeField] private Queue<string> sentences = new Queue<string>();
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator.SetBool("IsOpen", true);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.charName;

        sentences.Clear();

        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences == null) return;

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private void EndDialogue()
    {
        animator.SetBool("IsOpen", false);

    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
    }
}