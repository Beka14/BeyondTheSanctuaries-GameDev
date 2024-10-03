using UnityEngine;

public class AnimEvent : MonoBehaviour
{
    public IAnimEventAcceptor proxy;
    public void Fire(string func)
    {
        proxy?.FireEvent(func);
    }
}

public interface IAnimEventAcceptor
{
    void FireEvent(string func);
}