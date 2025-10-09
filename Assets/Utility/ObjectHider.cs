using UnityEngine;

public class ObjectHider : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToHideShow;
    private bool isActive;

    private void Awake()
    {
        isActive = objectsToHideShow[0].activeSelf;
        isActive = !isActive;

        foreach (GameObject obj in objectsToHideShow)
        {
            obj.SetActive(isActive);
        }
    }
}