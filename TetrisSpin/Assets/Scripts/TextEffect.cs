using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

///// ゲーム画面のテキストに関するスクリプト /////


// ↓このスクリプトで可能なこと↓ //

// テキストの表示
// テキストのアニメーション
// 攻撃ラインの値を計算

public class TextEffect : MonoBehaviour
{
    // Canvas //
    [SerializeField] private RectTransform Canvas;

    // 表示できるテキスト //
    [SerializeField] private TextMeshProUGUI Ready_Text;
    [SerializeField] private TextMeshProUGUI Go_Text;

    [SerializeField] private TextMeshProUGUI BackToBack_Text;
    [SerializeField] private TextMeshProUGUI PerfectClear_Text;
    [SerializeField] private TextMeshProUGUI Rens_Text;

    [SerializeField] private TextMeshProUGUI OneLineClear_Text;
    [SerializeField] private TextMeshProUGUI TwoLineClear_Text;
    [SerializeField] private TextMeshProUGUI ThreeLineClear_Text;
    [SerializeField] private TextMeshProUGUI Tetris_Text;
    [SerializeField] private TextMeshProUGUI Ispin_Text;
    [SerializeField] private TextMeshProUGUI IspinSingle_Text;
    [SerializeField] private TextMeshProUGUI IspinDouble_Text;
    [SerializeField] private TextMeshProUGUI IspinTriple_Text;
    [SerializeField] private TextMeshProUGUI IspinQuattro_Text;
    [SerializeField] private TextMeshProUGUI IspinMini_Text;
    [SerializeField] private TextMeshProUGUI Jspin_Text;
    [SerializeField] private TextMeshProUGUI JspinSingle_Text;
    [SerializeField] private TextMeshProUGUI JspinDouble_Text;
    [SerializeField] private TextMeshProUGUI JspinTriple_Text;
    [SerializeField] private TextMeshProUGUI Lspin_Text;
    [SerializeField] private TextMeshProUGUI LspinSingle_Text;
    [SerializeField] private TextMeshProUGUI LspinDouble_Text;
    [SerializeField] private TextMeshProUGUI LspinTriple_Text;
    [SerializeField] private TextMeshProUGUI Tspin_Text;
    [SerializeField] private TextMeshProUGUI TspinSingle_Text;
    [SerializeField] private TextMeshProUGUI TspinDouble_Text;
    [SerializeField] private TextMeshProUGUI TspinTriple_Text;
    [SerializeField] private TextMeshProUGUI TspinMini_Text;
    [SerializeField] private TextMeshProUGUI TspinDoubleMini_Text;

    // 透明度 //
    private int Alpha_0 = 0; // 0の時は透明
    private int Alpha_1 = 1; // 1の時は不透明

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

    // 攻撃ラインの値 //
    private int BackToBackBonus = 1;
    private int PerfectClearBonus = 10;
    private int[] RenBonus = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 4, 5 };

    // 干渉するスクリプト //
    private Board board;
    private GameStatus gameStatus;
    private SpinCheck spinCheck;

    // インスタンス化 //
    void Awake()
    {
        board = FindObjectOfType<Board>();
        gameStatus = FindObjectOfType<GameStatus>();
        spinCheck = FindObjectOfType<SpinCheck>();
    }

    // 表示するテキストを判別して、実際に表示する関数 //
    public void TextDisplay(int lineClearCount)
    {
        TextAnimation(DetermineTextToDisplay(spinCheck.spinTypeName, lineClearCount));

        // 鳴らすサウンドの決定も行う
        if (spinCheck.spinTypeName != SpinTypeNames.None)
        {
            if (lineClearCount >= 1)
            {
                AudioManager.Instance.PlaySound(AudioNames.SpinDestroy);
            }
            else
            {
                AudioManager.Instance.PlaySound(AudioNames.NormalDestroy);
            }
        }
        else
        {
            if (lineClearCount >= 1)
            {
                AudioManager.Instance.PlaySound(AudioNames.NormalDestroy);
            }
            else
            {
                AudioManager.Instance.PlaySound(AudioNames.NormalDrop);
            }
        }

        // if (spinCheck.spinTypeName == "None") // Spin判定がない場合
        // {
        //     DisplayNonSpinText(lineClearCount);
        // }
        // else // Spin判定がある場合
        // {
        //     DisplaySpinText(lineClearCount);
        // }
    }

    // // Spin判定がない場合のテキスト表示を処理する関数 //
    // private void DisplayNonSpinText(int lineClearCount)
    // {
    //     switch (lineClearCount)
    //     {
    //         case 0:
    //             gameStatus.Reset_Ren();
    //             break;
    //         case 1:
    //             gameStatus.Reset_BackToBack();
    //             RenAttackLines();
    //             PlaySoundAndAnimateText("Normal_Destroy", OneLineClear_Text);
    //             break;
    //         case 2:
    //             gameStatus.Reset_BackToBack();
    //             gameStatus.IncreaseAttackLines(TwoLineClearAttack);
    //             RenAttackLines();
    //             PlaySoundAndAnimateText("Normal_Destroy", TwoLineClear_Text);
    //             break;
    //         case 3:
    //             gameStatus.Reset_BackToBack();
    //             gameStatus.IncreaseAttackLines(ThreeLineClearAttack);
    //             RenAttackLines();
    //             PlaySoundAndAnimateText("Normal_Destroy", ThreeLineClear_Text);
    //             break;
    //         case 4:
    //             CheckBackToBack();
    //             RenAttackLines();
    //             gameStatus.IncreaseAttackLines(TetrisAttack);
    //             PlaySoundAndAnimateText("Tetris", Tetris_Text);
    //             break;
    //     }
    // }

    // 表示するテキストを特定する関数 //
    private TextMeshProUGUI DetermineTextToDisplay(SpinTypeNames spinType, int lineClearCount)
    {
        // スピンタイプと消去ライン数に対応するテキストをマッピングするディクショナリ
        Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>> spinTypeTextMapping = new Dictionary<SpinTypeNames, Dictionary<int, TextMeshProUGUI>>
    {
        { SpinTypeNames.I_SpinMini, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, IspinMini_Text },
                { 1, IspinMini_Text }
            }
        },
        { SpinTypeNames.I_Spin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, Ispin_Text },
                { 1, IspinSingle_Text },
                { 2, IspinDouble_Text },
                { 3, IspinTriple_Text },
                { 4, IspinQuattro_Text }
            }
        },
        { SpinTypeNames.J_Spin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, Jspin_Text },
                { 1, JspinSingle_Text },
                { 2, JspinDouble_Text },
                { 3, JspinTriple_Text }
            }
        },
        { SpinTypeNames.L_Spin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, Lspin_Text },
                { 1, LspinSingle_Text },
                { 2, LspinDouble_Text },
                { 3, LspinTriple_Text }
            }
        },
        { SpinTypeNames.T_Spin, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, Tspin_Text },
                { 1, TspinSingle_Text },
                { 2, TspinDouble_Text },
                { 3, TspinTriple_Text }
            }
        },
        { SpinTypeNames.T_SpinMini, new Dictionary<int, TextMeshProUGUI>
            {
                { 0, TspinMini_Text },
                { 1, TspinMini_Text },
                { 2, TspinDoubleMini_Text }
            }
        },
        { SpinTypeNames.None, new Dictionary<int, TextMeshProUGUI>
            {
                { 1, OneLineClear_Text },
                { 2, TwoLineClear_Text },
                { 3, ThreeLineClear_Text },
                { 4, Tetris_Text },
            }
        }

    };
        // スピンタイプと消去ライン数に対応する攻撃力をマッピングするディクショナリ
        Dictionary<SpinTypeNames, Dictionary<int, int>> spinTypeAttackMapping = new Dictionary<SpinTypeNames, Dictionary<int, int>>
    {
        { SpinTypeNames.J_Spin, new Dictionary<int, int>
            {
                { 1, 1 }, // JspinSingleAttack
                { 2, 3 }, // JspinDoubleAttack
                { 3, 6 }  // JspinTripleAttack
            }
        },
        { SpinTypeNames.L_Spin, new Dictionary<int, int>
            {
                { 1, 1 }, // LspinSingleAttack
                { 2, 3 }, // LspinDoubleAttack
                { 3, 6 }  // LspinTripleAttack
            }
        },
        { SpinTypeNames.I_SpinMini, new Dictionary<int, int>
            {
                { 1, 1 } // IspinMiniAttack
            }
        },
        { SpinTypeNames.I_Spin, new Dictionary<int, int>
            {
                { 1, 2 }, // IspinSingleAttack
                { 2, 4 }, // IspinDoubleAttack
                { 3, 6 }, // IspinTripleAttack
                { 4, 8 }  // IspinQuattroAttack
            }
        },
        { SpinTypeNames.T_Spin, new Dictionary<int, int>
            {
                { 1, 2 }, // TspinSingleAttack
                { 2, 4 }, // TspinDoubleAttack
                { 3, 6 }  // TspinTripleAttack
            }
        },
        { SpinTypeNames.T_SpinMini, new Dictionary<int, int>
            {
                { 1, 0 }, // TspinMiniAttack
                { 2, 1 }  // TspinDoubleMiniAttack
            }
        },
        { SpinTypeNames.None, new Dictionary<int, int>
            {
                { 1, 0 }, // OneLineClearAttack
                { 2, 1 }, // TwoLineClearAttack
                { 3, 2 }, // ThreeLineClearAttack
                { 4, 4 }  // TetrisAttack
            }
        }
    };

        // 初期化
        TextMeshProUGUI displayText = null;

        // スピンタイプと行消去数に基づいてテキストを選択
        if (spinTypeTextMapping.ContainsKey(spinType) && spinTypeTextMapping[spinType].ContainsKey(lineClearCount))
        {
            displayText = spinTypeTextMapping[spinType][lineClearCount]; // 対応したテキストを実際に表示させる
        }
        else
        {
            // エラー
        }
        // else
        // {
        //     // スピンタイプがない場合は、行消去テキストを選択
        //     switch (lineClearCount)
        //     {
        //         case 1:
        //             displayText = OneLineClear_Text;
        //             break;
        //         case 2:
        //             displayText = TwoLineClear_Text;
        //             break;
        //         case 3:
        //             displayText = ThreeLineClear_Text;
        //             break;
        //         case 4:
        //             displayText = Tetris_Text;
        //             break;
        //     }
        // }

        // スピンタイプと行消去数に基づいて攻撃力を選択
        if (spinTypeAttackMapping.ContainsKey(spinType) && spinTypeAttackMapping[spinType].ContainsKey(lineClearCount))
        {
            gameStatus.IncreaseAttackLines(spinTypeAttackMapping[spinType][lineClearCount]); // 対応した攻撃力を反映
        }

        CheckBackToBack(spinType, displayText); // BackToBack判定をチェック
        CheckRen(lineClearCount); // Ren判定をチェック
        if (board.CheckPerfectClear()) // PerfectClear判定をチェック
        {
            gameStatus.IncreaseAttackLines(PerfectClearBonus);
            PerfectClearAnimation();
        }

        return displayText;
    }

    // private TextMeshProUGUI DetermineTextToDisplay(string spinType, int lineClearCount)
    // {
    //     TextMeshProUGUI displayText = null;

    //     switch (spinType)
    //     {
    //         case "J-Spin":
    //             switch (lineClearCount)
    //             {
    //                 case 0:
    //                     displayText = Jspin_Text;
    //                     break;
    //                 case 1:
    //                     displayText = JspinSingle_Text;
    //                     break;
    //                 case 2:
    //                     displayText = JspinDouble_Text;
    //                     break;
    //                 case 3:
    //                     displayText = JspinTriple_Text;
    //                     break;
    //             }
    //             break;
    //         case "L-Spin":
    //             switch (lineClearCount)
    //             {
    //                 case 0:
    //                     displayText = Lspin_Text;
    //                     break;
    //                 case 1:
    //                     displayText = LspinSingle_Text;
    //                     break;
    //                 case 2:
    //                     displayText = LspinDouble_Text;
    //                     break;
    //                 case 3:
    //                     displayText = LspinTriple_Text;
    //                     break;
    //             }
    //             break;
    //         case "I-Spin Mini":
    //             switch (lineClearCount)
    //             {
    //                 case 0:
    //                 case 1:
    //                     displayText = IspinMini_Text;
    //                     break;
    //             }
    //             break;
    //         case "I-Spin":
    //             switch (lineClearCount)
    //             {
    //                 case 0:
    //                     displayText = Ispin_Text;
    //                     break;
    //                 case 1:
    //                     displayText = IspinSingle_Text;
    //                     break;
    //                 case 2:
    //                     displayText = IspinDouble_Text;
    //                     break;
    //                 case 3:
    //                     displayText = IspinTriple_Text;
    //                     break;
    //                 case 4:
    //                     displayText = IspinQuattro_Text;
    //                     break;
    //             }
    //             break;
    //         case "T-Spin":
    //             switch (lineClearCount)
    //             {
    //                 case 0:
    //                     displayText = Tspin_Text;
    //                     break;
    //                 case 1:
    //                     displayText = TspinSingle_Text;
    //                     break;
    //                 case 2:
    //                     displayText = TspinDouble_Text;
    //                     break;
    //                 case 3:
    //                     displayText = TspinTriple_Text;
    //                     break;
    //             }
    //             break;
    //         case "T-Spin Mini":
    //             switch (lineClearCount)
    //             {
    //                 case 0:
    //                     displayText = TspinMini_Text;
    //                     break;
    //                 case 1:
    //                     displayText = TspinMini_Text;
    //                     break;
    //                 case 2:
    //                     displayText = TspinDoubleMini_Text;
    //                     break;
    //             }
    //             break;
    //         default:
    //             switch (lineClearCount)
    //             {
    //                 case 1:
    //                     displayText = OneLineClear_Text;
    //                     break;
    //                 case 2:
    //                     displayText = TwoLineClear_Text;
    //                     break;
    //                 case 3:
    //                     displayText = ThreeLineClear_Text;
    //                     break;
    //                 case 4:
    //                     displayText = Tetris_Text;
    //                     break;
    //             }
    //             break;
    //     }

    //     CheckBackToBack(spinType, displayText);
    //     return displayText;
    // }


    // // Spin判定がある場合のテキスト表示を処理する関数 //
    // private void DisplaySpinText(int lineClearCount)
    // {
    //     CheckBackToBack();
    //     string spinType = spinCheck.spinTypeName;
    //     switch (spinType)
    //     {
    //         case "J-Spin":
    //             HandleSpinText(lineClearCount, Jspin_Text, JspinSingleAttack, JspinSingle_Text, JspinDoubleAttack, JspinDouble_Text, JspinTripleAttack, JspinTriple_Text);
    //             break;
    //         case "L-Spin":
    //             HandleSpinText(lineClearCount, Lspin_Text, LspinSingleAttack, LspinSingle_Text, LspinDoubleAttack, LspinDouble_Text, LspinTripleAttack, LspinTriple_Text);
    //             break;
    //         case "I-Spin Mini":
    //             HandleMiniSpinText(lineClearCount, IspinMini_Text, IspinMiniAttack, IspinMini_Text);
    //             break;
    //         case "I-Spin":
    //             HandleSpinText(lineClearCount, Ispin_Text, IspinSingleAttack, IspinSingle_Text, IspinDoubleAttack, IspinDouble_Text, IspinTripleAttack, IspinTriple_Text, IspinQuattroAttack, IspinQuattro_Text);
    //             break;
    //         case "T-Spin":
    //             HandleSpinText(lineClearCount, Tspin_Text, TspinSingleAttack, TspinSingle_Text, TspinDoubleAttack, TspinDouble_Text, TspinTripleAttack, TspinTriple_Text);
    //             break;
    //         case "T-Spin Mini":
    //             HandleMiniSpinText(lineClearCount, TspinMini_Text, TspinMiniAttack, TspinMini_Text, TspinDoubleMiniAttack, TspinDoubleMini_Text);
    //             break;
    //     }
    // }

    // private void HandleSpinText(int lineClearCount, TextMeshProUGUI baseText, int singleAttack, TextMeshProUGUI singleText, int doubleAttack, TextMeshProUGUI doubleText, int tripleAttack, TextMeshProUGUI tripleText, int quattroAttack = 0, TextMeshProUGUI quattroText = null)
    // {
    //     switch (lineClearCount)
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

    // private void HandleMiniSpinText(int lineClearCount, TextMeshProUGUI baseText, int miniAttack, TextMeshProUGUI miniText, int doubleMiniAttack = 0, TextMeshProUGUI doubleMiniText = null)
    // {
    //     switch (lineClearCount)
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

    // BackToBackの判定を確認し、ダメージの計算を行う関数 //
    private void CheckBackToBack(SpinTypeNames spinType, TextMeshProUGUI displayText)
    {
        // Spin判定がない、かつテトリスでない場合
        if (spinType == SpinTypeNames.None && displayText != Tetris_Text)
        {
            // BackToBack判定をリセット
            gameStatus.Reset_BackToBack();
        }
        else
        {
            if (!gameStatus.backToBack)
            {
                // BackToBack判定を付与
                gameStatus.Set_BackToBack();
            }
            else
            {
                // すでにBackToBack判定が付与されている場合(前回付与した判定をキープしていた場合)
                gameStatus.IncreaseAttackLines(BackToBackBonus);
                BackToBackAnimation();
            }
        }
    }

    // Renの判定を確認し、ダメージの計算を行う関数 //
    private void CheckRen(int lineClearCount)
    {
        if (lineClearCount >= 1)
        {
            // 1列以上消去していれば
            gameStatus.IncreaseRen();
            // int ren = Mathf.Min(gameStatus.ren, RenBonus.Length - 1);
            gameStatus.IncreaseAttackLines(RenBonus[gameStatus.ren]);
        }
        else
        {
            // 列消去ができていない場合、リセットする
            gameStatus.Reset_Ren();
        }
    }

    private void TextAnimation(TextMeshProUGUI displayText)
    {
        // 表示するテキストが存在しない場合、処理をスキップする
        if (displayText != null)
        {
            TextMeshProUGUI instantiatedText = Instantiate(displayText, Canvas);
            TextFadeInAndOut(instantiatedText);
            TextMove(instantiatedText.transform);
        }
    }

    private void TextFadeInAndOut(TextMeshProUGUI displayText)
    {
        float fadeInInterval = 0.3f;
        float fadeOutInterval = 1f;
        float waitInterval = 2f;

        var sequence = DOTween.Sequence();
        sequence
            .Append(displayText.DOFade(Alpha_1, fadeInInterval))
            .AppendInterval(waitInterval)
            .Append(displayText.DOFade(Alpha_0, fadeOutInterval));
    }

    private void TextMove(Transform displayText)
    {
        float moveInterval_x = 2f;
        float moveInterval_y = 1.2f;
        float moveDistance = 600f;

        float displayText_x = Mathf.RoundToInt(displayText.transform.position.x);
        float displayText_y = Mathf.RoundToInt(displayText.transform.position.y);
        float displayText_z = Mathf.RoundToInt(displayText.transform.position.z);

        var sequence = DOTween.Sequence();
        sequence
            .Append(displayText.DOMoveY(displayText_y + moveDistance, moveInterval_x).SetEase(Ease.OutSine))
            .Append(displayText.DOMoveX(displayText_x - moveDistance, moveInterval_y).SetEase(Ease.InQuint))
            .OnComplete(() =>
            {
                displayText.position = new Vector3(displayText_x, displayText_y, displayText_z);
            });
    }

    private void BackToBackAnimation()
    {
        TextMeshProUGUI instantiatedText = Instantiate(BackToBack_Text, Canvas);
        TextFadeInAndOut(instantiatedText);
    }

    private void PerfectClearAnimation()
    {
        TextMeshProUGUI instantiatedText = Instantiate(PerfectClear_Text, Canvas);
        TextFadeInAndOut(instantiatedText);
    }

    public void ReadyGoAnimation()
    {
        float fadeInInterval = 0.3f;
        float fadeOutInterval = 0f;
        float waitInterval_ready = 3f;
        float waitInterval_go = 2f;

        TextMeshProUGUI ready = Instantiate(Ready_Text, Canvas);
        TextMeshProUGUI go = Instantiate(Go_Text, Canvas);

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
    }

    // アニメーションを停止させる関数 //
    public void StopAnimation()
    {
        DOTween.KillAll();
    }
}


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
//     [SerializeField] private TextMeshProUGUI Ready_Text;
//     [SerializeField] private TextMeshProUGUI Go_Text;

//     [SerializeField] private TextMeshProUGUI BackToBack_Text;
//     [SerializeField] private TextMeshProUGUI PerfectClear_Text;
//     [SerializeField] private TextMeshProUGUI Rens_Text;

//     [SerializeField] private TextMeshProUGUI OneLineClear_Text;
//     [SerializeField] private TextMeshProUGUI TwoLineClear_Text;
//     [SerializeField] private TextMeshProUGUI ThreeLineClear_Text;
//     [SerializeField] private TextMeshProUGUI Tetris_Text;
//     [SerializeField] private TextMeshProUGUI Ispin_Text;
//     [SerializeField] private TextMeshProUGUI IspinSingle_Text;
//     [SerializeField] private TextMeshProUGUI IspinDouble_Text;
//     [SerializeField] private TextMeshProUGUI IspinTriple_Text;
//     [SerializeField] private TextMeshProUGUI IspinQuattro_Text;
//     [SerializeField] private TextMeshProUGUI IspinMini_Text;
//     [SerializeField] private TextMeshProUGUI Jspin_Text;
//     [SerializeField] private TextMeshProUGUI JspinSingle_Text;
//     [SerializeField] private TextMeshProUGUI JspinDouble_Text;
//     [SerializeField] private TextMeshProUGUI JspinTriple_Text;
//     [SerializeField] private TextMeshProUGUI Lspin_Text;
//     [SerializeField] private TextMeshProUGUI LspinSingle_Text;
//     [SerializeField] private TextMeshProUGUI LspinDouble_Text;
//     [SerializeField] private TextMeshProUGUI LspinTriple_Text;
//     [SerializeField] private TextMeshProUGUI Ospin_Text;
//     [SerializeField] private TextMeshProUGUI OspinSingle_Text;
//     [SerializeField] private TextMeshProUGUI OspinDouble_Text;
//     [SerializeField] private TextMeshProUGUI OspinTriple_Text;
//     [SerializeField] private TextMeshProUGUI Sspin_Text;
//     [SerializeField] private TextMeshProUGUI SspinSingle_Text;
//     [SerializeField] private TextMeshProUGUI SspinDouble_Text;
//     [SerializeField] private TextMeshProUGUI SspinTriple_Text;
//     [SerializeField] private TextMeshProUGUI Tspin_Text;
//     [SerializeField] private TextMeshProUGUI TspinSingle_Text;
//     [SerializeField] private TextMeshProUGUI TspinDouble_Text;
//     [SerializeField] private TextMeshProUGUI TspinTriple_Text;
//     [SerializeField] private TextMeshProUGUI TspinMini_Text;
//     [SerializeField] private TextMeshProUGUI TspinDoubleMini_Text;
//     [SerializeField] private TextMeshProUGUI Zspin_Text;
//     [SerializeField] private TextMeshProUGUI ZspinSingle_Text;
//     [SerializeField] private TextMeshProUGUI ZspinDouble_Text;
//     [SerializeField] private TextMeshProUGUI ZspinTriple_Text;


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
//     public void TextDisplay(int _LineClearCount)
//     {
//         switch (spinCheck.spinTypeName)
//         {
//             case "None": // Spin判定がない時
//                 switch (_LineClearCount) // 消去数で表示するテキストが変わる
//                 {
//                     case 0:
//                         gameStatus.Reset_Ren();
//                         break;

//                     case 1:
//                         gameStatus.Reset_BackToBack();

//                         RenAttackLines();

//                         AudioManager.Instance.PlaySound("Normal_Destroy");

//                         TextAnimation(OneLineClear_Text);

//                         break;

//                     case 2:
//                         gameStatus.Reset_BackToBack();

//                         gameStatus.IncreaseAttackLines(TwoLineClearAttack);

//                         RenAttackLines();

//                         AudioManager.Instance.PlaySound("Normal_Destroy");

//                         TextAnimation(TwoLineClear_Text);

//                         break;

//                     case 3:
//                         gameStatus.Reset_BackToBack();

//                         gameStatus.IncreaseAttackLines(ThreeLineClearAttack);

//                         RenAttackLines();

//                         AudioManager.Instance.PlaySound("Normal_Destroy");

//                         TextAnimation(ThreeLineClear_Text);

//                         break;

//                     case 4:
//                         CheckBackToBack();

//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TetrisAttack);

//                         AudioManager.Instance.PlaySound("Tetris");

//                         TextAnimation(Tetris_Text);

//                         break;
//                 }
//                 break;
//             case "J-Spin":
//                 CheckBackToBack();

//                 switch (_LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(Jspin_Text);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(JspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(JspinSingle_Text);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(JspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(JspinDouble_Text);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(JspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(JspinTriple_Text);

//                         break;
//                 }
//                 break;
//             case "L-Spin":
//                 CheckBackToBack();

//                 switch (_LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(Lspin_Text);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(LspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(LspinSingle_Text);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(LspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(LspinDouble_Text);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(LspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(LspinTriple_Text);

//                         break;
//                 }
//                 break;

//             case "I-Spin Mini":
//                 CheckBackToBack();

//                 switch (_LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(IspinMini_Text);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinMiniAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinMini_Text);

//                         break;
//                 }
//                 break;

//             case "I-Spin":
//                 CheckBackToBack();

//                 switch (_LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(Ispin_Text);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinSingle_Text);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinDouble_Text);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinTriple_Text);

//                         break;
//                     case 4:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(IspinQuattroAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(IspinQuattro_Text);

//                         break;
//                 }
//                 break;

//             case "T-Spin":
//                 CheckBackToBack();

//                 switch (_LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(Tspin_Text);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinSingleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinSingle_Text);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinDoubleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinDouble_Text);

//                         break;

//                     case 3:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinTripleAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinTriple_Text);

//                         break;
//                 }
//                 break;

//             case "T-Spin Mini":
//                 CheckBackToBack();

//                 switch (_LineClearCount)
//                 {
//                     case 0:
//                         AudioManager.Instance.PlaySound("Normal_Drop");

//                         TextAnimation(TspinMini_Text);

//                         break;

//                     case 1:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinMiniAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinMini_Text);

//                         break;

//                     case 2:
//                         RenAttackLines();

//                         gameStatus.IncreaseAttackLines(TspinDoubleMiniAttack);

//                         AudioManager.Instance.PlaySound("Spin_Destroy");

//                         TextAnimation(TspinDoubleMini_Text);

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
//         TextMeshProUGUI displayText_Text = InstantiatedText.GetComponent<TextMeshProUGUI>();
//         Transform displayText_Transform = InstantiatedText.GetComponent<Transform>();

//         TextFadeInAndOut(displayText_Text); // 選ばれたテキストのフェードインとフェードアウトを行う

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
//         TextMeshProUGUI InstantiatedText = Instantiate(BackToBack_Text, Canvas);

//         TextMeshProUGUI displayText_Text = InstantiatedText.GetComponent<TextMeshProUGUI>();

//         TextFadeInAndOut(displayText_Text); // 選ばれたテキストのフェードインとフェードアウトを行う
//     }

//     // PerfectClearの表示をする関数を呼ぶ関数 //
//     private void PerfectClearAnimation()
//     {
//         TextMeshProUGUI InstantiatedText = Instantiate(PerfectClear_Text, Canvas);

//         TextMeshProUGUI displayText_Text = InstantiatedText.GetComponent<TextMeshProUGUI>();

//         TextFadeInAndOut(displayText_Text); // 選ばれたテキストのフェードインとフェードアウトを行う
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

//         TextMeshProUGUI ready = Instantiate(Ready_Text, Canvas);
//         TextMeshProUGUI go = Instantiate(Go_Text, Canvas);

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