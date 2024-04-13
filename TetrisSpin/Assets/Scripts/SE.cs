using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    private List<AudioClip> SEs = new List<AudioClip>();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void CallSE(int i)
    {
        audioSource.clip = SEs[i];

        Debug.Log("CallSE");
        Debug.Log(i);

        switch (i)
        {
            case 1:
            case 2:
            case 3:
            case 16:
                audioSource.volume = 1;
                break;
            case 8:
            case 9:
            case 17:
                audioSource.volume = 0.4f;
                break;
            case 20:
                audioSource.volume = 1;
                audioSource.pitch = 1.3f;
                break;
        }
        audioSource.PlayOneShot(audioSource.clip);
        audioSource.pitch = 1;
    }
}
