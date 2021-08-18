using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Npc : Interactable
{
    private string[] m_npcDialogue;
    private Dialogue m_dialogue;

    //[SerializeField] private GameObject m_textObject;
    private Text m_textField;
    private int m_conversationNumber = 0;

    // When this is false the interaction will stop immediately.
    private bool wantsToInteract = true;
    private bool isInteracting = false;

    protected override void Start()
    {
        fuck = false;
        base.Start();
        m_dialogue = GetComponent<Dialogue>();
        m_npcDialogue = m_dialogue.dialogue;
        m_textField = m_dialogue.text;
    }

    protected override void Update()
    {
        base.Update();

        if (!MoreToDo())
        {
            StopInteracting();
        }
    }

    public override void Interact()
    {
        base.Interact();
   
        // Show the NPC's dialogue
        m_dialogue.textObject.SetActive(true);

        if (m_npcDialogue != null)
        {
            if (m_dialogue.textObject != null)
            {
                if (m_conversationNumber < m_npcDialogue.Length)
                {
                    m_textField.text = m_npcDialogue[m_conversationNumber];
                }
            }
        }

        if (m_conversationNumber < m_npcDialogue.Length)
        {
            //Debug.Log(m_conversationNumber);
            m_conversationNumber++;
        }
        else
        {
            wantsToInteract = false;
        }
    }

    public override void StopInteracting()
    {
        base.StopInteracting();
        m_conversationNumber = 0;
        wantsToInteract = true;
        m_dialogue.textObject.SetActive(false);
    }

    public override bool MoreToDo()
    {
        return wantsToInteract;
    }
}
