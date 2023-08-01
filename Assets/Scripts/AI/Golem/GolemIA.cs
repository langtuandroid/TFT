using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

public class GolemIA : MonoBehaviour
{
   [SerializeField] private GolemArmIA _leftHandIA;
   [SerializeField] private GolemArmIA _rightHandIA;
   public Camera mainCamera;
   public float PlayerDetectionRadious;
   public float speed;
   
   
   
   private GolemHealth _health;
   private Animator _anim;
   private GameObject _player;
   private Rigidbody2D _rb;

   private void Awake()
   {
      _health = GetComponent<GolemHealth>();
      _anim = GetComponent<Animator>();
      _rb = GetComponent<Rigidbody2D>();
   }

   private void Start()
   {
      _health.OnDamage += DamageAttack;
       _anim.Play("IdleAwake");
   }

   private void Update()
   {
      if(CheckPlayer()) GoToPlayer();
     // else //ataque de lejos con brazos
   }

   private bool CheckPlayer()
   {
      Collider2D results = Physics2D.OverlapCircle(transform.position, PlayerDetectionRadious, LayerMask.GetMask(Constants.TAG_PLAYER));

      if (results != null)
      {
         if (results.CompareTag(Constants.TAG_PLAYER))
         {
            _player = results.gameObject;
            return true;
         }
      }

      return false;
   }


   //Cuando recbo daño
   private void DamageAttack()
   {
      _leftHandIA.AttackPunch();
      _rightHandIA.AttackPunch();
   }

   //Camino hacia el jugador
   private void GoToPlayer()
   {
      _anim.Play("walk");

      Vector2 direction = (_player.transform.position - transform.position).normalized;
      Vector2 velocity = direction * speed;

      // Usamos Translate para mover el golem con la velocidad calculada.
      transform.Translate(velocity * Time.deltaTime);
   }

   
   private IEnumerator ShakeCamera(float duration, float magnitude)
   {
      Vector3 originalPosition = mainCamera.transform.localPosition;
      float elapsed = 0.0f;

      while (elapsed < duration)
      {
         float x = Random.Range(-1f, 1f) * magnitude;
         float y = Random.Range(-1f, 1f) * magnitude;

         mainCamera.transform.localPosition = originalPosition + new Vector3(x, y, 0f);

         elapsed += Time.deltaTime;

         yield return null;
      }

      mainCamera.transform.localPosition = originalPosition;
   }


   
   void OnDrawGizmos()
   {
      Gizmos.color = Color.blue;
      Gizmos.DrawWireSphere(transform.position, PlayerDetectionRadious);
   }
}
