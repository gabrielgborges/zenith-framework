using UnityEngine;

[CreateAssetMenu(fileName = "MatchRoundsData", menuName = "Data/Rounds", order = 1)]
public class MatchRoundsData : ScriptableObject
{
    [SerializeField] private int _roundsToWin;
    [SerializeField] private int _secondsPerRound;
    [SerializeField] private int _startCountdown;

    public int RoundsToWin => _roundsToWin;
    public int SecondsPerRound => _secondsPerRound;
    public int StartCountdown => _startCountdown;
}
