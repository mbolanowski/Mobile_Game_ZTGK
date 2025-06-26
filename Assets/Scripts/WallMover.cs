using TMPro;
using UnityEngine;
using DG.Tweening;

public class WallMover : MonoBehaviour
{
    public GameManager gm;

    public GameObject wall1;
    public GameObject wall2;
    public float moveDistance = 1f;
    public float moveDuration = 1f;

    private bool isMoving = false;
    public void WallMove()
    {
        if (isMoving) return;
        isMoving = true;

        // Move wall1 up
        wall1.transform.DOMoveY(wall1.transform.position.y + moveDistance, moveDuration)
            .SetEase(Ease.OutCubic);

        // Move wall2 down
        wall2.transform.DOMoveY(wall2.transform.position.y - moveDistance, moveDuration)
            .SetEase(Ease.OutCubic);
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.SetParent(null);
            isMoving = false;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gm.GameWon();
        }
    }
}
