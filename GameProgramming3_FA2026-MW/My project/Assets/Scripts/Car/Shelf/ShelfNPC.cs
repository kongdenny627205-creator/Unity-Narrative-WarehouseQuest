
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;
using System.Collections;

public class ShelfNPC : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public string interaction = "E";
    public bool canInteract;
    public DialogueRunner dialogue;
    public NPC_Data npcData;



    public Transform shelf;
    public Transform shelfMoveLoc;
    public float moveSpeed = 2f;

    public void Start()
    {
        setText("");
        canInteract = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            setText(interaction);
            canInteract = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            setText("");
            canInteract = false;
        }
    }

    public void setText(string txt)
    {
        interactText.text = txt;
    }

    public void Update()
    {
        if (canInteract)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (!dialogue.IsDialogueRunning)
                {
                    Debug.Log("Talked to Shelf NPC");

                    setText("");
                    dialogue.StartDialogue(npcData.startingNode);
                    canInteract = false;
                }
            }
        }
    }

    [YarnCommand("moveShelf")]
    public IEnumerator moveShelf()
    {
        while (Vector3.Distance(shelf.position,
            shelfMoveLoc.position) > 0.01f)
        {
            shelf.position = Vector3.MoveTowards(
                shelf.position,
                shelfMoveLoc.position,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        shelf.position = shelfMoveLoc.position;
    }

    [YarnCommand("setShelfPhase")]
    public void setPhase()
    {
        InMemoryVariableStorage vData =
            GameObject.FindAnyObjectByType<InMemoryVariableStorage>();

        vData.SetValue("$shelfPhase",
            npcData.currentPhase.ToString());
    }

    [YarnCommand("getShelfPhase")]
    public void getPhase(string phase)
    {
        if (phase == "quest_complete_return")
        {
            npcData.currentPhase =
                NPC_Data.dialogPhase.quest_complete_return;
        }
    }

    public void OnApplicationQuit()
    {
        npcData.resetData();
    }
}
