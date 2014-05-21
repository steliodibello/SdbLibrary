using System;
using System.Threading;

namespace SdbBackbone.Utils
{
    public class Retry
    {
        public static bool RetryUntilTrue(Func<bool> method, int retryNumber)
        {
            int counter = 0;
            bool result = false;
            while (!result && (counter <= retryNumber))
            {
                result = method();
                counter++;

                if (!result)
                {
                    Thread.Sleep(1000);
                }
            }
            return result;
        }
    }
}
