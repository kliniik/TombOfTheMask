using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    private AudioSource aSource;

    [SerializeField]
    private AudioClip collectSound;

    public void Start()
    {
        aSource = GetComponent<AudioSource>();

        if (aSource == null)
        {
            Debug.LogError("ERROR: AudioSource not found");
        }

    }

    public void Collect()
    {
        aSource.PlayOneShot(collectSound);
        Destroy(gameObject, collectSound.length); // destroy the coin after the sound is played
    }
}

