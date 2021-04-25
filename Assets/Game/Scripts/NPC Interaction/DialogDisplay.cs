using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sins.Interaction;


public class DialogDisplay : Interactable
{

    public GameObject player;
    public GameObject DialogueScreen;

    public GameObject speakerLeft;
    public GameObject speakerRight;

    public Conversation conversation;

    private SpeakerUI speakerUILeft;
    private SpeakerUI speakerUIRight;

    private int activeLineIndex = 0;

    private bool isInteracting = false;

    void Start()
    {
        
        speakerUILeft = speakerLeft.GetComponent<SpeakerUI>();
        speakerUIRight = speakerRight.GetComponent<SpeakerUI>();

        speakerUILeft.Speaker = conversation.speakerLeft;
        speakerUIRight.Speaker = conversation.speakerRight;
        
        DialogueScreen.SetActive(false);

    }

    public override void Update()
    {
     
        base.Update();

        if(isInteracting == true)
        {

            if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                AdvanceConversation();
            }
        }

    }

    public override void Interact()
    {
        base.Interact();

        if(player != null)
        {

            Debug.Log("Interacting with npc");

            DialogueScreen.SetActive(true); 

            isInteracting = true;

        }

    }

    void AdvanceConversation()
    {

        if(activeLineIndex < conversation.lines.Length)
        {
            DisplayLine();
            activeLineIndex += 1;
        } else { 
            speakerUILeft.Hide();    
            speakerUIRight.Hide();
            activeLineIndex = 0;
        }

    }

    void DisplayLine()
    {
        Line line = conversation.lines[activeLineIndex];
        Character character = line.character;

        if (speakerUILeft.SpeakerIs(character))
        {
            SetDialog(speakerUILeft, speakerUIRight, line.text);
        }
        else
        {
            SetDialog(speakerUIRight, speakerUILeft, line.text);
        }
    }

    void SetDialog(SpeakerUI activeSpeakerUI, SpeakerUI inactiveSpeakerUI, string text)
    {
        activeSpeakerUI.Dialog = text;
        activeSpeakerUI.Show();
        inactiveSpeakerUI.Hide();
    }

}
