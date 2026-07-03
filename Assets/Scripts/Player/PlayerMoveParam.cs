using System;
using UnityEngine;

[Serializable]
public class PlayerMoveParam
{
    [Header("速度の変化"), SerializeField] AnimationCurve _speedCurve;
    [Header("歩きの最大速度（X軸）"), SerializeField] float _walkMaxXAxis;
    [Header("走りの最大速度（X軸）"), SerializeField] float _runMaxXAxis;
    const float INPUT_VALID_RANGE = 0.01f;
    float _x;
    float _z;

    public Vector3 GetVelocity(bool isRunning, Vector2 dir)
    {
        // 入力を分割して取得
        var dirX = dir.x;
        var dirZ = dir.y;

        // 入力に応じた変化量を
        // 最大値を超えていれば減算
        // 最大値に到達していなければ加算
        _x += Time.deltaTime * (dirX == 0 ? Mathf.Sign(_x) : Mathf.Sign(dirX)) * (isRunning ? _runMaxXAxis : _walkMaxXAxis)
            * ((Mathf.Abs(_x) < (isRunning ? _runMaxXAxis : _walkMaxXAxis) * Mathf.Abs(dirX)) ? 1 : -1);
        _z += Time.deltaTime * (dirZ==0? Mathf.Sign(_z):Mathf.Sign(dirZ)) * (isRunning ? _runMaxXAxis : _walkMaxXAxis)
            * ((Mathf.Abs(_z) < (isRunning ? _runMaxXAxis : _walkMaxXAxis) * Mathf.Abs(dirZ)) ? 1 : -1);

        // 0に近くなったら0にする
        if (Mathf.Abs(_x) < 0.0001f) _x = 0;
        if (Mathf.Abs(_z) < 0.0001f) _z = 0;

        // カーブにおける数値に変換
        var speedX = _speedCurve.Evaluate(_x);
        var speedZ = _speedCurve.Evaluate(_z);
        return new Vector3(speedX, 0, speedZ);
    }
}
