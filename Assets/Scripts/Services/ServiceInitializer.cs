// ************ @autor: Álvaro Repiso Romero *************

using Honeti;
using UnityEngine;
using static ServiceLocator;

namespace Services
{
    [DefaultExecutionOrder(-5)]
    public class ServiceInitializer : MonoBehaviour
    {
        private void Awake()
        {
            if (!IsInitialized)
            {
                // Load or Initialize OptionsSave
                OptionsSave optionsSave = new SaveGame().LoadOptions();
                GameStatus gameStatus = new GameStatus();

                // Systems
                AddService( gameStatus );
                AddService(optionsSave);
                AddService(new GameInputs(optionsSave, gameStatus));
                AddService(new SceneLoader());

                // Events
                AddService(new MagicEvents());
                AddService(new InventoryEvents());
                AddService(new LevelEvents());
                AddService(new LifeEvents());
                AddService(new SoulEvents());

                IsInitialized = true;
            }
            Destroy(gameObject);
        }
    }
}