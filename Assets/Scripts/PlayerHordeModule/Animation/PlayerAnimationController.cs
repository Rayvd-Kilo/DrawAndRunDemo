using System;

using PlayerHordeModule.Enums;

using UnityEngine;

namespace PlayerHordeModule.Animation
{
    public class PlayerAnimationController
    {
        private static readonly int IdleAnimationID = Animator.StringToHash("Idle");
        private static readonly int RunAnimationID = Animator.StringToHash("Run");
        private static readonly int DeathAnimationID = Animator.StringToHash("Death");
        private static readonly int DanceAnimationID = Animator.StringToHash("Finish");
        
        private readonly Animator _animator;

        public PlayerAnimationController(Animator animator)
        {
            _animator = animator;
        }

        public void RunAnimation(PlayerAnimationType animationType)
        {
            switch (animationType)
            {
                case PlayerAnimationType.Idle:
                    _animator.SetTrigger(IdleAnimationID);
                    break;
                case PlayerAnimationType.Run:
                    _animator.SetTrigger(RunAnimationID);
                    break;
                case PlayerAnimationType.Death:
                    _animator.SetTrigger(DeathAnimationID);
                    break;
                case PlayerAnimationType.Dance:
                    _animator.SetTrigger(DanceAnimationID);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(animationType), animationType, null);
            }
        }
    }
}