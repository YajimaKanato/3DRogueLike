using Cysharp.Threading.Tasks;
using System.Threading;

public interface IState
{
    void Entry();
    StateName Update();
    UniTask Exit(CancellationToken token);
}
