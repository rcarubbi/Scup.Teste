using System;
using System.ComponentModel;

namespace ScupTel.Domain
{
    public class DiscountPlan
    {
        public DiscountPlan()
        {

        }
        public DiscountPlan(string name, int freeMinutes)
        {
            Name = name;
            FreeMinutes = freeMinutes;
        }

        public int Id { get; private set; }

       
        public string Name { get; private set; }
        public int FreeMinutes { get; private set; }

        internal decimal? Calculate(Call quotedCall)
        {
            int billedMinutes = CalculateMinutes(quotedCall);
            return (billedMinutes * quotedCall.AppliedCharge?.MinutePrice) * 1.1M;
        }

        private int CalculateMinutes(Call quotedCall)
        {
            int billedMinutes = quotedCall.Minutes;
            billedMinutes -= FreeMinutes;
            billedMinutes = billedMinutes < 0 ? 0 : billedMinutes;
            return billedMinutes;
        }
    }
}