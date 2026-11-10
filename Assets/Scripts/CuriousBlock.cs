using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class CuriousBlock : MonoBehaviour , IHittable
{
    public enum Countained
    {
        None,
        Coin,
        PowerUp
    }

    [Header("Curious Settings")]
    [SerializeField] public Countained contained = Countained.Coin;

    [Header("Prefabs")]
    [SerializeField] public GameObject powerUpPrefab;
    [SerializeField] public GameObject coinPrefab;


    [Header("Animation Settings")]
    [SerializeField] public float coinLaunchForce = 5f;
    [SerializeField] public float powerUpRiseEffect = 2f;
    [SerializeField] public float popAnimationDuration = 0.5f;

    [Header("Visual Settings")]
    [SerializeField] public ParticleSystem popEffect;
    [SerializeField] public Material usedMaterial;
    [SerializeField] public AudioClip popSfx;

    private AudioSource audioSource;
    private bool isHit = false;


    public void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.isTrigger = false;

        audioSource = gameObject.AddComponent<AudioSource>();
    }

    [ContextMenu("Hit From Below")]
    public void HitFromBelow()
    {
        if (isHit) return;
        isHit = true;

        if (usedMaterial != null)
        {
            GetComponent<Renderer>().material = usedMaterial;
        }

        if (popEffect != null)
        {
            var fx = Instantiate(popEffect, transform.position + Vector3.up * 0.5f, Quaternion.identity);
            fx.Play();
            Destroy(fx.gameObject, fx.main.duration);
        }

        if (popSfx != null)
        {
            audioSource.PlayOneShot(popSfx);
        }

        switch (contained)
        {
            case Countained.Coin:
                SpawnCoin();
                break;
            case Countained.PowerUp:
                StartCoroutine(RisePowerUp(powerUpPrefab));
                break;
            case Countained.None:
            default:
                break;
        }
        StartCoroutine(Pop());
    }

    private void SpawnCoin()
    {
        if (coinPrefab != null)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position + Vector3.up, Quaternion.LookRotation(Vector3.up));
            Rigidbody coinRb = coin.GetComponent<Rigidbody>();
            if (coinRb != null)
            {
                coinRb.AddForce(Vector3.up * coinLaunchForce, ForceMode.Impulse);
            }
            Destroy(coin, 1f); 
            UIManager.Instance.AddScore(10);
        }
    }

    private IEnumerator Pop()
    {
        Vector3 start = transform.position;
        Vector3 top = start + Vector3.up * 0.3f;
        float t = 0f;
        while (t < popAnimationDuration)
        {
            t += Time.deltaTime;
            float k = t / popAnimationDuration;

            // k goes from 0 to 1, so k * PI goes from 0 to PI.
            // sin goes 0 -> 1 -> 0 over that range, so the block rises to "top"
            // at the halfway point and then comes back down to "start".
            transform.position = Vector3.Lerp(start, top, Mathf.Sin(k * Mathf.PI));

            // Wait until the next frame, then continue the loop because without it the animation will not play smoothly.
            yield return null;
        }
        transform.position = start;
    }

    private IEnumerator RisePowerUp(GameObject powerUpPrefab)
    {
        GameObject powerUp = Instantiate(powerUpPrefab, transform.position, Quaternion.identity);

        if (powerUp == null)
        {
            yield break;
        }

        Vector3 start = powerUp.transform.position;
        Vector3 top = start + Vector3.up * powerUpRiseEffect;
        float elapsed = 0f;

        while (elapsed < popAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float k = Mathf.Clamp01(elapsed / popAnimationDuration);
            powerUp.transform.position = Vector3.Lerp(start, top, k);
            yield return null;
        }

        powerUp.transform.position = top;
    }
}
