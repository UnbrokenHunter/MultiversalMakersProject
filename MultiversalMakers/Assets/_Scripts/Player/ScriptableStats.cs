using Sirenix.OdinInspector;
using UnityEngine;

namespace MultiversalMakers {
    [CreateAssetMenu]
    public class ScriptableStats : ScriptableObject {
        [Title("LAYERS")]
        [Tooltip("Set this to the layer your player is on")]
        public LayerMask PlayerLayer;

        [Tooltip("Set this to the layer climbable walls are on")]
        public LayerMask ClimbableLayer;

        [Tooltip("Set this to the layer your ladders are on")]
        public LayerMask LadderLayer;

        [Title("INPUT")]
        [Tooltip("Makes all Input snap to an integer. Prevents gamepads from walking slowly. Recommended value is true to ensure gamepad/keybaord parity.")]
        public bool SnapInput = true;

        [Tooltip("Minimum input req'd before you mount a ladder or climb a ledge. Avoids unwanted climbing using controllers"), Range(0.01f, 0.99f)]
        public float VerticalDeadzoneThreshold = 0.3f;

        [Tooltip("Minimum input req'd before a left or right is recognized. Avoids drifting with sticky controllers"), Range(0.01f, 0.99f)]
        public float HorizontalDeadzoneThreshold = 0.1f;

        [Title("MOVEMENT")]
        [Tooltip("The top horizontal movement speed")]
        public float MaxSpeed = 14;

        [Tooltip("The player's capacity to gain horizontal speed")]
        public float Acceleration = 120;

        [Tooltip("The pace at which the player comes to a stop")]
        public float GroundDeceleration = 60;

        [Tooltip("Deceleration in air only after stopping input mid-air")]
        public float AirDeceleration = 30;

        [Tooltip("A constant downward force applied while grounded. Helps on slopes"), Range(0f, -10f)]
        public float GroundingForce = -1.5f;
        
        [Tooltip("The improved deceleration after landing without input. Helps accuracy while platforming. To disable, set to 1"), Range(1f, 10f)]
        public float StickyFeetMultiplier = 2f;

        [Tooltip("The detection distance for grounding and roof detection"), Range(0f, 0.5f)]
        public float GrounderDistance = 0.05f;

        [Title("CROUCHING")]
        [Tooltip("Allows crouching")]
        public bool AllowCrouching = true;

        [Tooltip("A speed multiplier while crouching"), Range(0f, 1f)]
        public float CrouchSpeedPenalty = 0.5f;

        [Tooltip("The amount of frames it takes to hit the full crouch speed penalty. Higher values provide more crouch sliding"), Min(0)]
        public int CrouchSlowdownFrames = 50;

        [Tooltip("Obstacle detection vertical offset to stand back up from a crouch. A 0 will detect the floor as an obstacle.")]
        public float CrouchBufferCheck = 0.01f;

        [Title("JUMP")] 
        [Tooltip("Amount of air jumps allowed. e.g. 1 is a standard double jump"), Min(0)]
        public int MaxAirJumps = 1;

        [Tooltip("The immediate velocity applied when jumping")]
        public float JumpPower = 36;

        [Tooltip("Whether or not the player can hover")]
        public bool CanHover = true;

        [Tooltip("The reduction in fall speed when hovering")]
        public float InAirHoverMultipler = 0.5f;

        [Tooltip("The maximum vertical movement speed")]
        public float MaxFallSpeed = 40;

        [Tooltip("The player's capacity to gain fall speed. a.k.a. In Air Gravity")]
        public float FallAcceleration = 110;

        [Tooltip("The gravity multiplier added when jump is released early")]
        public float JumpEndEarlyGravityModifier = 3;

        [Tooltip("The fixed frames before coyote jump becomes unusable. Coyote jump allows jump to execute even after leaving a ledge")]
        public int CoyoteFrames = 7;

        [Tooltip("The amount of fixed frames we buffer a jump. This allows jump input before actually hitting the ground")]
        public int JumpBufferFrames = 7;

        [Title("WALLS")] 
        [Tooltip("Allows wall climbing, sliding, and jumping")]
        public bool AllowWalls = true;
        
        [Tooltip("Only wall climb or slide when you're providing horizontal input into the wall")]
        public bool RequireInputPush = false;

        [Tooltip("How fast you climb walls.")]
        public float WallClimbSpeed = 5;
        
        [Tooltip("The player's capacity to gain wall sliding speed. 0 = stick to wall")]
        public float WallFallAcceleration = 8;

        [Tooltip("Clamps the maximum fall speed")]
        public float MaxWallFallSpeed = 15;

        [Tooltip("The immediate velocity horizontal velocity applied when wall jumping")]
        public Vector2 WallJumpPower = new(30, 25);
        
        [Tooltip("The frames before full horizontal movement is returned after a wall jump"), Min(1)]
        public int WallJumpInputLossFrames = 18;

        [Tooltip("The amount of fixed frames where you can still wall jump after pressing to leave a wall")]
        public int WallJumpCoyoteFrames = 5;

        [Tooltip("Bounds for detecting walls on either side. Ensure it's wider than your vertical capsule collider")]
        public Vector2 WallDetectorSize = new(0.75f, 1.25f);

        [Title("LADDERS")]
        [Tooltip("Allows climbing up and down ladders")]
        public bool AllowLadders = true;
        
        [Tooltip("Automatically climb ladders when you're close enough to them")]
        public bool AutoAttachToLadders;

        [Tooltip("When enabled, nudges the player to be centered on the ladder")]
        public bool SnapToLadders = true;

        [Tooltip("Time it takes to smoothly snap to the ladder's center")]
        public float LadderSnapTime = 0.05f;
        
        [Tooltip("Horizontal speed multiplier while attached to a ladder")]
        public float LadderShimmySpeedMultiplier = 0.5f;

        [Tooltip("How fast you climb up ladders")]
        public float LadderClimbSpeed = 8;

        [Tooltip("How fast you slide down ladders")]
        public float LadderSlideSpeed = 12;
        
        [Tooltip("How many frames can pass between re-interacting with ladders. Helps with dismounting and jumping glitches")]
        public int LadderCooldownFrames = 8;
        
        [Tooltip("The upward force given when leaving a ladder and holding up. This allows you to 'pop' on top of a ladder.")]
        public float LadderPopForce = 20f;

        [Title("Bounce On Head")]
        public bool canBounce = true;

        public Vector2 BounceForce = new (5, 0);

        public PlayerForce BounceForceType = PlayerForce.Decay;

        [Title("ATTACKS")]
        [Tooltip("Allows the player to attack")]
        public bool AllowAttacks = true;

        [Tooltip("The fixed frame cooldown of your players basic attack")]
        public int AttackFrameCooldown = 15;

        [Title("EXTERNAL")] 
        [Tooltip("The rate at which external velocity decays. Should be close to Fall Acceleration")]
        public int ExternalVelocityDecay = 100; // This may become deprecated in a future version

#if UNITY_EDITOR
        [Header("GIZMOS")] 
        [Tooltip("Color: White")]
        public bool ShowWallDetection = true;
        
        [Tooltip("Color: Red")]
        public bool ShowLedgeDetection = true;

        private void OnValidate() {
            if (PlayerLayer.value <= 1) Debug.LogWarning("Please assign a Player Layer that matches the one given to your Player", this);
            if (AllowWalls && ClimbableLayer.value <= 1) Debug.LogWarning("Please assign a Climbable Layer that matches your Climbable colliders", this);
            if (AllowLadders && LadderLayer.value <= 1) Debug.LogWarning("Please assign a Ladder Layer that matches your Ladders", this);
        }
#endif
    }
}