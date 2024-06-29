using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary> ゲームモード 列挙型 </summary>
public enum eGameMode
{
    TimeAttack_100,
    SpinMaster,
    Practice,
}

/// <summary> 難易度 列挙型 </summary>
public enum eLevel
{
    Easy,
    Normal,
    Hard
}

/// <summary>
/// 記録可能なデータ 列挙型
/// </summary>
public enum SaveDataTypes
{
    Time,
    BackToBack, PerfectClear, SpinComplete,
    Tetris,
    I_Spin, I_SpinSingle, I_SpinDouble, I_SpinTriple, I_SpinQuattro, I_SpinMini,
    J_Spin, J_SpinSingle, J_SpinDouble, J_SpinTriple, J_SpinMini, J_SpinDoubleMini,
    L_Spin, L_SpinSingle, L_SpinDouble, L_SpinTriple, L_SpinMini, L_SpinDoubleMini,
    S_Spin, S_SpinSingle, S_SpinDouble, S_SpinTriple, S_SpinMini, S_SpinDoubleMini,
    T_Spin, T_SpinSingle, T_SpinDouble, T_SpinTriple, T_SpinMini, T_SpinDoubleMini,
    Z_Spin, Z_SpinSingle, Z_SpinDouble, Z_SpinTriple, Z_SpinMini, Z_SpinDoubleMini
}

/// <summary>
/// ゲームのデータを保持するクラス
/// </summary>
[Serializable]
public class GameData : MonoBehaviour
{
    // 記録可能な合計データ //

    /// <summary> 各プレイのデータリスト </summary>
    public List<PlayRecord> playRecords;
    /// <summary> TimeAttack_100モードでの難易度ごとのクリア回数リスト </summary>
    public List<int> timeAttack_100_ClearCounts;
    /// <summary> SpinMasterモードでの難易度ごとのクリア回数リスト </summary>
    public List<int> spinMaster_ClearCounts;

    public int sumBackToBackCount;
    public int sumPerfectClearCount;
    public List<int> sumRenCount;
    public int sumSpinCompleteCount;
    public int sumTetrisCount;
    public List<int> sumISpinList;
    public List<int> sumJSpinList;
    public List<int> sumLSpinList;
    public List<int> sumSSpinList;
    public List<int> sumTSpinList;
    public List<int> sumZSpinList;

    /// <summary>
    /// 各プレイの詳細なデータを保持するクラス
    /// </summary>
    [Serializable]
    public class PlayRecord
    {
        // 記録可能なデータ //

        public float clearTime;
        public int backToBackCount;
        public int perfectClearCount;
        public List<int> RenCount;
        public int spinCompleteCount;
        public int tetrisCount;
        public List<int> iSpinCounts;
        public List<int> jSpinCounts;
        public List<int> lSpinCounts;
        public List<int> sSpinCounts;
        public List<int> tSpinCounts;
        public List<int> zSpinCounts;
    }
}

/// <summary>
/// 合計データを保持するクラス
/// </summary>
[Serializable]
public class AggregateData
{
    public List<int> timeAttack_100_ClearCounts;
    public List<int> spinMaster_ClearCounts;

    public int sumBackToBackCount;
    public int sumPerfectClearCount;
    public List<int> sumRenCount;
    public int sumSpinCompleteCount;
    public int sumTetrisCount;
    public Dictionary<string, int> sumSpinCounts;

    public AggregateData()
    {
        sumSpinCounts = new Dictionary<string, int>
        {
            { "ISpin", 0 }, { "ISpinSingle", 0 }, { "ISpinDouble", 0 }, { "ISpinTriple", 0 }, { "ISpinQuattro", 0 }, { "ISpinMini", 0 },
            { "JSpin", 0 }, { "JSpinSingle", 0 }, { "JSpinDouble", 0 }, { "JSpinTriple", 0 }, { "JSpinMini", 0 }, { "JSpinDoubleMini", 0 },
            { "LSpin", 0 }, { "LSpinSingle", 0 }, { "LSpinDouble", 0 }, { "LSpinTriple", 0 }, { "LSpinMini", 0 }, { "LSpinDoubleMini", 0 },
            { "SSpin", 0 }, { "SSpinSingle", 0 }, { "SSpinDouble", 0 }, { "SSpinTriple", 0 }, { "SSpinMini", 0 }, { "SSpinDoubleMini", 0 },
            { "TSpin", 0 }, { "TSpinSingle", 0 }, { "TSpinDouble", 0 }, { "TSpinTriple", 0 }, { "TSpinMini", 0 }, { "TSpinDoubleMini", 0 },
            { "ZSpin", 0 }, { "ZSpinSingle", 0 }, { "ZSpinDouble", 0 }, { "ZSpinTriple", 0 }, { "ZSpinMini", 0 }, { "ZSpinDoubleMini", 0 }
        };
    }
}

/// <summary>
/// 各プレイの詳細なデータを保持するクラス
/// </summary>
[Serializable]
public class PlayRecord
{
    public eGameMode gameMode;
    public eLevel level;

    public float clearTime;
    public int backToBackCount;
    public int perfectClearCount;
    public List<int> renCount;
    public int spinCompleteCount;
    public int tetrisCount;
    public Dictionary<string, int> spinCounts;

    public PlayRecord()
    {
        spinCounts = new Dictionary<string, int>
        {
            { "ISpin", 0 }, { "ISpinSingle", 0 }, { "ISpinDouble", 0 }, { "ISpinTriple", 0 }, { "ISpinQuattro", 0 }, { "ISpinMini", 0 },
            { "JSpin", 0 }, { "JSpinSingle", 0 }, { "JSpinDouble", 0 }, { "JSpinTriple", 0 }, { "JSpinMini", 0 }, { "JSpinDoubleMini", 0 },
            { "LSpin", 0 }, { "LSpinSingle", 0 }, { "LSpinDouble", 0 }, { "LSpinTriple", 0 }, { "LSpinMini", 0 }, { "LSpinDoubleMini", 0 },
            { "SSpin", 0 }, { "SSpinSingle", 0 }, { "SSpinDouble", 0 }, { "SSpinTriple", 0 }, { "SSpinMini", 0 }, { "SSpinDoubleMini", 0 },
            { "TSpin", 0 }, { "TSpinSingle", 0 }, { "TSpinDouble", 0 }, { "TSpinTriple", 0 }, { "TSpinMini", 0 }, { "TSpinDoubleMini", 0 },
            { "ZSpin", 0 }, { "ZSpinSingle", 0 }, { "ZSpinDouble", 0 }, { "ZSpinTriple", 0 }, { "ZSpinMini", 0 }, { "ZSpinDoubleMini", 0 }
        };
    }
}

/// <summary>
/// ゲームデータの保存と読み込みを管理するクラス
/// </summary>
public class GameDataManager : MonoBehaviour
{
    /// <summary> 現在のゲームデータ </summary>
    public GameData gameData;

    void Start()
    {
        // ゲーム開始時にデータを読み込む
        Load();
    }

    /// <summary>
    /// ゲームデータを指定されたファイルに保存する
    /// </summary>
    /// <param name="_fileName"> 保存するファイル名 </param>
    public void SaveData(string _fileName)
    {
        // GameDataオブジェクトをJSON形式にシリアライズ
        string json = JsonUtility.ToJson(gameData);
        // JSONデータをファイルに書き込む
        File.WriteAllText(Path.Combine(Application.persistentDataPath, _fileName), json);
    }

    /// <summary>
    /// ゲームデータをデフォルトのファイルに保存する
    /// </summary>
    public void Save()
    {
        SaveData("gameData.json");
    }

    /// <summary>
    /// 指定されたファイルからゲームデータを読み込む
    /// </summary>
    /// <param name="_fileName"> 読み込むファイル名 </param>
    /// <returns>読み込んだゲームデータ</returns>
    public GameData LoadData(string _fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, _fileName);
        if (File.Exists(filePath))
        {
            // ファイルからJSONデータを読み込む
            string json = File.ReadAllText(filePath);
            // JSONデータをGameDataオブジェクトにデシリアライズ
            return JsonUtility.FromJson<GameData>(json);
        }
        // ファイルが存在しない場合は新しいGameDataオブジェクトを返す
        return new GameData();
    }

    /// <summary>
    /// デフォルトのファイルからゲームデータを読み込む
    /// </summary>
    public void Load()
    {
        gameData = LoadData("gameData.json");
    }

    /// <summary>
    /// 新しいプレイレコードを追加し、データを保存する
    /// </summary>
    /// <param name="_clearTime"> クリアタイム </param>
    /// <param name="_backToBackCount"> BackToBackの回数 </param>
    /// <param name="_perfectClearCount"> Perfect Clearの回数 </param>
    /// <param name="_renCountsList"> Renの回数リスト </param>
    /// <param name="_spinCompleteCount"> Spin Completeの回数 </param>
    /// <param name="_tetrisCountList"> Tetrisの回数 </param>
    /// <param name="_iSpinCountsList"> I-Spinの回数リスト </param>
    /// <param name="_jSpinCountsList"> J-Spinの回数リスト </param>
    /// <param name="_lSpinCountsList"> L-Spinの回数リスト </param>
    /// <param name="_sSpinCountsList"> S-Spinの回数リスト </param>
    /// <param name="_tSpinCountsList"> T-Spinの回数リスト </param>
    /// <param name="_zSpinCountsList"> Z-Spinの回数リスト </param>
    void AddNewPlayRecord(float _clearTime, int _backToBackCount, int _perfectClearCount, List<int> _renCountList, int _spinCompleteCount, int _tetrisCount,
                          List<int> _iSpinCountsList, List<int> _jSpinCountsList, List<int> _lSpinCountsList, List<int> _sSpinCountsList, List<int> _tSpinCountsList, List<int> _zSpinCountsList)
    {
        // 新しいプレイレコードを作成
        GameData.PlayRecord newRecord = new GameData.PlayRecord
        {
            clearTime = _clearTime,
            backToBackCount = _backToBackCount,
            perfectClearCount = _perfectClearCount,
            RenCount = new List<int>(_renCountList),
            spinCompleteCount = _spinCompleteCount,
            tetrisCount = _tetrisCount,
            iSpinCounts = new List<int>(_iSpinCountsList),
            jSpinCounts = new List<int>(_jSpinCountsList),
            lSpinCounts = new List<int>(_lSpinCountsList),
            sSpinCounts = new List<int>(_sSpinCountsList),
            tSpinCounts = new List<int>(_tSpinCountsList),
            zSpinCounts = new List<int>(_zSpinCountsList)
        };

        // playRecordsリストに新しいプレイレコードを追加
        gameData.playRecords.Add(newRecord);

        // データを保存
        Save();
    }

}
