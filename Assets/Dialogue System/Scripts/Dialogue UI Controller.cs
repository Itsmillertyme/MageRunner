using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour {
    #region Variables
    [Header("UI References")]
    [SerializeField] GameObject dialogueUIPrefab;
    [SerializeField] Transform uiParent; // player HUD canvas

    [Header("UI Settings")]
    [SerializeField] float CharDelay = 0.03f;

    [Header("Misc References")]
    [SerializeField] PlayerController playerController;

    public event Action OnDialogueFinished;

    GameObject activeDialogueUI;
    TextMeshProUGUI dialogueText;
    TextMeshProUGUI speakerNameText;
    Image portraitImage;
    #endregion

    #region Utility Methods
    public void ShowDialogue(DialogueData data) {
        if (activeDialogueUI == null) {
            activeDialogueUI = Instantiate(dialogueUIPrefab, uiParent);
            var uiRefs = activeDialogueUI.GetComponent<DialogueUIReferences>();
            dialogueText = uiRefs.dialogueText;
            speakerNameText = uiRefs.speakerNameText;
            portraitImage = uiRefs.portraitImage;
        }

        StartCoroutine(PlayDialogue(data));
    }

    private IEnumerator PlayDialogue(DialogueData data) {
        playerController.InCutscene = true;
        foreach (var line in data.lines) {
            UpdateSpeakerUI(line);
            yield return TypeLine(line.lineText);
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        OnDialogueFinished?.Invoke();
        playerController.InCutscene = false;
    }

    private void UpdateSpeakerUI(DialogueLine line) {
        if (speakerNameText != null)
            speakerNameText.text = line.speakerName;

        if (portraitImage != null)
            portraitImage.sprite = line.portrait;
    }

    private IEnumerator TypeLine(string line) {
        dialogueText.text = "";
        foreach (char c in line) {
            dialogueText.text += c;
            yield return new WaitForSeconds(CharDelay);
        }
    }

    public void HideDialogue() {
        if (activeDialogueUI != null)
            Destroy(activeDialogueUI);
    }
    #endregion
}
