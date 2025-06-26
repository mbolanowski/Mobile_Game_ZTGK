using UnityEngine;

public class TutorialStarter : MonoBehaviour
{
    public GameManager gm;

    public GameObject obj1;
    public GameObject obj2;
    public GameObject obj3;

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("???");
        if (other.CompareTag("Player"))
        {
            Debug.Log("!!!");
            gm.tutorial = false;
            obj1.SetActive(true);
            obj2.SetActive(true);
            obj3.SetActive(true);
        }
    }
}
