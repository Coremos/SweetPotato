namespace UnitSystem.PickupSystem
{
    public class Seed : Pickupable
    {
        protected override void Awake()
        {
            base.Awake();
        }

        public override void Pickup()
        {
            GameManager.Instance.Player.AddSeed();
            gameObject.SetActive(false);
            Unregist();
        }
    }
}