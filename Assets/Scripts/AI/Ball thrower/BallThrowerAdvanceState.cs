using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrowerAdvanceState : FSMBallThrower
{
    public BallThrowerAdvanceState(BallThrower agent) : base(agent)
    {
        _agent = agent;
    }

    public override void Execute(Vector2 direction)
    {
        // Checking if we have to change to another state
        if (_timer >= _agent.TimeWalking)
            ChangeToRandomState();

        // We move the agent
        MoveAgent(direction);

        // Finally, we update the timer
        _timer += Time.deltaTime;
    }

    private void MoveAgent(Vector2 direction)
    {
        Vector2 pos = _agent.transform.position;
        pos += direction * .55f;

        Debug.DrawRay(
            pos,
            _agent.transform.TransformDirection(direction) * .1f,
            Color.green
            );

        // If there is a wall in front
        if (
            Physics2D.Raycast(
                pos, // Start position
                _agent.transform.TransformDirection(direction), // Direction
                .1f, // Distance
                ~LayerMask.GetMask("PLayer") // Layer to ignore
                )
            )
        {
            // We rotate
            _agent.ChangeToOtherState(new BallThrowerRotateState(_agent));
        }
        // In other case
        else
            // We move to the direction
            _agent.Rb.MovePosition(_agent.Rb.position + direction * Time.deltaTime * _agent.Velocity);
    }

    private void ChangeToRandomState()
    {
        int n = Random.Range(0, 3);
        switch (n)
        {
            case 0:
                _agent.ChangeToOtherState(new BallThrowerFrontThrowState(_agent));
                break;
            case 1:
                _agent.ChangeToOtherState(new BallThrowerThrowWithRotationState(_agent));
                break;
            case 2:
                _agent.ChangeToOtherState(new BallThrowerRotateState(_agent));
                break;
        }
    }
}
