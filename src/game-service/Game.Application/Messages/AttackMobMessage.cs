namespace Game.Messages
{
    public struct AttackMobMessage
    {
        public int MobId { get; set; }

        public float Distance { get; set; }

        public int DamageAmount { get; set; }
    }
}