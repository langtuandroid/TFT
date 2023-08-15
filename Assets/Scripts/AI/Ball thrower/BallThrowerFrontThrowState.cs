using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowerFrontThrowState : FSMBallThrower
{
    public BallThrowerFrontThrowState(BallThrower agent) : base(agent)
    {
        _agent = agent;
    }

    public override void Execute(Vector2 direction)
    {
        // If timer finalizes
        if (_timer >= _agent.TimeToRechargeBall)
        {
            // We execute the action
            FrontThrow(direction);

            // And we change to rotate state
            _agent.ChangeToOtherState(new BallThrowerRotateState(_agent));
        }

        // We increment the timer
        _timer += Time.deltaTime;
    }

    private void FrontThrow(Vector2 direction)
    {
        Vector2 pos = _agent.transform.position;
        pos += direction;


        GameObject fireball = MonoBehaviour.Instantiate(
            _agent.BallPrefab,
            pos,
            Quaternion.identity
            );

        fireball.GetComponent<Fireball>().SetDirection(direction);
    }
}
