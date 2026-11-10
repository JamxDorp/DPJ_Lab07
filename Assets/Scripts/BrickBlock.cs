using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BrickBlock : MonoBehaviour , IHittable
{
    [SerializeField] private ParticleSystem breakEffect;
    [SerializeField] private AudioClip breakSfx;
    private bool isHit = false;

    public void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = false;
    }

    [ContextMenu("Hit From Below")]
    public void HitFromBelow()
    {
        if (isHit) return;
        isHit = true;

        

        if (breakEffect != null)
        {
            var fx = Instantiate(breakEffect, transform.position, Quaternion.identity);
            fx.Play();
            Destroy(fx.gameObject, fx.main.duration);
        }

        if (breakSfx != null)
        {
            AudioSource.PlayClipAtPoint(breakSfx, transform.position);
        }

        Destroy(gameObject);
    }
}
