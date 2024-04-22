using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

//ゲーム画面のテキストに関するスクリプト
//表示できるテキストの一覧はData.csに記載

public class MainSceneText : MonoBehaviour
{
    //干渉するスクリプトの設定
    Calculate calculate;

    //表示できるテキストの一覧
    //変数宣言の文法上、実際にゲーム画面に表示するテキストと変数名が合致しない場合がある
    // _数値 と末尾についているテキストはコピー(同じテキストを複数呼び出せるようにするため)　
    [SerializeField] private Text[] one_Line_Clears;
    [SerializeField] private Text[] two_Line_Clears;
    [SerializeField] private Text[] three_Line_Clears;
    [SerializeField] private Text[] Tetrises;
    // [SerializeField] private Text[] Ispins;
    // [SerializeField] private Text[] Ispin_Singles;
    // [SerializeField] private Text[] Ispin_Doubles;
    // [SerializeField] private Text[] Ispin_Triples;
    // [SerializeField] private Text[] Ispin_Quattros;
    // [SerializeField] private Text[] Jspins;
    // [SerializeField] private Text[] Jspin_Singles;
    // [SerializeField] private Text[] Jspin_Doubles;
    // [SerializeField] private Text[] Jspin_Triples;
    // [SerializeField] private Text[] Lspins;
    // [SerializeField] private Text[] Lspin_Singles;
    // [SerializeField] private Text[] Lspin_Doubles;
    // [SerializeField] private Text[] Lspin_Triples;
    // [SerializeField] private Text[] Ospins;
    // [SerializeField] private Text[] Ospin_Singles;
    // [SerializeField] private Text[] Ospin_Doubles;
    // [SerializeField] private Text[] Ospin_Triples;
    // [SerializeField] private Text[] Sspins;
    // [SerializeField] private Text[] Sspin_Singles;
    // [SerializeField] private Text[] Sspin_Doubles;
    // [SerializeField] private Text[] Sspin_Triples;
    [SerializeField] private Text[] Tspins;
    [SerializeField] private Text[] Tspin_Singles;
    [SerializeField] private Text[] Tspin_Doubles;
    [SerializeField] private Text[] Tspin_Triples;
    [SerializeField] private Text[] Tspin_Minis;
    [SerializeField] private Text[] Tspin_Double_Minis;
    // [SerializeField] private Text[] Zspins;
    // [SerializeField] private Text[] Zspin_Singles;
    // [SerializeField] private Text[] Zspin_Doubles;
    // [SerializeField] private Text[] Zspin_Triples;

    //フェードインとフェードアウトする時間
    float fadeInInterval = 0.2f;
    float fadeOutInterval = 1f;

    //テキストを表示させる時間
    float waitInterval = 2f;

    //透明度
    //0の時は透明、1の時は不透明
    int alpha_0 = 0;
    int alpha_1 = 1;

    //x軸とy軸それぞれのテキストを移動させる時間
    float moveInterval_x = 2f;
    float moveInterval_y = 1.2f;

    //テキストの移動量
    float moveDistance = 600f;

    //アニメーションの合計時間
    float waitTime = 3.2f;

    //インスタンス化
    void Awake()
    {
        calculate = FindObjectOfType<Calculate>();
    }

    //表示するテキストを判別して、実際に表示する関数
    public void TextDisplay(int text)
    {
        //textと合致するGameObjectのTextコンポーネントを取得
        if (text == calculate.one_Line_Clear)
        {
            //one_Line_Clear_Arrayの数だけ繰り返す
            for (int count = 0; count < one_Line_Clears.Length; count++)
            {
                //count番目のone_Line_Clearsのテキストが未使用なら
                if (one_Line_Clears[count].gameObject.activeSelf == false)
                {
                    //アニメーションを行う
                    //コルーチンで演出が終わるまで待機し、完全にアニメーションが終了したら未使用に戻す
                    StartCoroutine(TextAnimationCoroutine(one_Line_Clears[count]));

                    //このfor文を抜ける
                    break;
                }
            }
            //↓以下同文
        }
        else if (text == calculate.two_Line_Clear)
        {
            for (int count = 0; count < two_Line_Clears.Length; count++)
            {
                if (two_Line_Clears[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(two_Line_Clears[count]));

                    break;
                }
            }
        }
        else if (text == calculate.three_Line_Clear)
        {
            for (int count = 0; count < three_Line_Clears.Length; count++)
            {
                if (three_Line_Clears[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(three_Line_Clears[count]));

                    break;
                }
            }
        }
        else if (text == calculate.Tetris)
        {
            for (int count = 0; count < Tetrises.Length; count++)
            {
                if (Tetrises[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(Tetrises[count]));

                    break;
                }
            }
        }
        else if (text == calculate.Tspin)
        {
            for (int count = 0; count < Tspins.Length; count++)
            {
                if (Tspins[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(Tspins[count]));

                    break;
                }
            }
        }
        else if (text == calculate.Tspin_Single)
        {
            for (int count = 0; count < Tspin_Singles.Length; count++)
            {
                if (Tspin_Singles[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(Tspin_Singles[count]));

                    break;
                }
            }
        }
        else if (text == calculate.Tspin_Double)
        {
            for (int count = 0; count < Tspin_Doubles.Length; count++)
            {
                if (Tspin_Doubles[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(Tspin_Doubles[count]));

                    break;
                }
            }
        }
        else if (text == calculate.Tspin_Triple)
        {
            for (int count = 0; count < Tspin_Triples.Length; count++)
            {
                if (Tspin_Triples[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(Tspin_Triples[count]));

                    break;
                }
            }
        }
        else if (text == calculate.Tspin_Mini)
        {
            for (int count = 0; count < Tspin_Minis.Length; count++)
            {
                if (Tspin_Minis[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(Tspin_Minis[count]));

                    break;
                }
            }
        }
        else if (text == calculate.Tspin_Double_Mini)
        {
            for (int count = 0; count < Tspin_Double_Minis.Length; count++)
            {
                if (Tspin_Double_Minis[count].gameObject.activeSelf == false)
                {
                    StartCoroutine(TextAnimationCoroutine(Tspin_Double_Minis[count]));

                    break;
                }
            }
        }
    }

    private IEnumerator TextAnimationCoroutine(Text selectText)
    {
        //アクティブにする
        selectText.gameObject.SetActive(true);

        //選ばれたテキストのテキストコンポーネントとトランスフォームコンポーネントを取得
        Text SelectText_Text = selectText.GetComponent<Text>();
        Transform SelectText_Transform = selectText.GetComponent<Transform>();

        //選ばれたテキストのフェードインとフェードアウトを行う
        TextFadeInAndOut(SelectText_Text);

        //選ばれたテキストの移動アニメーションを行う
        TextMove(SelectText_Transform);

        //3.2秒待つ
        yield return new WaitForSeconds(waitTime);

        //アニメーションが完了したら非表示に戻す
        selectText.gameObject.SetActive(false);
    }

    //選ばれたテキストのフェードインとフェードアウトを行う関数
    private void TextFadeInAndOut(Text selectText)
    {
        //Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        //0.3秒かけてアルファ値を1(=不透明)に変化させる
        //その後、2秒表示
        //最後に、1秒かけてアルファ値を0(=透明)に変化させる
        sequence.Append(selectText.DOFade(alpha_1, fadeInInterval))
                .AppendInterval(waitInterval)
                .Append(selectText.DOFade(alpha_0, fadeOutInterval));
    }


    //選ばれたテキストの移動アニメーションを行う関数
    private void TextMove(Transform selectText)
    {
        //selectTextの各座標を格納する変数を宣言
        float selectText_x = Mathf.RoundToInt(selectText.transform.position.x);
        float selectText_y = Mathf.RoundToInt(selectText.transform.position.y);
        float selectText_z = Mathf.RoundToInt(selectText.transform.position.z);

        //Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        //現在位置から3秒で、Y座標を600移動する
        //始点と終点の繋ぎ方はOutSineで設定
        //その後、現在位置から1秒で、X座標を-600移動する(移動中にフェードアウトする)
        //始点と終点の繋ぎ方はInQuintで設定
        sequence
            .Append(selectText.transform.DOMoveY(selectText_y + moveDistance, moveInterval_x).SetEase(Ease.OutSine))
            .Append(selectText.transform.DOMoveX(selectText_x - moveDistance, moveInterval_y).SetEase(Ease.InQuint))
            //.Join(selectText.transform.DOScale(new Vector3(0, 1f, 0), moveInterval_y))
            .OnComplete(() =>
            {
                // アニメーションが完了したら元の位置に戻す
                selectText.transform.position = new Vector3(selectText_x, selectText_y, selectText_z);
            });
    }
}