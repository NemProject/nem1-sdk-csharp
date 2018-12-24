using System;
using io.nem1.sdk.Model.Transactions;

namespace Tests.Model.Transactions
{
    public class FakeDeadline : Deadline
    {
        public FakeDeadline(TimeSpan time) : base(time)
        {
                
        }

        public static FakeDeadline Create()
        {
            return new FakeDeadline(new TimeSpan(1));
        }

        //public override int GetInstant()
        //{
        //    return 1;
        //}
    }
}
