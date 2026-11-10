using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hitbox : MonoBehaviour
{
    private Rigidbody body;
    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = true;
    }

    // the body.linearVelocity.y <= 0 check is to ensure that the player is moving upwards when hitting the block from below
    private void OnTriggerEnter(Collider other)
    {
        body = other.GetComponent<Collider>().attachedRigidbody;

        if (!body || !body.CompareTag("Player") || body.linearVelocity.y <= 0) return;

        IHittable block = GetComponentInParent<IHittable>();

        if (block != null)
        {
            block.HitFromBelow();
        }
    }
}
