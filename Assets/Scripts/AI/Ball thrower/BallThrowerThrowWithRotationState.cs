using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowerThrowWithRotationState : FSMBallThrower
{
    private int _throwings = 0; // Number of times it has thrown a ball

    public BallThrowerThrowWithRotationState(BallThrower agent) : base(agent)
    {
        _agent = agent;
    }

    public override void Execute(Vector2 direction)
    {
        if (_throwings == 4)
            _agent.ChangeToOtherState(new BallThrowerRotateState(_agent));

        else
        {
            if (_timer >= _agent.TimeToRechargeBall)
                // TODO: Execute action
                ThrowBallWithRotation(direction);

            // Finally, we update the timer
            _timer += Time.deltaTime;
        }
    }

    private void ThrowBallWithRotation(Vector2 direction)
    {
        // We reset the timer
        _timer = 0f;

        Vector2 pos = _agent.transform.position;
        pos += direction;

        // We throw the ball
        GameObject fireball = MonoBehaviour.Instantiate(
            _agent.BallPrefab,
            pos,
            Quaternion.identity
            );

        fireball.GetComponent<Fireball>().SetDirection(direction);

        // And set the direction
        int n = Array.IndexOf(_directions, direction);
        _agent.ChangeDirection(_directions[(n + 1) % _directions.Length]);

        // Finally, we increment the number of times it has thrown a ball
        _throwings++;
    }
}
