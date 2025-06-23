using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    void Update()
    {
        SceneManager.LoadScene(1);
    }
}
