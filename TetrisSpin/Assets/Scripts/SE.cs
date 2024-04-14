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

    //0 Start or Retry
    //1 GameOver
    //2 Move Left Right
    //3 Move Down
    //4 Rotation
    //5 Spin
    //6 Normal Drop
    //7 Hard Drop
    //8 Normal Destroy
    //9 Spin Destroy
    //10 Tetris!
    //11 Hold
    public void CallSE(int i)
    {
        audioSource.clip = SEs[i];

        Debug.Log("CallSE");
        Debug.Log(i);

        switch (i)
        {
            case 0:
            case 1:
            case 2:
            case 4:
            case 5:
            case 6:
            case 7:
            case 8:
            case 9:
            case 10:
                audioSource.volume = 0.5f;
                break;
            case 3:
                audioSource.volume = 0.2f;
                audioSource.pitch = 0.5f;
                break;
        }
        audioSource.PlayOneShot(audioSource.clip);
        audioSource.pitch = 1;
    }
}
