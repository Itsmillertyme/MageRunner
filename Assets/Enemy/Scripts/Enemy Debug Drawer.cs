//using UnityEngine;

//[ExecuteAlways]
//public class EnemyDebugDrawer : MonoBehaviour {
//    #region Variables
//    [Header("References")]
//    [SerializeField] private EnemyProfile profile;

//    [Header("Radius Colors")]
//    [SerializeField] private Color leashRadiusColor = Color.cyan;
//    [SerializeField] private Color aggroRadiusColor = Color.red;
//    [SerializeField] private Color minMeleeRangeColor = Color.yellow;
//    [SerializeField] private Color idealMeleeRangeColor = Color.green;
//    [SerializeField] private Color maxMeleeRangeColor = Color.yellow;
//    #endregion

//    #region Unity Methods
//    private void OnDrawGizmosSelected() {
//        if (profile == null || !profile.showCombatGizmos) return;

//        // center for all spheres
//        Vector3 center = transform.position + Vector3.up * 1.0f;

//        // ----- LEASH RADIUS -----
//        Gizmos.color = leashRadiusColor;
//        Gizmos.DrawWireSphere(center, profile.leashRadius);

//        // ----- AGGRO RADIUS -----
//        Gizmos.color = aggroRadiusColor;
//        Gizmos.DrawWireSphere(center, profile.aggroRadius);

//        // ----- MIN MELEE RANGE -----
//        Gizmos.color = minMeleeRangeColor;
//        Gizmos.DrawWireSphere(center, profile.meleeMinRange);

//        // ----- IDEAL MELEE RANGE -----
//        Gizmos.color = idealMeleeRangeColor;
//        Gizmos.DrawWireSphere(center, profile.meleeIdealRange);

//        // ----- MAX MELEE RANGE -----
//        Gizmos.color = maxMeleeRangeColor;
//        Gizmos.DrawWireSphere(center, profile.meleeMaxRange);
//    }

//    #endregion-
//}
