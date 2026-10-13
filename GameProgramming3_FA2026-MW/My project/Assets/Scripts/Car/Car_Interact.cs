
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class Car_Interact : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public string interaction;
    public bool canInteract;
    public DialogueRunner dialogue;

    public void Start()
    {
        setText("");
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
                Debug.Log("Interact Car");
                setText("");

                if (!dialogue.IsDialogueRunning)
                {
                    dialogue.StartDialogue("CarStart");
                }

                canInteract = false;
            }
        }
    }

}
