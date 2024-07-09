using UnityEngine;

namespace PlayerController.Data
{
    [CreateAssetMenu(fileName = "New PlayerMovementData", menuName = "Player/Data/Movement")]
    public class PlayerMovementData : ScriptableObject
    {
        #region GRAVITY
        [Header("GRAVITY")]
        [Tooltip("Force needed for the desired Jump Height and Jump Time To Apex")]
        [ReadOnly] public float gravityStrength;
        [Tooltip("Strength of the player's gravity as a multiplier of gravity (value of rigidbody2d.gravityScale)")]
        [ReadOnly] public float gravityScale;
        [Space(5)]
        [Tooltip("Multiplier to the player's gravityScale when falling")]
        public float fallGravityMult;
        [Tooltip("Maximum fall speed of the player when falling")]
        public float maxFallSpeed;
        [Space(5)]
        [Tooltip("Larger multiplier to the player's gravityScale when falling anf downwards input is pressed")]
        public float fastFallGravityMult;
        [Tooltip("Maximum fall speed of the player when falling when performing a faster fall")]
        public float maxFastFallSpeed;
        #endregion
        
        #region RUN
        [Space(20), Header("RUN")]
        [Tooltip("Target speed we want the player to reach")]
        public float runMaxSpeed;
        [Tooltip("The speed at which the player accelerates to max speed, can be set to runMaxSpeed to instant acceleration down to 0 for none at all")]
        public float runAcceleration;
        [Tooltip("The speed at which the player deceleration to max speed, can be set to runMaxSpeed to instant deceleration down to 0 for none at all")]
        public float runDeceleration;
        [Space(5)]
        [Tooltip("Actual force (multiplied with speedDiff) applied to the player to accelerate")]
        [ReadOnly] public float runAccelAmount;
        [Tooltip("Actual force (multiplied with speedDiff) applied to the player to decelerate")]
        [ReadOnly] public float runDecelAmount;
        [Space(5)]
        [Tooltip("Multiplier applied to acceleration rate when airborne")]
        [Range(0, 1)] public float accelInAirMult;
        [Tooltip("Multiplier applied to deceleration rate when airborne")]
        [Range(0, 1)] public float decelInAirMult;
        #endregion
        
        #region JUMP
        [Space(20), Header("JUMP")]
        [Tooltip("Height of the player's jump")]
        public float jumpHeight;
        [Tooltip("Time between applying the jump force and reaching the desired jump height")]
        public float jumpTimeToApex;
        [Tooltip("The actual force applied to the player when jumping")]
        [ReadOnly] public float jumpForce;
        [Space(5)]
        public int additionalJumps;

        [Header("BOTH JUMPS")]
        [Tooltip("Multiplier to increase gravity if the player releases the jump button while jumping")]
        public float jumpCutGravity;
        [Tooltip("Reduces gravity while close to the apex of the jump")]
        [Range(0, 1)] public float jumpHangGravityMult;
        [Tooltip("Speeds (close to 0) where the player will experience extra 'jump hang'. The player's velocity.y is closest to 0 at the jump's apex ")]
        public float jumpHangTimeThreshold;
        public float jumpHangAcceleration;
        public float jumpHangMaxSpeedMult;

        [Header("WALL JUMP")]
        [Tooltip("Force applied to the player when wall jumping")]
        public Vector2 wallJumpForce;
        [Space(5)]
        [Tooltip("Reduces the effect of player's movement while wall jumping")]
        [Range(0, 1)] public float wallJumpRunLerp;
        [Tooltip("Time after wall jumping the player's movement is slowed for")]
        [Range(0, 1)] public float wallJumpTime;
        #endregion
        
        #region SLIDE
        [Space(20), Header("SLIDE")]
        [Tooltip("Target speed we want the player to reach while sliding")]
        public float slideSpeed;
        [Tooltip("The speed at which the player accelerates to max speed, can be set to runMaxSpeed to instant acceleration down to 0 for none at all")]
        public float slideAccel;
        [Range(0f, 0.1f)] public float wallSlideReleaseTime;
        #endregion
        
        #region DASH
        [Space(20), Header("DASH")]
        public float dashSpeed;
        public float dashTime;
        public float dashRefillTime; // after dashing
        #endregion
        
        #region WALL IMPULSE
        [Space(20), Header("WALL IMPULSE")]
        public float wallImpulseVelocity;
        public float wallImpulseChargeTime;
        #endregion
        
        #region ATTACK
        [Space(20), Header("ATTACK MOVEMENT")]
        public float recoilSpeed;
        public float recoilDuration;
        public float pogoForce;
        [Space(5)]
        public Vector2 knockBackVelocity;
        public float knockBackDuration;
        [Space(5)]
        [Range(0, 1)] public float kbAccelInArMult;
        [Range(0, 1)] public float kbDecelInArMult;
        #endregion
        
        #region ASSISTS
        [Space(20), Header("ASSISTS")]
        [Range(0.01f, 0.5f)] public float coyoteTime;
        [Range(0.01f, 0.5f)] public float jumpInputBufferTime;
        [Range(0.01f, 0.5f)] public float dashInputBufferTime;
        #endregion
        
        private void OnValidate()
        {
            gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
            gravityScale = gravityStrength / Physics2D.gravity.y;
            
            jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

            runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
            runDeceleration = Mathf.Clamp(runDeceleration, 0.01f, runMaxSpeed);

            runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
            runDecelAmount = (50 * runDeceleration) / runMaxSpeed;
        }
    }
}
