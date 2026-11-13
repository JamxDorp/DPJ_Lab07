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

    // The break sound is played through the AudioManager singleton (a persistent
    // GameObject in the scene) using PlayOneShot, not PlayClipAtPoint, so its
    // volume is fixed and no longer depends on the listener/camera position.
    // PlayOneShot plays the clip independently on that AudioSource, so destroying
    // this Brick Block right after does not cut the sound short — it plays in full.
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
            AudioManager.Instance.PlaySound(breakSfx);
        }

        Destroy(gameObject);
    }
}
