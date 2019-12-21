﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.Experimental.PlayerLoop;

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
            ActiveMovementRaccoon = Raccoons.Single(r => r != ActiveMovementRaccoon);
    }

    private bool RaccoonsHaveLineOfSight
    {
        get
        {
            Raccoon first = Raccoons.First();
            Raccoon second = Raccoons.Last();

            var layerMask = ~((1 << first.gameObject.layer) | (1 << second.gameObject.layer));
            return !Physics.Linecast(first.transform.position, second.transform.position, layerMask);
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