using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public Movement playerMover;
    public FollowObject cameraMover;
    public Dialogue exampleDialogue;
    public Vector3 exampleAngle;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;

    bool inDialogue = false;
    Dialogue currentDialogue;
    ushort dialoguePosition = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartDialogue(exampleDialogue, exampleAngle);
    }

    // Update is called once per frame
    void Update()
    {
        if (inDialogue)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialoguePosition++;
                if (dialoguePosition < currentDialogue.dialogue.Length)
                {
                    dialogueText.text = currentDialogue.dialogue[dialoguePosition].speech;
                    nameText.text = currentDialogue.dialogue[dialoguePosition].speaker;
                }
                else
                {
                    EndDialogue();
                }
            }
        }
    }

    public void StartDialogue(Dialogue d, Vector3 cameraAngle)
    {
        playerMover.AllowMovement = false;
        cameraMover.InCutscene = true;
        cameraMover.cutscenePosition = cameraAngle;
        inDialogue = true;
        currentDialogue = d;
        dialoguePosition = 0;

        dialogueBox.SetActive(true);
        dialogueText.text = currentDialogue.dialogue[0].speech;
        nameText.text = currentDialogue.dialogue[0].speaker;
    }

    void EndDialogue()
    {
        playerMover.AllowMovement = true;
        cameraMover.InCutscene = false;
        inDialogue = false;
        dialogueBox.SetActive(false);
    }
}
