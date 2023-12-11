using PlayerHordeModule.Animation;

using UnityEngine;

namespace PlayerHordeModule.Views
{
    public class HordeUnitView : MonoBehaviour
    {
        public PlayerAnimationController PlayerAnimationController => _playerAnimationController;
        
        [SerializeField] private Animator _animator;
        
        private PlayerAnimationController _playerAnimationController;
        
        private void Awake()
        {
            _playerAnimationController = new PlayerAnimationController(_animator);
        }
    }
}