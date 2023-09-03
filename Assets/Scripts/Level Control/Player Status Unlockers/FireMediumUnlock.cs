using UnityEngine;

public class FireMediumUnlock : MonoBehaviour
{
    [SerializeField] private PlayerStatusSaveSO _playerStatusSaveSO;

    public void Unlock()
    {
        _playerStatusSaveSO.playerStatusSave.isFireMediumUnlocked = true;
    }
}
