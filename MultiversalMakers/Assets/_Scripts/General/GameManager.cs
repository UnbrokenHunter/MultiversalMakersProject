using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        }

        public static GameManager Instance;

        #endregion

        #region Game State

        public GameStates CurrentGameState { get => currentGameState; }
        private GameStates currentGameState;


        [Title("Game State")]
        [SerializeField] private UnityEvent playEvent;
        [SerializeField] private UnityEvent pausedEvent;


        public void SetGameStateToPlay() => SetGameState(GameStates.Play);

        public void SetStateToPaused() => SetGameState(GameStates.Paused);


        private void SetGameState(GameStates state)
        {
            currentGameState = state;

            if (state == GameStates.Play)
                playEvent?.Invoke();


            if (state == GameStates.Paused)
                pausedEvent?.Invoke();

        }

        public enum GameStates
        {
            Play,
            Paused
        }

        #endregion



    }
}
