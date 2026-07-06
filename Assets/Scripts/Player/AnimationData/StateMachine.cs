using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [Header("ステート情報"), SerializeField] StateDatas _stateDatas;
    AnimationPlayer _animator;
    Dictionary<StateName, RuntimeStateData> _stateDict;
    RuntimeStateData _currentState;
    CancellationTokenSource _cts;

    void Init()
    {
        _stateDict = new();
        _animator = new(GetComponent<Animator>(), _stateDatas);
        _cts = new CancellationTokenSource();

        foreach (var stateData in _stateDatas.Datas)
        {
            _stateDict[stateData.State] = new(_animator, stateData);
        }

        _stateDict.TryGetValue(StateName.Idle, out _currentState);
    }

    private async void Update()
    {
        if (_currentState == null) return;

        var nextState = _currentState.Update();

        if (nextState != _currentState.StateName)
        {
            await ChangeState(nextState, _cts.Token);
        }
    }

    private void OnDestroy()
    {
        _animator.Dispose();
    }

    async UniTask ChangeState(StateName stateName, CancellationToken token)
    {
        // HasExitTimeがOnの場合に待つ
        await _currentState.Exit(token);

        _stateDict.TryGetValue(stateName, out _currentState);

        _currentState?.Entry();
    }
}
