using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Playシーンのボタン情報を管理するクラス
/// </summary>
public class PlayScene_ButtonManager : MonoBehaviour
{
    /// <summary> PoseIconが押された時に表示するCanvas </summary>
    [SerializeField] private Canvas poseCanvas;
    /// <summary> PoseIconが押された時に表示するImage </summary>
    [SerializeField] private Image poseBackGround;
    /// <summary> PoseIconが押された時に表示するButtonPanel </summary>
    [SerializeField] private GameObject poseButtonPanel;

    /// <summary> PoseIconが押された時のコルーチン処理を呼ぶ関数 </summary>
    public void PressedPoseIcon()
    {
        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.PressedPoseIcon, eLogTitle.Start);

        StartCoroutine(PoseIconCoroutine());

        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.PressedPoseIcon, eLogTitle.End);
    }

    /// <summary> PoseIconが押された時の処理をする関数 </summary>
    private IEnumerator PoseIconCoroutine()
    {
        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.PoseIconCoroutine, eLogTitle.Start);

        GameSceneManagerStats.LoadPoseState();
        poseCanvas.gameObject.SetActive(true);
        //audio
        poseBackGround.DOFade(1, 0.3f);
        yield return new WaitForSeconds(0.3f);
        poseButtonPanel.SetActive(true);

        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.PoseIconCoroutine, eLogTitle.End);
    }

    /// <summary> Continueが押された時のコルーチン処理を呼ぶ関数 </summary>
    public void PressedContinue()
    {
        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.PressedContinue, eLogTitle.Start);

        StartCoroutine(ContinueCoroutine());

        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.PressedContinue, eLogTitle.End);
    }

    /// <summary> Continueが押された時の処理をする関数 </summary>
    private IEnumerator ContinueCoroutine()
    {
        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.ContinueCoroutine, eLogTitle.Start);

        poseButtonPanel.SetActive(false);
        poseBackGround.DOFade(0, 0.3f);
        //audio
        yield return new WaitForSeconds(0.5f);
        poseCanvas.gameObject.SetActive(false);
        GameSceneManagerStats.UnLoadPoseState();

        LogHelper.DebugLog(eClasses.PlayScene_DisplayManager, eMethod.ContinueCoroutine, eLogTitle.End);
    }
}