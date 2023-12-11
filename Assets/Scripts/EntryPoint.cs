using DrawAndRun.Controllers;
using DrawAndRun.DrawingModule.Views;
using DrawAndRun.Models;
using DrawAndRun.PlayerHordeModule.Views;
using DrawAndRun.Views;

using UnityEngine;

namespace DrawAndRun
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private CurveDrawerView _curveDrawerView;
        [SerializeField] private HordeUnitsHolder _hordeUnitsHolder;

        [SerializeField] private AnimatedView _drawPopup;
        [SerializeField] private EndgamePanelView _endgamePanel;

        [SerializeField] private ParticleSystem _deathParticles;

        [SerializeField] private AnimatedView[] _animatedViews;

        [SerializeField] private ParticleSystem[] _endgameParticleSystems;

        private void Start()
        {
            var gameplayModel = new GameplayModel();
            
            var hordeController = new HordeController(_hordeUnitsHolder, _curveDrawerView, _deathParticles, gameplayModel);
            
            var gameplayController =
                new GameplayController(_hordeUnitsHolder, _curveDrawerView, _drawPopup, _endgamePanel, _animatedViews,
                    gameplayModel, _endgameParticleSystems);

            gameplayController.Initialize();

            hordeController.Initialize();
        }
    }
}