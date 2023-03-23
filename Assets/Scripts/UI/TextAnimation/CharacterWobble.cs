using UnityEngine;
using TMPro;

namespace UI.TextAnimation
{
    public class CharacterWobble : MonoBehaviour
    {
        [SerializeField][Range(1, 20)] private float _xSpeed = 3.3f;
        [SerializeField][Range(1, 20)] private float _ySpeed = 2.5f;

        private TMP_Text _textMesh;
        private Mesh _mesh;
        private Vector3[] _vertices;

        private void Start()
        {
            _textMesh = GetComponent<TMP_Text>();
        }

        private void Update()
        {
            _textMesh.ForceMeshUpdate();
            _mesh = _textMesh.mesh;
            _vertices = _mesh.vertices;

            for ( int i = 0; i < _vertices.Length; i++ )
            {
                Vector3 offset = Wobble(Time.time + i);
                _vertices[i] = _vertices[i] + offset;
            }

            _mesh.vertices = _vertices;
            _textMesh.canvasRenderer.SetMesh( _mesh );
        }

        private Vector2 Wobble( float time )
        {
            return new Vector2( Mathf.Sin( time * _xSpeed ) , Mathf.Cos( time * _ySpeed ) );
        }
    }
}