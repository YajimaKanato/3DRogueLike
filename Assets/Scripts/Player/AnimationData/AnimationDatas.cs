using UnityEngine;

/// <summary>ステートにおける情報を管理するクラス</summary>
[CreateAssetMenu(fileName = "AnimationDatas", menuName = "AnimationData/AnimationDatas")]
public class AnimationDatas : ScriptableObject
{
    [SerializeField] AnimationData[] _datas;

    public AnimationData[] Datas => _datas;
}
