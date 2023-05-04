using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
//using Utils;

public class ShootLight : MonoBehaviour
{
    [SerializeField]
    private GameObject lightPrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GameObject _light = Instantiate(lightPrefab, transform.position, Quaternion.identity);

            LightMovement _lightScript = _light.GetComponent<LightMovement>();

            GetPlayerDirection(_lightScript);
        }
    }

    public void GetPlayerDirection(LightMovement _lightScript)
    {
        switch (PlayerMovement.Instance.Layer)
        {
            // Si miramos hacia abajo
            case PlayerMovement.AnimationLayers.WalkDown:
                //_lightScript.transform.SetY(transform.position.y - 2);
                _lightScript.HandleMovement(Vector3.down);
                break;
            // Si miramos en horizontal
            case PlayerMovement.AnimationLayers.WalkHorizontal:
                // Si est� mirando a la izquierda
                if (PlayerMovement.Instance.HorizontalFlip)
                {
                    //_lightScript.transform.SetX(transform.position.x - 2);
                    _lightScript.HandleMovement(Vector3.left);
                }
                // En otro caso
                else
                {
                    //_lightScript.transform.SetX(transform.position.x + 2);
                    _lightScript.HandleMovement(Vector3.right);
                }
                break;
            // Si miramos hacia arriba
            case PlayerMovement.AnimationLayers.WalkUp:
                //_lightScript.transform.SetY(transform.position.y + 2);
                _lightScript.HandleMovement(Vector3.up);
                break;
        }
    }
}
