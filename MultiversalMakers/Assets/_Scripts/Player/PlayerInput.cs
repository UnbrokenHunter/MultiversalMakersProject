using UnityEngine;
using UnityEngine.InputSystem;


namespace MultiversalMakers {
    public class PlayerInput : MonoBehaviour {
        public FrameInput FrameInput { get; private set; }

        private void Update() => FrameInput = Gather();

        private PlayerInputActions _actions;
        private InputAction _move, _jump, _attack, _settings;

        private void Awake() {
            _actions = new PlayerInputActions();
            _move = _actions.Player.Move;
            _jump = _actions.Player.Jump;
            _attack = _actions.Player.Attack;
            _settings = _actions.Player.Settings;
        }

        private void OnEnable() => _actions.Enable();

        private void OnDisable() => _actions.Disable();

        private FrameInput Gather() {
            return new FrameInput {
                JumpDown = _jump.WasPressedThisFrame(),
                JumpHeld = _jump.IsPressed(),
                AttackDown = _attack.WasPressedThisFrame(),
                Move = _move.ReadValue<Vector2>(),
                SettingsDown = _settings.WasPressedThisFrame(),
            };
        }

    }

    public struct FrameInput {
        public Vector2 Move;
        public bool JumpDown;
        public bool JumpHeld;
        public bool DashDown;
        public bool AttackDown;
        public bool SettingsDown;
    }
}