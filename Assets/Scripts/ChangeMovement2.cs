using UnityEngine;

public class ChangeMovement2 : MonoBehaviour
{
    public LineSwapping3D lineSwapping3D;
    public InvisibleSliderMovement invisibleSliderMovement;

    private bool SwappingActive = true;

    public void SwapMovement()
    {
        if (SwappingActive)
        {
            lineSwapping3D.enabled = false;
            invisibleSliderMovement.enabled = true;
        } else
        {
            lineSwapping3D.enabled=true;
            invisibleSliderMovement.enabled=false;   
        }
        SwappingActive = !SwappingActive;
    }

}
