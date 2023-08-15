using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowerRotateState : FSMBallThrower
{

    public BallThrowerRotateState(BallThrower agent) : base(agent)
    {
        agent = _agent;
    }

    public override void Execute(Vector2 direction)
    {
        // We change the direction
        RotateAgent(direction);

        // And walk
        _agent.ChangeToOtherState(new BallThrowerAdvanceState(_agent));
    }

    private void RotateAgent(Vector2 direction)
    {
        //int n = Random.Range(0, 3);

        //bool addOne = false;
        //for (int i = 0; i <= n || addOne; i++)
        //    addOne = _directions[i] == direction;

        //_agent.ChangeDirection(_directions[addOne ? n : n + 1]);

        int n = Random.Range(0, 4);

        _agent.ChangeDirection(_directions[n]);
    }
}
