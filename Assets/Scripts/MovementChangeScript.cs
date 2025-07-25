using UnityEngine;

public class MovementChangeScript : MonoBehaviour
{
    //Swipe dash - 0
    public SwipeDash SwipeDash;
    public LineRenderer LineRenderer;

    //Slider movement - 1
    public SliderMovement SliderMove;
    public GameObject Slider;

    //Slider movement - 2
    public LineSwaping Surfers;

    //Invisible slider - 3
    public InvisibleSliderMovement InvisibleSliderMove;
    public GameObject Plane;

    int currentIndex = 0;
    int previousIndex = 0;
    public int indexLimit = 3;

    private void Change()
    {
        //Disable previous
        switch (previousIndex)
        {
            case 0:
                {
                    LineRenderer.enabled = false;
                    SwipeDash.enabled = false;
                    break;
                }
            case 1:
                {
                    SliderMove.enabled = false;
                    Slider.SetActive(false);
                    break;
                }
            case 2:
                {
                    Surfers.enabled = false;
                    break;
                }
            case 3:
                {
                    //InvisibleSliderMove.enabled = false;
                    Plane.SetActive(false);
                    break;
                }
            case 4:
                {
                    InvisibleSliderMove.enabled = false;
                    InvisibleSliderMove.CanUseWholeScreen = false;
                    break;
                }
        }
        //Enable new
        switch (currentIndex)
        {
            case 0:
                {
                    LineRenderer.enabled = true;
                    SwipeDash.enabled = true;
                    break;
                }
                case 1:
                {
                    SliderMove.enabled = true;
                    Slider.SetActive(true);
                    break;
                }
                case 2:
                {
                    Surfers.enabled = true;
                    break;
                }
                case 3:
                {
                    InvisibleSliderMove.enabled= true;
                    //Plane.SetActive(true);
                    break;
                }
                case 4:
                {
                    InvisibleSliderMove.CanUseWholeScreen = true;
                    break;
                }
        }
    }

    public void Next()
    {
        previousIndex = currentIndex;
        currentIndex++;
        if(currentIndex >= indexLimit) currentIndex = 0;
        Change();
    }
}
