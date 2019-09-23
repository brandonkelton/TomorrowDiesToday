using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace TomorrowDiesToday.DB
{
    public class TDTCredentials : AWSCredentials
    {
        public override ImmutableCredentials GetCredentials()
        {
            var credentials = new ImmutableCredentials("AKIAJFVCBNZ2YA42PAHA", "ujCsJaRq44FvOMZFl55NIfAdHeVa0DO19GHUd9Lf", null);
            return credentials;
        }
    }
}
