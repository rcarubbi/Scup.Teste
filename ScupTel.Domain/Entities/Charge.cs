namespace ScupTel.Domain
{
    public class Charge
    {
        public Charge()
        {

        }
        public Charge(Zone from, Zone to, decimal minutePrice)
        {
            From = from;
            To = to;
            MinutePrice = minutePrice;
        }
        public int Id { get; private set; }

        public decimal MinutePrice { get; set; }

        public virtual Zone From { get; set; }

        public virtual Zone To { get; set; }



    }
}