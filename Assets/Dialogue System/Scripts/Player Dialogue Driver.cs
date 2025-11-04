using UnityEngine;

public class PlayerDialogueDriver : MonoBehaviour {
    [SerializeField] private DialogueUIController dialogueUIController;
    private DialogueManager dialogueManager;

    private void Awake() {
        dialogueManager = new DialogueManager(dialogueUIController);
    }

    public void TriggerDialogue(DialogueData dialogue) {
        dialogueManager.StartDialogue(dialogue);
    }
}
