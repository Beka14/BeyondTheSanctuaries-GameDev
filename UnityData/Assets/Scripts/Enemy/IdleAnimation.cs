using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour
{
    [SerializeField]
    private float _timeUntilIdle;

    [SerializeField]
    private int _numberOfIdleAnimations;

    private bool _isBored;
    private float _idleTime; 
    private int _boredAnimation; 

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isBored == false)
        {
            _idleTime += Time.deltaTime;

            if (_idleTime > _timeUntilIdle && stateInfo.normalizedTime % 1 < 0.02f) 
            {
                _isBored = true;
                _boredAnimation = Random.Range(0, _numberOfIdleAnimations);


                animator.SetFloat("IdleAnimation", _boredAnimation);
            }
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }

        animator.SetFloat("IdleAnimation", _boredAnimation, 0.2f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (_isBored)
        {
            _boredAnimation--;
        }

        _isBored = false;
        _idleTime = 0;
    }
}