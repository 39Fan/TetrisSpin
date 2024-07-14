using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// シーン遷移時のフェード処理をする関数
/// </summary>
public class FadeImage : MonoBehaviour
{
    [Header("最初からフェードインが完了しているかどうか")] public bool firstFadeInComp;

    private Image img = null;
    private RectTransform rectTransform = null;
    private int frameCount = 0;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool compFadeIn = false;
    private bool compFadeOut = false;

    /// <summary>
    /// フェードインを開始する
    /// </summary>
    public void StartFadeIn()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeIn = true;
        compFadeIn = false;
        img.color = new Color(1, 1, 1, 1);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -1800);
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードインが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeInComplete()
    {
        return compFadeIn;
    }

    /// <summary>
    /// フェードアウトを開始する
    /// </summary>
    public void StartFadeOut()
    {
        if (fadeIn || fadeOut)
        {
            return;
        }
        fadeOut = true;
        compFadeOut = false;
        img.color = new Color(1, 1, 1, 0);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        img.raycastTarget = true;
    }

    /// <summary>
    /// フェードアウトを完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeOutComplete()
    {
        return compFadeOut;
    }

    private void Awake()
    {
        img = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();

        if (firstFadeInComp)
        {
            FadeInComplete();
        }
        else
        {
            StartFadeIn();
        }
    }

    private void Update()
    {
        //シーン移行時の処理の重さでTime.deltaTimeが大きくなってしまうから2フレーム待つ
        if (frameCount > 2)
        {
            if (fadeIn)
            {
                FadeInUpdate();
            }
            else if (fadeOut)
            {
                FadeOutUpdate();
            }
        }
        ++frameCount;
    }

    //フェードイン中
    private void FadeInUpdate()
    {
        // DOTweenを使ってフェードイン処理を開始
        img.DOFade(0, 0.5f);
        rectTransform.DOAnchorPosY(0, 1f).OnComplete(FadeInComplete);

        fadeIn = false; // フェードイン処理を一度だけ行うため
    }

    //フェードアウト中
    private void FadeOutUpdate()
    {
        // DOTweenを使ってフェードアウト処理を開始
        img.DOFade(1, 1f);
        rectTransform.DOAnchorPosY(-1800, 1f).OnComplete(FadeOutComplete);

        fadeOut = false; // フェードアウト処理を一度だけ行うため
    }

    //フェードイン完了
    private void FadeInComplete()
    {
        img.color = new Color(1, 1, 1, 0);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, 0);
        img.raycastTarget = false;
        compFadeIn = true;
    }

    //フェードアウト完了
    private void FadeOutComplete()
    {
        img.color = new Color(1, 1, 1, 1);
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, -1800);
        img.raycastTarget = false;
        compFadeOut = true;
    }
}
