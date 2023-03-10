using DarkTonic.MasterAudio;
using MoreMountains.Feedbacks;
using System;
using UnityEngine;

namespace MultiversalMakers {
    [RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class PlayerAnimator : MonoBehaviour {
        private IPlayerController _player;
        private Animator _anim;
        private SpriteRenderer _renderer;
        [SerializeField] private MMF_Player _jumpFeedback;
        [SerializeField] private MMF_Player _idleFeedback;

        private void Awake() {
            _player = GetComponentInParent<IPlayerController>();
            _anim = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Start() {
            _player.GroundedChanged += OnGroundedChanged;
            _player.WallGrabChanged += OnWallGrabChanged;
            _player.Jumped += OnJumped;
            _player.AirJumped += OnAirJumped;
            _player.Attacked += OnAttacked;
        }

        private void Update() {
            HandleSpriteFlipping();
            HandleGroundEffects();
            HandleWallSlideEffects();
            SetParticleColor(Vector2.down, _moveParticles);
            HandleAnimations();
        }

        private void HandleSpriteFlipping() {
            if (_isOnWall &_player.WallDirection != 0) _renderer.flipX = _player.WallDirection == -1;
            else if (Mathf.Abs(_player.Input.x) > 0.1f) _renderer.flipX = _player.Input.x < 0;

            // Rocket Particles
            _rocketParticles.transform.localPosition = new Vector3 (_renderer.flipX ? Mathf.Abs(_rocketParticles.transform.localPosition.x) : -Mathf.Abs(_rocketParticles.transform.localPosition.x), _rocketParticles.transform.localPosition.y, 0);
        }

        #region Ground Movement

        [Header("GROUND MOVEMENT")] 
        [SerializeField] private ParticleSystem _moveParticles;
        [SerializeField] private float _tiltChangeSpeed = .05f;
        [SerializeField] private float _maxTiltAngle = 45;
        [SerializeField] private string _footstepClips;

        private ParticleSystem.MinMaxGradient _currentGradient = new(Color.white, Color.white);
        [SerializeField] private Vector2 _tiltVelocity;

        private void HandleGroundEffects() {
            // Move particles get bigger as you gain momentum
            var speedPoint = Mathf.InverseLerp(0, _player.PlayerStats.MaxSpeed, Mathf.Abs(_player.Speed.x));
            _moveParticles.transform.localScale = Vector3.MoveTowards(_moveParticles.transform.localScale, Vector3.one * speedPoint, 2 * Time.deltaTime);

            // Rotate
            var targetRotVector = new Vector3(0, 0, Mathf.Lerp(_maxTiltAngle, -_maxTiltAngle, Mathf.InverseLerp(-1, 1, _player.Input.x)));
            _anim.transform.rotation = Quaternion.RotateTowards(_anim.transform.rotation, Quaternion.Euler(targetRotVector), _tiltChangeSpeed * Time.deltaTime);

            if (_currentState == Idle)
            {
                _idleFeedback.PlayFeedbacks();
            }
        }

        private int _stepIndex;

        // Called from AnimationEvent
        public void PlayFootstepSound() {
            _stepIndex = (_stepIndex + 1) % _footstepClips.Length;
            MasterAudio.PlaySound(_footstepClips);
        }

        #endregion

        #region Wall Sliding and Climbing

        [Header("WALL")] 
        [SerializeField] private float _wallHitAnimTime = 0.167f;
        [SerializeField] private ParticleSystem _wallSlideParticles;
        [SerializeField] private string _wallClimbClips;
        [SerializeField] private float _maxWallSlideVolume = 0.2f;
        [SerializeField] private float _wallSlideVolumeSpeed = 0.6f;
        [SerializeField] private float _wallSlideParticleOffset = 0.3f;

        private bool _hitWall, _isOnWall, _isSliding, _dismountedWall;

        private void OnWallGrabChanged(bool onWall) {
            _hitWall = _isOnWall = onWall;
            _dismountedWall = !onWall;

            _rocketParticles.Stop();
        }

        private void HandleWallSlideEffects() {
            var slidingThisFrame = _isOnWall && !_grounded && _player.Speed.y < 0;

            if (!_isSliding && slidingThisFrame) {
                _isSliding = true;
                _wallSlideParticles.Play();
            }
            else if (_isSliding && !slidingThisFrame) {
                _isSliding = false;
                _wallSlideParticles.Stop();
            }

            SetParticleColor(new Vector2(_player.WallDirection, 0), _wallSlideParticles);
            _wallSlideParticles.transform.localPosition = new Vector3(_wallSlideParticleOffset * _player.WallDirection, 0, 0);

        }

        private int _wallClimbIndex = 0;

        // Called from AnimationEvent
        public void PlayWallClimbSound() {
            _wallClimbIndex = (_wallClimbIndex + 1) % _wallClimbClips.Length;
            MasterAudio.PlaySound(_wallClimbClips);
        }

        #endregion

        #region Ladders

        [Header("LADDER")]
        [SerializeField] private string _ladderClips;
        private int _climbIndex = 0;

        // Called from AnimationEvent
        public void PlayLadderClimbSound() {
            if (_player.Speed.y < 0) return;
            _climbIndex = (_climbIndex + 1) % _ladderClips.Length;
            MasterAudio.PlaySound(_ladderClips);
        }

        #endregion

        #region Jumping and Landing

        [Header("JUMPING")] 
        [SerializeField] private float _minImpactForce = 20;
        [SerializeField] private float _maxImpactForce = 40;
        [SerializeField] private float _landAnimDuration = 0.1f;
        [SerializeField] private string _landClip, _jumpClip, _doubleJumpClip;
        [SerializeField] private ParticleSystem _jumpParticles, _launchParticles, _rocketParticles, _doubleJumpParticles, _landParticles;
        [SerializeField] private Transform _jumpParticlesParent;

        private bool _jumpTriggered;
        private bool _landed;
        private bool _grounded;
        private bool _wallJumped;

        private void OnJumped(bool wallJumped) {
            
            _jumpTriggered = true;
            _wallJumped = wallJumped;
            MasterAudio.PlaySound(_jumpClip);

            _jumpParticlesParent.localRotation = Quaternion.Euler(0, 0, _player.WallDirection * 60f);

            _jumpFeedback.PlayFeedbacks();
            _rocketParticles.Play();

            SetColor(_jumpParticles);
            SetColor(_launchParticles);
            _jumpParticles.Play();
        }

        private void OnAirJumped() {
            _jumpTriggered = true;
            _wallJumped = false;
            MasterAudio.PlaySound(_doubleJumpClip);
            _doubleJumpParticles.Play();
        }

        private void OnGroundedChanged(bool grounded, float impactForce) {
            _grounded = grounded;

            if (impactForce >= _minImpactForce) {
                var p = Mathf.InverseLerp(_minImpactForce, _maxImpactForce, impactForce);
                _landed = true;
                 
                _landParticles.transform.localScale = p * Vector3.one;
                _landParticles.Play();
                SetColor(_landParticles);
                MasterAudio.PlaySound(_landClip);
            }

            if (_grounded) _moveParticles.Play();
            else _moveParticles.Stop();

            if(_grounded)
                _rocketParticles.Stop();

        }

        #endregion

        #region Attack

        [Header("ATTACK")] 
        [SerializeField] private float _attackAnimTime = 0.25f;
        [SerializeField] private string _attackClip;
        private bool _attacked;

        private void OnAttacked() => _attacked = true;

        // Called from AnimationEvent
        public void PlayAttackSound() => MasterAudio.PlaySound(_attackClip);

        #endregion

        #region Animation

        private float _lockedTill;

        private void HandleAnimations() {
            var state = GetState();
            ResetFlags();
            if (state == _currentState) return;
            
            _anim.Play(state, 0); //_anim.CrossFade(state, 0, 0);
            _currentState = state;

            int GetState() {
                if (Time.time < _lockedTill) return _currentState;

                if (_attacked) return LockState(Attack, _attackAnimTime);
                if (_player.ClimbingLadder) return _player.Speed.y == 0 || _grounded ? ClimbIdle : Climb;

                if (!_grounded) {
                    if (_hitWall) return LockState(WallHit, _wallHitAnimTime);
                    if (_isOnWall) {
                        if (_player.Speed.y < 0) return WallSlide;
                        if (_player.Speed.y > 0) return WallClimb;
                        if (_player.Speed.y == 0) return WallIdle;
                    }
                }

                if (_player.Crouching) return _player.Input.x == 0 || !_grounded ? Crouch : Crawl;
                if (_landed) return LockState(Land, _landAnimDuration);
                if (_jumpTriggered) return _wallJumped ? Backflip : Jump;

                if (_grounded) return _player.Input.x == 0 ? Idle : Walk;
                if (_player.Speed.y > 0) return _wallJumped ? Backflip : Jump;
                return _dismountedWall && _player.Input.x != 0 ? LockState(WallDismount, 0.167f) : Fall;
                // TODO: If WallDismount looks/feels good enough to keep, we should add clip duration (0.167f) to Stats

                int LockState(int s, float t) {
                    _lockedTill = Time.time + t;
                    return s;
                }
            }

            void ResetFlags() {
                _jumpTriggered = false;
                _landed = false;
                _attacked = false;
                _hitWall = false;
                _dismountedWall = false;
            }
        }

        #region Cached Properties

        private int _currentState;

        private static readonly int Idle = Animator.StringToHash("Idle");
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Crouch = Animator.StringToHash("Crouch");
        private static readonly int Crawl = Animator.StringToHash("Crawl");

        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Fall = Animator.StringToHash("Fall");
        private static readonly int Land = Animator.StringToHash("Land");
        
        private static readonly int ClimbIdle = Animator.StringToHash("ClimbIdle");
        private static readonly int Climb = Animator.StringToHash("Climb");
        
        private static readonly int WallHit = Animator.StringToHash("WallHit");
        private static readonly int WallIdle = Animator.StringToHash("WallIdle");
        private static readonly int WallClimb = Animator.StringToHash("WallClimb");
        private static readonly int WallSlide = Animator.StringToHash("WallSlide");
        private static readonly int WallDismount = Animator.StringToHash("WallDismount");
        private static readonly int Backflip = Animator.StringToHash("Backflip");

        private static readonly int Attack = Animator.StringToHash("Attack");
        #endregion

        #endregion

        #region Particles

        private readonly RaycastHit2D[] _groundHits = new RaycastHit2D[2];

        private void SetParticleColor(Vector2 detectionDir, ParticleSystem system) {
            var hitCount = Physics2D.RaycastNonAlloc(transform.position, detectionDir, _groundHits, 2);
            if (hitCount <= 0) return;

            _currentGradient = _groundHits[0].transform.TryGetComponent(out SpriteRenderer r) 
                ? new ParticleSystem.MinMaxGradient(r.color * 0.9f, r.color * 1.2f) 
                : new ParticleSystem.MinMaxGradient(Color.white);

            SetColor(system);
        }

        private void SetColor(ParticleSystem ps) {
            var main = ps.main;
            main.startColor = _currentGradient;
        }

        #endregion

    }
}