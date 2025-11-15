using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeveloperScript : MonoBehaviour
{
    #region SINGLETON // CALL INSTANCE
    private static DeveloperScript instance;

    public static DeveloperScript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<DeveloperScript>();
            }

            return instance;
        }
    }
    #endregion

    public string testGameObject = "";
    private GameObject testingObject; // OBJECT TO TEST
    public GameObject testTargetPosition;
    public Button testButton;
    public Button clearButton;
    public Button hideButton;
    public Button consoleClearButton;
    public Button consoleSendButton;
    public Button consoleVarButton;
    public TextMeshProUGUI consoleSetVarText;
    public KeyCode keyboardInput;
    public KeyCode mouseInput;
    private string debugMessage;
    public TextMeshProUGUI debugText;
    public TMP_InputField consoleInputField1; // METHOD
    public TMP_InputField consoleInputField2; // VAR
    public TMP_InputField consoleInputField3; // GAMEOBJECT TO FIND
    public bool debugEnabled = true;
    public bool debugHidden = false;
    private int debugCallCounter = 0;
    public GameObject[] uiElements;

    [Header("Testing Data")]
    public DialogueData testDialogueData;

    private void Awake()
    {
        if (!string.IsNullOrEmpty(testGameObject))
        {
            testingObject = GameObject.Find(testGameObject);
        }

        ConsoleSetVar();
        SetUIVis();
        HideButtons();
    }

    public void FindGameObject()
    {
        if (!string.IsNullOrEmpty(consoleInputField3.text))
        {
            testGameObject = consoleInputField3.text;
            testingObject = GameObject.Find(testGameObject);
            consoleInputField3.gameObject.SetActive(false);
        }
    }

    public void SendInvokeMessage()
    {
        string msg = consoleInputField1.text;

        if (string.IsNullOrEmpty(consoleInputField2.text))
        {
            testingObject.SendMessage(msg);
        }
        else
        {
            // LAZY CHANGING OF VALUES. 
            if (consoleSetVarText.text == "int")
            {
                var varType = int.Parse(consoleInputField2.text);
                testingObject.SendMessage(msg, varType);
            }
            else if (consoleSetVarText.text == "float")
            {
                var varType = float.Parse(consoleInputField2.text);
                testingObject.SendMessage(msg, varType);
            }
            else if (consoleSetVarText.text == "double")
            {
                var varType = double.Parse(consoleInputField2.text);
                testingObject.SendMessage(msg, varType);
            }
            else if (consoleSetVarText.text == "string")
            {
                var varType = consoleInputField2.text;
                testingObject.SendMessage(msg, varType);
            }
        }
    }

    public void Invoke() // BUTTON INVOKES THIS
    {
        SpawnLoot();
        //CreateDialogue();
    }

    public void Update() // KBM INPUT INVOKES THIS
    {
        // CHECK FOR INPUT
        if (Input.GetKeyDown(keyboardInput) && debugEnabled)
        {
            Invoke();
        }
        else if (Input.GetKeyDown(mouseInput) && debugEnabled)
        {
            Invoke();
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
        {
            consoleInputField3.gameObject.SetActive(true);
        }
    }

    private void SetUIVis()
    {
        for (int i = 0; i < uiElements.Length; i++)
        {
            uiElements[i].SetActive(debugEnabled);
        }
    }

    public void HideButtons()
    {
        debugHidden = !debugHidden;
        testButton.gameObject.SetActive(debugHidden);
        clearButton.gameObject.SetActive(debugHidden);
        consoleClearButton.gameObject.SetActive(debugHidden);
        consoleSendButton.gameObject.SetActive(debugHidden);
        consoleInputField1.gameObject.SetActive(debugHidden);
        consoleInputField2.gameObject.SetActive(debugHidden);
        consoleVarButton.gameObject.SetActive(debugHidden);

        RectTransform rectTransform = hideButton.GetComponent<RectTransform>();
        Image image = hideButton.GetComponent<Image>();

        if (debugHidden)
        {
            hideButton.GetComponentInChildren<TextMeshProUGUI>().text = "HIDE";
            rectTransform.sizeDelta = testButton.GetComponent<RectTransform>().sizeDelta;
            rectTransform.anchoredPosition = new(3.75f, -45f);
            image.color = testButton.image.color;
        }
        else
        {
            hideButton.GetComponentInChildren<TextMeshProUGUI>().text = ">";
            rectTransform.sizeDelta = new(35, 260);
            rectTransform.anchoredPosition = new(0, 0);
            image.color = new Color32(255, 255, 255, 30);
        }
    }

    public void ConsoleSetVar()
    {
        string Variable = consoleSetVarText.text;

        // LAZY CHANGING OF VALUES. 
        if (Variable == "int")
        {
            Variable = "float";
        }
        else if (Variable == "float")
        {
            Variable = "double";
        }
        else if (Variable == "double")
        {
            Variable = "string";
        }
        else if (Variable == "string")
        {
            Variable = "int";
        } 
        else if (Variable == "VAR") // FOR STARTUP
        {
            Variable = "int";
        }

        consoleSetVarText.text = Variable;
    }

    public void debug(string message)
    {
        debugMessage = message;
        Debug.Log(debugMessage);
    }

    public void debug(string message, bool printToScreen)
    {
        if (printToScreen)
        {
            debugCallCounter++;
            debugText.text = $"{message}: ({debugCallCounter})";
        }
        else
        {
            debugMessage = message;
            Debug.Log(debugMessage);
        }
    }

    public void ClearDebugToScreenText()
    {
        debugText.text = "";
        debugCallCounter = 0;
    }

    public void ClearConsole()
    {
        consoleInputField1.text = "";
        consoleInputField2.text = "";
    }

    #region DEBUGGING METHODS
    public void SpawnLoot()
    {
        LootSystem lotDropper = testingObject.GetComponent<LootSystem>();
        lotDropper.SelectThenDropLoot(testTargetPosition.transform.position);
    }

    public void CreateDialogue()
    {
        PlayerDialogueDriver pdd = testingObject.GetComponent<PlayerDialogueDriver>();
        pdd.TriggerDialogue(testDialogueData);
    }
    #endregion
}