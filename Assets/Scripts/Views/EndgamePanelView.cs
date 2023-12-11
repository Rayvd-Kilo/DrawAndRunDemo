using UnityEngine;

namespace DrawAndRun.Views
{
    public class EndgamePanelView : AnimatedView
    {
        [SerializeField] private GameObject _victoryPanel;
        [SerializeField] private GameObject _loosePanel;

        public void ActivateEndgamePanel(bool winCondition)
        {
            StartAnimation();
            
            gameObject.SetActive(true);
            _victoryPanel.SetActive(winCondition);
            _loosePanel.SetActive(!winCondition);
        }
    }
}