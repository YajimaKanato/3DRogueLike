using UnityEngine.InputSystem;

/// <summary>InputActionのキャッシュをするPlayerViewの分割クラス</summary>
public partial class PlayerView
{
    InputAction _move;
    InputAction _run;
    InputAction _rotation;
    InputAction _attack;
    InputAction _jump;

    /// <summary>
    /// アクションをキャッシュするメソッド
    /// </summary>
    /// <param name="actions">InputActionのクラス</param>
    void CacheAction(Actions actions)
    {
        _actions = actions;
        _move = _actions.Ingame.Move;
        _run = _actions.Ingame.Run;
        _rotation = _actions.Ingame.Rotation;
        _attack = _actions.Ingame.Attack;
        _jump = _actions.Ingame.Jump;
    }
}
