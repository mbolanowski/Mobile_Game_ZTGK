using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public float RotationSpeed;
    public Transform modelHandle;
    public float healValue;

    private float currentRotation = 0;

    private void FixedUpdate()
    {
        currentRotation += RotationSpeed * Time.fixedDeltaTime;
        modelHandle.localRotation = Quaternion.Euler(0, currentRotation, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<TurningScript>().BoostSpeed();
            other.gameObject.GetComponent<PlayerHealth>().Heal(healValue);
            Destroy(gameObject);
        }
    }
}
