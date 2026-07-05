using UnityEngine;

public partial class PlayerView
{
    [Header("戦闘準備・攻撃の入力有効時間"), SerializeField] InputBuffer _attackBuffer;
    [Header("戦闘体制解除の入力有効時間"), SerializeField] InputBuffer _sheatheBuffer;

    void Attack()
    {
        if (_attack.WasPressedThisFrame()) _attackBuffer?.Push();
    }

    void Sheathe()
    {
        if (_sheathe.WasPressedThisFrame()) _sheatheBuffer?.Push();
    }
}
