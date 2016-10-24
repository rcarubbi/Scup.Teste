using System.ComponentModel.DataAnnotations;

namespace ScupTel.Domain
{
    public class CallQuote
    {
        public CallQuote(Call quotedCall)
        {
            QuotedCall = quotedCall;
            QuoteAmount = Calculate();
        }

        public CallQuote(Call quotedCall, DiscountPlan hiredPlan)
        {
            QuotedCall = quotedCall;
            HiredPlan = hiredPlan;
            QuoteAmount = HiredPlan.Calculate(quotedCall);
        }

        private decimal? Calculate()
        {
            return QuotedCall.Minutes * QuotedCall.AppliedCharge?.MinutePrice;
        }

        public decimal? QuoteAmount { get; private set; }

        public DiscountPlan HiredPlan { get; private set; }

        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        public Call QuotedCall { get; private set; }
    }
}