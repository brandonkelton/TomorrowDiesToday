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
            var credentials = new ImmutableCredentials("YOUR-ACCESS-KEY", "YOUR-SECRET-ACCESS-KEY", null);
            return credentials;
        }
    }
}
