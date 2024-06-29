using System.Collections;
using UnityEngine;

public class ModeSelectDisplayManager : MonoBehaviour
{
    /// <summary> 100-TimeAttackが押された時のコルーチン処理を呼ぶ関数 </summary>
    public void PressedTimeAttack_100()
    {
        LogHelper.DebugLog(eClasses.ModeSelectDisplayManager, eMethod.PressedTimeAttack_100, eLogTitle.Start);

        StartCoroutine(TimeAttack_100Coroutine());

        LogHelper.DebugLog(eClasses.ModeSelectDisplayManager, eMethod.PressedTimeAttack_100, eLogTitle.End);
    }

    /// <summary> 100-TimeAttackが押された時の処理をする関数 </summary>
    private IEnumerator TimeAttack_100Coroutine()
    {
        LogHelper.DebugLog(eClasses.ModeSelectDisplayManager, eMethod.TimeAttack_100Coroutine, eLogTitle.Start);

        // poseButtonPanel.SetActive(false);
        // poseBackGround.DOFade(0, 0.3f);
        // //audio
        yield return new WaitForSeconds(0.5f);
        // poseCanvas.gameObject.SetActive(false);
        GameSceneManagerStats.UnLoadPoseState();

        LogHelper.DebugLog(eClasses.ModeSelectDisplayManager, eMethod.TimeAttack_100Coroutine, eLogTitle.End);
    }
}
