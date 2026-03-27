using System.Collections;
using UnityEngine;

public class FireWork : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireworkLeft;
    [SerializeField] private ParticleSystem fireworkRight;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private int howManyShots = 3;
    [SerializeField] private float delayBetweenShots = 2f;

    private void Start()
    {
        StartCoroutine(PlayFireworks());
    }

    private IEnumerator PlayFireworks()
    {
        for (int i = 0; i < howManyShots; i++)
        {
            fireworkLeft.Play();
            fireworkRight.Play();
            audioSource.Play();

            yield return new WaitForSeconds(delayBetweenShots);
        }
    }
}