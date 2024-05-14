using System.Collections.Generic;
using UnityEngine;

///// ゲームのオーディオに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// オーディオの再生
// ボリュームやピッチの調節


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

    // SEのボリュームの値 //
    float LowVolume = 0.2f;
    float MediumVolume = 0.5f;
    float HighVolume = 0.9f;
    int MaxVolume = 1;

    // SEのピッチの値 //
    float LowPitch = 0.6f;
    int NormalPitch = 1;

    // インスタンス化 //
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

        BuildAudioClipDictionary(); // 辞書の作成
    }

    // Audios と AudioNames の辞書を作成する関数 //
    private void BuildAudioClipDictionary()
    {
        if (Audios.Length != AudioNames.Length)
        {
            Debug.LogError("[AudioManager BuildAudioClipDictionary()] AudiosとAudioNamesの数が一致しません。");
            return;
        }

        for (int i = 0; i < Audios.Length; i++)
        {
            if (AudioClipDictionary.ContainsKey(AudioNames[i]))
            {
                Debug.LogWarning($"[AudioManager BuildAudioClipDictionary()] {AudioNames[i]} はすでに登録されています。");
            }
            else
            {
                AudioClipDictionary.Add(AudioNames[i], Audios[i]);
            }
        }
    }

    // 名前に基づいてサウンドを再生する関数 //
    public void PlaySound(string _AudioName)
    {
        if (AudioClipDictionary.TryGetValue(_AudioName, out AudioClip clip))
        {
            SetVolume(_AudioName); // 音量の調整

            audioSource.PlayOneShot(clip); // 出力
        }
        else
        {
            Debug.LogError($"[AudioManager PlaySound()] オーディオ {_AudioName} は見つかりませんでした。");
        }
    }

    // 音量を設定する関数 //
    private void SetVolume(string _AudioName)
    {
        switch (_AudioName)
        {
            case "Start_or_Retry":
            case "Move_Down":
            case "Move_Left_Right": // これらのオーディオは音量を下げるように調整
                audioSource.volume = LowVolume;
                break;

            default:
                audioSource.volume = MediumVolume;
                break;
        }
    }
}