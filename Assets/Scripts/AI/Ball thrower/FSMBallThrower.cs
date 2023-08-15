using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMBallThrower
{
    #region Internal variables

    internal Vector2[] _directions =
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    internal float _timer = 0f;
    internal BallThrower _agent;

    #endregion

    public FSMBallThrower (BallThrower agent)
    {
        _agent = agent;
    }

    public abstract void Execute(Vector2 direction);
}
