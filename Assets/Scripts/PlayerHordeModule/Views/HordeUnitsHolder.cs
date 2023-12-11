using System;

using Dreamteck.Splines;

using UnityEngine;

namespace DrawAndRun.PlayerHordeModule.Views
{
    public class HordeUnitsHolder : MonoBehaviour
    {
        public event Action EndReached = delegate { }; 
        
        public event Action<HordeUnitView> UnitAdded = delegate {  };
        
        public event Action<HordeUnitView> UnitRemoved = delegate {  };
        
        public HordeUnitView[] StartUnits => _startUnits;

        public Transform HordeCenter => _hordeCenter.transform;
        
        [SerializeField] private SplineFollower _splineFollower;
        
        [SerializeField] private HordeUnitView[] _startUnits;

        [SerializeField] private Rigidbody _hordeCenter;

        private void Start()
        {
            _splineFollower.onEndReached += OnEndReached; 
        }

        public void SetMoveSpeed(float newSpeed)
        {
            _splineFollower.followSpeed = newSpeed;
        }

        public void AddUnit(HordeUnitView hordeUnitView)
        {
            UnitAdded.Invoke(hordeUnitView);
        }

        public void RemoveUnit(HordeUnitView hordeUnitView)
        {
            UnitRemoved.Invoke(hordeUnitView);
        }
        
        private void OnEndReached(double obj)
        {
            EndReached.Invoke();
        }
    }
}