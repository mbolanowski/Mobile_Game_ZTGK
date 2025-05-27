using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    public GameObject destroyedPrefab;
    public float maxForce = 500f;
    public float falloffRadius = 5f;
    public float destroyedLifetime = 5f;

    public TurningScript ts;

    private bool hasExploded = false;

    private void Start()
    {
        ts = FindFirstObjectByType<TurningScript>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded && collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            hasExploded = true;

            ts.BoostSpeed();

            Vector3 contactPoint = collision.GetContact(0).point;
            Vector3 forceDirection = -collision.transform.forward.normalized;

            GameObject destroyedWall = Instantiate(destroyedPrefab, transform.position, transform.rotation);

            // Schedule it for cleanup
            Destroy(destroyedWall, destroyedLifetime);

            // Apply force to pieces based on distance
            Rigidbody[] rigidbodies = destroyedWall.GetComponentsInChildren<Rigidbody>();
            foreach (Rigidbody rb in rigidbodies)
            {
                if (rb == null || rb.gameObject == collision.gameObject || collision.transform.IsChildOf(rb.transform))
                    continue;

                float distance = Vector3.Distance(rb.worldCenterOfMass, contactPoint);
                float forceScale = Mathf.Clamp01(1f - (distance / falloffRadius));
                Vector3 finalForce = forceDirection * maxForce * forceScale;

                rb.AddForce(finalForce, ForceMode.Impulse);
            }

            // Destroy the original wall
            Destroy(gameObject);
        }
    }
}
