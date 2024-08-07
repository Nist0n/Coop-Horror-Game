using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator;
    private float _animInterpolX;
    private float _animInterpolY;
    
    public void Run()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, -1f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 1f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void Idle()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 0, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 0, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void Walk()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 1, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 0.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void WalkLeft()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 1.5f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 0.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void WalkRight()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 1, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, 1f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void Sneak()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, -1f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, -0.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void SneakBackward()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 1f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, -0.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void SneakLeft()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, 0.5f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, -1.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }
    
    public void SneakRight()
    {
        _animInterpolX = Mathf.Lerp(_animInterpolX, -0.5f, Time.deltaTime * 3);
        _animInterpolY = Mathf.Lerp(_animInterpolY, -1.5f, Time.deltaTime * 3);
        animator.SetFloat("x", _animInterpolX);
        animator.SetFloat("y", _animInterpolY);
    }

    public void Setup(Animator _animator, float y, float x)
    {
        animator = _animator;
    }
}
