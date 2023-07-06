using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoBreakableGround : MonoBehaviour
{
    [SerializeField] private List<AutoBreakableUnitController> _breakGroundList;

    private bool _isActivated;

    public void StartBreakingGround( bool reverseDirection )
    {
        if ( _isActivated ) 
            return;

        _isActivated = true;

        if ( reverseDirection )
            _breakGroundList.Reverse();

        StartCoroutine( BreakGroundCor() );
    }

    private IEnumerator BreakGroundCor()
    {
        WaitForSeconds waitSecondsBetweenBreak = new( 0.8f );

        yield return waitSecondsBetweenBreak;

        waitSecondsBetweenBreak = new( 0.3f );

        while ( _breakGroundList.Count > 0 )
        {
            _breakGroundList[0].Break();
            _breakGroundList.RemoveAt( 0 );
            yield return waitSecondsBetweenBreak;
        }
    }
}
