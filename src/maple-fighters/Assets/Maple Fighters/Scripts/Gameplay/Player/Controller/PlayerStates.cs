namespace Scripts.Gameplay.Player
{
    public enum PlayerStates
    {
        /// <summary>
        /// The idle state.
        /// </summary>
        Idle,

        /// <summary>
        /// The moving state.
        /// </summary>
        Moving,

        /// <summary>
        /// The jumping state.
        /// </summary>
        Jumping,

        /// <summary>
        /// The falling state.
        /// </summary>
        Falling,

        /// <summary>
        /// The rope climb state.
        /// </summary>
        Rope,

        /// <summary>
        /// The ladder climb state.
        /// </summary>
        Ladder,

        /// <summary>
        /// The primary attack state (e.g. Sword Attack).
        /// </summary>
        PrimaryAttack,

        /// <summary>
        /// The secondary attack state (e.g. Arrow Attack).
        /// </summary>
        SecondaryAttack
    }
}