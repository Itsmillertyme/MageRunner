using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    #region Variables
    //**FIELDS**
    [Header("Component References")]
    [SerializeField] Transform projectileSpawn;
    [SerializeField] Transform player;
    [SerializeField] RectTransform crosshairRect;
    [SerializeField] RectTransform loadingScreen;
    [SerializeField] Image imgLoadingWheel;
    [SerializeField] LevelGenerator levelCreator;
    [SerializeField] SaveManager saveManager;

    MusicManager musicManager;
    //
    Vector3 playerPivot;
    //
    ControlScheme currentScheme = ControlScheme.KEYBOARDMOUSE;
    //
    int currentLevel = 1;

    //DEV ONLY - REMOVE BEFORE BUILD
    [Header("DEV ONLY")]
    Transform cursorPositionObject;
    Transform playerPositionObject;
    public Mesh debugObjectMesh;
    public Material debugMaterial;
    bool debugMode;
    Material currentPathNodeOriginalMaterial;
    [SerializeField] Material playerRoomMaterial;

    //**PROPERTIES**
    public ControlScheme CurrentScheme { get => currentScheme; }
    public Vector3 CrosshairPositionIn3DSpace { get => cursorPositionObject.transform.position; }
    public Transform Player { get => player; }
    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }
    public RectTransform LoadingScreen { get => loadingScreen; set => loadingScreen = value; }

    #endregion

    #region Unity Methods
    private void Awake() {
        //DEV ONLY - REMOVE BEFORE BUILD - setup debug object
        cursorPositionObject = new GameObject("CursorPosObject", typeof(MeshFilter), typeof(MeshRenderer)).transform;
        cursorPositionObject.transform.parent = GameObject.FindWithTag("Player").transform;
        playerPositionObject = new GameObject("PlayerPosObject", typeof(MeshFilter), typeof(MeshRenderer)).transform;
        playerPositionObject.transform.parent = GameObject.FindWithTag("Player").transform;

        musicManager = GameObject.Find("Music Manager").GetComponent<MusicManager>();

    }
    //
    private void Start() {

        //load default spell and player SO values
        saveManager.InitilalizeDefaults();

        // Check if SplashManager requested a save load
        if (PlayerPrefs.HasKey("LoadSaveOnStart")) {
            int flag = PlayerPrefs.GetInt("LoadSaveOnStart", 0);
            if (flag == 1 && saveManager != null) {
                saveManager.LoadGame();
                PlayerPrefs.DeleteKey("LoadSaveOnStart");
                if (saveManager.DebugMode) Debug.Log("[GameManager] Loaded existing save on start.");
                return;
            }
        }

        // Otherwise start a new run
        StartNewGame();
    }
    //
    private void Update() {
        MoveProjectileSpawn();

        //Determine Input Scheme
        string controlScheme = player.GetComponent<PlayerInput>().currentControlScheme;
        if (controlScheme == "Keyboard and Mouse") {
            currentScheme = ControlScheme.KEYBOARDMOUSE;
        }
        else if (controlScheme == "Gamepad") {
            currentScheme = ControlScheme.GAMEPAD;
        }

    }
    //
    private void OnApplicationFocus(bool focus) {
        Cursor.visible = false;
    }
    //
    private void OnApplicationQuit() {
        if (saveManager != null) {
            saveManager.SaveGame();
            if (saveManager.DebugMode) Debug.Log("[GameManager] Autosaved on application quit.");
        }
    }
    #endregion

    #region Utility Methods
    void MoveProjectileSpawn() {

        //get mouse input position
        Vector3 screenPos = Vector3.zero;


        // Get position in center of player model
        playerPivot = new Vector3(player.position.x, player.position.y + 1.162f, 2.5f);
        // Get position of crosshair in world
        Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(null, crosshairRect.position);
        Vector3 worldPosition = Camera.main.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, Mathf.Abs(Camera.main.GetComponent<Camera>().transform.position.z - 2.5f)));
        worldPosition.z = 2.5f;

        //Set debug object positions
        playerPositionObject.position = playerPivot;
        cursorPositionObject.position = new Vector3(worldPosition.x, worldPosition.y, -2.5f);

        //Setup ray 
        Ray ray = new Ray(playerPivot, (cursorPositionObject.position - playerPivot).normalized);

        //spell spawn point offset from centermass of player        
        float offset = 1.25f;//DEFAULT IS .783f ONCE SPELL COLLISION DONE

        //move projectile spawn point
        Vector3 newPoint = ray.GetPoint(offset);
        projectileSpawn.transform.position = new Vector3(newPoint.x, newPoint.y, newPoint.z);

        //DEV ONLY - REMOVE BEFORE BUILD - draw ray
        //Debug.DrawRay(centerMass, debugObject.position - centerMass, Color.red);
    }
    //
    public Vector3 GetMousePositionInWorldSpace() {
        return cursorPositionObject.position;
    }
    //
    public Vector3 GetPlayerPivot() {
        return playerPivot;
    }
    //
    public void Quit() {
        Application.Quit();
    }
    //
    public void LoadMainMenu() {
        SceneManager.LoadScene("Splash");
    }
    //  
    public void RespawnPlayer() {
        //hide screen
        //ShowLoadingScreenForDuration(0.25f, 3f);

        //move player
        levelCreator.PlacePlayerInStartRoom();
    }
    //
    public void TeleportToBossRoom() {
        PlayerController pc = player.GetComponent<PlayerController>();
        CharacterController cc = player.GetComponent<CharacterController>();

        //freeze player and controller
        pc.InCutscene = true;
        cc.enabled = false;

        levelCreator.PlacePlayerInBossRoom();

        //unfreeze player 
        pc.InCutscene = false;
        cc.enabled = true;
    }
    //
    private void StartNewGame() {
        if (debugMode) Debug.Log("[GameManager] Starting new game.");

        //set default values
        CurrentLevel = 1;
        levelCreator.Seed = Random.Range(int.MinValue, int.MaxValue);
        levelCreator.GenerateLevel();

        if (saveManager != null) {
            saveManager.SaveGame();
        }
    }
    //


    #endregion
}

public enum ControlScheme {
    KEYBOARDMOUSE = 0,
    GAMEPAD = 1
}