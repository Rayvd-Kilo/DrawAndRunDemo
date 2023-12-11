using DrawAndRun.PlayerHordeModule.Views;

using UnityEngine;

namespace DrawAndRun.Views
{
    public class AgentObstacle : BaseObstacleView
    {
        [SerializeField] private HordeUnitView _hordeUnitView;

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
            InteractWithHordeModel(obj, holder =>
            {
                var targetPosition = obj.transform.localPosition;

                transform.SetParent(holder.transform);

                transform.localPosition = targetPosition;
                
                holder.AddUnit(_hordeUnitView);
            });
            
            Destroy(this);
        }
    }
}