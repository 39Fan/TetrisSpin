using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; //アニメーションに関するライブラリ

//ゲーム画面のテキストに関するスクリプト
//表示できるテキストの一覧はData.csに記載
public class MainSceneText : MonoBehaviour
{
    //干渉するスクリプトの設定
    Data data;

    //表示できるテキストの一覧
    //変数宣言の文法上、実際にゲーム画面に表示するテキストと変数名が合致しない場合がある
    [SerializeField] private Text one_Line_Clear;
    [SerializeField] private Text two_Line_Clear;
    [SerializeField] private Text three_Line_Clear;
    [SerializeField] private Text Tetris;
    // [SerializeField] private Text Ispin;
    // [SerializeField] private Text Ispin_Single;
    // [SerializeField] private Text Ispin_Double;
    // [SerializeField] private Text Ispin_Triple;
    // [SerializeField] private Text Ispin_Quattro;
    // [SerializeField] private Text Jspin;
    // [SerializeField] private Text Jspin_Single;
    // [SerializeField] private Text Jspin_Double;
    // [SerializeField] private Text Jspin_Triple;
    // [SerializeField] private Text Lspin;
    // [SerializeField] private Text Lspin_Single;
    // [SerializeField] private Text Lspin_Double;
    // [SerializeField] private Text Lspin_Triple;
    // [SerializeField] private Text Ospin;
    // [SerializeField] private Text Ospin_Single;
    // [SerializeField] private Text Ospin_Double;
    // [SerializeField] private Text Ospin_Triple;
    // [SerializeField] private Text Sspin;
    // [SerializeField] private Text Sspin_Single;
    // [SerializeField] private Text Sspin_Double;
    // [SerializeField] private Text Sspin_Triple;
    [SerializeField] private Text Tspin;
    [SerializeField] private Text Tspin_Single;
    [SerializeField] private Text Tspin_Double;
    [SerializeField] private Text Tspin_Triple;
    [SerializeField] private Text Tspin_Mini;
    [SerializeField] private Text Tspin_Double_Mini;
    // [SerializeField] private Text Zspin;
    // [SerializeField] private Text Zspin_Single;
    // [SerializeField] private Text Zspin_Double;
    // [SerializeField] private Text Zspin_Triple;

    //フェードインとフェードアウトする時間
    float fadeInInterval = 0.2f;
    float fadeOutInterval = 1f;

    //テキストを表示させる時間
    float waitInterval = 2f;

    //透明度
    //0の時は透明、1の時は不透明
    int alpha = 1;

    //インスタンス化
    void Start()
    {
        data = FindObjectOfType<Data>();
    }

    //表示するテキストを判別して、実際に表示する関数
    public void TextDisplay(int text)
    {
        //textと合致するGameObjectのTextコンポーネントを取得
        if (text == data.one_Line_Clear)
        {
            //選ばれたテキストのテキストコンポーネントを取得
            one_Line_Clear.GetComponent<Text>();

            //選ばれたテキストのフェードインとフェードアウトを行う関数
            TextFadeInAndOut(one_Line_Clear);

            //↓以下同文
        }
        else if (text == data.two_Line_Clear)
        {
            two_Line_Clear.GetComponent<Text>();

            TextFadeInAndOut(two_Line_Clear);
        }
        else if (text == data.three_Line_Clear)
        {
            three_Line_Clear.GetComponent<Text>();

            TextFadeInAndOut(three_Line_Clear);
        }
        else if (text == data.Tetris)
        {
            Tetris.GetComponent<Text>();

            TextFadeInAndOut(Tetris);
        }
        else if (text == data.Tspin)
        {
            Tspin.GetComponent<Text>();

            TextFadeInAndOut(Tspin);
        }
        else if (text == data.Tspin_Single)
        {
            Tspin_Single.GetComponent<Text>();

            TextFadeInAndOut(Tspin_Single);
        }
        else if (text == data.Tspin_Double)
        {
            Tspin_Double.GetComponent<Text>();

            TextFadeInAndOut(Tspin_Double);
        }
        else if (text == data.Tspin_Triple)
        {
            Tspin_Triple.GetComponent<Text>();

            TextFadeInAndOut(Tspin_Triple);
        }
        else if (text == data.Tspin_Mini)
        {
            Tspin_Mini.GetComponent<Text>();

            TextFadeInAndOut(Tspin_Mini);
        }
        else if (text == data.Tspin_Double_Mini)
        {
            Tspin_Double_Mini.GetComponent<Text>();

            TextFadeInAndOut(Tspin_Double_Mini);
        }
    }

    //選ばれたテキストのフェードインとフェードアウトを行う関数
    private void TextFadeInAndOut(Text selectText)
    {
        //Sequenceのインスタンス化
        var sequence = DOTween.Sequence();

        //0.3秒かけてアルファ値を256(=不透明)に変化させる
        sequence.Append(selectText.DOFade(alpha, fadeInInterval));

        //秒表示
        sequence.AppendInterval(waitInterval);

        //透明度を0にする
        alpha = 0;

        //1秒かけてアルファ値を0(=透明)に変化させる
        sequence.Append(selectText.DOFade(alpha, fadeOutInterval));

        //透明度を1に戻す
        alpha = 1;
    }
}
