using Cysharp.Threading.Tasks;

using DefaultNamespace.Models;
using DefaultNamespace.Views;

using DrawingModule.Views;

using Dreamteck;

using PlayerHordeModule.Enums;
using PlayerHordeModule.Views;

using UnityEngine;

namespace DefaultNamespace.Controllers
{
    public class GameplayController
    {
        private readonly HordeUnitsHolder _hordeUnitsHolder;
        private readonly CurveDrawerView _curveDrawerView;
        private readonly AnimatedView _drawPopup;
        private readonly EndgamePanelView _endgamePanelView;
        private readonly AnimatedView[] _animatedViews;
        private readonly GameplayModel _gameplayModel;
        private readonly ParticleSystem[] _endgameParticleSystems;

        public GameplayController(HordeUnitsHolder hordeUnitsHolder, CurveDrawerView curveDrawerView,
            AnimatedView drawPopup, EndgamePanelView endgamePanelView, AnimatedView[] animatedViews,
            GameplayModel gameplayModel, ParticleSystem[] endgameParticleSystems)
        {
            _hordeUnitsHolder = hordeUnitsHolder;
            _curveDrawerView = curveDrawerView;
            _drawPopup = drawPopup;
            _endgamePanelView = endgamePanelView;
            _animatedViews = animatedViews;
            _gameplayModel = gameplayModel;
            _endgameParticleSystems = endgameParticleSystems;
        }

        public void Initialize()
        {
            _curveDrawerView.PointerReleased += StartGame;

            _drawPopup.StartAnimation();

            _animatedViews.ForEach(x => x.StartAnimation());

            StartGameplayAsync().Forget();
        }

        private void StartGame(Vector3[] _)
        {
            _curveDrawerView.PointerReleased -= StartGame;

            _drawPopup.StopAnimation();

            _drawPopup.gameObject.SetActive(false);

            _hordeUnitsHolder.SetMoveSpeed(3);

            _hordeUnitsHolder.StartUnits.ForEach(x =>
                x.PlayerAnimationController.RunAnimation(PlayerAnimationType.Run));
        }

        private async UniTaskVoid StartGameplayAsync()
        {
            await UniTask.WaitUntil(() => _gameplayModel.IsEndgame);
            
            _hordeUnitsHolder.SetMoveSpeed(0);

            if (_gameplayModel.IsWinCondition)
            {
                _endgameParticleSystems.ForEach(x => x.Play());
            }
            
            _endgamePanelView.ActivateEndgamePanel(_gameplayModel.IsWinCondition);
        }
    }
}