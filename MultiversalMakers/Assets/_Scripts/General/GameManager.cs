using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace MultiversalMakers
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton

        private void Awake()
        {
            // Not a DontDestroyOnLoad, so we can assign scene specific things in the inspector
            if (Instance == null)
                Instance = this;

            else
                Destroy(gameObject);

            _input = GetComponent<PlayerInput>();
        }

        public static GameManager Instance;

        #endregion

        #region Game State

        public GameStates CurrentGameState { get => currentGameState; }
        private GameStates currentGameState = GameStates.Play;


        [Title("Game State")]
        [SerializeField] private UnityEvent playEvent;
        [SerializeField] private UnityEvent pausedEvent;

        public void TogglePausedMenu()
        {
            SetGameState(
                (currentGameState == GameStates.Play) ?
                GameStates.Paused :
                GameStates.Play);

        }

        private void SetGameState(GameStates state)
        {
            currentGameState = state;

            if (state == GameStates.Play)
            {
                playEvent?.Invoke();
                Time.timeScale = 1f; // DO THIS BETTER
            }


            if (state == GameStates.Paused)
            {
                pausedEvent?.Invoke();
                Time.timeScale = 0f;
            }

        }

        public enum GameStates
        {
            Play,
            Paused
        }

        #endregion

        #region Input

        private PlayerInput _input;

        private FrameInput _frameInput;

        protected virtual void Update()
        {
            GatherInputs();
        }

        private void GatherInputs()
        {
            _frameInput = _input.FrameInput;

            if(_frameInput.SettingsDown)
            {
                TogglePausedMenu();
            }
        }


        #endregion

    }
}
