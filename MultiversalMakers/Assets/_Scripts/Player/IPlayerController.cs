using System;
using UnityEngine;

namespace MultiversalMakers
{
    public interface IPlayerController
    {
        /// <summary>
        /// true = Landed. false = Left the Ground. float is Impact Speed
        /// </summary>
        public event Action<bool, float> GroundedChanged;

        public event Action<bool> WallGrabChanged;
        public event Action<bool> Jumped; // Is wall jump
        public event Action AirJumped;
        public event Action Attacked;

        public ScriptableStats PlayerStats { get; }
        public Vector2 Input { get; }
        public Vector2 Speed { get; }
        public Vector2 Velocity { get; }
        public Vector2 GroundNormal { get; }
        public int WallDirection { get; }
        public bool Crouching { get; }
        public bool ClimbingLadder { get; }
        public void ApplyVelocity(Vector2 vel, PlayerForce forceType);
        public void SetVelocity(Vector2 vel, PlayerForce velocityType);
    }

}
