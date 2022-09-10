namespace Game.Messages
{
    public struct AttackMobMessage
    {
        public int MobId { get; set; }

        public int DamageAmount { get; set; }
    }
}