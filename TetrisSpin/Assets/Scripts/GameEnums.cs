/// <summary>
/// オーディオ名一覧
/// </summary>
public enum AudioNames
{
    GameOver,
    HardDrop,
    Hold,
    MoveDown,
    MoveLeftRight,
    NormalDestroy,
    NormalDrop,
    Rotation,
    Spin,
    SpinDestroy,
    StartOrRetry,
    Tetris
}

/// <summary>
/// ミノの種類一覧
/// </summary>
public enum MinoType
{
    I_Mino,
    J_Mino,
    L_Mino,
    O_Mino,
    S_Mino,
    T_Mino,
    Z_Mino
}

/// <summary>
/// ミノの向き一覧
/// </summary>
public enum MinoDirections
{
    North, // 初期(未回転)状態
    East,  // 右回転後の向き
    South, // 2回右回転または左回転した時の向き
    West   // 左回転後の向き
}

/// <summary>
/// ミノの回転方向一覧
/// </summary>
public enum MinoRotationDirections
{
    RotateRight,
    RotateLeft
}

/// <summary>
/// スピン判定一覧
/// </summary>
public enum SpinTypeNames
{
    I_Spin,
    I_SpinMini,
    J_Spin,
    L_Spin,
    O_Spin,
    S_Spin,
    T_Spin,
    T_SpinMini,
    Z_Spin,
    None
}

/// <summary>
/// ブロックの存在判定一覧
/// </summary>
public enum Existence
{
    Exist,
    NotExist
}


