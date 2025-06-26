using UnityEngine;
using DG.Tweening;

public class CameraAdjust : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float adjustAmount = 200f;
    public float widthWorldSize = 6.0f;

    [SerializeField] private Vector3 targetLocalPosition = new Vector3(0f, 5f, -5f); // Desired relative position to player
    [SerializeField] private float zoomDuration = 1.5f;
    [SerializeField] private Ease easeType = Ease.OutCubic;

    void Start()
    {
        Camera.main.orthographicSize = widthWorldSize; // Adjust 200f as needed
        //Debug.Log(Screen.height);
        //Debug.Log(Screen.width);
        //Debug.Log(Camera.main.orthographicSize);
        transform.DOLocalMove(targetLocalPosition, zoomDuration).SetEase(easeType);
    }


    

}
