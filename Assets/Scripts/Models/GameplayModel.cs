namespace DefaultNamespace.Models
{
    public class GameplayModel
    {
        public bool IsEndgame { get; private set; }
        
        public bool IsWinCondition { get; private set; }

        public void SetEndgame(bool isWin)
        {
            IsEndgame = true;

            IsWinCondition = isWin;
        }
    }
}