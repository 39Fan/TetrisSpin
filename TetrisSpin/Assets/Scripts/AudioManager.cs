using System.Collections.Generic;
using UnityEngine;

/// <summary> オーディオ名 列挙型 </summary>
public enum AudioNames
{
    GameOver,
    HardDrop,
    Hold,
    MoveDown,
    MoveLeftRight,
    NormalDestroy,
    NormalDrop,
    Rotation,
    Spin,
    SpinDestroy,
    StartOrRetry,
    Tetris
}

/// <summary>
/// オーディオクリップの再生を管理するクラス<br/>
/// </summary>
/// <remarks>
/// ボリュームやピッチの調節も可能
/// </remarks>
public class AudioManager : MonoBehaviour
{
    /// <summary> シングルトンインスタンス </summary>
    public static AudioManager Instance;
    /// <summary> オーディオソース </summary>
    private AudioSource audioSource;

    /// <summary> 各種オーディオクリップの配列 </summary>
    /// <remarks>
    /// AudioNamesと対応
    /// </remarks>
    [SerializeField] private AudioClip[] Audios;

    /// <summary>AudioNames と AudioClip の辞書</summary>
    private Dictionary<AudioNames, AudioClip> AudioClipDictionary;

    /// <summary>低ボリュームの値(0.2f)</summary>
    private float LowVolume = 0.2f;
    /// <summary>中ボリュームの値(0.5f)</summary>
    private float MediumVolume = 0.5f;
    /// <summary>高ボリュームの値(0.7f)</summary>
    private float HighVolume = 0.7f;
    /// <summary>最大ボリュームの値(1)/// </summary>
    private int MaxVolume = 1;

    /// <summary> 初期化処理 </summary>
    /// <remarks>
    /// シングルトンインスタンスの設定と、オーディオクリップの辞書の生成も行う
    /// </remarks>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンが変わっても破壊されない
            audioSource = gameObject.AddComponent<AudioSource>();
            BuildAudioClipDictionary(); // 辞書の作成
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary> AudioName と AudioClip の辞書を作成する関数 </summary>
    private void BuildAudioClipDictionary()
    {
        AudioClipDictionary = new Dictionary<AudioNames, AudioClip>();

        if (Audios.Length != System.Enum.GetValues(typeof(AudioNames)).Length)
        {
            // Debug.LogError("[AudioManager BuildAudioClipDictionary()] Audiosの数とAudioNameの数が一致しません。");
            return;
        }

        for (int i = 0; i < Audios.Length; i++)
        {
            AudioNames audioName = (AudioNames)i;

            if (!AudioClipDictionary.ContainsKey(audioName))
            {
                AudioClipDictionary.Add(audioName, Audios[i]);
            }
            else
            {
                // Debug.LogWarning($"[AudioManager BuildAudioClipDictionary()] {audioName} はすでに登録されています。");
            }
        }
    }

    /// <summary> 指定されたオーディオクリップを再生する関数 </summary>
    /// <param name="audioName">再生するオーディオクリップの名前</param>
    public void PlaySound(AudioNames audioName)
    {
        if (AudioClipDictionary.TryGetValue(audioName, out AudioClip clip))
        {
            SetVolume(audioName); // 音量の調整

            audioSource.PlayOneShot(clip); // 出力
        }
        else
        {
            // Debug.LogError($"[AudioManager PlaySound()] オーディオ {_AudioName} は見つかりませんでした。");
        }
    }

    /// <summary> 音量を設定する関数 </summary>
    /// <param name="audioName">再生するオーディオクリップの名前</param>
    private void SetVolume(AudioNames audioName)
    {
        switch (audioName)
        {
            case AudioNames.StartOrRetry:
            case AudioNames.MoveDown:
            case AudioNames.MoveLeftRight: // これらのオーディオは音量を下げるように調整
                audioSource.volume = LowVolume;
                break;

            default:
                audioSource.volume = MediumVolume;
                break;
        }
    }
}

// // オーディオの名前 //
// public string[] AudioNames =
// {
//     "GameOver",
//     "Hard_Drop",
//     "Hold",
//     "Move_Down",
//     "Move_Left_Right",
//     "Normal_Destroy",
//     "Normal_Drop",
//     "Rotation",
//     "Spin",
//     "Spin_Destroy",
//     "Start_or_Retry",
//     "Tetris"
// };

// // SEのピッチの値 //
// float LowPitch = 0.6f;
// int NormalPitch = 1;
// // Audios と AudioNames の辞書を作成する関数 //
// private void BuildAudioClipDictionary()
// {
//     if (Audios.Length != AudioNames.Length)
//     {
//         Debug.LogError("[AudioManager BuildAudioClipDictionary()] AudiosとAudioNamesの数が一致しません。");
//         return;
//     }

//     for (int i = 0; i < Audios.Length; i++)
//     {
//         if (AudioClipDictionary.ContainsKey(AudioNames[i]))
//         {
//             Debug.LogWarning($"[AudioManager BuildAudioClipDictionary()] {AudioNames[i]} はすでに登録されています。");
//         }
//         else
//         {
//             AudioClipDictionary.Add(AudioNames[i], Audios[i]);
//         }
//     }
// }