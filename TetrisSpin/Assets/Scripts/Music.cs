using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    private List<AudioClip> SEs = new List<AudioClip>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SE()
    {
        audioSource.clip = SEs[6];
        audioSource.Play();
    }
}
