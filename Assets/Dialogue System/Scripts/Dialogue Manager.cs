public class DialogueManager {
    #region Variables
    DialogueUIController uiController;
    bool isDialogueActive;
    #endregion

    public DialogueManager(DialogueUIController controller) {
        uiController = controller;
    }

    #region Utility Methods
    public void StartDialogue(DialogueData data) {
        if (isDialogueActive) return;
        isDialogueActive = true;

        uiController.OnDialogueFinished += HandleDialogueFinished;
        uiController.ShowDialogue(data);
    }

    private void HandleDialogueFinished() {
        EndDialogue();
    }

    public void EndDialogue() {
        isDialogueActive = false;
        uiController.HideDialogue();
    }
    #endregion
}
