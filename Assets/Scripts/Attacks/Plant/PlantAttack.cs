using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Player;
using DG.Tweening;

namespace Attack
{
    public class PlantAttack : MagicAttack
    {
        #region Private Variables

        // DATA
        private PlantAttackSettingsSO _plantSettingsSO => (PlantAttackSettingsSO)_magicSettingsSO;

        // VARIABLES
        // Weak Attack
        private float _healTimer; // Timer to heal
        private int _chargingHeal; // Times it has charge healing

        // Medium Attack
        private List<GameObject> _plantOrbs;

        #endregion

        #region Constructor

        public PlantAttack()
        {
            // Inicializamos las variables de estado
            base.Initialize();
            _healTimer = 0f;
            _plantOrbs = new List<GameObject>();
            _chargingHeal = 0;
        }

        #endregion

        #region Abstract class methods

        #region Initialization & Updating data

        public override void Init(MagicAttackSettingsSO magicSettings, PlayerStatus playerStatus, MagicEvents magicEvents, GameStatus gameStatus, IAudioSpeaker audioSpeaker, Transform transform)
        {
            base.Init(magicSettings, playerStatus, magicEvents, gameStatus, audioSpeaker, transform);

        }

        public override void Destroy()
        {
        }

        public override void Run(Vector2 direction)
        {
            if (_isUsingWeakAttack)
            {
                _healTimer += Time.deltaTime;

                if (_healTimer >= _plantSettingsSO.TimeToHeal / 4)
                {
                    WeakAttack(direction);
                }

            }


        }

        #endregion

        #region Attacks

        public override void WeakAttack(Vector2 direction)
        {
            _isUsingWeakAttack = true;

            _chargingHeal++;
            _healTimer = 0f;

            _magicEvents.UseOfMagicValue(_magicSettingsSO.Costs[1]);
            if (_chargingHeal == 4)
            {
                // TODO: Cure
                Debug.Log("Me curo 1 corazón");
                _chargingHeal = 0;
            }

        }

        public override void StopWeakAttack()
        {
            _isUsingWeakAttack = false;
            _chargingHeal = 0;
            _healTimer = 0f;
        }

        public override void MediumAttack(Vector2 direction)
        {
            _isUsingMediumAttack = true;


            _isUsingMediumAttack = false;
        }


        public override void StrongAttack(Vector2 direction)
        {
        }


        #endregion

        #endregion
    }
}
