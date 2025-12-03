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
    TextMeshProUGUI dialogueTMP;
    TextMeshProUGUI speakerNameTMP;
    TextMeshProUGUI continuePromptTMP;
    Image portraitImage;
    #endregion

    #region Utility Methods
    public void ShowDialogue(DialogueData data) {
        if (activeDialogueUI == null) {
            activeDialogueUI = Instantiate(dialogueUIPrefab, uiParent);
            var uiRefs = activeDialogueUI.GetComponent<DialogueUIReferences>();
            dialogueTMP = uiRefs.dialogueTMP;
            speakerNameTMP = uiRefs.speakerNameTMP;
            continuePromptTMP = uiRefs.continuePromptTMP;
            portraitImage = uiRefs.portraitImage;
        }

        StartCoroutine(PlayDialogue(data));
    }

    private IEnumerator PlayDialogue(DialogueData data) {
        playerController.InCutscene = true;
        foreach (var line in data.lines) {
            UpdateSpeakerUI(line);
            yield return TypeLine(line.lineText);
            continuePromptTMP.color = new Color(continuePromptTMP.color.r, continuePromptTMP.color.g, continuePromptTMP.color.b, 1f);
            yield return new WaitUntil(() => Input.anyKeyDown);
        }
        OnDialogueFinished?.Invoke();
        playerController.InCutscene = false;
    }

    private void UpdateSpeakerUI(DialogueLine line) {
        if (speakerNameTMP != null)
            speakerNameTMP.text = line.speakerName;

        if (portraitImage != null)
            portraitImage.sprite = line.portrait;
    }

    private IEnumerator TypeLine(string line) {
        dialogueTMP.text = "";
        foreach (char c in line) {
            dialogueTMP.text += c;
            yield return new WaitForSeconds(CharDelay);
        }
    }

    public void HideDialogue() {
        if (activeDialogueUI != null)
            Destroy(activeDialogueUI);
    }
    #endregion
}
