using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

/// <summary>
/// DisplayManagerの統計情報を保持する静的クラス
/// </summary>
internal static class GameDisplayManagerStats
{
    // SpinIconの判定 //
    private static bool i_spinIcon = false;
    private static bool j_spinIcon = false;
    private static bool l_spinIcon = false;
    private static bool s_spinIcon = false;
    private static bool t_spinIcon = false;
    private static bool z_spinIcon = false;

    // SpinCompleteのリーチ判定 //
    private static bool spinCompleteReach = false;

    // SpinCompleteがリーチの際に点滅させる色 //
    private static Color reachColor;

    // SpinComplete判定 //
    private static bool spinComplete = false;

    // ゲッタープロパティ //
    public static bool I_spinIcon => i_spinIcon;
    public static bool J_spinIcon => j_spinIcon;
    public static bool L_spinIcon => l_spinIcon;
    public static bool S_spinIcon => s_spinIcon;
    public static bool T_spinIcon => t_spinIcon;
    public static bool Z_spinIcon => z_spinIcon;
    public static bool SpinCompleteReach => spinCompleteReach;
    public static Color ReachColor => reachColor;
    public static bool SpinComplete => spinComplete;

    /// <summary> スタッツログの詳細 </summary>
    private static string logStatsDetail;

    /// <summary> 指定されたフィールドの値を更新する関数 </summary>
    /// <remarks>
    /// 指定されていない引数は現在の値を維持
    /// </remarks>
    public static void UpdateStats(
        bool? _i_spinIcon = null, bool? _j_spinIcon = null, bool? _l_spinIcon = null, bool? _s_spinIcon = null,
        bool? _t_spinIcon = null, bool? _z_spinIcon = null, bool? _spinCompleteReach = null, Color? _reachColor = null,
        bool? _spinComplete = null)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManagerStats, eMethod.UpdateStats, eLogTitle.Start);

        i_spinIcon = _i_spinIcon ?? i_spinIcon;
        j_spinIcon = _j_spinIcon ?? j_spinIcon;
        l_spinIcon = _l_spinIcon ?? l_spinIcon;
        s_spinIcon = _s_spinIcon ?? s_spinIcon;
        t_spinIcon = _t_spinIcon ?? t_spinIcon;
        z_spinIcon = _z_spinIcon ?? z_spinIcon;
        spinCompleteReach = _spinCompleteReach ?? spinCompleteReach;
        reachColor = _reachColor ?? reachColor;
        spinComplete = _spinComplete ?? spinComplete;

        logStatsDetail = $"i_spinIcon : {i_spinIcon}, j_spinIcon : {j_spinIcon}, l_spinIcon : {l_spinIcon}, s_spinIcon : {s_spinIcon}, t_spinIcon : {t_spinIcon}, z_spinIcon : {z_spinIcon}, spinCompleteReach : {spinCompleteReach}, reachColor : {reachColor}, spinComplete : {spinComplete}";
        LogHelper.InfoLog(eClasses.GameDisplayManagerStats, eMethod.UpdateStats, eLogTitle.StatsInfo, logStatsDetail);

        LogHelper.DebugLog(eClasses.GameDisplayManagerStats, eMethod.UpdateStats, eLogTitle.End);
    }

    /// <summary> false状態のスピンアイコンを確認する関数 </summary>
    /// <remarks>
    /// false状態のSpinIconのImageをoutする。
    /// </remarks>
    /// <returns> falseの数(int) </returns>
    public static int CheckFalseSpinIconCount(Dictionary<SpinTypeNames, Image> spinIconImages, out Image falseIconImage)
    {
        falseIconImage = null;
        int falseCount = 0;

        if (!i_spinIcon)
        {
            falseCount++;
            falseIconImage = spinIconImages[SpinTypeNames.Ispin];
        }
        if (!j_spinIcon)
        {
            falseCount++;
            falseIconImage = spinIconImages[SpinTypeNames.Jspin];
        }
        if (!l_spinIcon)
        {
            falseCount++;
            falseIconImage = spinIconImages[SpinTypeNames.Lspin];
        }
        if (!s_spinIcon)
        {
            falseCount++;
            falseIconImage = spinIconImages[SpinTypeNames.Sspin];
        }
        if (!t_spinIcon)
        {
            falseCount++;
            falseIconImage = spinIconImages[SpinTypeNames.Tspin];
        }
        if (!z_spinIcon)
        {
            falseCount++;
            falseIconImage = spinIconImages[SpinTypeNames.Zspin];
        }

        return falseCount;
    }

    /// <summary> デフォルトの <see cref="GameDisplayManagerStats"/> にリセットする関数 </summary>
    public static void ResetStats()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManagerStats, eMethod.ResetStats, eLogTitle.Start);

        i_spinIcon = false;
        j_spinIcon = false;
        l_spinIcon = false;
        s_spinIcon = false;
        t_spinIcon = false;
        z_spinIcon = false;
        spinCompleteReach = false;
        reachColor = Color.clear; // Color.clear で透明な色に初期化
        spinComplete = false;

        LogHelper.DebugLog(eClasses.GameDisplayManagerStats, eMethod.ResetStats, eLogTitle.End);
    }
}



/// <summary>
/// ゲーム画面のテキストおよび数値を管理するクラス
/// </summary>
public class GameDisplayManager : MonoBehaviour
{
    // Canvas //TODO
    [SerializeField] private RectTransform canvas;
    /// <summary> PoseIconが押された時に表示するCanvas </summary>
    [SerializeField] private Canvas poseCanvas;
    /// <summary> PoseIconが押された時に表示するImage </summary>
    [SerializeField] private Image poseBackGround;

    // Panel //
    [SerializeField] private RectTransform gameBoardPanel;
    [SerializeField] private RectTransform spinTextsPanel;

    // フレームのImage //
    [SerializeField] private Image gameFrameImage;
    [SerializeField] private Image spinTextsFrameImage1;
    [SerializeField] private Image spinTextsFrameImage2;

    // SpinIconのImage //
    [SerializeField] private Image i_spinIconImage;
    [SerializeField] private Image j_spinIconImage;
    [SerializeField] private Image l_spinIconImage;
    [SerializeField] private Image s_spinIconImage;
    [SerializeField] private Image t_spinIconImage;
    [SerializeField] private Image z_spinIconImage;

    /// <summary> SpinIconのリスト </summary>
    List<Image> spinIconImages;

    // 数値のテキスト //
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI attackLinesText;
    [SerializeField] private TextMeshProUGUI sumAttackLinesText;
    [SerializeField] private TextMeshProUGUI renText;

    // 表示できるテキスト //
    [SerializeField] private TextMeshProUGUI readyText;
    [SerializeField] private TextMeshProUGUI goText;
    [SerializeField] private TextMeshProUGUI backToBackText;
    [SerializeField] private TextMeshProUGUI perfectClearText;
    [SerializeField] private TextMeshProUGUI tetrisText;
    [SerializeField] private TextMeshProUGUI spinCompleteText;
    [SerializeField] private TextMeshProUGUI i_spinText;
    [SerializeField] private TextMeshProUGUI i_spinSingleText;
    [SerializeField] private TextMeshProUGUI i_spinDoubleText;
    [SerializeField] private TextMeshProUGUI i_spinTripleText;
    [SerializeField] private TextMeshProUGUI i_spinQuattroText;
    [SerializeField] private TextMeshProUGUI i_spinMiniText;
    [SerializeField] private TextMeshProUGUI j_spinText;
    [SerializeField] private TextMeshProUGUI j_spinSingleText;
    [SerializeField] private TextMeshProUGUI j_spinDoubleText;
    [SerializeField] private TextMeshProUGUI j_spinTripleText;
    [SerializeField] private TextMeshProUGUI j_spinMiniText;
    [SerializeField] private TextMeshProUGUI j_spinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI l_spinText;
    [SerializeField] private TextMeshProUGUI l_spinSingleText;
    [SerializeField] private TextMeshProUGUI l_spinDoubleText;
    [SerializeField] private TextMeshProUGUI l_spinTripleText;
    [SerializeField] private TextMeshProUGUI l_spinMiniText;
    [SerializeField] private TextMeshProUGUI l_spinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI s_spinText;
    [SerializeField] private TextMeshProUGUI s_spinSingleText;
    [SerializeField] private TextMeshProUGUI s_spinDoubleText;
    [SerializeField] private TextMeshProUGUI s_spinTripleText;
    [SerializeField] private TextMeshProUGUI s_spinMiniText;
    [SerializeField] private TextMeshProUGUI s_spinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI t_spinText;
    [SerializeField] private TextMeshProUGUI t_spinSingleText;
    [SerializeField] private TextMeshProUGUI t_spinDoubleText;
    [SerializeField] private TextMeshProUGUI t_spinTripleText;
    [SerializeField] private TextMeshProUGUI t_spinMiniText;
    [SerializeField] private TextMeshProUGUI t_spinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI z_spinText;
    [SerializeField] private TextMeshProUGUI z_spinSingleText;
    [SerializeField] private TextMeshProUGUI z_spinDoubleText;
    [SerializeField] private TextMeshProUGUI z_spinTripleText;
    [SerializeField] private TextMeshProUGUI z_spinMiniText;
    [SerializeField] private TextMeshProUGUI z_spinDoubleMiniText;

    // 各ミノに対応する色 //
    private readonly Color i_Color = new Color(0f, 0.97f, 1f); // #00F7FF
    private readonly Color j_Color = new Color(0f, 0.02f, 1f); // #0005FF
    private readonly Color l_Color = new Color(1f, 0.47f, 0f); // #FF7900
    private readonly Color s_Color = new Color(0.24f, 1f, 0f); // #3EFF00
    private readonly Color t_Color = new Color(0.52f, 0f, 1f); // #8500FF
    private readonly Color z_Color = new Color(1f, 0f, 0.07f); // #FF0011

    /// <summary> ミノの色のリスト </summary>
    private Color[] colors;

    /// <summary> SpinTypeNamesからColorに対応した辞書 </summary>
    private Dictionary<SpinTypeNames, Color> spinTypeNamesToColorDictionary;
    /// <summary> SpinTypeNamesからImageに対応した辞書 </summary>
    private Dictionary<SpinTypeNames, Image> spinTypeNamesToImageDictionary;
    /// <summary> imageからSpinTypeNamesに対応した辞書 </summary>
    private Dictionary<Image, SpinTypeNames> imageToSpinTypeNamesDictionary;

    /// <summary> 点滅させるspinIcon </summary>
    private Image falseIconImage;

    /// <summary> 攻撃ライン数テキストのTween </summary>
    private Tween attackLinesTextTween;

    /// <summary> タイマーの開始時刻 </summary>
    private float startTime;
    /// <summary> ポーズ時のタイマーの時間を記録する変数 </summary>
    private float pauseTime;
    /// <summary> 累積のポーズ時間 </summary>
    private float totalPauseDuration;

    /// <summary> 乱数 </summary>
    private System.Random random;

    // 干渉するクラス
    private Effects effects;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        effects = FindObjectOfType<Effects>();

        colors = new Color[]
        {
            i_Color,
            j_Color,
            l_Color,
            s_Color,
            t_Color,
            z_Color
        };

        spinTypeNamesToColorDictionary = new Dictionary<SpinTypeNames, Color>
        {
            { SpinTypeNames.Ispin, i_Color },
            { SpinTypeNames.IspinMini, i_Color },
            { SpinTypeNames.Jspin, j_Color },
            { SpinTypeNames.Lspin, l_Color },
            { SpinTypeNames.Sspin, s_Color },
            { SpinTypeNames.SspinMini, s_Color },
            { SpinTypeNames.Tspin, t_Color },
            { SpinTypeNames.TspinMini, t_Color },
            { SpinTypeNames.Zspin, z_Color },
            { SpinTypeNames.ZspinMini, z_Color }
        };

        spinTypeNamesToImageDictionary = new Dictionary<SpinTypeNames, Image>
        {
            { SpinTypeNames.Ispin, i_spinIconImage },
            { SpinTypeNames.IspinMini, i_spinIconImage },
            { SpinTypeNames.Jspin, j_spinIconImage },
            { SpinTypeNames.Lspin, l_spinIconImage },
            { SpinTypeNames.Sspin, s_spinIconImage },
            { SpinTypeNames.SspinMini, s_spinIconImage },
            { SpinTypeNames.Tspin, t_spinIconImage },
            { SpinTypeNames.TspinMini, t_spinIconImage },
            { SpinTypeNames.Zspin, z_spinIconImage },
            { SpinTypeNames.ZspinMini, z_spinIconImage }
        };

        spinIconImages = new List<Image>
        {
            i_spinIconImage,
            j_spinIconImage,
            l_spinIconImage,
            s_spinIconImage,
            t_spinIconImage,
            z_spinIconImage
        };

        // ImageからSpinTypeNamesを取得するための逆引き辞書を初期化
        imageToSpinTypeNamesDictionary = new Dictionary<Image, SpinTypeNames>();
        foreach (var pair in spinTypeNamesToImageDictionary)
        {
            imageToSpinTypeNamesDictionary[pair.Value] = pair.Key;
        }

        random = new System.Random();
    }

    /// <summary>
    /// 現在時刻を取得
    /// </summary>
    private void Start()
    {
        startTime = Time.time;
    }

    /// <summary>
    /// タイマーとSpinIconEffectを表示
    /// </summary>
    private void Update()
    {
        if (!GameSceneManagerStats.GameOverScene && !GameSceneManagerStats.GameClearScene)
        {
            DisplayTimer();
            if (GameSceneManagerStats.PoseState)
            {
                // ポーズが開始されたとき
                if (pauseTime == 0f)
                {
                    pauseTime = Time.time; // ポーズ開始時の時間を記録
                }
            }
            else
            {
                // ポーズが解除されたとき
                if (pauseTime > 0f)
                {
                    totalPauseDuration += Time.time - pauseTime; // ポーズ期間を累積
                    pauseTime = 0f; // ポーズ時間をリセット
                }
                DisplayTimer();
            }
        }
    }

    /// <summary>
    /// フレームごとにタイマーを表示させる
    /// </summary>
    private void DisplayTimer()
    {
        float t = Time.time - startTime;

        string minutes = ((int)t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        timerText.text = $"{minutes}:{seconds}";
    }

    /// <summary> スピンに関するアニメーションを呼ぶ関数 </summary>
    /// <param name="spinTypeName"> スピンタイプ </param>
    /// <param name="lineClearCount"> 消去ライン数 </param>
    public void SpinAnimation(SpinTypeNames spinTypeName, int lineClearCount)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinAnimation, eLogTitle.Start);

        SpinTextAnimation(DetermineTextToDisplay(spinTypeName, lineClearCount));
        SpinColorAnimation(spinTypeName);

        if (spinTypeName != SpinTypeNames.None)
        {
            if (lineClearCount >= 1)
            {
                AudioManager.Instance.PlaySound(eAudioName.SpinDestroy);
            }
            else
            {
                AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
            }
        }
        else
        {
            if (lineClearCount >= 1)
            {
                AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
            }
            else
            {
                AudioManager.Instance.PlaySound(eAudioName.NormalDrop);
            }
            switch (lineClearCount)
            {
                case 0:
                    AudioManager.Instance.PlaySound(eAudioName.NormalDrop);
                    break;
                case 1:
                case 2:
                case 3:
                    AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
                    break;
                case 4:
                    AudioManager.Instance.PlaySound(eAudioName.Tetris);
                    break;
            }
        }

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinAnimation, eLogTitle.End);
    }



    /// <summary> スピンタイプに応じて表示するテキストを決定する関数 </summary>
    /// <param name="spinTypeName"> スピンタイプ </param>
    /// <param name="lineClearCount"> 消去ライン数 </param>
    /// <returns> 表示するテキスト </returns>
    private TextMeshProUGUI DetermineTextToDisplay(SpinTypeNames spinTypeName, int lineClearCount)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.DetermineTextToDisplay, eLogTitle.Start);

        Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>> spinTypeTextMapping = new Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>>
        {
            { SpinTypeNames.Ispin, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, i_spinText },
                    { 1, i_spinSingleText },
                    { 2, i_spinDoubleText },
                    { 3, i_spinTripleText },
                    { 4, i_spinQuattroText }
                }
            },
            { SpinTypeNames.IspinMini, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, i_spinMiniText },
                    { 1, i_spinMiniText }
                }
            },
            { SpinTypeNames.Jspin, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, j_spinText },
                    { 1, j_spinSingleText },
                    { 2, j_spinDoubleText },
                    { 3, j_spinTripleText }
                }
            },
            // { SpinTypeNames.JspinMini, new Dictionary<int, TextMeshProUGUI>
            //     {
            //         { 0, j_spinMiniText },
            //         { 1, j_spinMiniText },
            //         { 2, j_spinDoubleMiniText }
            //     }
            // },
            { SpinTypeNames.Lspin, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, l_spinText },
                    { 1, l_spinSingleText },
                    { 2, l_spinDoubleText },
                    { 3, l_spinTripleText }
                }
            },
            // { SpinTypeNames.LspinMini, new Dictionary<int, TextMeshProUGUI>
            //     {
            //         { 0, l_spinMiniText },
            //         { 1, l_spinMiniText },
            //         { 2, l_spinDoubleMiniText }
            //     }
            // },
            { SpinTypeNames.Sspin, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, s_spinText },
                    { 1, s_spinSingleText },
                    { 2, s_spinDoubleText },
                    { 3, s_spinTripleText }
                }
            },
            { SpinTypeNames.SspinMini, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, s_spinMiniText },
                    { 1, s_spinMiniText },
                    { 2, s_spinDoubleMiniText },
                }
            },
            { SpinTypeNames.Tspin, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, t_spinText },
                    { 1, t_spinSingleText },
                    { 2, t_spinDoubleText },
                    { 3, t_spinTripleText }
                }
            },
            { SpinTypeNames.TspinMini, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, t_spinMiniText },
                    { 1, t_spinMiniText },
                    { 2, t_spinDoubleMiniText }
                }
            },
            { SpinTypeNames.Zspin, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, z_spinText },
                    { 1, z_spinSingleText },
                    { 2, z_spinDoubleText },
                    { 3, z_spinTripleText }
                }
            },
            { SpinTypeNames.ZspinMini, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, z_spinMiniText },
                    { 1, z_spinMiniText },
                    { 2, z_spinDoubleMiniText },
                }
            },
            { SpinTypeNames.None, new Dictionary<int, TextMeshProUGUI>
                {
                    { 0, null },
                    { 1, null },
                    { 2, null },
                    { 3, null },
                    { 4, tetrisText }
                }
            }
        };

        TextMeshProUGUI displayText = null;

        if (spinTypeTextMapping.ContainsKey(spinTypeName) && spinTypeTextMapping[spinTypeName].ContainsKey(lineClearCount))
        {
            if (spinTypeTextMapping[spinTypeName][lineClearCount] == tetrisText)
            {
                TetrisAnimation();
            }
            else
            {
                displayText = spinTypeTextMapping[spinTypeName][lineClearCount];
            }
        }
        else
        {
            LogHelper.ErrorLog(eClasses.GameDisplayManager, eMethod.DetermineTextToDisplay, eLogTitle.KeyNotFound);
        }

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.DetermineTextToDisplay, eLogTitle.End);
        return displayText;
    }

    /// <summary> スピンテキストのアニメーションを決定する関数 </summary>
    /// <param name="displayText"> 表示するテキスト </param>
    private void SpinTextAnimation(TextMeshProUGUI displayText)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinTextAnimation, eLogTitle.Start);

        if (displayText != null)
        {
            TextMeshProUGUI instantiatedText = Instantiate(displayText, spinTextsPanel);
            TextFadeInAndOutType1(instantiatedText);
            effects.SpinEffect();
        }

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinTextAnimation, eLogTitle.End);
    }

    /// <summary> スピンの色表示に関するアニメーションを決定する関数 </summary>
    /// <param name="spinTypeName"> スピンタイプ </param>
    /// <returns> 表示するテキスト </returns>
    private void SpinColorAnimation(SpinTypeNames spinTypeName)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinColorAnimation, eLogTitle.Start);

        Image spinIconImage = null;

        switch (spinTypeName)
        {
            case SpinTypeNames.Ispin:
            case SpinTypeNames.IspinMini:
                GameDisplayManagerStats.UpdateStats(_i_spinIcon: true);
                spinIconImage = i_spinIconImage;
                break;
            case SpinTypeNames.Jspin:
                GameDisplayManagerStats.UpdateStats(_j_spinIcon: true);
                spinIconImage = j_spinIconImage;
                break;
            case SpinTypeNames.Lspin:
                GameDisplayManagerStats.UpdateStats(_l_spinIcon: true);
                spinIconImage = l_spinIconImage;
                break;
            case SpinTypeNames.Sspin:
            case SpinTypeNames.SspinMini:
                GameDisplayManagerStats.UpdateStats(_s_spinIcon: true);
                spinIconImage = s_spinIconImage;
                break;
            case SpinTypeNames.Tspin:
            case SpinTypeNames.TspinMini:
                GameDisplayManagerStats.UpdateStats(_t_spinIcon: true);
                spinIconImage = t_spinIconImage;
                break;
            case SpinTypeNames.Zspin:
            case SpinTypeNames.ZspinMini:
                GameDisplayManagerStats.UpdateStats(_z_spinIcon: true);
                spinIconImage = z_spinIconImage;
                break;
            default:
                return; // Spin判定がない場合、何もしない
        }

        if (spinIconImage != null)
        {
            ImageFadeInAndOutType1(spinTypeName);
            ImageFadeInAndOutType2(spinTypeName, spinIconImage);
        }

        int falseSpinIconImageCount = GameDisplayManagerStats.CheckFalseSpinIconCount(spinTypeNamesToImageDictionary, out Image _falseIconImage);
        switch (falseSpinIconImageCount)
        {
            case 1:
                if (GameDisplayManagerStats.SpinCompleteReach == false)
                {
                    GameDisplayManagerStats.UpdateStats(_spinCompleteReach: true, _reachColor: spinTypeNamesToColorDictionary[imageToSpinTypeNamesDictionary[_falseIconImage]]);
                    falseIconImage = _falseIconImage;
                    ImageFadeInAndOutType3(spinTypeNamesToColorDictionary[imageToSpinTypeNamesDictionary[_falseIconImage]]);
                }
                break;
            case 0:
                GameDisplayManagerStats.UpdateStats(_spinComplete: true);
                DOTween.Kill(falseIconImage); // 点滅しているspinIconのアニメーションを停止
                falseIconImage.color = GameDisplayManagerStats.ReachColor; // 点滅しているspinIconに色を加える
                foreach (var _spinIconImage in spinIconImages)
                {
                    SpinCompleteImageAnimation(_spinIconImage);
                }
                StartCoroutine(SpinCompleteTextAnimation());
                break;
            default:
                break;
        }
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinColorAnimation, eLogTitle.End);
    }

    /// <summary> フェードインとフェードアウトのアニメーションタイプ1(TextMeshProUGUI) </summary>
    /// <param name="displayText"> 表示するテキスト </param>
    private void TextFadeInAndOutType1(TextMeshProUGUI displayText)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.TextFadeInAndOutType1, eLogTitle.Start);

        // 現在のアニメーションを停止
        DOTween.Kill(displayText);

        displayText.gameObject.SetActive(true);
        var sequence = DOTween.Sequence();
        sequence
            .Append(displayText.DOFade(1, 0))
            .Append(displayText.DOFade(0, 0.02f).SetLoops(20, LoopType.Yoyo))
            .AppendInterval(1f)
            .Append(displayText.DOFade(0, 0.5f))
            .OnComplete(() => displayText.gameObject.SetActive(false));

        sequence.Play();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.TextFadeInAndOutType1, eLogTitle.End);
    }

    /// <summary> フェードインとフェードアウトのアニメーションタイプ1(Image) </summary>
    /// <param name="spinTypeName"> スピンタイプ </param>
    private void ImageFadeInAndOutType1(SpinTypeNames spinTypeName)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ImageFadeInAndOutType1, eLogTitle.Start);

        // 現在のアニメーションを停止
        DOTween.Kill(spinTextsFrameImage1);
        DOTween.Kill(spinTextsFrameImage2);

        // 色と透明度を初期化
        spinTextsFrameImage1.color = new Color(spinTextsFrameImage1.color.r, spinTextsFrameImage1.color.g, spinTextsFrameImage1.color.b, 1f);
        spinTextsFrameImage2.color = new Color(spinTextsFrameImage2.color.r, spinTextsFrameImage2.color.g, spinTextsFrameImage2.color.b, 1f);

        spinTextsFrameImage1.gameObject.SetActive(true);
        spinTextsFrameImage2.gameObject.SetActive(true);
        var sequence1 = DOTween.Sequence();
        var sequence2 = DOTween.Sequence();
        sequence1
            .Append(spinTextsFrameImage1.DOColor(spinTypeNamesToColorDictionary[spinTypeName], 0.3f))
            .Append(spinTextsFrameImage1.DOFade(0, 0.02f).SetLoops(20, LoopType.Yoyo))
            .AppendInterval(1f)
            .Append(spinTextsFrameImage1.DOFade(0, 1.1f))
            .OnComplete(() => spinTextsFrameImage1.gameObject.SetActive(false));

        sequence2
            .Append(spinTextsFrameImage2.DOColor(spinTypeNamesToColorDictionary[spinTypeName], 0.3f))
            .Append(spinTextsFrameImage2.DOFade(0, 0.02f).SetLoops(20, LoopType.Yoyo))
            .AppendInterval(1f)
            .Append(spinTextsFrameImage2.DOFade(0, 0.8f))
            .OnComplete(() => spinTextsFrameImage2.gameObject.SetActive(false));

        sequence1.Play();
        sequence2.Play();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ImageFadeInAndOutType1, eLogTitle.End);
    }

    /// <summary> フェードインとフェードアウトのアニメーションタイプ2(Image) </summary>
    /// <param name="spinTypeName"> スピンタイプ </param>
    /// <param name="spinIconImage"> 表示する画像 </param>
    private void ImageFadeInAndOutType2(SpinTypeNames spinTypeName, Image spinIconImage)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ImageFadeInAndOutType2, eLogTitle.Start);

        var sequence1 = DOTween.Sequence();
        sequence1.Append(spinIconImage.DOColor(spinTypeNamesToColorDictionary[spinTypeName], 0.3f));

        sequence1.Play();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ImageFadeInAndOutType2, eLogTitle.End);
    }

    /// <summary> フェードインとフェードアウトのアニメーションタイプ3(Image) </summary>
    /// <param name="color"> 表示する色 </param>
    private void ImageFadeInAndOutType3(Color color)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ImageFadeInAndOutType3, eLogTitle.Start);

        falseIconImage.DOColor(color, 0.2f).SetLoops(-1, LoopType.Yoyo);

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ImageFadeInAndOutType3, eLogTitle.End);
    }

    /// <summary> Tetrisアニメーションを行う関数 </summary>
    public void TetrisAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.TetrisAnimation, eLogTitle.Start);

        TextMeshProUGUI instantiatedText = Instantiate(tetrisText, gameBoardPanel);
        TextFadeInAndOutType1(instantiatedText);

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.TetrisAnimation, eLogTitle.End);
    }

    /// <summary> BackToBackアニメーションを行う関数 </summary>
    public void BackToBackAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.BackToBackAnimation, eLogTitle.Start);

        TextMeshProUGUI instantiatedText = Instantiate(backToBackText, gameBoardPanel);
        TextFadeInAndOutType1(instantiatedText);

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.BackToBackAnimation, eLogTitle.End);
    }

    /// <summary> PerfectClearアニメーションを行う関数 </summary>
    public void PerfectClearAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.PerfectClearAnimation, eLogTitle.Start);

        TextMeshProUGUI instantiatedText = Instantiate(perfectClearText, gameBoardPanel);
        TextFadeInAndOutType1(instantiatedText);

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.PerfectClearAnimation, eLogTitle.End);
    }

    /// <summary> ReadyGoアニメーションを行う関数 </summary>
    public void ReadyGoAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ReadyGoAnimation, eLogTitle.Start);

        TextMeshProUGUI ready = Instantiate(readyText, gameBoardPanel);
        TextMeshProUGUI go = Instantiate(goText, gameBoardPanel);

        Sequence sequenceReady = DOTween.Sequence();
        Sequence sequenceGo = DOTween.Sequence();

        sequenceReady
            .Append(ready.DOFade(1, 0.3f))
            .AppendInterval(1.5f)
            .Append(ready.DOFade(0, 1))
            .OnComplete(() =>
            {
                sequenceGo
                    .Append(go.DOFade(1, 0.3f))
                    .AppendInterval(1)
                    .Append(go.DOFade(0, 0.5f));
            });

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.ReadyGoAnimation, eLogTitle.End);
    }

    /// <summary> 合計攻撃ライン数のアニメーションを行う関数 </summary>
    /// <remarks>
    /// 攻撃ライン数によって色の変化も行う。 <br/>
    /// ゲームフレームの色も同時に変化させる。
    /// </remarks>
    public void SumAttackLinesAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SumAttackLinesAnimation, eLogTitle.Start);

        int sumAttackLines = AttackCalculatorStats.SumAttackLines;

        Color targetColor = Color.white;

        if (sumAttackLines >= 0 && sumAttackLines < 20)
        {
            // 白から緑へのグラデーション
            targetColor = Color.Lerp(Color.white, Color.green, (float)sumAttackLines / 20);
        }
        else if (sumAttackLines >= 20 && sumAttackLines < 40)
        {
            // 緑から黄色へのグラデーション
            targetColor = Color.Lerp(Color.green, Color.yellow, (float)(sumAttackLines - 20) / 20);
        }
        else if (sumAttackLines >= 40 && sumAttackLines < 60)
        {
            // 黄色からオレンジへのグラデーション
            targetColor = Color.Lerp(Color.yellow, new Color(1.0f, 0.65f, 0.0f), (float)(sumAttackLines - 40) / 20);
        }
        else if (sumAttackLines >= 60 && sumAttackLines < 80)
        {
            // オレンジから濃いオレンジへのグラデーション
            targetColor = Color.Lerp(new Color(1.0f, 0.65f, 0.0f), new Color(1.0f, 0.3f, 0.0f), (float)(sumAttackLines - 60) / 20);
        }
        else if (sumAttackLines >= 80 && sumAttackLines < 99)
        {
            // 濃いオレンジから薄い赤へのグラデーション
            targetColor = Color.Lerp(new Color(1.0f, 0.3f, 0.0f), new Color(1.0f, 0.4f, 0.4f), (float)(sumAttackLines - 80) / 19);
        }
        else if (sumAttackLines == 99)
        {
            // 薄い赤
            targetColor = new Color(1.0f, 0.4f, 0.4f);
        }
        else if (sumAttackLines == 100)
        {
            // 真っ赤（Color.red）
            targetColor = Color.red;
        }

        sumAttackLinesText.color = targetColor;
        gameFrameImage.color = targetColor;
        sumAttackLinesText.text = $"{sumAttackLines}";

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SumAttackLinesAnimation, eLogTitle.End);
    }



    /// <summary> 攻撃ライン数のアニメーションを行う関数 </summary>
    /// <param name="_attackLines"> 今回の攻撃値 </param>
    public void AttackLinesAnimation(int attackLines)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.AttackLinesAnimation, eLogTitle.Start);

        if (attackLinesTextTween != null && attackLinesTextTween.IsActive())
        {
            attackLinesTextTween.Kill();
        }

        string attackLinesTextString = "";

        if (attackLines >= 0 && attackLines < 10)
        {
            attackLinesText.color = Color.Lerp(Color.white, Color.red, (float)attackLines / 10);
            float fontSize = Mathf.Lerp(30, 40, (float)attackLines / 10);
            attackLinesTextString = $"+<size={fontSize}>{attackLines}</size>";
        }
        else if (attackLines >= 10)
        {
            attackLinesText.color = Color.red;
            attackLinesTextString = $"+<size=50>{attackLines}</size>";
        }

        attackLinesText.text = attackLinesTextString;
        attackLinesText.alpha = 0;

        var sequence = DOTween.Sequence();
        sequence
            .Append(attackLinesText.DOFade(1, 0.3f))
            .AppendInterval(2)
            .Append(attackLinesText.DOFade(0, 1))
            .OnComplete(() => attackLinesText.text = "");

        sequence.Play();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.AttackLinesAnimation, eLogTitle.End);
    }

    /// <summary> RENのアニメーションをする関数 </summary>
    public void RenAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.RenAnimation, eLogTitle.Start);

        int ren = AttackCalculatorStats.Ren;
        string renTextString = "";

        if (ren >= 2 && ren < 5)
        {
            renText.color = Color.Lerp(Color.white, Color.green, (float)(ren - 2) / 3);
            renTextString = $"<size={Mathf.Lerp(60, 70, (float)(ren - 2) / 3)}>{ren}</size><size=50><color=white>REN!</color></size>";
        }
        else if (ren >= 5 && ren < 10)
        {
            renText.color = Color.Lerp(Color.green, Color.yellow, (float)(ren - 5) / 5);
            renTextString = $"<size={Mathf.Lerp(70, 80, (float)(ren - 5) / 5)}>{ren}</size><size=50><color=white>REN!</color></size>";
        }
        else if (ren >= 10 && ren < 15)
        {
            renText.color = Color.Lerp(Color.yellow, Color.red, (float)(ren - 10) / 5);
            renTextString = $"<size={Mathf.Lerp(80, 90, (float)(ren - 10) / 5)}>{ren}</size><size=50><color=white>REN!</color></size>";
        }
        else if (ren >= 15 && ren <= 21)
        {
            renText.color = Color.red;
            renTextString = $"<size=45>{ren}</size><size=30><color=white>REN!</color></size>";
        }

        renText.text = renTextString;

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.RenAnimation, eLogTitle.End);
    }

    /// <summary> REN表示の終了アニメーションを行う関数 </summary>
    public void EndingRenAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.EndingRenAnimation, eLogTitle.Start);

        var originalPosition = renText.transform.localPosition;

        var sequence = DOTween.Sequence();
        sequence
            .Append(renText.DOFade(0, 1f))
            .Join(renText.transform.DOLocalMoveX(-1000, 1f).SetRelative().SetEase(Ease.InCubic))
            .OnComplete(() =>
            {
                renText.text = "";
                renText.transform.localPosition = originalPosition;
            });

        sequence.Play();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.EndingRenAnimation, eLogTitle.End);
    }

    /// <summary> SpinCompleteアニメーションを行う関数(Image) </summary>
    /// <param name="spinIconImage"> 表示する画像 </param>
    public void SpinCompleteImageAnimation(Image spinIconImage)
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinCompleteAnimation, eLogTitle.Start);

        var sequence = DOTween.Sequence();
        sequence
            .Append(spinIconImage.transform.DOScale(new Vector3(1.2f, 1.2f, 1.2f), 0.1f).SetLoops(30, LoopType.Yoyo))
            .OnComplete(() => spinIconImage.DOColor(Color.white, 0.5f));

        sequence.Play();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinCompleteAnimation, eLogTitle.End);
    }

    /// <summary> SpinCompleteアニメーションを行う関数(Text) </summary>
    private IEnumerator SpinCompleteTextAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinCompleteTextAnimation, eLogTitle.Start);

        spinCompleteText.gameObject.SetActive(true);
        spinCompleteText.alpha = 0;
        spinCompleteText.ForceMeshUpdate();
        TMP_TextInfo spinCompleteTextInfo = spinCompleteText.textInfo;
        int totalCharacters = spinCompleteTextInfo.characterCount;

        for (int i = 0; i < totalCharacters; i++)
        {
            var charInfo = spinCompleteTextInfo.characterInfo[i];
            if (!charInfo.isVisible)
                continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;
            Color32[] vertexColors = spinCompleteTextInfo.meshInfo[materialIndex].colors32;

            // 最初は透明にする
            for (int jj = 0; jj < 4; jj++)
            {
                vertexColors[vertexIndex + jj].a = 0;
            }

            spinCompleteText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            // フェードインのアニメーション
            float fadeDuration = 0.02f;
            Sequence sequence1 = DOTween.Sequence();
            sequence1
                .Append(DOTween.ToAlpha(() => vertexColors[vertexIndex + 0], x => { vertexColors[vertexIndex + 0] = x; spinCompleteText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32); }, 1f, fadeDuration))
                .Join(DOTween.ToAlpha(() => vertexColors[vertexIndex + 1], x => { vertexColors[vertexIndex + 1] = x; spinCompleteText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32); }, 1f, fadeDuration))
                .Join(DOTween.ToAlpha(() => vertexColors[vertexIndex + 2], x => { vertexColors[vertexIndex + 2] = x; spinCompleteText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32); }, 1f, fadeDuration))
                .Join(DOTween.ToAlpha(() => vertexColors[vertexIndex + 3], x => { vertexColors[vertexIndex + 3] = x; spinCompleteText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32); }, 1f, fadeDuration));

            sequence1.Play();

            yield return sequence1.WaitForCompletion();
            yield return new WaitForSeconds(0.02f);
        }

        yield return new WaitForSeconds(0.02f);

        int randomIndex = random.Next(0, 6); // 0から5までのランダムな数字を生成
        Sequence sequence2 = DOTween.Sequence();
        sequence2
            .Append(spinCompleteText.DOFade(1, 0))
            .Join(spinCompleteText.DOColor(colors[randomIndex], 0.3f))
            .AppendInterval(2)
            .Append(spinCompleteText.DOFade(0, 0.5f))
            .OnComplete(() =>
            {
                spinCompleteText.color = Color.white;
                spinCompleteText.gameObject.SetActive(false);
            });

        sequence2.Play();
        yield return sequence2.WaitForCompletion();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.SpinCompleteTextAnimation, eLogTitle.End);
    }


    /// <summary> すべてのアニメーションを停止させる関数 </summary>
    public void StopAnimation()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.StopAnimation, eLogTitle.Start);

        DOTween.KillAll();

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.StopAnimation, eLogTitle.End);
    }

    /// <summary> PoseIconが押された時の処理をする関数 </summary>
    public void PressedPoseIcon()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.PressedPoseIcon, eLogTitle.Start);

        GameSceneManagerStats.LoadPoseState();
        poseCanvas.gameObject.SetActive(true);
        //audio
        poseBackGround.DOFade(1, 0.3f);

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.PressedPoseIcon, eLogTitle.End);
    }

    /// <summary> BackToGameが押された時のコルーチン処理を呼ぶ関数 </summary>
    public void PressedBackToGame()
    {
        StartCoroutine(BackToGameCoroutine());
    }

    /// <summary> BackToGameが押された時の処理をする関数 </summary>
    private IEnumerator BackToGameCoroutine()
    {
        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.PressedBackToGame, eLogTitle.Start);

        poseBackGround.DOFade(0, 0.3f);
        //audio
        yield return new WaitForSeconds(0.5f); // 短い待機
        poseCanvas.gameObject.SetActive(false);
        GameSceneManagerStats.UpdateStats(_poseState: false);

        LogHelper.DebugLog(eClasses.GameDisplayManager, eMethod.PressedBackToGame, eLogTitle.End);
    }
}


/////////////////// 旧コード ///////////////////

// using UnityEngine;
// using TMPro;
// using DG.Tweening;

// ///// ゲーム画面のテキストに関するスクリプト /////


// // ↓このスクリプトで可能なこと↓ //

// // テキストの表示
// // テキストのアニメーション
// // 攻撃ラインの値を計算

// public class TextEffect : MonoBehaviour
// {
//     // Canvas //
//     [SerializeField] private RectTransform Canvas;

//     // 表示できるテキスト //
//     [SerializeField] private TextMeshProUGUI ReadyText;
//     [SerializeField] private TextMeshProUGUI GoText;

//     [SerializeField] private TextMeshProUGUI BackToBackText;
//     [SerializeField] private TextMeshProUGUI PerfectClearText;
//     [SerializeField] private TextMeshProUGUI RensText;

//     [SerializeField] private TextMeshProUGUI OneLineClearText;
//     [SerializeField] private TextMeshProUGUI TwoLineClearText;
//     [SerializeField] private TextMeshProUGUI ThreeLineClearText;
//     [SerializeField] private TextMeshProUGUI TetrisText;
//     [SerializeField] private TextMeshProUGUI IspinText;
//     [SerializeField] private TextMeshProUGUI IspinSingleText;
//     [SerializeField] private TextMeshProUGUI IspinDoubleText;
//     [SerializeField] private TextMeshProUGUI IspinTripleText;
//     [SerializeField] private TextMeshProUGUI IspinQuattroText;
//     [SerializeField] private TextMeshProUGUI IspinMiniText;
//     [SerializeField] private TextMeshProUGUI JspinText;
//     [SerializeField] private TextMeshProUGUI JspinSingleText;
//     [SerializeField] private TextMeshProUGUI JspinDoubleText;
//     [SerializeField] private TextMeshProUGUI JspinTripleText;
//     [SerializeField] private TextMeshProUGUI LspinText;
//     [SerializeField] private TextMeshProUGUI LspinSingleText;
//     [SerializeField] private TextMeshProUGUI LspinDoubleText;
//     [SerializeField] private TextMeshProUGUI LspinTripleText;
//     [SerializeField] private TextMeshProUGUI OspinText;
//     [SerializeField] private TextMeshProUGUI OspinSingleText;
//     [SerializeField] private TextMeshProUGUI OspinDoubleText;
//     [SerializeField] private TextMeshProUGUI OspinTripleText;
//     [SerializeField] private TextMeshProUGUI SspinText;
//     [SerializeField] private TextMeshProUGUI SspinSingleText;
//     [SerializeField] private TextMeshProUGUI SspinDoubleText;
//     [SerializeField] private TextMeshProUGUI SspinTripleText;
//     [SerializeField] private TextMeshProUGUI TspinText;
//     [SerializeField] private TextMeshProUGUI TspinSingleText;
//     [SerializeField] private TextMeshProUGUI TspinDoubleText;
//     [SerializeField] private TextMeshProUGUI TspinTripleText;
//     [SerializeField] private TextMeshProUGUI TspinMiniText;
//     [SerializeField] private TextMeshProUGUI TspinDoubleMiniText;
//     [SerializeField] private TextMeshProUGUI ZspinText;
//     [SerializeField] private TextMeshProUGUI ZspinSingleText;
//     [SerializeField] private TextMeshProUGUI ZspinDoubleText;
//     [SerializeField] private TextMeshProUGUI ZspinTripleText;


//     // 透明度 //
//     int Alpha_0 = 0; // 0の時は透明
//     int ALPHA1 = 1; // 1の時は不透明

//     // 攻撃ラインの値 //
//     int TwoLineClearAttack = 1;
//     int ThreeLineClearAttack = 2;
//     int TetrisAttack = 4;
//     int JspinSingleAttack = 1;
//     int JspinDoubleAttack = 3;
//     int JspinTripleAttack = 6;
//     int LspinSingleAttack = 1;
//     int LspinDoubleAttack = 3;
//     int LspinTripleAttack = 6;
//     int IspinSingleAttack = 2;
//     int IspinDoubleAttack = 4;
//     int IspinTripleAttack = 6;
//     int IspinQuattroAttack = 8;
//     int IspinMiniAttack = 1;
//     int TspinSingleAttack = 2;
//     int TspinDoubleAttack = 4;
//     int TspinTripleAttack = 6;
//     int TspinMiniAttack = 0;
//     int TspinDoubleMiniAttack = 1;
//     int BackToBackBonus = 1;
//     int PerfectClearBonus = 10;
//     int RenBonus_0or1 = 0;
//     int RenBonus_2or3 = 1;
//     int RenBonus_4or5 = 2;
//     int RenBonus_6or7 = 3;
//     int RenBonus_8or9or10 = 4;
//     int RenBonus_over11 = 5;

//     // 干渉するスクリプト //
//     Board board;
//     GameStatus gameStatus;
//     SpinCheck spinCheck;

//     // インスタンス化 //
//     void Awake()
//     {
//         board = FindObjectOfType<Board>();
//         gameStatus = FindObjectOfType<GameStatus>();
//         spinCheck = FindObjectOfType<SpinCheck>();
//     }

//     // 表示するテキストを判別して、実際に表示する関数 //
//     public void TextDisplay(int __LineClearCount)
//     {
//         switch (spinCheck.spinTypeName)
//         {
//             case "None": // Spin判定がない時
//                 switch (__LineClearCount) // 消去数で表示するテキストが変わる
//                 {
//                     case 0:
//                         gameStatus.ResetRen();
//                         break;

//                     case 1:
//                         gameStatus.ResetBackToBack();

//                         RenAttackLines();

//                         AudioManager.Instance.PlaySound("Normal_Destroy");

//                         TextAnimation(OneLineClearText);

//                         break;

//                     case 2:
//                         gameStatus.ResetBackToBack();

//                         gameStatus.IncreaseAttackLines(TwoLineClearAttack);

//                         RenAttackLines();

//                         AudioManager.Instance.PlaySound("Normal_Destroy");

//                         TextAnimation(TwoLineClearText);

//                         break;

//                     case 3:
//                         gameStatus.ResetBackToBack();

//                         gameStatus.IncreaseAttackLines(ThreeLineClearAttack);

//                         RenAttackLines();

//                         AudioManager.Instance.PlaySound("Normal_Destroy");

//                         TextAnimation(ThreeLineClearText);

//                         break;

//                     case 4:
//                         CheckBackToBack();

//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TetrisAttack);

//                         AudioManager.Instance.PlaySound("Tetris");

//                         TextAnimation(TetrisText);

//                         break;
//                 }
//                 break;
//             case "J-Spin":
//                 CheckBackToBack();

//                 switch (__LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(JspinText);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(JspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(JspinSingleText);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(JspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(JspinDoubleText);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(JspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(JspinTripleText);

//                         break;
//                 }
//                 break;
//             case "L-Spin":
//                 CheckBackToBack();

//                 switch (__LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(LspinText);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(LspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(LspinSingleText);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(LspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(LspinDoubleText);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(LspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(LspinTripleText);

//                         break;
//                 }
//                 break;

//             case "I-Spin Mini":
//                 CheckBackToBack();

//                 switch (__LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(IspinMiniText);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinMiniAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinMiniText);

//                         break;
//                 }
//                 break;

//             case "I-Spin":
//                 CheckBackToBack();

//                 switch (__LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(IspinText);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinSingleText);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinDoubleText);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinTripleText);

//                         break;
//                     case 4:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinQuattroAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinQuattroText);

//                         break;
//                 }
//                 break;

//             case "T-Spin":
//                 CheckBackToBack();

//                 switch (__LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(TspinText);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinSingleText);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinDoubleText);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinTripleText);

//                         break;
//                 }
//                 break;

//             case "T-Spin Mini":
//                 CheckBackToBack();

//                 switch (__LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(TspinMiniText);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinMiniAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinMiniText);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinDoubleMiniAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinDoubleMiniText);

//                         break;
//                 }
//                 break;
//         }

//         // PerfectClearか判定する
//         if (board.CheckPerfectClear())
//         {
//             gameStatus.IncreaseAttackLines(PerfectClearBonus);

//             // AudioManager.Instance.PlaySound("");

//             PerfectClearAnimation();
//         }
//     }

//     // BackToBackの判定をチェックする関数
//     private void CheckBackToBack()
//     {
//         if (gameStatus.backToBack == true) // BackToBack 条件をすでに満たしている時
//         {
//             gameStatus.IncreaseAttackLines(BackToBackBonus);

//             BackToBackAnimation();
//         }
//         else
//         {
//             gameStatus.Set_BackToBack(); // BackToBack 判定を付与
//         }
//     }

//     // Renの攻撃ラインを計算する関数 //
//     private void RenAttackLines()
//     {
//         gameStatus.IncreaseRen(); // Renのカウントを1あげる

//         switch (gameStatus.ren)
//         {
//             case 0:
//             case 1:
//                 gameStatus.IncreaseAttackLines(RenBonus_0or1);
//                 break;

//             case 2:
//             case 3:
//                 gameStatus.IncreaseAttackLines(RenBonus_2or3);
//                 break;

//             case 4:
//             case 5:
//                 gameStatus.IncreaseAttackLines(RenBonus_4or5);
//                 break;

//             case 6:
//             case 7:
//                 gameStatus.IncreaseAttackLines(RenBonus_6or7);
//                 break;

//             case 8:
//             case 9:
//             case 10:
//                 gameStatus.IncreaseAttackLines(RenBonus_8or9or10);
//                 break;

//             default:
//                 gameStatus.IncreaseAttackLines(RenBonus_over11);
//                 break;
//         }
//     }

//     // テキストのアニメーションを行う関数を呼ぶ関数 //
//     private void TextAnimation(TextMeshProUGUI _displayText)
//     {
//         TextMeshProUGUI InstantiatedText = Instantiate(_displayText, Canvas);

//         // 選ばれたテキストのテキストコンポーネントとトランスフォームコンポーネントを取得
//         TextMeshProUGUI displayTextText = InstantiatedText.GetComponent<TextMeshProUGUI>();
//         Transform displayText_Transform = InstantiatedText.GetComponent<Transform>();

//         TextFadeInAndOutType1(displayTextText); // 選ばれたテキストのフェードインとフェードアウトを行う

//         TextMove(displayText_Transform); // 選ばれたテキストの移動アニメーションを行う
//     }

//     //選ばれたテキストのフェードインとフェードアウトを行う関数
//     private void TextFadeInAndOutType1(TextMeshProUGUI displayText)
//     {
//         // フェードインとフェードアウトする時間 //
//         float fadeInInterval = 0.3f;
//         float fadeOutInterval = 1f;

//         // テキストを表示させる時間 //
//         float waitInterval = 2f;

//         // Sequenceのインスタンス化
//         var sequence = DOTween.Sequence();

//         // 0.3秒かけてアルファ値を1(=不透明)に変化させる
//         // その後、2秒表示
//         // 最後に、1秒かけてアルファ値を0(=透明)に変化させる
//         sequence
//             .Append(displayText.DOFade(ALPHA1, fadeInInterval))
//             .AppendInterval(waitInterval)
//             .Append(displayText.DOFade(Alpha_0, fadeOutInterval));
//     }


//     //選ばれたテキストの移動アニメーションを行う関数
//     private void TextMove(Transform displayText)
//     {
//         // テキストを移動させる時間 //
//         float moveInterval_x = 2f; // x軸
//         float moveInterval_y = 1.2f; // y軸

//         // テキストの移動量 //
//         float moveDistance = 600f;

//         // displayTextの各座標を格納する変数を宣言
//         float displayText_x = Mathf.RoundToInt(displayText.transform.position.x);
//         float displayText_y = Mathf.RoundToInt(displayText.transform.position.y);
//         float displayText_z = Mathf.RoundToInt(displayText.transform.position.z);

//         // Sequenceのインスタンス化
//         var sequence = DOTween.Sequence();

//         // 現在位置から3秒で、Y座標を600移動する
//         // 始点と終点の繋ぎ方はOutSineで設定
//         // その後、現在位置から1秒で、X座標を-600移動する(移動中にフェードアウトする)
//         // 始点と終点の繋ぎ方はInQuintで設定
//         sequence
//             .Append(displayText.transform.DOMoveY(displayText_y + moveDistance, moveInterval_x).SetEase(Ease.OutSine))
//             .Append(displayText.transform.DOMoveX(displayText_x - moveDistance, moveInterval_y).SetEase(Ease.InQuint))
//             //.Join(displayText.transform.DOScale(new Vector3(0, 1f, 0), moveInterval_y))
//             .OnComplete(() =>
//             {
//                 // アニメーションが完了したら元の位置に戻す
//                 displayText.transform.position = new Vector3(displayText_x, displayText_y, displayText_z);
//             });
//     }

//     // BackToBackの表示をする関数を呼ぶ関数 //
//     private void BackToBackAnimation()
//     {
//         TextMeshProUGUI InstantiatedText = Instantiate(BackToBackText, Canvas);

//         TextMeshProUGUI displayTextText = InstantiatedText.GetComponent<TextMeshProUGUI>();

//         TextFadeInAndOutType1(displayTextText); // 選ばれたテキストのフェードインとフェードアウトを行う
//     }

//     // PerfectClearの表示をする関数を呼ぶ関数 //
//     private void PerfectClearAnimation()
//     {
//         TextMeshProUGUI InstantiatedText = Instantiate(PerfectClearText, Canvas);

//         TextMeshProUGUI displayTextText = InstantiatedText.GetComponent<TextMeshProUGUI>();

//         TextFadeInAndOutType1(displayTextText); // 選ばれたテキストのフェードインとフェードアウトを行う
//     }

//     // Ready Go の表示をする関数 //
//     public void ReadyGoAnimation()
//     {
//         // フェードインとフェードアウトする時間 //
//         float fadeInInterval = 0.3f;
//         float fadeOutInterval = 0f;

//         // テキストを表示させる時間 //
//         float waitInterval_ready = 3f;
//         float waitInterval_go = 2f;

//         TextMeshProUGUI ready = Instantiate(ReadyText, Canvas);
//         TextMeshProUGUI go = Instantiate(GoText, Canvas);

//         Sequence sequence_ready = DOTween.Sequence();
//         Sequence sequence_go = DOTween.Sequence(); // Sequenceの作成

//         sequence_ready
//             .Append(ready.DOFade(ALPHA1, fadeInInterval))
//             .AppendInterval(waitInterval_ready)
//             .Append(ready.DOFade(Alpha_0, fadeOutInterval))
//             .OnComplete(() =>
//             {
//                 // Readyアニメーションが完了したらGoアニメーションを開始
//                 sequence_go
//                     .Append(go.DOFade(ALPHA1, fadeInInterval))
//                     // .Join(go.DOScale(Vector3.one, 0.2f))
//                     .AppendInterval(waitInterval_go)
//                     .Append(go.DOFade(Alpha_0, fadeOutInterval));
//             });

//         // // フェードイン＆拡大
//         // sequence.Append(textTransform.DOScale(Vector3.one, 0.2f)); // 0.2秒で拡大
//         // sequence.Join(textTransform.DOFade(1, 0.2f)); // 同時にフェードイン

//         // // フェードアウト
//         // sequence.AppendInterval(1f); // 1秒待機
//         // sequence.Append(textTransform.DOFade(0, 0.5f)); // 0.5秒でフェードアウト
//     }

//     // アニメーションを停止させる関数 //
//     public void StopAnimation()
//     {
//         DOTween.KillAll();
//     }
// }


// // 攻撃ラインの値 //
// private int OneLineClearAttack = 0;
// private int TwoLineClearAttack = 1;
// private int ThreeLineClearAttack = 2;
// private int TetrisAttack = 4;
// private int JspinSingleAttack = 1;
// private int JspinDoubleAttack = 3;
// private int JspinTripleAttack = 6;
// private int LspinSingleAttack = 1;
// private int LspinDoubleAttack = 3;
// private int LspinTripleAttack = 6;
// private int IspinSingleAttack = 2;
// private int IspinDoubleAttack = 4;
// private int IspinTripleAttack = 6;
// private int IspinQuattroAttack = 8;
// private int IspinMiniAttack = 1;
// private int TspinSingleAttack = 2;
// private int TspinDoubleAttack = 4;
// private int TspinTripleAttack = 6;
// private int TspinMiniAttack = 0;
// private int TspinDoubleMiniAttack = 1;

// // 干渉するスクリプト //
// private Board board;
// private GameStatus gameStatus;
// private SpinCheck spinCheck;

// /// <summary>
// /// インスタンス化
// /// </summary>
// private void Awake()
// {
//     board = FindObjectOfType<Board>();
//     gameStatus = FindObjectOfType<GameStatus>();
//     spinCheck = FindObjectOfType<SpinCheck>();
// }

// if (spinCheck.spinTypeName == "None") // Spin判定がない場合
// {
//     DisplayNonSpinText(_lineClearCount);
// }
// else // Spin判定がある場合
// {
//     DisplaySpinText(_lineClearCount);
// }

// // Spin判定がない場合のテキスト表示を処理する関数 //
// private void DisplayNonSpinText(int _lineClearCount)
// {
//     switch (_lineClearCount)
//     {
//         case 0:
//             gameStatus.ResetRen();
//             break;
//         case 1:
//             gameStatus.ResetBackToBack();
//             RenAttackLines();
//             PlaySoundAndAnimateText("Normal_Destroy", OneLineClearText);
//             break;
//         case 2:
//             gameStatus.ResetBackToBack();
//             gameStatus.IncreaseAttackLines(TwoLineClearAttack);
//             RenAttackLines();
//             PlaySoundAndAnimateText("Normal_Destroy", TwoLineClearText);
//             break;
//         case 3:
//             gameStatus.ResetBackToBack();
//             gameStatus.IncreaseAttackLines(ThreeLineClearAttack);
//             RenAttackLines();
//             PlaySoundAndAnimateText("Normal_Destroy", ThreeLineClearText);
//             break;
//         case 4:
//             CheckBackToBack();
//             RenAttackLines();
//             gameStatus.IncreaseAttackLines(TetrisAttack);
//             PlaySoundAndAnimateText("Tetris", TetrisText);
//             break;
//     }
// }

// private TextMeshProUGUI DetermineTextToDisplay(string spinType, int _lineClearCount)
// {
//     TextMeshProUGUI displayText = null;

//     switch (spinType)
//     {
//         case "J-Spin":
//             switch (_lineClearCount)
//             {
//                 case 0:
//                     displayText = JspinText;
//                     break;
//                 case 1:
//                     displayText = JspinSingleText;
//                     break;
//                 case 2:
//                     displayText = JspinDoubleText;
//                     break;
//                 case 3:
//                     displayText = JspinTripleText;
//                     break;
//             }
//             break;
//         case "L-Spin":
//             switch (_lineClearCount)
//             {
//                 case 0:
//                     displayText = LspinText;
//                     break;
//                 case 1:
//                     displayText = LspinSingleText;
//                     break;
//                 case 2:
//                     displayText = LspinDoubleText;
//                     break;
//                 case 3:
//                     displayText = LspinTripleText;
//                     break;
//             }
//             break;
//         case "I-Spin Mini":
//             switch (_lineClearCount)
//             {
//                 case 0:
//                 case 1:
//                     displayText = IspinMiniText;
//                     break;
//             }
//             break;
//         case "I-Spin":
//             switch (_lineClearCount)
//             {
//                 case 0:
//                     displayText = IspinText;
//                     break;
//                 case 1:
//                     displayText = IspinSingleText;
//                     break;
//                 case 2:
//                     displayText = IspinDoubleText;
//                     break;
//                 case 3:
//                     displayText = IspinTripleText;
//                     break;
//                 case 4:
//                     displayText = IspinQuattroText;
//                     break;
//             }
//             break;
//         case "T-Spin":
//             switch (_lineClearCount)
//             {
//                 case 0:
//                     displayText = TspinText;
//                     break;
//                 case 1:
//                     displayText = TspinSingleText;
//                     break;
//                 case 2:
//                     displayText = TspinDoubleText;
//                     break;
//                 case 3:
//                     displayText = TspinTripleText;
//                     break;
//             }
//             break;
//         case "T-Spin Mini":
//             switch (_lineClearCount)
//             {
//                 case 0:
//                     displayText = TspinMiniText;
//                     break;
//                 case 1:
//                     displayText = TspinMiniText;
//                     break;
//                 case 2:
//                     displayText = TspinDoubleMiniText;
//                     break;
//             }
//             break;
//         default:
//             switch (_lineClearCount)
//             {
//                 case 1:
//                     displayText = OneLineClearText;
//                     break;
//                 case 2:
//                     displayText = TwoLineClearText;
//                     break;
//                 case 3:
//                     displayText = ThreeLineClearText;
//                     break;
//                 case 4:
//                     displayText = TetrisText;
//                     break;
//             }
//             break;
//     }

//     CheckBackToBack(spinType, displayText);
//     return displayText;
// }


// // Spin判定がある場合のテキスト表示を処理する関数 //
// private void DisplaySpinText(int _lineClearCount)
// {
//     CheckBackToBack();
//     string spinType = spinCheck.spinTypeName;
//     switch (spinType)
//     {
//         case "J-Spin":
//             HandleSpinText(_lineClearCount, JspinText, JspinSingleAttack, JspinSingleText, JspinDoubleAttack, JspinDoubleText, JspinTripleAttack, JspinTripleText);
//             break;
//         case "L-Spin":
//             HandleSpinText(_lineClearCount, LspinText, LspinSingleAttack, LspinSingleText, LspinDoubleAttack, LspinDoubleText, LspinTripleAttack, LspinTripleText);
//             break;
//         case "I-Spin Mini":
//             HandleMiniSpinText(_lineClearCount, IspinMiniText, IspinMiniAttack, IspinMiniText);
//             break;
//         case "I-Spin":
//             HandleSpinText(_lineClearCount, IspinText, IspinSingleAttack, IspinSingleText, IspinDoubleAttack, IspinDoubleText, IspinTripleAttack, IspinTripleText, IspinQuattroAttack, IspinQuattroText);
//             break;
//         case "T-Spin":
//             HandleSpinText(_lineClearCount, TspinText, TspinSingleAttack, TspinSingleText, TspinDoubleAttack, TspinDoubleText, TspinTripleAttack, TspinTripleText);
//             break;
//         case "T-Spin Mini":
//             HandleMiniSpinText(_lineClearCount, TspinMiniText, TspinMiniAttack, TspinMiniText, TspinDoubleMiniAttack, TspinDoubleMiniText);
//             break;
//     }
// }

// private void HandleSpinText(int _lineClearCount, TextMeshProUGUI baseText, int singleAttack, TextMeshProUGUI singleText, int doubleAttack, TextMeshProUGUI doubleText, int tripleAttack, TextMeshProUGUI tripleText, int quattroAttack = 0, TextMeshProUGUI quattroText = null)
// {
//     switch (_lineClearCount)
//     {
//         case 0:
//             PlaySoundAndAnimateText("Normal_Drop", baseText);
//             break;
//         case 1:
//             RenAttackLines();
//             gameStatus.IncreaseAttackLines(singleAttack);
//             PlaySoundAndAnimateText("Spin_Destroy", singleText);
//             break;
//         case 2:
//             RenAttackLines();
//             gameStatus.IncreaseAttackLines(doubleAttack);
//             PlaySoundAndAnimateText("Spin_Destroy", doubleText);
//             break;
//         case 3:
//             RenAttackLines();
//             gameStatus.IncreaseAttackLines(tripleAttack);
//             PlaySoundAndAnimateText("Spin_Destroy", tripleText);
//             break;
//         case 4:
//             if (quattroAttack > 0 && quattroText != null)
//             {
//                 RenAttackLines();
//                 gameStatus.IncreaseAttackLines(quattroAttack);
//                 PlaySoundAndAnimateText("Spin_Destroy", quattroText);
//             }
//             break;
//     }
// }

// private void HandleMiniSpinText(int _lineClearCount, TextMeshProUGUI baseText, int miniAttack, TextMeshProUGUI miniText, int doubleMiniAttack = 0, TextMeshProUGUI doubleMiniText = null)
// {
//     switch (_lineClearCount)
//     {
//         case 0:
//             PlaySoundAndAnimateText("Normal_Drop", baseText);
//             break;
//         case 1:
//             RenAttackLines();
//             gameStatus.IncreaseAttackLines(miniAttack);
//             PlaySoundAndAnimateText("Spin_Destroy", miniText);
//             break;
//         case 2:
//             if (doubleMiniAttack > 0 && doubleMiniText != null)
//             {
//                 RenAttackLines();
//                 gameStatus.IncreaseAttackLines(doubleMiniAttack);
//                 PlaySoundAndAnimateText("Spin_Destroy", doubleMiniText);
//             }
//             break;
//     }
// }

// private void PlaySoundAndAnimateText(string sound, TextMeshProUGUI text)
// {
//     AudioManager.Instance.PlaySound(sound);
//     TextAnimation(text);
// }

// // 攻撃ラインの値 //
// private int BackToBackBonus = 1;
// private int PerfectClearBonus = 10;
// private int[] RenBonus = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5 };

//     // スピンタイプと消去ライン数に対応する攻撃力をマッピングするディクショナリ
//     Dictionary<SpinTypeNames, Dictionary<int, int>> spinTypeAttackMapping = new Dictionary<SpinTypeNames, Dictionary<int, int>>
// {
//     { SpinTypeNames.J_Spin, new Dictionary<int, int>
//         {
//             { 1, 1 }, // JspinSingleAttack
//             { 2, 3 }, // JspinDoubleAttack
//             { 3, 6 }  // JspinTripleAttack
//         }
//     },
//     { SpinTypeNames.L_Spin, new Dictionary<int, int>
//         {
//             { 1, 1 }, // LspinSingleAttack
//             { 2, 3 }, // LspinDoubleAttack
//             { 3, 6 }  // LspinTripleAttack
//         }
//     },
//     { SpinTypeNames.I_SpinMini, new Dictionary<int, int>
//         {
//             { 1, 1 } // IspinMiniAttack
//         }
//     },
//     { SpinTypeNames.I_Spin, new Dictionary<int, int>
//         {
//             { 1, 2 }, // IspinSingleAttack
//             { 2, 4 }, // IspinDoubleAttack
//             { 3, 6 }, // IspinTripleAttack
//             { 4, 8 }  // IspinQuattroAttack
//         }
//     },
//     { SpinTypeNames.T_Spin, new Dictionary<int, int>
//         {
//             { 1, 2 }, // TspinSingleAttack
//             { 2, 4 }, // TspinDoubleAttack
//             { 3, 6 }  // TspinTripleAttack
//         }
//     },
//     { SpinTypeNames.T_SpinMini, new Dictionary<int, int>
//         {
//             { 1, 0 }, // TspinMiniAttack
//             { 2, 1 }  // TspinDoubleMiniAttack
//         }
//     },
//     { SpinTypeNames.None, new Dictionary<int, int>
//         {
//             { 1, 0 }, // OneLineClearAttack
//             { 2, 1 }, // TwoLineClearAttack
//             { 3, 2 }, // ThreeLineClearAttack
//             { 4, 4 }  // TetrisAttack
//         }
//     }
// };

// // スピンタイプと行消去数に基づいて攻撃力を選択
// if (spinTypeAttackMapping.ContainsKey(_spinType) && spinTypeAttackMapping[_spinType].ContainsKey(_lineClearCount))
// {
//     gameStatus.IncreaseAttackLines(spinTypeAttackMapping[_spinType][_lineClearCount]); // 対応した攻撃力を反映
// }

// CheckBackToBack(_spinType, displayText); // BackToBack判定をチェック
// CheckRen(_lineClearCount); // Ren判定をチェック
// if (board.CheckPerfectClear()) // PerfectClear判定をチェック
// {
//     gameStatus.IncreaseAttackLines(PerfectClearBonus);
//     PerfectClearAnimation();
// }

// /// <summary> BackToBackの判定を確認し、ダメージの計算を行う関数 </summary>
// /// <param name="_spinType">スピンタイプ</param>
// /// <param name="_displayText">表示するテキスト</param>
// private void CheckBackToBack(SpinTypeNames _spinType, TextMeshProUGUI _displayText)
// {
//     // Spin判定がない、かつテトリスでない場合
//     if (_spinType == SpinTypeNames.None && _displayText != TetrisText)
//     {
//         // BackToBack判定をリセット
//         gameStatus.ResetBackToBack();
//     }
//     else
//     {
//         if (!gameStatus.BackToBack)
//         {
//             // BackToBack判定を付与
//             gameStatus.SetBackToBack();
//         }
//         else
//         {
//             // すでにBackToBack判定が付与されている場合(前回付与した判定をキープしていた場合)
//             gameStatus.IncreaseAttackLines(BackToBackBonus);
//             BackToBackAnimation();
//         }
//     }
// }

// /// <summary> Renの判定を確認し、ダメージの計算を行う関数 </summary>
// /// <param name="_lineClearCount">消去したラインの数</param>
// private void CheckRen(int _lineClearCount)
// {
//     if (_lineClearCount >= 1)
//     {
//         // 1列以上消去していれば
//         gameStatus.IncreaseRen();
//         // int ren = Mathf.Min(gameStatus.ren, RenBonus.Length - 1);
//         gameStatus.IncreaseAttackLines(RenBonus[gameStatus.Ren]);
//     }
//     else
//     {
//         // 列消去ができていない場合、リセットする
//         gameStatus.ResetRen();
//     }
// }


// using System.Collections.Generic;
// using UnityEngine;
// using TMPro;
// using DG.Tweening;

// /// <summary>
// /// TextEffectのテキストの表示に関する時間情報を保持する静的クラス
// /// </summary>
// internal static class TextMovementSettings
// {
//     // Ready テキストの設定値 //
//     public static float READY_FADE_IN_INTERVAL { get; set; } = 0.3f;
//     public static float READY_FADE_OUT_INTERVAL { get; set; } = 1f;
//     public static float READY_WAIT_INTERVAL { get; set; } = 1.5f;

//     // Go テキストの設定値 //
//     public static float GO_FADE_IN_INTERVAL { get; set; } = 0.3f;
//     public static float GO_FADE_OUT_INTERVAL { get; set; } = 0.5f;
//     public static float GO_WAIT_INTERVAL { get; set; } = 1f;

//     // Spin, LineClear テキストの設定値 //
//     public static float SPIN_FADE_IN_INTERVAL { get; set; } = 0.3f;
//     public static float SPIN_FADE_OUT_INTERVAL { get; set; } = 0.5f;
//     public static float SPIN_WAIT_INTERVAL_1 { get; set; } = 2f;
//     public static float SPIN_WAIT_INTERVAL_2 { get; set; } = 2f;
//     public static float SPIN_MOVE_INTERVAL_X { get; set; } = 2f;
//     public static float SPIN_MOVE_INTERVAL_Y { get; set; } = 1.2f;
//     public static float SPIN_MOVE_DISTANCE { get; set; } = 600f;

//     // BackToBack テキストの設定値 //
//     public static float BACK_TO_BACK_FADE_IN_INTERVAL { get; set; } = 0.3f;
//     public static float BACK_TO_BACK_FADE_OUT_INTERVAL { get; set; } = 1f;
//     public static float BACK_TO_BACK_WAIT_INTERVAL { get; set; } = 2f;

//     // PerfectClear テキストの設定値 //
//     public static float PERFECT_CLEAR_FADE_IN_INTERVAL { get; set; } = 0.3f;
//     public static float PERFECT_CLEAR_FADE_OUT_INTERVAL { get; set; } = 1f;
//     public static float PERFECT_CLEAR_WAIT_INTERVAL { get; set; } = 2f;
// }

// /// <summary>
// /// ゲーム画面のテキストを管理するクラス
// /// </summary>
// public class TextMovement : MonoBehaviour
// {
//     // Canvas //
//     [SerializeField] private RectTransform Canvas;

//     // Panels //
//     [SerializeField] private RectTransform SpinTextsPanel;

//     // 表示できるテキスト //
//     [SerializeField] private TextMeshProUGUI ReadyText;
//     [SerializeField] private TextMeshProUGUI GoText;
//     [SerializeField] private TextMeshProUGUI BackToBackText;
//     [SerializeField] private TextMeshProUGUI PerfectClearText;
//     [SerializeField] private TextMeshProUGUI TetrisText;
//     [SerializeField] private TextMeshProUGUI IspinText;
//     [SerializeField] private TextMeshProUGUI IspinSingleText;
//     [SerializeField] private TextMeshProUGUI IspinDoubleText;
//     [SerializeField] private TextMeshProUGUI IspinTripleText;
//     [SerializeField] private TextMeshProUGUI IspinQuattroText;
//     [SerializeField] private TextMeshProUGUI IspinMiniText;
//     [SerializeField] private TextMeshProUGUI JspinText;
//     [SerializeField] private TextMeshProUGUI JspinSingleText;
//     [SerializeField] private TextMeshProUGUI JspinDoubleText;
//     [SerializeField] private TextMeshProUGUI JspinTripleText;
//     [SerializeField] private TextMeshProUGUI JpinMiniText;
//     [SerializeField] private TextMeshProUGUI JspinDoubleMiniText;
//     [SerializeField] private TextMeshProUGUI JspinTripleMiniText;
//     [SerializeField] private TextMeshProUGUI LspinText;
//     [SerializeField] private TextMeshProUGUI LspinSingleText;
//     [SerializeField] private TextMeshProUGUI LspinDoubleText;
//     [SerializeField] private TextMeshProUGUI LspinTripleText;
//     [SerializeField] private TextMeshProUGUI LspinMiniText;
//     [SerializeField] private TextMeshProUGUI LspinDoubleMiniText;
//     [SerializeField] private TextMeshProUGUI LspinTripleMiniText;
//     [SerializeField] private TextMeshProUGUI SspinText;
//     [SerializeField] private TextMeshProUGUI SspinSingleText;
//     [SerializeField] private TextMeshProUGUI SspinDoubleText;
//     [SerializeField] private TextMeshProUGUI SspinTripleText;
//     [SerializeField] private TextMeshProUGUI SspinMiniText;
//     [SerializeField] private TextMeshProUGUI SspinDoubleMiniText;
//     [SerializeField] private TextMeshProUGUI SspinTripleMiniText;
//     [SerializeField] private TextMeshProUGUI TspinText;
//     [SerializeField] private TextMeshProUGUI TspinSingleText;
//     [SerializeField] private TextMeshProUGUI TspinDoubleText;
//     [SerializeField] private TextMeshProUGUI TspinTripleText;
//     [SerializeField] private TextMeshProUGUI TspinMiniText;
//     [SerializeField] private TextMeshProUGUI TspinDoubleMiniText;
//     [SerializeField] private TextMeshProUGUI ZspinText;
//     [SerializeField] private TextMeshProUGUI ZspinSingleText;
//     [SerializeField] private TextMeshProUGUI ZspinDoubleText;
//     [SerializeField] private TextMeshProUGUI ZspinTripleText;
//     [SerializeField] private TextMeshProUGUI ZspinMiniText;
//     [SerializeField] private TextMeshProUGUI ZspinDoubleMiniText;
//     [SerializeField] private TextMeshProUGUI ZspinTripleMiniText;

//     // 干渉するクラス
//     Effects effects;

//     /// <summary>
//     /// インスタンス化
//     /// </summary>
//     private void Awake()
//     {
//         effects = FindObjectOfType<Effects>();
//     }

//     /// <summary> 表示するスピンまたは列消去のテキストを判別する関数 </summary>
//     /// <param name="_lineClearCount"> 消去ライン数 </param>
//     public void SpinTextDisplay(SpinTypeNames _spinType, int _lineClearCount)
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.SpinTextDisplay, eLogTitle.Start);

//         SpinTextAnimation(DetermineTextToDisplay(_spinType, _lineClearCount), _lineClearCount);

//         // 鳴らすサウンドの決定も行う
//         if (_spinType != SpinTypeNames.None)
//         {
//             if (_lineClearCount >= 1)
//             {
//                 AudioManager.Instance.PlaySound(eAudioName.SpinDestroy);
//             }
//             else
//             {
//                 AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
//             }
//         }
//         else
//         {
//             if (_lineClearCount >= 1)
//             {
//                 AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
//             }
//             else
//             {
//                 AudioManager.Instance.PlaySound(eAudioName.NormalDrop);
//             }
//         }

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.SpinTextDisplay, eLogTitle.End);
//     }

//     /// <summary> 表示するスピンまたは列消去のテキストを特定する関数 </summary>
//     /// <param name="_spinType"> スピンタイプ </param>
//     /// <param name="_lineClearCount"> 消去ライン数 </param>
//     /// <returns> 表示するテキスト </returns>
//     private TextMeshProUGUI DetermineTextToDisplay(SpinTypeNames _spinType, int _lineClearCount)
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.DetermineTextToDisplay, eLogTitle.Start);

//         // スピンタイプと消去ライン数に対応するテキストをマッピングするディクショナリ
//         Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>> spinTypeTextMapping = new Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>>
//     {
//         { SpinTypeNames.Ispin, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, IspinText },
//                 { 1, IspinSingleText },
//                 { 2, IspinDoubleText },
//                 { 3, IspinTripleText },
//                 { 4, IspinQuattroText }
//             }
//         },
//         { SpinTypeNames.IspinMini, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, IspinMiniText },
//                 { 1, IspinMiniText }
//             }
//         },
//         { SpinTypeNames.Jspin, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, JspinText },
//                 { 1, JspinSingleText },
//                 { 2, JspinDoubleText },
//                 { 3, JspinTripleText }
//             }
//         },
//         { SpinTypeNames.Lspin, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, LspinText },
//                 { 1, LspinSingleText },
//                 { 2, LspinDoubleText },
//                 { 3, LspinTripleText }
//             }
//         },
//         { SpinTypeNames.Sspin, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, SspinText },
//                 { 1, SspinSingleText },
//                 { 2, SspinDoubleText },
//                 { 3, SspinTripleText }
//             }
//         },
//         { SpinTypeNames.SspinMini, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, SspinMiniText },
//                 { 1, SspinMiniText },
//                 { 2, SspinDoubleMiniText },
//                 { 3, SspinTripleMiniText }
//             }
//         },
//         { SpinTypeNames.Tspin, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, TspinText },
//                 { 1, TspinSingleText },
//                 { 2, TspinDoubleText },
//                 { 3, TspinTripleText }
//             }
//         },
//         { SpinTypeNames.TspinMini, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, TspinMiniText },
//                 { 1, TspinMiniText },
//                 { 2, TspinDoubleMiniText }
//             }
//         },
//         { SpinTypeNames.Zspin, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, ZspinText },
//                 { 1, ZspinSingleText },
//                 { 2, ZspinDoubleText },
//                 { 3, ZspinTripleText }
//             }
//         },
//         { SpinTypeNames.ZspinMini, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, ZspinMiniText },
//                 { 1, ZspinMiniText },
//                 { 2, ZspinDoubleMiniText },
//                 { 3, ZspinTripleMiniText }
//             }
//         },
//         { SpinTypeNames.None, new Dictionary<int, TextMeshProUGUI>
//             {
//                 { 0, null},
//                 { 1, null },
//                 { 2, null },
//                 { 3, null },
//                 { 4, TetrisText },
//             }
//         }

//     };
//         // 初期化
//         TextMeshProUGUI displayText = null;

//         // スピンタイプまたは消去ライン数に対応したテキストを選択
//         if (spinTypeTextMapping.ContainsKey(_spinType) && spinTypeTextMapping[_spinType].ContainsKey(_lineClearCount))
//         {
//             displayText = spinTypeTextMapping[_spinType][_lineClearCount];
//         }
//         else
//         {
//             LogHelper.ErrorLog(eClasses.TextEffect, eMethod.DetermineTextToDisplay, eLogTitle.KeyNotFound);
//         }

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.DetermineTextToDisplay, eLogTitle.End);
//         return displayText;
//     }

//     /// <summary> スピンまたは列消去のテキストのアニメーションを行う関数を呼ぶ関数 </summary>
//     /// <param name="_displayText"> 表示するテキスト </param>
//     private void SpinTextAnimation(TextMeshProUGUI _displayText, int __LineClearCount)
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.SpinTextAnimation, eLogTitle.Start);

//         // 表示するテキストが存在しない場合、処理をスキップする
//         if (_displayText != null)
//         {
//             TextMeshProUGUI instantiatedText = Instantiate(_displayText, SpinTextsPanel);
//             FadeInAndOutType1(instantiatedText);
//             // SpinTextMove(instantiatedText.transform);
//             if (_displayText == null && __LineClearCount != 0)
//             {
//                 effects.LineClearEffect();
//             }
//             else
//             {
//                 effects.SpinEffect();
//             }
//         }
//         else
//         {
//             // LogHelper.ErrorLog(eClasses.TextEffect, eMethod.SpinTextAnimation, eLogTitle.NullDisplayText);
//         }

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.SpinTextAnimation, eLogTitle.End);
//     }

//     /// <summary> スピンまたは列消去のテキストのフェードインとフェードアウトを行う関数 </summary>
//     /// <param name="_displayText"> 表示するテキスト </param>
//     private void FadeInAndOutType1(TextMeshProUGUI _displayText)
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.FadeInAndOutType1, eLogTitle.Start);

//         _displayText.gameObject.SetActive(true);
//         var sequence = DOTween.Sequence();
//         sequence
//             .Append(_displayText.DOFade(0, 0.02f).SetLoops(20, LoopType.Yoyo)) // チカチカ（0.02秒で透明化を10回）
//             .AppendInterval(1f) // 1秒間表示
//             .Append(_displayText.DOFade(0, 0.5f)) // 0.5秒でフェードアウト
//             .OnComplete(() =>
//             {
//                 _displayText.gameObject.SetActive(false);
//             });
//         sequence.Play();

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.FadeInAndOutType1, eLogTitle.End);
//     }

//     // /// <summary> スピンまたは列消去のテキストの移動を行う関数 </summary>
//     // /// <param name="_displayText"> 表示するテキストのトランスフォーム </param>
//     // private void SpinTextMove(Transform _displayText)
//     // {
//     //     LogHelper.DebugLog(eClasses.TextEffect, eMethod.SpinTextMove, eLogTitle.Start);

//     //     // 元の表示するテキストの座標 //
//     //     float displayTextX = Mathf.RoundToInt(_displayText.transform.position.x);
//     //     float displayTextY = Mathf.RoundToInt(_displayText.transform.position.y);
//     //     float displayTextZ = Mathf.RoundToInt(_displayText.transform.position.z);

//     //     var sequence = DOTween.Sequence();
//     //     sequence
//     //         .Append(_displayText.DOMoveY(displayTextY + TextMovementSettings.SPIN_MOVE_DISTANCE, TextMovementSettings.SPIN_MOVE_INTERVAL_X).SetEase(Ease.OutSine))
//     //         .Append(_displayText.DOMoveX(displayTextX - TextMovementSettings.SPIN_MOVE_DISTANCE, TextMovementSettings.SPIN_MOVE_INTERVAL_Y).SetEase(Ease.InQuint))
//     //         .OnComplete(() =>
//     //         {
//     //             _displayText.position = new Vector3(displayTextX, displayTextY, displayTextZ);
//     //         });

//     //     LogHelper.DebugLog(eClasses.TextEffect, eMethod.SpinTextMove, eLogTitle.End);
//     // }

//     /// <summary> BackToBackアニメーションを行う関数 </summary>
//     public void BackToBackAnimation()
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.BackToBackAnimation, eLogTitle.Start);

//         TextMeshProUGUI instantiatedText = Instantiate(BackToBackText, Canvas);
//         FadeInAndOutType1(instantiatedText);

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.BackToBackAnimation, eLogTitle.End);
//     }

//     /// <summary> PerfectClearアニメーションを行う関数 </summary>
//     public void PerfectClearAnimation()
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.PerfectClearAnimation, eLogTitle.Start);

//         TextMeshProUGUI instantiatedText = Instantiate(PerfectClearText, Canvas);
//         FadeInAndOutType1(instantiatedText);

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.PerfectClearAnimation, eLogTitle.End);
//     }

//     /// <summary> ReadyGoアニメーションを行う関数 </summary>
//     public void ReadyGoAnimation()
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.ReadyGoAnimation, eLogTitle.Start);

//         TextMeshProUGUI ready = Instantiate(ReadyText, Canvas);
//         TextMeshProUGUI go = Instantiate(GoText, Canvas);

//         Sequence sequence_ready = DOTween.Sequence();
//         Sequence sequence_go = DOTween.Sequence();

//         sequence_ready
//             .Append(ready.DOFade(1, TextMovementSettings.READY_FADE_IN_INTERVAL))
//             .AppendInterval(TextMovementSettings.READY_WAIT_INTERVAL)
//             .Append(ready.DOFade(0, TextMovementSettings.READY_FADE_OUT_INTERVAL))
//             .OnComplete(() =>
//             {
//                 sequence_go
//                     .Append(go.DOFade(1, TextMovementSettings.GO_FADE_IN_INTERVAL))
//                     .AppendInterval(TextMovementSettings.GO_WAIT_INTERVAL)
//                     .Append(go.DOFade(0, TextMovementSettings.GO_FADE_OUT_INTERVAL));
//             });

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.ReadyGoAnimation, eLogTitle.End);
//     }

//     /// <summary> すべてのアニメーションを停止させる関数 </summary>
//     public void StopAnimation()
//     {
//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.StopAnimation, eLogTitle.Start);

//         DOTween.KillAll();

//         LogHelper.DebugLog(eClasses.TextEffect, eMethod.StopAnimation, eLogTitle.End);
//     }
// }

/////////////////////////////////////////////////////////