using System;
using UnityEngine;
using DG.Tweening;

namespace Player
{
    public class AnimatorBrain : MonoBehaviour
    {
        public event Action<OnJumpableHasLandedArgs> OnJumpableHasLanded;
        public class OnJumpableHasLandedArgs
        {
            public float yLandPosition;
        }

        public event Action<OnJumpDownHasLandedArgs> OnJumpDownHasLanded;
        public class OnJumpDownHasLandedArgs
        {
            public Vector3 landedPosition;
        }


        [SerializeField] private Transform _playerVisuals;
        [SerializeField] private Transform _shadowVisuals;
        [SerializeField] private AnimationCurve _jumpCurve;

        private Vector3 _playerVisualInitialPos;
        private Vector3 _shadowVisualInitialPos;

        private Animator _playerAnimator;
        private SpriteRenderer _spriteRender;
        private Vector2 _lookDirection;
        private Vector2 _initialLookDirection;
        private bool _throwAnimationEnded = true;

        [Header("States")]
        private const string IDLE = "IdleTree";
        private const string JUMP = "JumpTree";
        private const string DEATH = "DeathTree";
        private const string FALL = "FallTree";
        private const string PICKUP = "PickItUpTree";
        private const string THROW = "ThrowItemTree";
        private const string PHYSICAL_ATTACK = "PhysicalAtkTree";
        private const string MAGIC_ATTACK = "MagicAtkTree";

        [Header("Parameters")]
        private const string X_DIR = "x";
        private const string Y_DIR = "y";
        private const string IS_WALKING = "IsWalking";
        private const string HAS_ITEM = "HasItem";

        private LifeEvents _lifeEvents;
        private MagicEvents _magicEvents;

        private Tween _tween;

        private void OnDestroy()
        {
            _lifeEvents.OnDeathValue -= Death_OnDeath;
            _lifeEvents.OnStartTemporalInvencibility -= StartTemporalInvencibility;
            _magicEvents.OnMaxPowerUsedValue -= MaxPower_OnUsed;
        }

        public void Init(Vector2 startLookDirection, Jump jump)
        {
            _playerAnimator = GetComponent<Animator>();
            _spriteRender = GetComponent<SpriteRenderer>();

            _playerVisualInitialPos = _playerVisuals.localPosition;
            _shadowVisualInitialPos = _shadowVisuals.localPosition;

            jump.OnJumpStarted += Jump_OnJumpStarted;
            jump.OnJumpFinished += Jump_OnJumpFinished;
            jump.OnJumpableActionStarted += Jump_OnJumpableActionStarted;
            jump.OnJumpDownStarted += Jump_OnJumpDownStarted;

            LookDirection(startLookDirection);
            _initialLookDirection = startLookDirection;

            _lifeEvents = ServiceLocator.GetService<LifeEvents>();
            _lifeEvents.OnDeathValue += Death_OnDeath;
            _lifeEvents.OnStartTemporalInvencibility += StartTemporalInvencibility;

            _magicEvents = ServiceLocator.GetService<MagicEvents>();
            _magicEvents.OnMaxPowerUsedValue += MaxPower_OnUsed;
        }

        private void PlayPlayer(string stateName) => _playerAnimator.Play(stateName);

        private void Jump_OnJumpDownStarted(Jump.OnJumpDownStartedArgs jumpDownArgs)
        {
            PlayPlayer(JUMP);

            var jumpPower = 2f;
            _playerVisuals.DOLocalJump(jumpDownArgs.landedRelativePosition, jumpPower, 1, 1)
                .SetEase( _jumpCurve )
                .OnComplete(HasLandedAfterJumpDown_Callback)
                .Play();

            if ( jumpDownArgs.descendDirection == Vector3.up || jumpDownArgs.descendDirection == Vector3.down )
            {
                _shadowVisuals.DOLocalMove( jumpDownArgs.landedRelativePosition , 0.9f )
                    .Play();
            }
            else
            {
                var moveXPixels = 6f / 16;
                var lateralJumpSeq = DOTween.Sequence();
                lateralJumpSeq.Append( _shadowVisuals.DOLocalMoveX( jumpDownArgs.descendDirection.x * moveXPixels , 0.1f ) );
                lateralJumpSeq.Append( _shadowVisuals.DOLocalMoveY( jumpDownArgs.landedRelativePosition.y , 0f ) );
                lateralJumpSeq.Append( _shadowVisuals.DOLocalMoveX( jumpDownArgs.landedRelativePosition.x , 0.7f ) );
                lateralJumpSeq.Play();
            }

        }

        private void HasLandedAfterJumpDown_Callback()
        {
            PlayPlayer(IDLE);

            Vector3 landPosition = new Vector3(_playerVisuals.position.x, _playerVisuals.position.y - _playerVisualInitialPos.y); // rectify relative to world
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;

            OnJumpDownHasLanded?.Invoke(new OnJumpDownHasLandedArgs()
            {
                landedPosition = landPosition
            });
        }


        private void Jump_OnJumpableActionStarted()
        {
            PlayPlayer(JUMP);

            Vector3 endJumpRelativePos = new Vector3(0, 2.5f, 0);
            float jumpPower = 2;
            _playerVisuals.DOLocalJump(endJumpRelativePos, jumpPower, 1, 1 )
                .SetEase( _jumpCurve )
                .OnComplete(HasLandedAfterJumpable_Callback)
                .Play();
            _shadowVisuals.DOLocalMoveY(2.5f, 0.9f)
                .Play();
        }

        public void HasLandedAfterJumpable_Callback()
        {
            PlayPlayer(IDLE);

            float yLandPos = _playerVisuals.localPosition.y - _playerVisualInitialPos.y; // rectify relative to world
            _playerVisuals.localPosition = _playerVisualInitialPos;
            _shadowVisuals.localPosition = _shadowVisualInitialPos;

            OnJumpableHasLanded?.Invoke(new OnJumpableHasLandedArgs()
            {
                yLandPosition = yLandPos
            });
        }

        private void Jump_OnJumpFinished()
        {
            PlayPlayer(IDLE);
        }

        private void Jump_OnJumpStarted()
        {
            PlayPlayer(JUMP);
        }

        private void MaxPower_OnUsed(float value)
        {
            IsWalking(false);
        }

        private void ReturnToMainMenu()
        {
            ServiceLocator.GetService<IAudioSpeaker>().ChangeMusic(MusicName.Main_Menu);
            ServiceLocator.GetService<SceneLoader>().Load(SceneName.S00_MainMenuScene.ToString());
        }

        private void Death_OnDeath()
        {
            if (_tween != null)
                _tween.Kill();

            _spriteRender.DOFade(1f, 0f).Play();

            PlayPlayer(DEATH);
            Invoke(nameof(ReturnToMainMenu), 5f);
        }

        private void StartTemporalInvencibility(float time)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(_spriteRender.DOFade(60 / 255f, 0f));
            seq.Append(_spriteRender.DOFade(1f, time))
                .SetEase(Ease.InOutFlash, 14, -1)
                ;

            seq.OnComplete(() => _lifeEvents.StopTemporalInvencibility());
            seq.Play();

            _tween = seq;
        }

        public void SetFall()
        {
            _shadowVisuals.gameObject.SetActive(false);
            PlayPlayer(FALL);
        }

        public void RecoverFromFall()
        {
            LookDirection(_initialLookDirection);
            _spriteRender.enabled = true;
            _shadowVisuals.gameObject.SetActive(true);
            PlayPlayer(IDLE);
        }

        public void SetThrow()
        {
            _throwAnimationEnded = false;
            PlayPlayer(THROW);
        }

        public void SetIdle() => PlayPlayer(IDLE);

        public void ThrowAnimationEnded()
        {
            _throwAnimationEnded = true;
        }

        public bool HasThrowAnimationEnded() => _throwAnimationEnded;

        public void IsWalking(bool isWalking)
        {
            _playerAnimator.SetBool(IS_WALKING, isWalking);
        }

        public void HasItem(bool hasItem)
        {
            _playerAnimator.SetBool(HAS_ITEM, hasItem);
        }

        public void SetMagicAttack()
        {
            PlayPlayer(MAGIC_ATTACK);
        }

        public void SetPhysicalAttack()
        {
            PlayPlayer(PHYSICAL_ATTACK);
        }

        public void PickUpItem()
        {
            PlayPlayer(PICKUP);
        }

        public bool HasCurrentAnimationEnded()
        {
            return _playerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.75f;
        }

        // Se llama desde el animator
        public void PlayerFalledDown()
        {
            ServiceLocator.GetService<LifeEvents>().FallDown();
            _spriteRender.enabled = false;
        }

        public Vector2 LookDirection(Vector2 direction)
        {
            if (direction.magnitude > 0)
            {
                float x = direction.x;
                float y = direction.y;

                if (Mathf.Abs(x) > Mathf.Abs(y))
                {
                    _lookDirection.x = x > 0f ? 1f : -1f;
                    _lookDirection.y = 0f;
                }
                else if (Mathf.Abs(y) > Mathf.Abs(x))
                {
                    _lookDirection.x = 0f;
                    _lookDirection.y = y > 0f ? 1f : -1f;
                }
                else
                {
                    if (x == 0f && y == 0f)
                    {
                        _lookDirection.x = x;
                        _lookDirection.y = y;
                    }
                    else if (Mathf.Abs(_lookDirection.x) > Mathf.Abs(_lookDirection.y))
                    {
                        _lookDirection.x = x > 0f ? 1f : -1f;
                        _lookDirection.y = 0f;
                    }
                    else
                    {
                        _lookDirection.x = 0f;
                        _lookDirection.y = y > 0f ? 1f : -1f;
                    }
                }

                _playerAnimator.SetFloat(X_DIR, _lookDirection.x);
                _playerAnimator.SetFloat(Y_DIR, _lookDirection.y);
            }
            return _lookDirection;
        }

    }
}