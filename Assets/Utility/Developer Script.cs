using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperScript : MonoBehaviour {
    #region SINGLETON // CALL INSTANCE
    private static DeveloperScript instance;

    public static DeveloperScript Instance {
        get {
            if (instance == null) {
                instance = FindFirstObjectByType<DeveloperScript>();
            }

            return instance;
        }
    }
    #endregion

    public GameObject testingObject;
    public GameObject testObjectPosition;
    public Button testButton;
    public Button clearButton;
    public Button hideButton;
    public KeyCode keyboardInput;
    public KeyCode mouseInput;
    private string debugMessage;
    public TextMeshProUGUI debugText;
    public bool debugEnabled = true;
    public bool debugHidden = false;
    private int debugCallCounter = 0;
    public GameObject[] uiElements;

    [Header("Testing Data")]
    public DialogueData testDialogueData;

    private void Awake() {
        SetUIVis();
        HideButtons();
    }

    #region METHOD TRIGGERS
    public void Invoke() // BUTTON INVOKES THIS
    {
        //SpawnLoot();
        CreateDialogue();
    }

    public void Update() // KBM INPUT INVOKES THIS
    {
        // CHECK FOR INPUT
        if (Input.GetKeyDown(keyboardInput) && debugEnabled) {
            Invoke();
        }
        else if (Input.GetKeyDown(mouseInput) && debugEnabled) {
            Invoke();
        }
    }
    #endregion

    #region SET UI VIS
    private void SetUIVis() {
        for (int i = 0; i < uiElements.Length; i++) {
            uiElements[i].SetActive(debugEnabled);
        }
    }

    public void HideButtons() {
        debugHidden = !debugHidden;
        testButton.gameObject.SetActive(debugHidden);
        clearButton.gameObject.SetActive(debugHidden);
        RectTransform rectTransform = hideButton.GetComponent<RectTransform>();
        Image image = hideButton.GetComponent<Image>();

        if (debugHidden) {
            hideButton.GetComponentInChildren<TextMeshProUGUI>().text = "HIDE";
            rectTransform.sizeDelta = testButton.GetComponent<RectTransform>().sizeDelta;
            rectTransform.anchoredPosition = new(3.75f, -45f);
            image.color = testButton.image.color;
        }
        else {
            hideButton.GetComponentInChildren<TextMeshProUGUI>().text = ">";
            rectTransform.sizeDelta = new(35, 260);
            rectTransform.anchoredPosition = new(0, 0);
            image.color = new Color32(255, 255, 255, 30);
        }
    }
    #endregion

    #region DEBUG
    public void debug(string message) {
        debugMessage = message;
        Debug.Log(debugMessage);
    }

    public void debug(string message, bool printToScreen) {
        if (printToScreen) {
            debugCallCounter++;
            debugText.text = $"{message}: ({debugCallCounter})";
        }
        else {
            debugMessage = message;
            Debug.Log(debugMessage);
        }
    }

    public void ClearDebugToScreenText() {
        debugText.text = "";
        debugCallCounter = 0;
    }
    #endregion

    #region DEBUGGING METHODS
    public void SpawnLoot()
    {
        LootSystem lotDropper = testingObject.GetComponent<LootSystem>();
        lotDropper.SelectThenDropLoot(testObjectPosition.transform.position);
    }

    public void CreateDialogue() {
        PlayerDialogueDriver pdd = testingObject.GetComponent<PlayerDialogueDriver>();
        pdd.TriggerDialogue(testDialogueData);
    }
    #endregion
}