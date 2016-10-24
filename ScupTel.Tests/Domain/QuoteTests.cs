using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ScupTel.Domain;
using ScupTel.Domain.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace ScupTel.WebUI.Tests.Domain
{
    [TestClass]
    public class QuoteTests
    {
        private static Mock<ScupTel.DataAccess.DbContext> _dbContext;

        #region Initialize
        [ClassInitialize]
        public static void ConfigureContext(TestContext testContext)
        {
            Mock<IDbSet<DiscountPlan>> discountPlans = ConfigureDiscountPlansDbSet();
            Mock<IDbSet<Zone>> zones = ConfigureZonesDbSet();
            Mock<IDbSet<Charge>> charges = ConfigureChargesDbSet(zones);

            _dbContext = new Mock<ScupTel.DataAccess.DbContext>();

            _dbContext
                .Setup(x => x.DiscountPlans)
                .Returns(discountPlans.Object);


            _dbContext
                .Setup(x => x.Zones)
                .Returns(zones.Object);

            _dbContext
                .Setup(x => x.Charges)
                .Returns(charges.Object);



            _dbContext.Setup(x => x.Set<DiscountPlan>()).Returns(discountPlans.Object);
            _dbContext.Setup(x => x.Set<Zone>()).Returns(zones.Object);
            _dbContext.Setup(x => x.Set<Charge>()).Returns(charges.Object);

        }

        private static Mock<IDbSet<Charge>> ConfigureChargesDbSet(Mock<IDbSet<Zone>> zones)
        {
            var charges = new Charge[] {
                new Charge(zones.Object.Single(z => z.Code == 11),  zones.Object.Single(z => z.Code == 16), 1.9M),
                new Charge(zones.Object.Single(z => z.Code == 16),  zones.Object.Single(z => z.Code == 11), 2.9M),
                new Charge(zones.Object.Single(z => z.Code == 11),  zones.Object.Single(z => z.Code == 17), 1.7M),
                new Charge(zones.Object.Single(z => z.Code == 17),  zones.Object.Single(z => z.Code == 11), 2.7M),
                new Charge(zones.Object.Single(z => z.Code == 11),  zones.Object.Single(z => z.Code == 18), 0.9M),
                new Charge(zones.Object.Single(z => z.Code == 18),  zones.Object.Single(z => z.Code == 11), 1.9M)
            }.AsQueryable();

            var dbSetMock = new Mock<IDbSet<Charge>>();
            dbSetMock.Setup(m => m.Provider).Returns(charges.Provider);
            dbSetMock.Setup(m => m.Expression).Returns(charges.Expression);
            dbSetMock.Setup(m => m.ElementType).Returns(charges.ElementType);
            dbSetMock.Setup(m => m.GetEnumerator()).Returns(() => charges.GetEnumerator());
            return dbSetMock;
        }

        private static Mock<IDbSet<Zone>> ConfigureZonesDbSet()
        {
            var zones = new Zone[] {
                new Zone("São Paulo (011)", 11),
                new Zone("São Paulo (016)", 16),
                new Zone("São Paulo (017)", 17),
                new Zone("São Paulo (018)", 18)
            }.AsQueryable();

            var dbSetMock = new Mock<IDbSet<Zone>>();
            dbSetMock.Setup(m => m.Provider).Returns(zones.Provider);
            dbSetMock.Setup(m => m.Expression).Returns(zones.Expression);
            dbSetMock.Setup(m => m.ElementType).Returns(zones.ElementType);
            dbSetMock.Setup(m => m.GetEnumerator()).Returns(() => zones.GetEnumerator());
            return dbSetMock;
        }

        private static Mock<IDbSet<DiscountPlan>> ConfigureDiscountPlansDbSet()
        {
            var availablePlans = new DiscountPlan[] {
                new DiscountPlan("Fale Mais 30", 30),
                new DiscountPlan("Fale Mais 60", 60),
                new DiscountPlan("Fale Mais 120", 120)
            }.AsQueryable();

            var dbSetMock = new Mock<IDbSet<DiscountPlan>>();
            dbSetMock.Setup(m => m.Provider).Returns(availablePlans.Provider);
            dbSetMock.Setup(m => m.Expression).Returns(availablePlans.Expression);
            dbSetMock.Setup(m => m.ElementType).Returns(availablePlans.ElementType);
            dbSetMock.Setup(m => m.GetEnumerator()).Returns(() => availablePlans.GetEnumerator());

            return dbSetMock;
        }
        #endregion

        #region Scenarios

        [TestMethod]
        public void Minutes_are_less_than_all_plans()
        {
            var call = GivenCallWithMinutesLessThanAllPlans();
            var callQuotes = WhenQuoted(call);
            ThenFirstPlanShouldBeOfferedAndShoudBeEqualsZero(callQuotes);
        }

        [TestMethod]
        public void Quote_Call_with_35_minutes()
        {
            var call = GivenCallWithMinutes(35);
            var callQuotes = WhenQuoted(call);
            Then30MinutesPlanShouldBeOfferedAndShoudBeGreaterThanZero(callQuotes);
        }


        [TestMethod]
        public void Quote_Call_with_55_minutes()
        {
            var call = GivenCallWithMinutes(55);
            var callQuotes = WhenQuoted(call);
            Then60MinutesPlanShouldBeOfferedAndShoudBeEqualsZero(callQuotes);
        }

        [TestMethod]
        public void Quote_Call_with_65_minutes()
        {
            var call = GivenCallWithMinutes(65);
            var callQuotes = WhenQuoted(call);
            Then60MinutesPlanShouldBeOfferedAndShoudBeGreaterThanZero(callQuotes);
        }

      

        [TestMethod]
        public void Quote_Call_with_95_minutes()
        {
            var call = GivenCallWithMinutes(95);
            var callQuotes = WhenQuoted(call);
            Then120MinutesPlanShouldBeOfferedAndShoudBeEqualsZero(callQuotes);
        }

       
        [TestMethod]
        public void Quote_Call_with_125_minutes()
        {
            var call = GivenCallWithMinutes(125);
            var callQuotes = WhenQuoted(call);
            Then120MinutesPlanShouldBeOfferedAndShoudBeGreaterThanZero(callQuotes);
        }

      

        [TestMethod]
        public void Call_with_no_valid_charge()
        {
            var call = GivenCallWithInvalidCharge();
            var callQuotes = WhenQuoted(call);
            ThenAllQuotesShouldBeNull(callQuotes);
        }

        #endregion

        #region Given
        private Call GivenCallWithInvalidCharge()
        {
            var charge = new ChargeRepository(_dbContext.Object)
             .FindByZones(99, 20);

            return new Call(charge, 40);
        }

        private Call GivenCallWithMinutes(int minutes)
        {
            var charge = new ChargeRepository(_dbContext.Object)
              .FindByZones(11, 16);

            return new Call(charge, minutes);
        }
 
        private Call GivenCallWithMinutesLessThanAllPlans()
        {
            var charge = new ChargeRepository(_dbContext.Object)
                .FindByZones(11, 16);

            return new Call(charge, 10);
        }

      
        #endregion

        #region When
        private IEnumerable<CallQuote> WhenQuoted(Call call)
        {
            return call.Quote(_dbContext.Object.DiscountPlans).ToList();
        }

        #endregion

        #region Then
        private void ThenAllQuotesShouldBeNull(IEnumerable<CallQuote> callQuotes)
        {
            Assert.IsTrue(callQuotes.ToList().TrueForAll(x => x.QuoteAmount == null));
        }

        private void ThenFirstPlanShouldBeOfferedAndShoudBeEqualsZero(IEnumerable<CallQuote> callQuotes)
        {
            var planOption = callQuotes.First();
            var noPlanOption = callQuotes.Last();
            Assert.IsTrue(planOption.HiredPlan.FreeMinutes == 30
                && planOption.QuoteAmount == 0
                && noPlanOption.HiredPlan == null
                && noPlanOption.QuoteAmount > 0);
        }

        private void Then30MinutesPlanShouldBeOfferedAndShoudBeGreaterThanZero(IEnumerable<CallQuote> callQuotes)
        {
            var planOption = callQuotes.First();
            var noPlanOption = callQuotes.Last();
            Assert.IsTrue(planOption.HiredPlan.FreeMinutes == 30
                && planOption.QuoteAmount > 0
                && noPlanOption.HiredPlan == null
                && noPlanOption.QuoteAmount > 0);
        }

        private void Then60MinutesPlanShouldBeOfferedAndShoudBeEqualsZero(IEnumerable<CallQuote> callQuotes)
        {
            var planOption = callQuotes.First();
            var noPlanOption = callQuotes.Last();
            Assert.IsTrue(planOption.HiredPlan.FreeMinutes == 60
                && planOption.QuoteAmount == 0
                && noPlanOption.HiredPlan == null
                && noPlanOption.QuoteAmount > 0);
        }

        private void Then60MinutesPlanShouldBeOfferedAndShoudBeGreaterThanZero(IEnumerable<CallQuote> callQuotes)
        {
            var planOption = callQuotes.First();
            var noPlanOption = callQuotes.Last();
            Assert.IsTrue(planOption.HiredPlan.FreeMinutes == 60
                && planOption.QuoteAmount > 0
                && noPlanOption.HiredPlan == null
                && noPlanOption.QuoteAmount > 0);
        }
        private void Then120MinutesPlanShouldBeOfferedAndShoudBeEqualsZero(IEnumerable<CallQuote> callQuotes)
        {
            var planOption = callQuotes.First();
            var noPlanOption = callQuotes.Last();
            Assert.IsTrue(planOption.HiredPlan.FreeMinutes == 120
                && planOption.QuoteAmount == 0
                && noPlanOption.HiredPlan == null
                && noPlanOption.QuoteAmount > 0);
        }
        private void Then120MinutesPlanShouldBeOfferedAndShoudBeGreaterThanZero(IEnumerable<CallQuote> callQuotes)
        {
            var planOption = callQuotes.First();
            var noPlanOption = callQuotes.Last();
            Assert.IsTrue(planOption.HiredPlan.FreeMinutes == 120
                && planOption.QuoteAmount > 0
                && noPlanOption.HiredPlan == null
                && noPlanOption.QuoteAmount > 0);
        }
        #endregion
    }
}
