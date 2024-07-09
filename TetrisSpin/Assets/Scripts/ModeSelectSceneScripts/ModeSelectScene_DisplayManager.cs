using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ゲームモード毎の表示する Image や Text を管理する構造体
/// </summary>
/// <remarks>
/// ゲームモードの種類は GameManager スクリプトに記載
/// </remarks>
[System.Serializable]
public struct GameModeUI
{
    /// <summary> GameMode 選択の Panel </summary>
    [SerializeField] private GameObject modeSelectPanel;
    /// <summary> ゲームモード毎の ButtonImage </summary>
    [SerializeField] private Image[] gameModeButtonImages;
    /// <summary> ゲームモード毎の フレームImage </summary>
    [SerializeField] private Image[] gameModeFrameImages;
    /// <summary> ゲームモード毎の タイトルText </summary>
    [SerializeField] private TextMeshProUGUI[] gameModeTitleTexts;
    /// <summary> ゲームモード毎の クリア条件Text </summary>
    [SerializeField] private TextMeshProUGUI[] clearConditionTexts;
    /// <summary> ゲームモード毎の 制約Text </summary>
    [SerializeField] private TextMeshProUGUI[] constraintTexts;
    /// <summary> ゲームモード毎の クリア条件タイトルText </summary>
    [SerializeField] private TextMeshProUGUI clearConditionTitleText;
    /// <summary> ゲームモード毎の 制約TextタイトルText </summary>
    [SerializeField] private TextMeshProUGUI constraintTitleText;

    // ゲッタープロパティ //
    public Image[] GameModeButtonImages => gameModeButtonImages;
    public TextMeshProUGUI ClearConditionTitleText => clearConditionTitleText;
    public TextMeshProUGUI ConstraintTitleText => constraintTitleText;

    /// <summary> modeSelectPanel をアクティブにする関数 </summary>
    internal void SetActiveModeSelectPanel()
    {
        modeSelectPanel.gameObject.SetActive(true);
    }

    /// <summary> modeSelectPanel を非アクティブにする関数 </summary>
    internal void ResetActiveModeSelectPanel()
    {
        modeSelectPanel.gameObject.SetActive(false);
    }

    /// <summary> 指定したゲームモードの フレームImage を取得する関数 </summary>
    /// <param name="_gameModeType"> ゲームモード </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public Image GetGameModeFrameImage(eGameModeType _gameModeType)
    {
        Image gameModeFrameImage = gameModeFrameImages[(int)_gameModeType];

        return gameModeFrameImage;
    }

    /// <summary> 指定したゲームモードの タイトルText を取得する関数 </summary>
    /// <param name="_gameModeType"> ゲームモード </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public TextMeshProUGUI GetGameModeTitleText(eGameModeType _gameModeType)
    {
        TextMeshProUGUI gameModeTitleText = gameModeTitleTexts[(int)_gameModeType];

        return gameModeTitleText;
    }

    /// <summary> 指定したゲームモードの クリア条件Text を取得する関数 </summary>
    /// <param name="_gameModeType"> ゲームモード </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public TextMeshProUGUI GetGameModeClearConditionText(eGameModeType _gameModeType)
    {
        TextMeshProUGUI clearConditionText = clearConditionTexts[(int)_gameModeType];

        return clearConditionText;
    }

    /// <summary> 指定したゲームモードの 制約Text を取得する関数 </summary>
    /// <param name="_gameModeType"> ゲームモード </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public TextMeshProUGUI GetGameModeConstraintText(eGameModeType _gameModeType)
    {
        TextMeshProUGUI constraintText = constraintTexts[(int)_gameModeType];

        return constraintText;
    }
}

/// <summary>
/// ゲームレベル毎の表示する Image や Text を管理する構造体
/// </summary>
/// <remarks>
/// ゲームレベルの種類は GameManager スクリプトに記載
/// </remarks>
[System.Serializable]
public struct GameLevelUI
{
    /// <summary> GameLevel 選択の Panel </summary>
    [SerializeField] private GameObject levelSelectPanel;
    /// <summary> ゲームレベル毎の ButtonImage </summary>
    [SerializeField] private Image[] gameLevelButtonImages;
    /// <summary> ゲームレベル毎の フレームImage </summary>
    [SerializeField] private Image[] gameLevelFrameImages;
    /// <summary> ゲームレベル毎の タイトルText </summary>
    [SerializeField] private TextMeshProUGUI[] gameLevelTitleImages;
    /// <summary> ゲームレベル毎の 落下速度Text </summary>
    [SerializeField] private TextMeshProUGUI[] fallingSpeedTexts;
    /// <summary> ゲームレベル毎の 落下速度タイトルText </summary>
    [SerializeField] private TextMeshProUGUI fallingSpeedTitleText;

    // ゲッタープロパティ //
    public Image[] GameLevelButtonImages => gameLevelButtonImages;
    public TextMeshProUGUI FallingSpeedTitleText => fallingSpeedTitleText;

    /// <summary> 指定したゲームレベルの フレームImage を取得する関数 </summary>
    /// <param name="_gameLevelType"> ゲームレベル </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public Image GetGameLevelFrameImage(eGameLevelType _gameLevelType)
    {
        Image gameLevelFrameImage = gameLevelFrameImages[(int)_gameLevelType];

        return gameLevelFrameImage;
    }

    /// <summary> 指定したゲームレベルの タイトルText を取得する関数 </summary>
    /// <param name="_gameLevelType"> ゲームレベル </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public TextMeshProUGUI GetGameModeTitleText(eGameLevelType _gameLevelType)
    {
        TextMeshProUGUI gameLevelTitleText = gameLevelTitleImages[(int)_gameLevelType];

        return gameLevelTitleText;
    }

    /// <summary> 指定したゲームモードの 落下速度Text を取得する関数 </summary>
    /// <param name="_gameLevelType"> ゲームレベル </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public TextMeshProUGUI GetFallingSpeedText(eGameLevelType _gameLevelType)
    {
        TextMeshProUGUI fallingSpeedText = fallingSpeedTexts[(int)_gameLevelType];

        return fallingSpeedText;
    }
}

/// <summary>
/// TetRank 関連のゲームオブジェクトを管理する構造体
/// </summary>
[System.Serializable]
public struct TetRankUI //TODO staticにしていいか
{
    /// <summary> TetRank の種類 列挙型 </summary>
    public enum eTetRankType
    {
        I_Rank, J_Rank, L_Rank, O_Rank, S_Rank, T_Rank, Z_Rank
    }
    /// <summary> TetRank 毎の フレームImage </summary>
    [SerializeField] private Image[] tetRankFrameImages;
    /// <summary> TetRank 毎の タイトルText </summary>
    [SerializeField] private TextMeshProUGUI[] tetRankTitleTexts;

    /// <summary> 指定した TetRank の フレームImage を取得する関数 </summary>
    /// <param name="_tetRankType"> TetRank </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public Image GetTetRankFrameImage(eTetRankType _tetRankType)
    {
        Image tetRankFrameImage = tetRankFrameImages[(int)_tetRankType];

        return tetRankFrameImage;
    }

    /// <summary> 指定した TetRank の タイトルText を取得する関数 </summary>
    /// <param name="_tetRankType"> TetRank </param>
    /// <remarks>
    /// アクティブ状態にする処理を含む
    /// </remarks>
    public TextMeshProUGUI GetTetRankTitleText(eTetRankType _tetRankType)
    {
        TextMeshProUGUI tetRankTitleText = tetRankTitleTexts[(int)_tetRankType];

        return tetRankTitleText;
    }
}


/// <summary>
/// ModeSelectシーンのUI演出を管理するクラス
/// </summary>
public class ModeSelectScene_DisplayManager : MonoBehaviour
{
    // Game Modes //
    [SerializeField] private GameModeUI gameModeUI;

    // Game Levels //
    [SerializeField] private GameLevelUI gameLevelUI;

    // TetRanks //
    [SerializeField] private TetRankUI tetRankUI;

    /// <summary> 自己ベストタイム </summary>
    private float bestTime;

    /// <summary> 現在のハイライトされているゲームモード </summary>
    private eGameModeType currentHighlightedGameMode = eGameModeType.None;
    /// <summary> 現在選択されているゲームモード </summary>
    private eGameModeType currentPressededGameMode;
    /// <summary> 現在のハイライトされているゲームレベル </summary>
    private eGameLevelType currentHighlightedGameLevel = eGameLevelType.None;
    /// <summary> 現在選択されているゲームレベル </summary>
    private eGameLevelType currentPressededGameLevel;

    /// <summary> ゲームモード選択のトリガー </summary>
    private bool gameModeSelected = false;


    ///// ゲームモード選択時の処理 /////

    /// <summary> 100-TimeAttackModeがハイライトされた時の処理をする関数 </summary>
    public void HighlightedTimeAttack_100Mode()
    {
        HighlightedGameMode(eGameModeType.TimeAttack_100);
        currentHighlightedGameMode = eGameModeType.TimeAttack_100;
    }

    /// <summary> SpinMasterModeがハイライトされた時の処理をする関数 </summary>
    public void HighlightedSpinMasterMode()
    {
        HighlightedGameMode(eGameModeType.SpinMaster);
        currentHighlightedGameMode = eGameModeType.SpinMaster;
    }

    /// <summary> PracticeModeがハイライトされた時の処理をする関数 </summary>
    public void HighlightedPracticeMode()
    {
        HighlightedGameMode(eGameModeType.Practice);
        currentHighlightedGameMode = eGameModeType.Practice;
    }

    /// <summary> ハイライトが解除された時の処理をする関数 </summary>
    public void UnHighlighted()
    {
        if (gameModeSelected == false)
        {
            UnHighlightedGameMode(currentHighlightedGameMode);
            currentHighlightedGameMode = eGameModeType.None;
        }
    }

    /// <summary> 特定のモードがハイライトされた時の処理をする関数 </summary>
    /// <param name="_gameModeType"> ゲームモード </param>
    private void HighlightedGameMode(eGameModeType _gameModeType)
    {
        LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightGameModeCoroutine, eLogTitle.Start);

        gameModeUI.GetGameModeFrameImage(_gameModeType).gameObject.SetActive(true);
        gameModeUI.GetGameModeTitleText(_gameModeType).gameObject.SetActive(true);
        gameModeUI.GetGameModeClearConditionText(_gameModeType).gameObject.SetActive(true);
        gameModeUI.GetGameModeConstraintText(_gameModeType).gameObject.SetActive(true);
        gameModeUI.ClearConditionTitleText.gameObject.SetActive(true);
        gameModeUI.ConstraintTitleText.gameObject.SetActive(true);

        LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightGameModeCoroutine, eLogTitle.End);
    }

    /// <summary> 特定のモードのハイライトが解除された時の処理をする関数 </summary>
    /// <param name="_gameModeType"> ゲームモード </param>
    private void UnHighlightedGameMode(eGameModeType _gameModeType)
    {
        LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.UnHighlightGameModeCoroutine, eLogTitle.Start);

        gameModeUI.GetGameModeFrameImage(_gameModeType).gameObject.SetActive(false);
        gameModeUI.GetGameModeTitleText(_gameModeType).gameObject.SetActive(false);
        gameModeUI.GetGameModeClearConditionText(_gameModeType).gameObject.SetActive(false);
        gameModeUI.GetGameModeConstraintText(_gameModeType).gameObject.SetActive(false);
        gameModeUI.ClearConditionTitleText.gameObject.SetActive(false);
        gameModeUI.ConstraintTitleText.gameObject.SetActive(false);

        LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.UnHighlightGameModeCoroutine, eLogTitle.End);
    }

    /// <summary> 100-TimeAttackModeがプレスされた時の処理をする関数 </summary>
    public void PressedTimeAttack_100Mode()
    {

        gameModeSelected = true;
        currentPressededGameMode = currentHighlightedGameMode;


    }

    //////////

    // /// <summary> Image または Text を下からフェードインで表示する関数 </summary>
    // /// <param name="_graphic"> 表示する Image または Text </param>
    // /// <param name="_startY"> 開始Y座標 </param>
    // private IEnumerator FadeInFromBelowImageOrTextCoroutine(Graphic _graphic, float _startY)
    // {
    //     LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInFromBelowImageOrTextCoroutine, eLogTitle.Start);

    //     _graphic.gameObject.SetActive(true);

    //     var sequence = DOTween.Sequence();
    //     sequence.Append(_graphic.DOFade(0, 0f))
    //             .Append(_graphic.DOFade(1, 0.1f))
    //             .Join(_graphic.transform.DOMoveY(_startY, 0.1f).From(_startY - 100));

    //     yield return sequence.Play().WaitForCompletion();

    //     LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInFromBelowImageOrTextCoroutine, eLogTitle.End);
    // }

    // /// <summary> Image または Text をフェードアウトで表示する関数 </summary>
    // /// <param name="_graphic"> 表示する Image または Text </param>
    // /// <param name="_startY"> 開始Y座標 </param>
    // private IEnumerator FadeOutToAboveImageOrTextCoroutine(Graphic _graphic, float _startY)
    // {
    //     LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeOutToAboveImageOrTextCoroutine, eLogTitle.Start);

    //     _graphic.gameObject.SetActive(true);

    //     var sequence = DOTween.Sequence();
    //     sequence.Append(_graphic.DOFade(1, 0f))
    //             .Append(_graphic.DOFade(0, 0.2f));

    //     yield return sequence.Play().WaitForCompletion();

    //     LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeOutToAboveImageOrTextCoroutine, eLogTitle.End);
    // }
}


// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro;
// using DG.Tweening;

// /// <summary>
// /// ModeSelectシーンのUI演出を管理するクラス
// /// </summary>
// public class ModeSelectScene_DisplayManager : MonoBehaviour
// {
//     // public class Display
//     // {
//     //     public virtual void Highlighted()
//     //     {

//     //     }

//     //     public virtual void UnHighlighted()
//     //     {

//     //     }

//     //     public virtual void Pressed()
//     //     {

//     //     }

//     // }

//     // public class TimeAttack_100Mode : Display
//     // {
//     //     /// <summary> 100-TimeAttackModeのImage </summary>
//     //     [SerializeField] private Image timeAttack_100Image;
//     //     /// <summary> 100-TimeAttackのText </summary>
//     //     [SerializeField] private TextMeshProUGUI timeAttack_100Text;
//     //     /// <summary> 100-TimeAttackModeにおけるクリア条件のText </summary>
//     //     [SerializeField] private TextMeshProUGUI timeAttack_100_ClearConditionText;
//     //     /// <summary> 100-TimeAttackModeにおける制約のText </summary>
//     //     [SerializeField] private TextMeshProUGUI timeAttack_100_ConstraintText;

//     //     public override void Highlighted()
//     //     {
//     //         StartCoroutine(TimeAttack_100Coroutine());
//     //     }
//     // }

//     // Mode //

//     /// <summary> 100-TimeAttackModeのImage </summary>
//     [SerializeField] private Image timeAttack_100Image;
//     /// <summary> SpinMasterModeのImage </summary>
//     [SerializeField] private Image spinMasterImage;
//     /// <summary> PracticeModeのImage </summary>
//     [SerializeField] private Image practiceImage;

//     // GameLevel //

//     /// <summary> EasyLevelのImage </summary>
//     [SerializeField] private Image easyImage;
//     /// <summary> NormalLevelのImage </summary>
//     [SerializeField] private Image normalImage;
//     /// <summary> HardLevelのImage </summary>
//     [SerializeField] private Image hardImage;
//     /// <summary> PracticeモードにおけるEasyLevelのImage </summary>
//     [SerializeField] private Image practiceEasyImage;
//     /// <summary> PracticeモードにおけるNormalLevelのImage </summary>
//     [SerializeField] private Image practiceNormalImage;
//     /// <summary> PracticeモードにおけるHardLevelのImage </summary>
//     [SerializeField] private Image practiceHardImage;

//     // TetRank //

//     /// <summary> TetRank_I のImage </summary>
//     [SerializeField] private Image tetRankIImage;
//     /// <summary> TetRank_J のImage </summary>
//     [SerializeField] private Image tetRankJImage;
//     /// <summary> TetRank_L のImage </summary>
//     [SerializeField] private Image tetRankLImage;
//     /// <summary> TetRank_O のImage </summary>
//     [SerializeField] private Image tetRankOImage;
//     /// <summary> TetRank_S のImage </summary>
//     [SerializeField] private Image tetRankSImage;
//     /// <summary> TetRank_T のImage </summary>
//     [SerializeField] private Image tetRankTImage;
//     /// <summary> TetRank_Z のImage </summary>
//     [SerializeField] private Image tetRankZImage;

//     // FixedText //

//     /// <summary> 100-TimeAttackのText </summary>
//     [SerializeField] private TextMeshProUGUI timeAttack_100Text;
//     /// <summary> SpinMasterのText </summary>
//     [SerializeField] private TextMeshProUGUI spinMasterText;
//     /// <summary> PracticeのText </summary>
//     [SerializeField] private TextMeshProUGUI practiceText;
//     /// <summary> クリア条件のText </summary>
//     [SerializeField] private TextMeshProUGUI clearConditionText;
//     /// <summary> 制約のText </summary>
//     [SerializeField] private TextMeshProUGUI constraintText;
//     /// <summary> 落下速度のText </summary>
//     [SerializeField] private TextMeshProUGUI downSpeedText;
//     /// <summary> BestTimeのText </summary>
//     [SerializeField] private TextMeshProUGUI bestTimeText;
//     /// <summary> TetRank_I のText </summary>
//     [SerializeField] private TextMeshProUGUI tetRankIText;
//     /// <summary> TetRank_J のText </summary>
//     [SerializeField] private TextMeshProUGUI tetRankJText;
//     /// <summary> TetRank_L のText </summary>
//     [SerializeField] private TextMeshProUGUI tetRankLText;
//     /// <summary> TetRank_O のText </summary>
//     [SerializeField] private TextMeshProUGUI tetRankOText;
//     /// <summary> TetRank_S のText </summary>
//     [SerializeField] private TextMeshProUGUI tetRankSText;
//     /// <summary> TetRank_T のText </summary>
//     [SerializeField] private TextMeshProUGUI tetRankTText;
//     /// <summary> TetRank_Z のText </summary>
//     [SerializeField] private TextMeshProUGUI tetRankZText;
//     /// <summary> 100-TimeAttackModeにおけるクリア条件のText </summary>
//     [SerializeField] private TextMeshProUGUI timeAttack_100_ClearConditionText;
//     /// <summary> SpinMasterModeにおけるクリア条件のText </summary>
//     [SerializeField] private TextMeshProUGUI spinMaster_ClearConditionText;
//     /// <summary> PracticeModeにおけるクリア条件のText </summary>
//     [SerializeField] private TextMeshProUGUI practice_ClearConditionText;
//     /// <summary> 100-TimeAttackModeにおける制約のText </summary>
//     [SerializeField] private TextMeshProUGUI timeAttack_100_ConstraintText;
//     /// <summary> SpinMasterModeにおける制約のText </summary>
//     [SerializeField] private TextMeshProUGUI spinMaster_ConstraintText;
//     /// <summary> PracticeModeにおける制約のText </summary>
//     [SerializeField] private TextMeshProUGUI practice_ConstraintText;

//     /// <summary> 100-TimeAttackModeがハイライトされた時の処理をする関数 </summary>
//     public void HighlightedTimeAttack_100Mode()
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightedTimeAttack_100Mode, eLogTitle.Start);

//         StartCoroutine(TimeAttack_100Coroutine());

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightedTimeAttack_100Mode, eLogTitle.End);
//     }

//     /// <summary> SpinMasterModeがハイライトされた時の処理をする関数 </summary>
//     public void HighlightedSpinMasterMode()
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightedSpinMasterMode, eLogTitle.Start);

//         StartCoroutine(SpinMasterCoroutine());

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightedSpinMasterMode, eLogTitle.End);
//     }

//     /// <summary> PracticeModeがハイライトされた時の処理をする関数 </summary>
//     public void HighlightedPracticeMode()
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightedPracticeMode, eLogTitle.Start);

//         StartCoroutine(PracticeCoroutine());

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightedPracticeMode, eLogTitle.End);
//     }

//     /// <summary> 100-TimeAttackModeがハイライトされた時に複数のコルーチン処理を呼ぶ関数 </summary>
//     private IEnumerator TimeAttack_100Coroutine()
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.TimeAttack_100Coroutine, eLogTitle.Start);

//         // Footerの表示
//         yield return FadeInImageFromBelowCoroutine(timeAttack_100Image);

//         // ゲームモードのテキストの表示
//         yield return FadeInTextFromBelowCoroutine(timeAttack_100Text);

//         // クリア条件のテキストの表示
//         yield return FadeInTextFromBelowCoroutine(timeAttack_100_ClearConditionText);

//         // 制約のテキストの表示
//         yield return FadeInTextFromBelowCoroutine(timeAttack_100_ConstraintText);

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.TimeAttack_100Coroutine, eLogTitle.End);
//     }

//     /// <summary> SpinMasterModeがハイライトされた時に複数のコルーチン処理を呼ぶ関数 </summary>
//     private IEnumerator SpinMasterCoroutine()
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.SpinMasterCoroutine, eLogTitle.Start);

//         // Footerの表示
//         yield return FadeInImageFromBelowCoroutine(spinMasterImage);

//         // ゲームモードのテキストの表示
//         yield return FadeInTextFromBelowCoroutine(spinMasterText);

//         // クリア条件のテキストの表示
//         yield return FadeInTextFromBelowCoroutine(spinMaster_ClearConditionText);

//         // 制約のテキストの表示
//         yield return FadeInTextFromBelowCoroutine(spinMaster_ConstraintText);

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.SpinMasterCoroutine, eLogTitle.End);
//     }

//     /// <summary> PracticeModeがハイライトされた時に複数のコルーチン処理を呼ぶ関数 </summary>
//     private IEnumerator PracticeCoroutine()
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.PracticeCoroutine, eLogTitle.Start);

//         // Footerの表示
//         yield return FadeInImageFromBelowCoroutine(practiceImage);

//         // ゲームモードのテキストの表示
//         yield return FadeInTextFromBelowCoroutine(practiceText);

//         // クリア条件のテキストの表示
//         yield return FadeInTextFromBelowCoroutine(practice_ClearConditionText);

//         // 制約のテキストの表示
//         yield return FadeInTextFromBelowCoroutine(practice_ConstraintText);

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.PracticeCoroutine, eLogTitle.End);
//     }

//     /// <summary> Imageを下からフェードインで表示する関数 </summary>
//     /// <param name="_displayImage"> 表示するImage </param>
//     private IEnumerator FadeInImageFromBelowCoroutine(Image _displayImage)
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInImageFromBelowCoroutine, eLogTitle.Start);

//         _displayImage.gameObject.SetActive(true);
//         float startY = _displayImage.transform.position.y; // 相対的に移動させるため、テキストの初期Y座標を保存

//         var sequence = DOTween.Sequence();
//         sequence
//             .Append(_displayImage.DOFade(0, 0f))
//             .Append(_displayImage.DOFade(1, 0.2f)) // テキストを0から1へフェード
//             .Join(_displayImage.transform.DOMoveY(startY, 0.2f).From(startY - 100)); // y座標を-100から現在の位置へ相対的に移動

//         yield return sequence.Play().WaitForCompletion(); // シーケンスの完了を待機

//         // _displayImage.gameObject.SetActive(false);

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInImageFromBelowCoroutine, eLogTitle.End);
//     }

//     /// <summary> テキストを下からフェードインで表示する関数 </summary>
//     /// <param name="_displayText"> 表示するText </param>
//     private IEnumerator FadeInTextFromBelowCoroutine(TextMeshProUGUI _displayText)
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInTextFromBelowCoroutine, eLogTitle.Start);

//         _displayText.gameObject.SetActive(true);
//         float startY = _displayText.transform.position.y; // 相対的に移動させるため、テキストの初期Y座標を保存

//         var sequence = DOTween.Sequence();
//         sequence
//             .Append(_displayText.DOFade(0, 0f))
//             .Append(_displayText.DOFade(1, 0.2f)) // テキストを0から1へフェード
//             .Join(_displayText.transform.DOMoveY(startY, 0.2f).From(startY - 100)); // y座標を-100から現在の位置へ相対的に移動

//         yield return sequence.Play().WaitForCompletion(); // シーケンスの完了を待機

//         // _displayText.gameObject.SetActive(false);

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInTextFromBelowCoroutine, eLogTitle.End);
//     }

//     // /// <summary> コンポーネントを下からフェードインで表示する汎用関数 </summary>
//     // /// <param name="_displayComponent"> 表示するComponent </param>
//     // private IEnumerator FadeInFromBelowCoroutine(Component _displayComponent)
//     // {
//     //     LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInFromBelowCoroutine, eLogTitle.Start);

//     //     _displayComponent.gameObject.SetActive(true);
//     //     float startY = _displayComponent.transform.position.y; // 相対的に移動させるため、初期Y座標を保存

//     //     var sequence = DOTween.Sequence();
//     //     sequence
//     //         .Append(_displayComponent.GetComponent<CanvasRenderer>().SetAlpha(0))
//     //         .Append(DOTween.To(() => _displayComponent.GetComponent<CanvasRenderer>().GetAlpha(), x => _displayComponent.GetComponent<CanvasRenderer>().SetAlpha(x), 1, 0.2f)) // アルファ値を0から1へフェード
//     //         .Join(_displayComponent.transform.DOMoveY(startY, 0.2f).From(startY - 100)); // Y座標を-100から現在の位置へ相対的に移動

//     //     yield return sequence.Play().WaitForCompletion(); // シーケンスの完了を待機

//     //     // _displayComponent.gameObject.SetActive(false);

//     //     LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInFromBelowCoroutine, eLogTitle.End);
//     // }
// }

// /// <summary>
// /// ModeSelectシーンのUI演出を管理するクラス
// /// </summary>
// public class ModeSelectScene_DisplayManager : MonoBehaviour
// {
//     // Game Modes
//     [SerializeField] private GameModeUI timeAttack100;
//     [SerializeField] private GameModeUI spinMaster;
//     [SerializeField] private GameModeUI practice;
//     [SerializeField] private TetRankUI tetRank;


//     // Mode Images
//     [SerializeField] private Image timeAttack_100Image;
//     [SerializeField] private Image spinMasterImage;
//     [SerializeField] private Image practiceImage;

//     // Game Level Images
//     [SerializeField] private Image easyImage;
//     [SerializeField] private Image normalImage;
//     [SerializeField] private Image hardImage;
//     [SerializeField] private Image practiceEasyImage;
//     [SerializeField] private Image practiceNormalImage;
//     [SerializeField] private Image practiceHardImage;

//     // TetRank Images
//     [SerializeField] private Image tetRankIImage;
//     [SerializeField] private Image tetRankJImage;
//     [SerializeField] private Image tetRankLImage;
//     [SerializeField] private Image tetRankOImage;
//     [SerializeField] private Image tetRankSImage;
//     [SerializeField] private Image tetRankTImage;
//     [SerializeField] private Image tetRankZImage;

//     // FixedText
//     [SerializeField] private TextMeshProUGUI timeAttack_100Text;
//     [SerializeField] private TextMeshProUGUI spinMasterText;
//     [SerializeField] private TextMeshProUGUI practiceText;
//     [SerializeField] private TextMeshProUGUI clearConditionText;
//     [SerializeField] private TextMeshProUGUI constraintText;
//     [SerializeField] private TextMeshProUGUI downSpeedText;
//     [SerializeField] private TextMeshProUGUI bestTimeText;
//     [SerializeField] private TextMeshProUGUI tetRankIText;
//     [SerializeField] private TextMeshProUGUI tetRankJText;
//     [SerializeField] private TextMeshProUGUI tetRankLText;
//     [SerializeField] private TextMeshProUGUI tetRankOText;
//     [SerializeField] private TextMeshProUGUI tetRankSText;
//     [SerializeField] private TextMeshProUGUI tetRankTText;
//     [SerializeField] private TextMeshProUGUI tetRankZText;
//     [SerializeField] private TextMeshProUGUI timeAttack_100_ClearConditionText;
//     [SerializeField] private TextMeshProUGUI spinMaster_ClearConditionText;
//     [SerializeField] private TextMeshProUGUI practice_ClearConditionText;
//     [SerializeField] private TextMeshProUGUI timeAttack_100_ConstraintText;
//     [SerializeField] private TextMeshProUGUI spinMaster_ConstraintText;
//     [SerializeField] private TextMeshProUGUI practice_ConstraintText;

//     /// <summary> 100-TimeAttackModeがハイライトされた時の処理をする関数 </summary>
//     public void HighlightedTimeAttack_100Mode()
//     {
//         StartCoroutine(HighlightCoroutine(timeAttack_100Image, timeAttack_100Text, timeAttack_100_ClearConditionText, timeAttack_100_ConstraintText));
//     }

//     /// <summary> SpinMasterModeがハイライトされた時の処理をする関数 </summary>
//     public void HighlightedSpinMasterMode()
//     {
//         StartCoroutine(HighlightCoroutine(spinMasterImage, spinMasterText, spinMaster_ClearConditionText, spinMaster_ConstraintText));
//     }

//     /// <summary> PracticeModeがハイライトされた時の処理をする関数 </summary>
//     public void HighlightedPracticeMode()
//     {
//         StartCoroutine(HighlightCoroutine(practiceImage, practiceText, practice_ClearConditionText, practice_ConstraintText));
//     }

//     /// <summary> 特定のモードがハイライトされた時に複数のコルーチン処理を呼ぶ関数 </summary>
//     private IEnumerator HighlightCoroutine(Image modeImage, TextMeshProUGUI modeText, TextMeshProUGUI clearConditionText, TextMeshProUGUI constraintText)
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightCoroutine, eLogTitle.Start);

//         yield return FadeInFromBelowImageOrTextCoroutine(modeImage, modeImage.transform.position.y);
//         yield return FadeInFromBelowImageOrTextCoroutine(modeText, modeText.transform.position.y);
//         yield return FadeInFromBelowImageOrTextCoroutine(clearConditionText, clearConditionText.transform.position.y);
//         yield return FadeInFromBelowImageOrTextCoroutine(constraintText, constraintText.transform.position.y);

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.HighlightCoroutine, eLogTitle.End);
//     }

//     /// <summary> Image または Text を下からフェードインで表示する関数 </summary>
//     /// <param name="_graphic"> 表示する Image または Text </param>
//     /// <param name="_startY"> 開始Y座標 </param>
//     private IEnumerator FadeInFromBelowImageOrTextCoroutine(Graphic _graphic, float _startY)
//     {
//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInFromBelowImageOrTextCoroutine, eLogTitle.Start);

//         _graphic.gameObject.SetActive(true);

//         var sequence = DOTween.Sequence();
//         sequence.Append(_graphic.DOFade(0, 0f))
//                 .Append(_graphic.DOFade(1, 0.2f))
//                 .Join(_graphic.transform.DOMoveY(_startY, 0.2f).From(_startY - 100));

//         yield return sequence.Play().WaitForCompletion();

//         LogHelper.DebugLog(eClasses.ModeSelectScene_DisplayManager, eMethod.FadeInFromBelowImageOrTextCoroutine, eLogTitle.End);
//     }
// }

// /// <summary>
// /// グラフィック要素をリセットおよび準備する関数
// /// </summary>
// /// <param name="_graphic"> 対象のグラフィック要素 </param>
// private void ResetAndPrepareGraphic(Graphic _graphic)
// {
//     _graphic.DOKill(); // 現在のアニメーションを停止
//     _graphic.color = new Color(_graphic.color.r, _graphic.color.g, _graphic.color.b, 0); // 透明度を0にリセット
//     _graphic.transform.localPosition =
//         new Vector3(_graphic.transform.localPosition.x, _graphic.transform.localPosition.y, _graphic.transform.localPosition.z); // Y座標をリセット
//     _graphic.gameObject.SetActive(true); // グラフィック要素をアクティブに設定
// }

// // ハイライトするゲームモードに対応するグラフィック要素をリストに追加
//         List<Graphic> graphicsToAnimate = new List<Graphic>
//         {
//             gameModeUI.GetGameModeFrameImage(_gameModeType),
//             gameModeUI.GetGameModeTitleText(_gameModeType),
//             gameModeUI.GetGameModeClearConditionText(_gameModeType),
//             gameModeUI.GetGameModeConstraintText(_gameModeType)
//         };

//         // graphicsToAnimate の各座標をリスト化
//         List<Transform> graphicsToAnimateTransform = new List<Transform>
//         {
//             gameModeUI.GameModeFrameImageTransform,
//             gameModeUI.GameModeTitleTextTransform,
//             gameModeUI.ClearConditionTextTransform,
//             gameModeUI.ConstraintTextTransform,
//         };

//         // すべてのグラフィック要素を順番にフェードイン
//         for (int ii = 0; ii < graphicsToAnimate.Count; ii++)
//         {
//             yield return FadeOutToAboveImageOrTextCoroutine(graphicsToAnimate[ii], graphicsToAnimateTransform[ii].position.y);
//         }

// /// <summary> ゲームモード毎の フレームImageの初期座標 </summary>
// private Transform gameModeFrameImageTransform;
// /// <summary> ゲームモード毎の タイトルTextの初期座標 </summary>
// private Transform gameModeTitleTextTransform;
// /// <summary> ゲームモード毎の クリア条件Textの初期座標 </summary>
// private Transform clearConditionTextTransform;
// /// <summary> ゲームモード毎の 制約Textの初期座標 </summary>
// private Transform constraintTextTransform;

// // ゲッタープロパティ //
// internal Transform GameModeFrameImageTransform => gameModeFrameImageTransform;
// internal Transform GameModeTitleTextTransform => gameModeTitleTextTransform;
// internal Transform ClearConditionTextTransform => clearConditionTextTransform;
// internal Transform ConstraintTextTransform => constraintTextTransform;

// /// <summary> 各ゲームオブジェクトの初期座標を記録する関数 </summary>
// internal void InitializeGameModeUITransforms()
// {
//     gameModeFrameImageTransform = gameModeFrameImages[0].transform;
//     gameModeTitleTextTransform = gameModeTitleTexts[0].transform;
//     clearConditionTextTransform = clearConditionTexts[0].transform;
//     constraintTextTransform = constraintTexts[0].transform;
// }

// /// <summary> ゲームレベル毎の フレームImageの初期座標 </summary>
// private Transform gameLevelFrameImageTransform;
// /// <summary> ゲームレベル毎の タイトルTextの初期座標 </summary>
// private Transform gameLevelTitleTextTransform;
// /// <summary> ゲームレベル毎の 落下速度Textの初期座標 </summary>
// private Transform fallingSpeedTextTransform;

// // ゲッタープロパティ //
// public Transform GameLevelFrameImageTransform => gameLevelFrameImageTransform;
// public Transform GameLevelTitleTextTransform => gameLevelTitleTextTransform;
// public Transform FallingSpeedTextTransform => fallingSpeedTextTransform;

// /// <summary> 各ゲームオブジェクトの初期座標を記録する関数 </summary>
// internal void InitializeGameLevelUITransforms()
// {
//     gameLevelFrameImageTransform = gameLevelFrameImages[0].transform;
//     gameLevelTitleTextTransform = gameLevelTitleImages[0].transform;
//     fallingSpeedTextTransform = fallingSpeedTexts[0].transform;
// }

// /// <summary> TetRank 毎の フレームImageの初期座標 </summary>
// private Transform tetRankFrameImageTransform;
// /// <summary> TetRank 毎の タイトルTextの初期座標 </summary>
// private Transform tetRankTitleTextTransform;

// // ゲッタープロパティ //
// public Transform TetRankFrameImageTransform => tetRankFrameImageTransform;
// public Transform TetRankTitleTextTransform => tetRankTitleTextTransform;

// /// <summary> 各ゲームオブジェクトの初期座標を記録する関数 </summary>
// internal void InitializeTetRankUITransforms()
// {
//     tetRankFrameImageTransform = tetRankFrameImages[0].transform;
//     tetRankTitleTextTransform = tetRankTitleTexts[0].transform;
// }

// /// <summary>
// /// 各ゲームオブジェクトの初期座標を記録
// /// </summary>
// private void Start()
// {
//     gameModeUI.InitializeGameModeUITransforms();
//     gameLevelUI.InitializeGameLevelUITransforms();
//     tetRankUI.InitializeTetRankUITransforms();
// }

// /// <summary>  </summary>
// /// <param name="_gameModeType"> ゲームモード </param>
// private void StartHighlightCoroutine(eGameModeType _gameModeType)
// {
//     if (currentCoroutine != null)
//     {
//         StopCoroutine(currentCoroutine);
//     }
//     currentCoroutine = StartCoroutine(UnHighlightGameModeCoroutine(_gameModeType));
// }