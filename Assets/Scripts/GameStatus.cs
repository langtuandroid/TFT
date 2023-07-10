using System;
using UnityEngine;

public class GameStatus
{
    public GameStatus() 
    {
        state = GameState.MenuUI;
    }

    public enum GameState
    {
        MenuUI,   // menu pausa, main menu, cuando hay texto en pantalla
        GamePlay, // Jugando normalmente
        Inactive  // Durante el juego los objetos no se mueven -> ataque definitivo, cambio de escena
    }
    private GameState state;

    public event Action<GameState> OnGameStateChanged;

    public void AskChangeToMenuUIState()
    {
        switch ( state )
        {
            // Si el estado YA es MenuUI no pasa nada
            case GameState.MenuUI:
                ConfirmState(); 
                break;

            // Si el estado es GamePlay se cambia a MenuUI
            case GameState.GamePlay:
                ChangeState( GameState.MenuUI );
                break;

            // En cualquier otro caso no podemos cambiar de estado
            default:
                CannotChangeState( GameState.MenuUI ); break;
        }
    }

    public void AskChangeToGamePlayState()
    {
        switch( state )
        {
            // Si estamos en MenuUI podemos cambiar a GamePlay
            case GameState.MenuUI:
                ChangeState( GameState.GamePlay );
                break;

            // Si el estado YA es GamePlay no pasa nada
            case GameState.GamePlay: 
                ConfirmState(); 
                break;

            // Si el juego está inactivo cambiamos a GamePlay
            case GameState.Inactive:
                ChangeState( GameState.GamePlay );
                break;

            // En cualquier otro caso no podemos cambiar de estado
            default: 
                CannotChangeState( GameState.GamePlay ); break;
        }
    }

    public void AskChangeToInactiveState()
    {
        switch ( state )
        {
            case GameState.GamePlay:
                ChangeState( GameState.Inactive );
                break;

            case GameState.Inactive:
                ConfirmState(); 
                break;

            default:
                CannotChangeState( GameState.Inactive ); break;
        }
    }


    private void ChangeState( GameState newState )
    {
        Debug.Log( $"Se cambió del estado {state} a {newState}" );
        state = newState;
        OnGameStateChanged?.Invoke( state );
    }

    private void CannotChangeState( GameState newState )
    {
        Debug.LogWarning( $"[Advertencia:] Estando en {state}, NO puedes cambiar a {newState}" );
    }
    private void ConfirmState()
    { 
        Debug.LogWarning( $"[¿Hay algún duplicado de llamada?] Ya está en {state}" ); 
    }
}
