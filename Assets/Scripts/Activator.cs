using UnityEngine;

public class Activator : MonoBehaviour
{
    void Start()
    {
        int childCount = transform.childCount;

        if (childCount == 0)
        {
            Debug.LogWarning("No children found on this GameObject.");
            return;
        }

        int randomIndex = Random.Range(0, childCount);
        transform.GetChild(randomIndex).gameObject.SetActive(true);

        Debug.Log($"Activated child at index {randomIndex}: {transform.GetChild(randomIndex).name}");
    }
}
