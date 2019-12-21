using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class RaccoonMovementManager : MonoBehaviour
{
    /// <summary>
    /// All Raccoons in the game. Has to be 2.
    /// </summary>
    public List<Raccoon> Raccoons;

    /// <summary>
    /// The soul which will be shown on movement swap.
    /// </summary>
    public Soul Soul;

    /// <summary>
    /// Change player info (UI)
    /// </summary>
    public PlayerInfoScript playerInfo;

    private Raccoon _activeMovementRaccoon = null;

    /// <summary>
    /// The initially active raccoon.
    /// </summary>
    public Raccoon ActiveMovementRaccoon
    {
        get { return _activeMovementRaccoon; }
        set
        {
            if (_activeMovementRaccoon != value)
            {
                // remove for old
                if (_activeMovementRaccoon != null)
                {
                    _activeMovementRaccoon.IsMovementActive = false;
                    _activeMovementRaccoon.HandsOverMovement.RemoveAllListeners();
                    _activeMovementRaccoon.Died.RemoveAllListeners();

                    // show soul
                    MoveSoul(_activeMovementRaccoon, value);
                }
                else
                    SetActiveMovementRaccoon(value);
            }
        }
    }

    private void SetActiveMovementRaccoon(Raccoon value)
    {
        // add for new
        _activeMovementRaccoon = value;
        _activeMovementRaccoon.IsMovementActive = true;
        _activeMovementRaccoon.HandsOverMovement.AddListener(HandleHandOverMovement);
        _activeMovementRaccoon.Died.AddListener(HandleDied);

        if (playerInfo != null)
        {
            playerInfo.SetActivePlayer(value == Raccoons.First() ? 0 : 1);
        }
    }

    private void MoveSoul(Raccoon from, Raccoon to)
    {
        Vector3 start = from.transform.position;
        var s = Instantiate(Soul, start, Quaternion.identity);
        s.MovementFinished.AddListener(SoulMovementFinished);
        s.Move(from, to);
    }

    private void SoulMovementFinished(Raccoon goal)
    {
        SetActiveMovementRaccoon(goal);
    }

    public void HandleHandOverMovement()
    {
        if (RaccoonsHaveLineOfSight)
        {
            SoundManager.Instance.playRacSwitch();
            ActiveMovementRaccoon = Raccoons.Single(r => r != ActiveMovementRaccoon);
        }
    }
    
    private void HandleDied(Raccoon deadRaccoon)
    {
        if (deadRaccoon == _activeMovementRaccoon)
        {
            // TODO game over
            Debug.Log("GAME OVER");
        }
    }

    private bool RaccoonsHaveLineOfSight
    {
        get
        {
            Raccoon first = Raccoons.First();
            Raccoon second = Raccoons.Last();

            return !(first.IsDead || second.IsDead);
        }
    }

    void Start()
    {
        Assert.IsTrue(Raccoons.Count == 2);
        if (ActiveMovementRaccoon == null)
            ActiveMovementRaccoon = Raccoons.First();
    }

    void Update()
    {
    }
}