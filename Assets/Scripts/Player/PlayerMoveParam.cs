using System;
using UnityEngine;

[Serializable]
public class PlayerMoveParam
{
    [Header("速度の変化"), SerializeField] AnimationCurve _speedCurve;
    [Header("歩きの最大速度（X軸）"), SerializeField] float _walkMaxXAxis;
    [Header("走りの最大速度（X軸）"), SerializeField] float _runMaxXAxis;
    [Header("入力なし時の減衰率"), SerializeField] float _noInputDamping = 2;
    const float INPUT_VALID_RANGE = 0.01f;
    float _x;
    float _z;

    public Vector3 GetVelocity(bool isRunning, ref Vector2 dir)
    {
        // 入力を分割して取得
        var dirX = dir.x;
        var dirZ = dir.y;
        // 前方に走っている場合は走りの最大速度で比較
        var compareValue = isRunning && dirX >= 0 && dirZ >= 0 ? _runMaxXAxis : _walkMaxXAxis;

        if (dirX != 0 && Mathf.Abs(_x) < compareValue * Mathf.Abs(dirX))
        {
            // 入力量に対する最大速度でないなら
            // 入力方向に数値を増やす
            _x += Time.deltaTime * Mathf.Sign(dirX) * compareValue;
        }
        else
        {
            // 入力がない
            if (Mathf.Abs(_x) < INPUT_VALID_RANGE)
            {
                // 一定量を下回ったら0にする
                _x = 0;
            }
            else
            {
                // 入力状態に応じて減少
                _x -= Time.deltaTime * Mathf.Sign(_x) * compareValue * (dirX != 0 ? 1 : _noInputDamping);
            }
        }

        if (dirZ != 0 && Mathf.Abs(_z) < compareValue * Mathf.Abs(dirZ))
        {
            // 入力量に対する最大速度でないなら
            // 入力方向に数値を増やす
            _z += Time.deltaTime * Mathf.Sign(dirZ) * compareValue;
        }
        else
        {
            // 入力がない
            if (Mathf.Abs(_z) < INPUT_VALID_RANGE)
            {
                _z = 0;
            }
            else
            {
                _z -= Time.deltaTime * Mathf.Sign(_z) * compareValue * (dirZ != 0 ? 1 : _noInputDamping);
            }
        }

        // カーブにおける数値に変換
        var speedX = _speedCurve.Evaluate(_x);
        var speedZ = _speedCurve.Evaluate(_z);
        dir.x = _x;
        dir.y = _z;
        return new Vector3(speedX, 0, speedZ);
    }
}
