using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMB_Falling : StateMachineBehaviour
{
    private SurvivalCharacter_CharacterAnimator _animatorController = null;
    private SurvivalCharacter_CharacterController _characterController = null;
    private bool _isLanding = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _isLanding = false;
        _animatorController = animator.transform.GetComponentInParent<SurvivalCharacter_CharacterAnimator>();
        _characterController = animator.transform.GetComponentInParent<SurvivalCharacter_CharacterController>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_isLanding) return;
        if (_animatorController == null) return;
        if (_characterController == null) return;

        var landingHeight = _animatorController.LandingHeight;
        var rayOrigin = animator.transform.parent.position + new Vector3(0f, 0.15f, 0f);
        var ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, _animatorController.LandingHeight + 0.15f, _characterController.GroundMasks.value))
        {
            _isLanding = true;
            animator.CrossFadeInFixedTime("Landing_Unarmed", 0.15f);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
