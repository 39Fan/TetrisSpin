using UnityEngine;

/// <summary>
/// エフェクトを管理するクラス
/// </summary>
public class Effects : MonoBehaviour
{
    // 干渉するコンポーネント
    Animator animator;

    /// <summary>
    /// インスタンス化
    /// </summary>
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /// <summary> スピン判定のエフェクトを表示する関数 </summary>
    public void SpinEffect()
    {
        animator.SetTrigger("SpinEffect");
    }

    // /// <summary> 列消去のエフェクトを表示する関数 </summary>
    // public void LineClearEffect()
    // {
    //     animator.SetTrigger("LineClearEffect");
    // }
}