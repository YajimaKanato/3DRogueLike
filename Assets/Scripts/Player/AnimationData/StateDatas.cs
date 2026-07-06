using UnityEngine;

/// <summary>ステートにおける情報を管理するクラス</summary>
[CreateAssetMenu(fileName = "StateDatas", menuName = "StateData/StateDatas")]
public class StateDatas : ScriptableObject
{
    [SerializeField] StateData[] _datas;

    public StateData[] Datas => _datas;
}
