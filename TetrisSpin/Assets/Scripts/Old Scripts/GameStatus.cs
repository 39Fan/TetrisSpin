// using System.Collections.Generic;
// using UnityEngine;

// /// <summary>
// /// ゲームの状態を管理するクラス
// /// </summary>
// public class GameStatus : MonoBehaviour
// {
//     // /// <summary>ミノの回転後の向き
//     // /// 初期値はNorthの状態
//     // /// </summary>
//     // /// <remarks>Spin判定を確認する際、回転後の向きと回転前の向きの情報が必要なため</remarks>
//     // private MinoDirections minoAngleAfter = MinoDirections.North;

//     // /// <summary>ミノの回転前の向き
//     // /// 初期値はNorthの状態
//     // /// </summary>
//     // /// <remarks>Spin判定を確認する際、回転後の向きと回転前の向きの情報が必要なため</remarks>
//     // private MinoDirections minoAngleBefore = MinoDirections.North;

//     // /// <summary>スーパーローテーションシステム(SRS)の段階
//     // /// </summary>
//     // /// <remarks>SRSが使用されていないときは0, 1〜4の時は、SRSの段階を表す</remarks>
//     // /// <value>0~4</value>
//     // [SerializeField] private int stepsSRS = 0;

//     // ゲッタープロパティ //
//     // public bool BackToBack => backToBack;
//     // public int Ren => ren;
//     // public List<int> LineClearCountHistory => lineClearCountHistory;
//     // public MinoDirections MinoAngleAfter => minoAngleAfter;
//     // public MinoDirections MinoAngleBefore => minoAngleBefore;
//     // public int StepsSRS => stepsSRS;

//     // // 干渉するスクリプト //
//     // Spawner spawner;

//     // /// <summary>
//     // /// インスタンス化
//     // /// </summary>
//     // private void Awake()
//     // {
//     //     spawner = FindObjectOfType<Spawner>();
//     // }

//     // /// <summary>ゲームオーバー判定をオンにする</summary>
//     // public void SetGameOver() => gameOver = true;

//     // /// <summary>ゲームオーバー判定をオフにする</summary>
//     // public void ResetGameOver() => gameOver = false;

//     // /// <summary>BackToBack判定をオンにする</summary>
//     // public void SetBackToBack() => backToBack = true;

//     // /// <summary>BackToBack判定をオフにする</summary>
//     // public void ResetBackToBack() => backToBack = false;

//     // /// <summary>Renの値をリセットする</summary>
//     // public void ResetRen() => ren = -1;

//     // /// <summary>Renの値を1増加させる</summary>
//     // public void IncreaseRen() => ren++;

//     // /// <summary>AttackLinesの値を増加させる
//     // /// </summary>
//     // /// <param name="_addAttackLines">追加する攻撃ライン数</param>
//     // public void IncreaseAttackLines(int _addAttackLines) => attackLines += _addAttackLines;

//     // /// <summary>ライン消去数の履歴を追加する
//     // /// </summary>
//     // /// <param name="_lineClearCount">消去したライン数</param>
//     // public void AddLineClearCountHistory(int _lineClearCount) => LineClearCountHistory.Add(_lineClearCount);

//     // /// <summary>ミノの向きを初期化する</summary>
//     // public void ResetAngle()
//     // {
//     //     minoAngleBefore = MinoDirections.North;
//     //     minoAngleAfter = MinoDirections.North;
//     // }

//     // /// <summary>MinoAngleAfterをリセットする</summary>
//     // public void ResetMinoAngleAfter() => minoAngleAfter = minoAngleBefore;

//     // /// <summary>通常回転のリセットをする関数</summary>
//     // public void ResetRotate()
//     // {
//     //     // 通常回転が右回転だった時
//     //     if ((MinoAngleBefore == MinoDirections.North && MinoAngleAfter == MinoDirections.East) ||
//     //     (MinoAngleBefore == MinoDirections.East && MinoAngleAfter == MinoDirections.South) ||
//     //     (MinoAngleBefore == MinoDirections.South && MinoAngleAfter == MinoDirections.West) ||
//     //     (MinoAngleBefore == MinoDirections.West && MinoAngleAfter == MinoDirections.North))
//     //     {
//     //         spawner.activeMino.RotateLeft(); // 左回転で回転前の状態に戻す
//     //     }
//     //     else // 通常回転が左回転だった時
//     //     {
//     //         spawner.activeMino.RotateRight(); // 右回転で回転前の状態に戻す
//     //     }
//     // }

//     // /// <summary>MinoAngleAfterの更新をする関数
//     // /// </summary>
//     // /// <param name="_rotateDirection">回転方向</param>
//     // public void UpdateMinoAngleAfter(MinoRotationDirections _rotateDirection)
//     // {
//     //     switch (_rotateDirection)
//     //     {
//     //         case MinoRotationDirections.RotateRight:
//     //             minoAngleAfter = minoAngleAfter switch
//     //             {
//     //                 MinoDirections.North => MinoDirections.East,
//     //                 MinoDirections.East => MinoDirections.South,
//     //                 MinoDirections.South => MinoDirections.West,
//     //                 MinoDirections.West => MinoDirections.North,
//     //                 _ => minoAngleAfter
//     //             };
//     //             break;
//     //         case MinoRotationDirections.RotateLeft:
//     //             minoAngleAfter = minoAngleAfter switch
//     //             {
//     //                 MinoDirections.North => MinoDirections.West,
//     //                 MinoDirections.East => MinoDirections.North,
//     //                 MinoDirections.South => MinoDirections.East,
//     //                 MinoDirections.West => MinoDirections.South,
//     //                 _ => minoAngleAfter
//     //             };
//     //             break;
//     //     }
//     // }

//     // /// <summary>MinoAngleBeforeを更新する</summary>
//     // public void UpdateMinoAngleBefore() => minoAngleBefore = minoAngleAfter;

//     // /// <summary>StepsSRSの値をリセットする</summary>
//     // public void ResetStepsSRS() => stepsSRS = 0;

//     // /// <summary>StepsSRSの値を1増加させる</summary>
//     // public void IncreaseStepsSRS() => stepsSRS++;
// }

// /////////////////// 旧コード ///////////////////

// // // 各種判定 //
// // private bool gameOver;
// // private bool backToBack = false;
// // private int ren = -1; // 3回連続で列消去すると「2REN」なので、初期値は-1に設定
// //
// // /// <summary>
// // /// PerfectClearの判定
// // /// </summary>
// // private bool perfectClear = false;
// //
// // // 攻撃ライン数 //
// // [SerializeField] private int attackLines = 0;
// //
// // // ライン消去の履歴を記録するリスト //
// // private List<int> lineClearCountHistory = new List<int>();
// //
// // /*
// //    // ミノの回転前、回転後の向き //
// //    初期値はNorthの状態
// //    Spin判定を確認する際、回転後の向きと回転前の向きの情報が必要
// // */
// // private MinoDirections minoAngleAfter = MinoDirections.North;
// // private MinoDirections minoAngleBefore = MinoDirections.North;
// //
// // /*
// //    // スーパーローテーションシステム(SRS)の段階 //
// //    SRSが使用されていないときは0, 1〜4の時は、SRSの段階を表す
// //    Spin判定の条件に必要
// // */
// // [SerializeField] private int stepsSRS = 0;
// //
// // // 向きの定義 //
// // private string North = "North"; // 初期(未回転)状態をNorthとして、
// // private string East = "East"; // 右回転後の向きをEast
// // private string South = "South"; // 左回転後の向きをWest
// // private string West = "West"; // 2回右回転または左回転した時の向きをSouthとする
// //
// // /// <summary>gameOverのゲッタープロパティ</summary>
// // public bool GameOver
// // {
// //     get { return gameOver; }
// // }
// // /// <summary>backToBackのゲッタープロパティ</summary>
// // public bool BackToBack
// // {
// //     get { return backToBack; }
// // }
// // /// <summary>PerfectClearのゲッタープロパティ</summary>
// // public bool perfectClear
// // {
// //     get { return PerfectClear; }
// // }
// // /// <summary>renのゲッタープロパティ</summary>
// // public int Ren
// // {
// //     get { return ren; }
// // }
// // /// <summary>lineClearCountHistoryのゲッタープロパティ</summary>
// // public List<int> LineClearCountHistory
// // {
// //     get { return lineClearCountHistory; }
// // }
// // /// <summary>minoAngleAfterのゲッタープロパティ</summary>
// // public MinoDirections MinoAngleAfter
// // {
// //     get { return minoAngleAfter; }
// // }
// // /// <summary>minoAngleBeforeのゲッタープロパティ</summary>
// // public MinoDirections MinoAngleBefore
// // {
// //     get { return minoAngleBefore; }
// // }
// // /// <summary>stepsSRSのゲッタープロパティ</summary>
// // public int StepsSRS
// // {
// //     get { return stepsSRS; }
// // }


// // /// <summary>
// // /// ゲームオーバー判定をオンにする関数
// // /// </summary>
// // public void SetGameOver()
// // {
// //     gameOver = true;
// // }
// //
// // /// <summary>
// // /// ゲームオーバー判定をオフにする関数
// // /// </summary>
// // public void ResetGameOver()
// // {
// //     gameOver = false;
// // }
// //
// // /// <summary>
// // /// BackToBack判定をオンにする関数
// // /// </summary>
// // public void SetBackToBack()
// // {
// //     backToBack = true;
// // }
// //
// // /// <summary>
// // /// BackToBack判定をオフにする関数
// // /// </summary>
// // public void ResetBackToBack()
// // {
// //     backToBack = false;
// // }
// //
// // /// <summary>
// // /// PerfectClear判定をオンにする関数
// // /// </summary>
// // public void SetPerfectClear()
// // {
// //     perfectClear = true;
// // }
// //
// // /// <summary>
// // /// PerfectClear判定をオフにする関数
// // /// </summary>
// // public void ResetPerfectClear()
// // {
// //     perfectClear = false;
// // }
// // /// <summary>
// // /// Renの値をリセットする関数
// // /// </summary>
// // public void ResetRen()
// // {
// //     ren = -1;
// // }
// //
// // /// <summary>
// // /// Renの値を1増加させる関数
// // /// </summary>
// // public void IncreaseRen()
// // {
// //     ren++;
// // }
// //
// // /// <summary>
// // /// AttackLinesの値を足していく関数
// // /// </summary>
// // /// <param name="_addAttackLines">追加する攻撃ライン数</param>
// // public void IncreaseAttackLines(int _addAttackLines)
// // {
// //     attackLines += _addAttackLines;
// // }
// //
// // /// <summary>
// // /// ライン消去数の履歴を追加する関数
// // /// </summary>
// // /// <param name="_lineClearCount">消去したライン数</param>
// // public void AddLineClearCountHistory(int _lineClearCount)
// // {
// //     LineClearCountHistory.Add(_lineClearCount);
// // }
// //
// // /// <summary>
// // /// ミノの向きを初期化する関数
// // /// </summary>
// // public void ResetAngle()
// // {
// //     minoAngleBefore = MinoDirections.North;
// //     minoAngleAfter = MinoDirections.North;
// // }
// //
// // /// <summary>
// // /// MinoAngleAfterのリセットをする関数
// // /// </summary>
// // public void ResetMinoAngleAfter()
// // {
// //     minoAngleAfter = minoAngleBefore;
// // }
// //
// // /// <summary>
// // /// MinoAngleAfterの更新をする関数
// // /// </summary>
// // /// <param name="_rotateDirection">回転方向</param>
// // public void UpdateMinoAngleAfter(MinoRotationDirections _rotateDirection)
// // {
// //     switch (_rotateDirection)
// //     {
// //         case MinoRotationDirections.RotateRight:
// //             minoAngleAfter = minoAngleAfter switch
// //             {
// //                 MinoDirections.North => MinoDirections.East,
// //                 MinoDirections.East => MinoDirections.South,
// //                 MinoDirections.South => MinoDirections.West,
// //                 MinoDirections.West => MinoDirections.North,
// //                 _ => minoAngleAfter
// //             };
// //             break;
// //         case MinoRotationDirections.RotateLeft:
// //             minoAngleAfter = minoAngleAfter switch
// //             {
// //                 MinoDirections.North => MinoDirections.West,
// //                 MinoDirections.East => MinoDirections.North,
// //                 MinoDirections.South => MinoDirections.East,
// //                 MinoDirections.West => MinoDirections.South,
// //                 _ => minoAngleAfter
// //             };
// //             break;
// //     }

// //     if (_rotateDirection == MinoRotationDirections.RotateRight)
// //     {
// //         switch (MinoAngleAfter)
// //         {
// //             case MinoDirections.North:
// //                 minoAngleAfter = MinoDirections.East;
// //                 break;
// //             case MinoDirections.East:
// //                 minoAngleAfter = MinoDirections.South;
// //                 break;
// //             case MinoDirections.South:
// //                 minoAngleAfter = MinoDirections.West;
// //                 break;
// //             case MinoDirections.West:
// //                 minoAngleAfter = MinoDirections.North;
// //                 break;
// //         }
// //     }
// //     else if (_rotateDirection == MinoRotationDirections.RotateLeft)
// //     {
// //         switch (MinoAngleAfter)
// //         {
// //             case MinoDirections.North:
// //                 minoAngleAfter = MinoDirections.West;
// //                 break;
// //             case MinoDirections.East:
// //                 minoAngleAfter = MinoDirections.North;
// //                 break;
// //             case MinoDirections.South:
// //                 minoAngleAfter = MinoDirections.East;
// //                 break;
// //             case MinoDirections.West:
// //                 minoAngleAfter = MinoDirections.South;
// //                 break;
// //         }
// //     }
// // }
// //
// // /// <summary>
// // /// MinoAngleBeforeの更新をする関数
// // /// </summary>
// // public void UpdateMinoAngleBefore()
// // {
// //     minoAngleBefore = minoAngleAfter;
// // }

// // /// <summary>
// // /// StepsSRSの値をリセットする関数
// // /// </summary>
// // public void ResetStepsSRS()
// // {
// //     stepsSRS = 0;
// // }

// // /// <summary>
// // /// StepsSRSの値を1増加させる関数
// // /// </summary>
// // public void IncreaseStepsSRS()
// // {
// //     stepsSRS++;
// // }

// /////////////////////////////////////////////////////////