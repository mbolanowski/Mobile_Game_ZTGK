using UnityEngine;

public class HealthbarScript : MonoBehaviour
{
    public RectTransform HealthBarHandle;

    public void ChangeHealthBarState(float percentValue)
    {
        float newValue = Mathf.Clamp01(percentValue);
        HealthBarHandle.localScale = new Vector3(newValue, 1, 1);
    }
}
