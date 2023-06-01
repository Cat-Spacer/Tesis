using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] private TextMeshProUGUI nameText, dialogueText;
    [SerializeField, Range(0f, 60f)] private float textSpeed = 0.0f;
    [SerializeField] private Queue<string> sentences = new Queue<string>();
    [SerializeField] private Animator animator;
    public SaveManager saveManager;

    private void Awake()
    {
        Instance = this;

        if (!saveManager)
            saveManager = GetComponent<SaveManager>();
    }

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

    public void EndDialogue()
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