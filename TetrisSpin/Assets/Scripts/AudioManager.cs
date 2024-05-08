using System.Collections.Generic;
using UnityEngine;

// ゲームのSEに関してまとめたスクリプト //
// ボリュームやピッチの調節、再生などを行う

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // シングルトンインスタンス
    private AudioSource audioSource; // 主オーディオソース

    // オーディオ //
    [SerializeField] private AudioClip[] Audios;

    // オーディオの名前 //
    public string[] AudioNames =    // アルファベット順
    {
        "GameOver",
        "Hard_Drop",
        "Hold",
        "Move_Down",
        "Move_Left_Right",
        "Normal_Destroy",
        "Normal_Drop",
        "Rotation",
        "Spin",
        "Spin_Destroy",
        "Start_or_Retry",
        "Tetris"
    };

    // 辞書 //
    private Dictionary<string, AudioClip> AudioClipDictionary = new Dictionary<string, AudioClip>();
    // private Dictionary<string, int> AudioClipVolumeDictionary = new Dictionary<string, int>();

    // SEのボリュームの値 //
    float LowVolume = 0.2f;
    float MediumVolume = 0.5f;
    float HighVolume = 0.9f;
    int MaxVolume = 1;

    // SEのピッチの値 //
    float LowPitch = 0.6f;
    int NormalPitch = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わっても破壊されない
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }

        BuildAudioClipDictionary();
    }

    private void BuildAudioClipDictionary()
    {
        if (Audios.Length != AudioNames.Length)
        {
            Debug.LogError("Audios and AudioNames length mismatch!");
            return;
        }

        for (int i = 0; i < Audios.Length; i++)
        {
            if (AudioClipDictionary.ContainsKey(AudioNames[i]))
            {
                Debug.LogWarning($"{AudioNames[i]} はすでに登録されています。");
            }
            else
            {
                AudioClipDictionary.Add(AudioNames[i], Audios[i]);
            }
        }
    }

    // 名前に基づいてサウンドを再生
    public void PlaySound(string _AudioName)
    {
        if (AudioClipDictionary.TryGetValue(_AudioName, out AudioClip clip))
        {
            SetVolume(_AudioName);

            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"オーディオ {_AudioName} は見つかりませんでした。");
        }
    }

    // 音量を設定
    private void SetVolume(string _AudioName)
    {
        switch (_AudioName)
        {
            case "Start_or_Retry":
            case "Move_Down":
            case "Move_Left_Right":
                audioSource.volume = LowVolume;
                break;
            default:
                audioSource.volume = MediumVolume;
                break;
        }
    }
}


// public class SE : MonoBehaviour
// {
//     //AudioSource型の変数audioSourceを宣言
//     AudioSource audioSource;

//     //各種SEの番号振り分け
//     public int Start_or_Retry = 0;
//     public int GameOver = 1;
//     public int Move_Left_Right = 2;
//     public int Move_Down = 3;
//     public int Rotation = 4;
//     public int Spin = 5;
//     public int Normal_Drop = 6;
//     public int Hard_Drop = 7;
//     public int Normal_Destroy = 8;
//     public int Spin_Destroy = 9;
//     public int Tetris = 10;
//     public int Hold = 11;

//     //SEのボリュームの値
//     float LowVolume = 0.2f;
//     float MediumVolume = 0.5f;
//     float HighVolume = 0.9f;
//     int MaxVolume = 1;

//     //SEのピッチの値
//     float LowPitch = 0.6f;
//     int NormalPitch = 1;

//     //各種SEデータを、配列SEsに格納
//     [SerializeField]
//     private List<AudioClip> SEs = new List<AudioClip>();

//     private void Awake()
//     {
//         //audioSourceにAudioSourceコンポーネントを付与
//         audioSource = GetComponent<AudioSource>();
//     }

//     //呼び出されたSEを再生する関数
//     //ボリュームやピッチの調整もここで調整
//     public void CallSE(int selectSE)
//     {
//         //audioSourceのclipに選ばれたSEを格納
//         audioSource.clip = SEs[selectSE];

//         if (selectSE == Start_or_Retry)
//         {
//             audioSource.volume = LowVolume;
//         }
//         else if (selectSE == GameOver)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Move_Left_Right)
//         {
//             audioSource.volume = LowVolume;
//         }
//         else if (selectSE == Move_Down)
//         {
//             audioSource.volume = LowVolume;
//         }
//         else if (selectSE == Rotation)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Spin)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Normal_Drop)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Hard_Drop)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Normal_Destroy)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Spin_Destroy)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Tetris)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else if (selectSE == Hold)
//         {
//             audioSource.volume = MediumVolume;
//         }
//         else
//         {
//             Debug.Log("未設定のSE");
//         }

//         //設定したボリュームやピッチで再生
//         audioSource.PlayOneShot(audioSource.clip);
//     }
// }
