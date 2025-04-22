using UnityEngine;

public class CameraAdjust : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float adjustAmount = 200f;
    void Start()
    {
        Camera.main.orthographicSize = Screen.height / adjustAmount; // Adjust 200f as needed
    }

}
