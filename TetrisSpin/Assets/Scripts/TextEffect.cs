using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

/// <summary>
/// ゲーム画面のテキストを管理するクラス
/// </summary>
public class TextEffect : MonoBehaviour
{
    // Canvas //
    [SerializeField] private RectTransform Canvas;

    // 表示できるテキスト //
    [SerializeField] private TextMeshProUGUI ReadyText;
    [SerializeField] private TextMeshProUGUI GoText;

    [SerializeField] private TextMeshProUGUI BackToBackText;
    [SerializeField] private TextMeshProUGUI PerfectClearText;
    [SerializeField] private TextMeshProUGUI RensText;

    [SerializeField] private TextMeshProUGUI OneLineClearText;
    [SerializeField] private TextMeshProUGUI TwoLineClearText;
    [SerializeField] private TextMeshProUGUI ThreeLineClearText;
    [SerializeField] private TextMeshProUGUI TetrisText;
    [SerializeField] private TextMeshProUGUI IspinText;
    [SerializeField] private TextMeshProUGUI IspinSingleText;
    [SerializeField] private TextMeshProUGUI IspinDoubleText;
    [SerializeField] private TextMeshProUGUI IspinTripleText;
    [SerializeField] private TextMeshProUGUI IspinQuattroText;
    [SerializeField] private TextMeshProUGUI IspinMiniText;
    [SerializeField] private TextMeshProUGUI JspinText;
    [SerializeField] private TextMeshProUGUI JspinSingleText;
    [SerializeField] private TextMeshProUGUI JspinDoubleText;
    [SerializeField] private TextMeshProUGUI JspinTripleText;
    [SerializeField] private TextMeshProUGUI JpinMiniText;
    [SerializeField] private TextMeshProUGUI JspinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI JspinTripleMiniText;
    [SerializeField] private TextMeshProUGUI LspinText;
    [SerializeField] private TextMeshProUGUI LspinSingleText;
    [SerializeField] private TextMeshProUGUI LspinDoubleText;
    [SerializeField] private TextMeshProUGUI LspinTripleText;
    [SerializeField] private TextMeshProUGUI LspinMiniText;
    [SerializeField] private TextMeshProUGUI LspinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI LspinTripleMiniText;
    [SerializeField] private TextMeshProUGUI SspinText;
    [SerializeField] private TextMeshProUGUI SspinSingleText;
    [SerializeField] private TextMeshProUGUI SspinDoubleText;
    [SerializeField] private TextMeshProUGUI SspinTripleText;
    [SerializeField] private TextMeshProUGUI SspinMiniText;
    [SerializeField] private TextMeshProUGUI SspinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI SspinTripleMiniText;
    [SerializeField] private TextMeshProUGUI TspinText;
    [SerializeField] private TextMeshProUGUI TspinSingleText;
    [SerializeField] private TextMeshProUGUI TspinDoubleText;
    [SerializeField] private TextMeshProUGUI TspinTripleText;
    [SerializeField] private TextMeshProUGUI TspinMiniText;
    [SerializeField] private TextMeshProUGUI TspinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI ZspinText;
    [SerializeField] private TextMeshProUGUI ZspinSingleText;
    [SerializeField] private TextMeshProUGUI ZspinDoubleText;
    [SerializeField] private TextMeshProUGUI ZspinTripleText;
    [SerializeField] private TextMeshProUGUI ZspinMiniText;
    [SerializeField] private TextMeshProUGUI ZspinDoubleMiniText;
    [SerializeField] private TextMeshProUGUI ZspinTripleMiniText;

    // 透明度 //
    private int Alpha_0 = 0; // 0の時は透明
    private int Alpha_1 = 1; // 1の時は不透明

    /// <summary> 表示するテキストを判別する関数 </summary>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    public void TextDisplay(SpinTypeNames _spinType, int _lineClearCount)
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextDisplay, eLogTitle.Start);

        TextAnimation(DetermineTextToDisplay(_spinType, _lineClearCount));

        // 鳴らすサウンドの決定も行う
        if (_spinType != SpinTypeNames.None)
        {
            if (_lineClearCount >= 1)
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
            if (_lineClearCount >= 1)
            {
                AudioManager.Instance.PlaySound(eAudioName.NormalDestroy);
            }
            else
            {
                AudioManager.Instance.PlaySound(eAudioName.NormalDrop);
            }
        }

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextDisplay, eLogTitle.End);
    }

    /// <summary> 表示するテキストを特定する関数 </summary>
    /// <param name="_spinType"> スピンタイプ </param>
    /// <param name="_lineClearCount"> 消去ライン数 </param>
    /// <returns> 表示するテキスト </returns>
    private TextMeshProUGUI DetermineTextToDisplay(SpinTypeNames _spinType, int _lineClearCount)
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.DetermineTextToDisplay, eLogTitle.Start);

        // スピンタイプと消去ライン数に対応するテキストをマッピングするディクショナリ
        Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>> spinTypeTextMapping = new Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>>
    {
        { SpinTypeNames.Ispin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, IspinText },
                { 1, IspinSingleText },
                { 2, IspinDoubleText },
                { 3, IspinTripleText },
                { 4, IspinQuattroText }
            }
        },
        { SpinTypeNames.IspinMini, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, IspinMiniText },
                { 1, IspinMiniText }
            }
        },
        { SpinTypeNames.Jspin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, JspinText },
                { 1, JspinSingleText },
                { 2, JspinDoubleText },
                { 3, JspinTripleText }
            }
        },
        { SpinTypeNames.Lspin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, LspinText },
                { 1, LspinSingleText },
                { 2, LspinDoubleText },
                { 3, LspinTripleText }
            }
        },
        { SpinTypeNames.Sspin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, SspinText },
                { 1, SspinSingleText },
                { 2, SspinDoubleText },
                { 3, SspinTripleText }
            }
        },
        { SpinTypeNames.SspinMini, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, SspinMiniText },
                { 1, SspinMiniText },
                { 2, SspinDoubleMiniText },
                { 3, SspinTripleMiniText }
            }
        },
        { SpinTypeNames.Tspin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, TspinText },
                { 1, TspinSingleText },
                { 2, TspinDoubleText },
                { 3, TspinTripleText }
            }
        },
        { SpinTypeNames.TspinMini, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, TspinMiniText },
                { 1, TspinMiniText },
                { 2, TspinDoubleMiniText }
            }
        },
        { SpinTypeNames.Zspin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, ZspinText },
                { 1, ZspinSingleText },
                { 2, ZspinDoubleText },
                { 3, ZspinTripleText }
            }
        },
        { SpinTypeNames.ZspinMini, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, ZspinMiniText },
                { 1, ZspinMiniText },
                { 2, ZspinDoubleMiniText },
                { 3, ZspinTripleMiniText }
            }
        },
        { SpinTypeNames.None, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, null},
                { 1, OneLineClearText },
                { 2, TwoLineClearText },
                { 3, ThreeLineClearText },
                { 4, TetrisText },
            }
        }

    };
        // 初期化
        TextMeshProUGUI displayText = null;

        // スピンタイプと消去ライン数に対応したテキストを選択
        if (spinTypeTextMapping.ContainsKey(_spinType) && spinTypeTextMapping[_spinType].ContainsKey(_lineClearCount))
        {
            displayText = spinTypeTextMapping[_spinType][_lineClearCount]; // 対応したテキストを実際に表示させる
        }
        else
        {
            LogHelper.ErrorLog(eClasses.TextEffect, eMethod.DetermineTextToDisplay, eLogTitle.KeyNotFound);
        }

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.DetermineTextToDisplay, eLogTitle.End);
        return displayText;
    }

    /// <summary> テキストのアニメーションを行う関数 </summary>
    /// <param name="_displayText"> 表示するテキスト </param>
    private void TextAnimation(TextMeshProUGUI _displayText)
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextAnimation, eLogTitle.Start);

        // 表示するテキストが存在しない場合、処理をスキップする
        if (_displayText != null)
        {
            TextMeshProUGUI instantiatedText = Instantiate(_displayText, Canvas);
            TextFadeInAndOut(instantiatedText);
            TextMove(instantiatedText.transform);
        }

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextAnimation, eLogTitle.End);
    }

    /// <summary> テキストのフェードインとフェードアウトを行う関数 </summary>
    /// <param name="_displayText"> 表示するテキスト </param>
    private void TextFadeInAndOut(TextMeshProUGUI _displayText)
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextFadeInAndOut, eLogTitle.Start);

        float fadeInInterval = 0.3f;
        float fadeOutInterval = 1f;
        float waitInterval = 2f; // TODO 拡張性を広げる

        var sequence = DOTween.Sequence();
        sequence
            .Append(_displayText.DOFade(Alpha_1, fadeInInterval))
            .AppendInterval(waitInterval)
            .Append(_displayText.DOFade(Alpha_0, fadeOutInterval));

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextFadeInAndOut, eLogTitle.End);
    }

    /// <summary> テキストの移動を行う関数 </summary>
    /// <param name="_displayText"> 表示するテキストのトランスフォーム </param>
    private void TextMove(Transform _displayText)
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextMove, eLogTitle.Start);

        float moveInterval_x = 2f;
        float moveInterval_y = 1.2f;
        float moveDistance = 600f; // TODO 拡張性を広げる

        float displayText_x = Mathf.RoundToInt(_displayText.transform.position.x);
        float displayText_y = Mathf.RoundToInt(_displayText.transform.position.y);
        float displayText_z = Mathf.RoundToInt(_displayText.transform.position.z);

        var sequence = DOTween.Sequence();
        sequence
            .Append(_displayText.DOMoveY(displayText_y + moveDistance, moveInterval_x).SetEase(Ease.OutSine))
            .Append(_displayText.DOMoveX(displayText_x - moveDistance, moveInterval_y).SetEase(Ease.InQuint))
            .OnComplete(() =>
            {
                _displayText.position = new Vector3(displayText_x, displayText_y, displayText_z);
            });

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.TextMove, eLogTitle.End);
    }

    /// <summary> BackToBackアニメーションを行う関数 </summary>
    public void BackToBackAnimation()
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.BackToBackAnimation, eLogTitle.Start);

        TextMeshProUGUI instantiatedText = Instantiate(BackToBackText, Canvas);
        TextFadeInAndOut(instantiatedText);

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.BackToBackAnimation, eLogTitle.End);
    }

    /// <summary> PerfectClearアニメーションを行う関数 </summary>
    public void PerfectClearAnimation()
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.PerfectClearAnimation, eLogTitle.Start);

        TextMeshProUGUI instantiatedText = Instantiate(PerfectClearText, Canvas);
        TextFadeInAndOut(instantiatedText);

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.PerfectClearAnimation, eLogTitle.End);
    }

    /// <summary> ReadyGoアニメーションを行う関数 </summary>
    public void ReadyGoAnimation()
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.ReadyGoAnimation, eLogTitle.Start);

        float fadeInInterval = 0.3f;
        float fadeOutInterval = 0f;
        float waitInterval_ready = 3f;
        float waitInterval_go = 2f;

        TextMeshProUGUI ready = Instantiate(ReadyText, Canvas);
        TextMeshProUGUI go = Instantiate(GoText, Canvas);

        Sequence sequence_ready = DOTween.Sequence();
        Sequence sequence_go = DOTween.Sequence();

        sequence_ready
            .Append(ready.DOFade(Alpha_1, fadeInInterval))
            .AppendInterval(waitInterval_ready)
            .Append(ready.DOFade(Alpha_0, fadeOutInterval))
            .OnComplete(() =>
            {
                sequence_go
                    .Append(go.DOFade(Alpha_1, fadeInInterval))
                    .AppendInterval(waitInterval_go)
                    .Append(go.DOFade(Alpha_0, fadeOutInterval));
            });

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.ReadyGoAnimation, eLogTitle.End);
    }

    /// <summary> すべてのアニメーションを停止させる関数 </summary>
    public void StopAnimation()
    {
        LogHelper.DebugLog(eClasses.TextEffect, eMethod.StopAnimation, eLogTitle.Start);

        DOTween.KillAll();

        LogHelper.DebugLog(eClasses.TextEffect, eMethod.StopAnimation, eLogTitle.End);
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
//     int Alpha_1 = 1; // 1の時は不透明

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

//         TextFadeInAndOut(displayTextText); // 選ばれたテキストのフェードインとフェードアウトを行う

//         TextMove(displayText_Transform); // 選ばれたテキストの移動アニメーションを行う
//     }

//     //選ばれたテキストのフェードインとフェードアウトを行う関数
//     private void TextFadeInAndOut(TextMeshProUGUI displayText)
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
//             .Append(displayText.DOFade(Alpha_1, fadeInInterval))
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

//         TextFadeInAndOut(displayTextText); // 選ばれたテキストのフェードインとフェードアウトを行う
//     }

//     // PerfectClearの表示をする関数を呼ぶ関数 //
//     private void PerfectClearAnimation()
//     {
//         TextMeshProUGUI InstantiatedText = Instantiate(PerfectClearText, Canvas);

//         TextMeshProUGUI displayTextText = InstantiatedText.GetComponent<TextMeshProUGUI>();

//         TextFadeInAndOut(displayTextText); // 選ばれたテキストのフェードインとフェードアウトを行う
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
//             .Append(ready.DOFade(Alpha_1, fadeInInterval))
//             .AppendInterval(waitInterval_ready)
//             .Append(ready.DOFade(Alpha_0, fadeOutInterval))
//             .OnComplete(() =>
//             {
//                 // Readyアニメーションが完了したらGoアニメーションを開始
//                 sequence_go
//                     .Append(go.DOFade(Alpha_1, fadeInInterval))
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

/////////////////////////////////////////////////////////