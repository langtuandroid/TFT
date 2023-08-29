using UnityEngine;

[CreateAssetMenu(fileName = "ChangeSceneInfoSO", menuName = "Level Control/ChangeSceneInfoSO")]
public class ChangeSceneInfoSO :ScriptableObject
{
    [SerializeField][Range( 0, 15 )] private int _nextStartPointRefID;
    [SerializeField] private string _nextScene;
    [SerializeField] private Color _fadeOutColor;
    [SerializeField] private MusicName _musicName;
    [SerializeField] private float _fadeOutSeconds = 1f;

    public int NextStartPointRefID => _nextStartPointRefID;
    public string NextScene => _nextScene;
    public Color FadeOutColor => _fadeOutColor;
    public MusicName MusicName => _musicName;
    public float FadeOutSeconds => _fadeOutSeconds;
}
