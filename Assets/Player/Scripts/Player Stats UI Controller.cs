using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsUIController : MonoBehaviour {
    #region Variables
    [Header("References")]
    [SerializeField] Player playerAbilities;
    [SerializeField] Player defaultAbilities;
    [SerializeField] Button[] statButtons;

    [Header("Settings")]
    [SerializeField] Color barFillColor;

    List<PlayerStatMetadata> playerStatMetadatas;
    #endregion

    #region Unity Methods
    private void Awake() {
        //Construct metadata list
        playerStatMetadatas = new List<PlayerStatMetadata>();

        //Health Metadata

        //Update UI
        UpdateStatsUI();
    }
    #endregion

    #region Utility Methods
    private void UpdateStatsUI() {

    }

    #endregion

}
public struct PlayerStatMetadata {
    public string StatName;
    public int CurrentLevel;
    public int MaxLevel;

    public PlayerStatMetadata(string statNameIn, int currentLevelIn, int maxLevelIn) {
        StatName = statNameIn;
        CurrentLevel = currentLevelIn;
        MaxLevel = maxLevelIn;
    }
}