using System;
using System.Collections.Generic;
using System.Linq;

using DrawAndRun.Models;

using DG.Tweening;

using DrawAndRun.DrawingModule.Views;
using DrawAndRun.PlayerHordeModule.Enums;
using DrawAndRun.PlayerHordeModule.Views;

using Unity.VisualScripting;

using UnityEngine;

using Object = UnityEngine.Object;

namespace DrawAndRun.Controllers
{
    public class HordeController : IDisposable
    {
        private readonly HordeUnitsHolder _hordeUnitsHolder;
        private readonly CurveDrawerView _curveDrawerView;
        private readonly ParticleSystem _particleSystem;
        private readonly GameplayModel _gameplayModel;
        private readonly HashSet<HordeUnitView> _hordeUnits = new();
        
        private Vector3[] _offsetFromTarget;
        
        public HordeController(HordeUnitsHolder hordeUnitsHolder, CurveDrawerView curveDrawerView,
            ParticleSystem particleSystem, GameplayModel gameplayModel)
        {
            _hordeUnitsHolder = hordeUnitsHolder;
            _curveDrawerView = curveDrawerView;
            _particleSystem = particleSystem;
            _gameplayModel = gameplayModel;
        }
        
        public void Initialize()
        {
            _curveDrawerView.PointerReleased += CurveUpdate;
            
            _hordeUnitsHolder.UnitAdded += HordeUnitsHolderOnUnitAdded;
            
            _hordeUnitsHolder.UnitRemoved += HordeUnitsHolderOnUnitRemoved;
            
            _hordeUnitsHolder.EndReached += OnEndReached;
            
            InitializeHorde();
        }

        public void Dispose()
        {
            _curveDrawerView.PointerReleased -= CurveUpdate;
            
            _hordeUnitsHolder.UnitAdded -= HordeUnitsHolderOnUnitAdded;
            
            _hordeUnitsHolder.UnitRemoved -= HordeUnitsHolderOnUnitRemoved;
            
            _hordeUnitsHolder.EndReached -= OnEndReached;
        }
        
        private void HordeUnitsHolderOnUnitRemoved(HordeUnitView removedUnit)
        {
            _hordeUnits.Remove(removedUnit);
            
            removedUnit.PlayerAnimationController.RunAnimation(PlayerAnimationType.Death);

            var deathParticle = Object.Instantiate(_particleSystem, removedUnit.transform.position, Quaternion.identity);

            deathParticle.transform.localScale *= 0.3f;
            
            deathParticle.Play();
            
            removedUnit.gameObject.transform.SetParent(null);

            if (_hordeUnits.Count == 0)
            {
                _gameplayModel.SetEndgame(false);
            }
        }

        private void HordeUnitsHolderOnUnitAdded(HordeUnitView newView)
        {
            _hordeUnits.Add(newView);

            newView.transform.eulerAngles = Vector3.zero;
            
            newView.PlayerAnimationController.RunAnimation(PlayerAnimationType.Run);

            newView.gameObject.tag = "Agent";

            RelocateUnits();
        }
        
        private void OnEndReached()
        {
            _gameplayModel.SetEndgame(true);
            
            SetRectangleOffset();
            
            RelocateUnits();
            
            foreach (var hordeUnitView in _hordeUnits)
            {
                hordeUnitView.transform.DOLocalRotate(new Vector3(0, 180, 0), 0.2f);
                
                hordeUnitView.PlayerAnimationController.RunAnimation(PlayerAnimationType.Dance);
            }
        }
        
        private void CurveUpdate(Vector3[] offsetPoints)
        {
            if (_gameplayModel.IsEndgame)
            {
                return;
            }
            
            _offsetFromTarget = offsetPoints;
            RelocateUnits();
        }
        
        private void InitializeHorde()
        {
            _hordeUnits.AddRange(_hordeUnitsHolder.StartUnits);

            _hordeUnitsHolder.SetMoveSpeed(0);
            
            SetRectangleOffset();

            RelocateUnits();
        }
        
        private void SetRectangleOffset()
        {
            int row = _hordeUnits.Count / 3, col = 3;
            float colSpace = .1f, rowSpace = .1f;
            var offsets = new Vector3[row * col];
            var startPoint = _hordeUnitsHolder.HordeCenter.position.normalized + Vector3.forward * (rowSpace / 2 * row) +
                             Vector3.left * (colSpace / 2 * col);
            for (var i = 0; i < row; ++i)
            {
                for (var j = 0; j < col; ++j)
                {
                    offsets[i * col + j] = startPoint + Vector3.back * ((i + 1) * rowSpace) +
                                           Vector3.right * ((j + 1) * colSpace);
                }
            }

            _offsetFromTarget = offsets;
        }
        
        private void RelocateUnits()
        {
            if (_hordeUnits.Count > 0)
            {
                var units = _hordeUnits.ToArray();
                var pointsPerUnit = (float) _offsetFromTarget.Length / _hordeUnits.Count;
                var unitIndex = 0;
                for (var i = 0f; i < _offsetFromTarget.Length && unitIndex < _hordeUnits.Count; i += pointsPerUnit)
                {
                    units[unitIndex++].transform.DOLocalMove(_offsetFromTarget[Mathf.FloorToInt(i)], 0.1f);
                }
            }
        }
    }
}