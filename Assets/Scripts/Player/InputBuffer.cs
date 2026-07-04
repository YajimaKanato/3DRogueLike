using System;
using UnityEngine;

/// <summary>入力を管理するクラス</summary>
[Serializable]
public class InputBuffer
{
    /// <summary>入力有効時間の幅</summary>
    [SerializeField, Min(0)] float _bufferDuration;

    /// <summary>入力有効時間</summary>
    float _expireTime = -1;

    /// <summary>
    /// 入力有効時間を保存するメソッド
    /// </summary>
    public void Push()
    {
        if (HasInput) return; // 入力中は無視
        _expireTime = Time.time + _bufferDuration; // 入力有効時間を保存
    }

    /// <summary>入力が残っているか</summary>
    public bool HasInput => Time.time <= _expireTime;

    /// <summary>
    /// 入力を消費するメソッド
    /// </summary>
    /// <returns>正常に入力を消費できたか</returns>
    public bool Consume()
    {
        if (!HasInput) return false; // 入力有効時間を超えていたら無視
        _expireTime = -1; // 入力有効時間をリセット
        return true; // 正常に入力を消費できた
    }

    /// <summary>
    /// 入力をリセットするメソッド
    /// </summary>
    public void Clear()
    {
        _expireTime = -1;
    }
}
