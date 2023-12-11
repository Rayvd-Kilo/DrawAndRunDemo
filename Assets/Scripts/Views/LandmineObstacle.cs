using DrawAndRun.PlayerHordeModule.Views;

using UnityEngine;

namespace DrawAndRun.Views
{
    public class LandmineObstacle : BaseObstacleView
    {
        [SerializeField] private ParticleSystem _particleSystem;

        private bool _interacted;
        
        private void Start()
        {
            playerEnter += OnPlayerEnter;
        }

        private void OnDestroy()
        {
            playerEnter -= OnPlayerEnter;
        }

        private void OnPlayerEnter(HordeUnitView obj)
        {
            if (_interacted)
            {
                return;
            }
            
            if (obj == null)
            {
                return;
            }

            _interacted = true;
            
            _particleSystem.transform.SetParent(null);
            
            _particleSystem.Play();
            
            InteractWithHordeModel(obj, holder =>
            {
                holder.RemoveUnit(obj);
            });
            
            gameObject.SetActive(false);
        }
    }
}