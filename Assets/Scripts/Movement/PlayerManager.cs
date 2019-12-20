using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Experimental.PlayerLoop;

public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// All Raccoons in the game. Has to be 2.
    /// </summary>
    public List<Player> Raccoons;

    private Player _activeMovementRaccoon = null;

    /// <summary>
    /// The initially active raccoon.
    /// </summary>
    public Player ActiveMovementRaccoon
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
                }

                // add for new
                _activeMovementRaccoon = value;
                _activeMovementRaccoon.IsMovementActive = true;
                _activeMovementRaccoon.HandsOverMovement.AddListener(HandleHandOverMovement);
            }
        }
    }

    public void HandleHandOverMovement()
    {
        ActiveMovementRaccoon = Raccoons.Single(r => r != ActiveMovementRaccoon);
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