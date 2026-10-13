
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class Wheel_Interact : MonoBehaviour
{
    public TextMeshProUGUI interactText;
    public string interaction;
    public bool canInteract;
    public DialogueRunner dialogue;

    public Wheel_Data wheelData;
    public string wheelID;

    public static Wheel_Interact currentWheel;

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
            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (!dialogue.IsDialogueRunning)
                {
                    currentWheel = this;

                    setText("");
                    canInteract = false;
                    Debug.Log("WHEEL");
                    dialogue.StartDialogue("WheelStart");
                }
            }
        }
    }

    [YarnCommand("collectWheel")]
    public static void collectWheel()
    {
        if (currentWheel != null)
        {
            GameController.instance.wheelCollect(currentWheel.wheelID);

            Destroy(currentWheel.gameObject);
            currentWheel = null;
        }
    }
}
