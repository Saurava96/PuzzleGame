using DarkDemon;
using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DialogueQueue : MonoBehaviour
{
    public string[] Dialogues;

    public Queue<string> QueueOfDialogue;
    [SerializeField] ButtonManagerBasic ButtonBasic;

    private UIViewer Viewer;

    public UIViewer GetViewer()
    {
        if(Viewer == null) { Viewer = GetComponentInChildren<UIViewer>(); }

        return Viewer;

    }



    private void Start()
    {
        AddDialogues();
    }

    private void AddDialogues()
    {
        for(int i = 0; i < Dialogues.Length; i++)
        {
            QueueOfDialogue.Enqueue(Dialogues[i]);

        }

    }

    public void Speak()
    {
        if (QueueOfDialogue.Count <= 0) return;

        ButtonBasic.normalText.text = QueueOfDialogue.Dequeue();

    }

    
}
