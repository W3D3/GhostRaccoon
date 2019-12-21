using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

public class RaccoonManager : MonoBehaviour
{
    /// <summary>
    /// All Raccoons in the game. Has to be 2.
    /// </summary>
    public List<Raccoon> Raccoons;

    /// <summary>
    /// The soul which will be shown on movement swap.
    /// </summary>
    public Soul Soul;

    public UIController uiController;

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
                    _activeMovementRaccoon.StopMoving();
                    RemoveRaccoonHandlers(_activeMovementRaccoon);

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
        AddRaccoonHandlers(_activeMovementRaccoon);
        
        uiController.SetActivePlayer(value == Raccoons.First() ? 0 : 1);
    }

    private void AddRaccoonHandlers(Raccoon r)
    {
        r.HandsOverMovement.AddListener(HandleHandOverMovement);
        r.Died.AddListener(HandleDied);
        r.Won.AddListener(HandleWon);
    }

    private void RemoveRaccoonHandlers(Raccoon r)
    {
        r.HandsOverMovement.RemoveAllListeners();
        r.Died.RemoveAllListeners();
        r.Won.RemoveAllListeners();
    }

    private void MoveSoul(Raccoon from, Raccoon to)
    {
        Vector3 start = from.transform.position;
        start.y += 1.3f;
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
            uiController.DisplayGameOver();
        }
    }

    private void HandleWon()
    {
        uiController.DisplayNextLevel();
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