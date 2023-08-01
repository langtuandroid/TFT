using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemIA : MonoBehaviour
{
   [SerializeField] private GolemArmIA _leftHandIA;
   [SerializeField] private GolemArmIA _rightHandIA;
   private GolemHealth _health;

   private void Awake()
   {
      _health = GetComponent<GolemHealth>();
   }

   private void Start()
   {
      _health.OnDamage += DamageAttack;
   }

   private void DamageAttack()
   {
      _leftHandIA.AttackPunch();
      _rightHandIA.AttackPunch();
   }
}
