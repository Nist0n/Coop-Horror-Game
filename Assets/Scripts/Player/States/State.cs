using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete { get; protected set; }

    private float _startTime;

    private float time => Time.time - _startTime;
    
    public virtual void Enter()
    {
        
    }
    public virtual void Do()
    {
        
    }
    public virtual void Exit()
    {
        
    }
}
