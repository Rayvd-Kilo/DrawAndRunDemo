using System;

using DrawAndRun.PlayerHordeModule.Views;

using UnityEngine;

namespace DrawAndRun.Views
{
    public abstract class BaseObstacleView : MonoBehaviour
    {
        protected event Action<HordeUnitView> playerEnter = delegate { };
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag.Equals("Agent") && other.TryGetComponent(out HordeUnitView hordeUnitView))
            {
                playerEnter.Invoke(hordeUnitView);
            }
        }

        protected void InteractWithHordeModel(HordeUnitView unitView, Action<HordeUnitsHolder> holderDelegate)
        {
            var hordeHolder = unitView.GetComponentInParent<HordeUnitsHolder>();

            if (hordeHolder == null)
            {
                return;
            }
            
            holderDelegate.Invoke(hordeHolder);
        }
    }
}