using System.Collections.Generic;
using System.Linq;
namespace ScupTel.Domain
{
    public class Call
    {

        public int Minutes { get; private set; }

        public Charge AppliedCharge { get; private set; }
        public IEnumerable<CallQuote> Quote(IEnumerable<DiscountPlan> availablePlans)
        {
            DiscountPlan currentPlan = null;
            DiscountPlan lastPlan = null;
            var freeMinutesAverageBetweenTwoPlans = 0;
            foreach (var plan in availablePlans.OrderBy(x => x.FreeMinutes))
            {
                currentPlan = plan;

                freeMinutesAverageBetweenTwoPlans = (currentPlan.FreeMinutes + (lastPlan?.FreeMinutes ?? 0)) / 2;
                
                if (this.Minutes < freeMinutesAverageBetweenTwoPlans)
                {
                    yield return new CallQuote(this, lastPlan ?? currentPlan);
                    break; 
                }
                else if (currentPlan == availablePlans.Last())
                {
                    yield return new CallQuote(this, currentPlan);
                    break;
                }
                lastPlan = plan;
            }

            yield return new CallQuote(this);
        }

        

        public Call(Charge appliedCharge, int minutes)
        {
            AppliedCharge = appliedCharge;
            Minutes = minutes;
        }
    }
}
