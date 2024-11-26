using System;
using System.Collections.Generic;

[Serializable]
public struct GameplayScreenModel
{
    public MatchRoundsData RoundsConfig;
    public int CurrentRound;
    private Dictionary<string, int> _winByPlayer;

    public void ResetWins()
    {
        _winByPlayer = new Dictionary<string, int>();
    }

    public void AddWinner(string winner)
    {
        if(!_winByPlayer.TryAdd(winner, 1))
        {
            _winByPlayer[winner] += 1;
        }
    }

    public int GetPlayerWins(string player)
    {
        int playerWins = 0;
        _winByPlayer.TryGetValue(player, out playerWins);
        return playerWins;
    }
}
