using PlayerHordeModule.Views;

using UnityEngine;

namespace DefaultNamespace.Views
{
    public class AnimatedObstacle : BaseObstacleView
    {
        [SerializeField] private AnimatedView _animatedView;
        
        private void Start()
        {
            _animatedView.StartAnimation();
            
            playerEnter += OnPlayerEnter;
        }

        private void OnDestroy()
        {
            _animatedView.StopAnimation();
            
            playerEnter -= OnPlayerEnter;
        }

        private void OnPlayerEnter(HordeUnitView obj)
        {
            if (obj == null)
            {
                return;
            }
            
            InteractWithHordeModel(obj, holder => holder.RemoveUnit(obj));
        }
    }
}