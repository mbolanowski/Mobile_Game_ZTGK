using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float adjustAmount = 200f;
    public float widthWorldSize = 6.0f;
    void Start()
    {
        Camera.main.orthographicSize = widthWorldSize; // Adjust 200f as needed
        //Debug.Log(Screen.height);
        //Debug.Log(Screen.width);
        //Debug.Log(Camera.main.orthographicSize);
    }

}
