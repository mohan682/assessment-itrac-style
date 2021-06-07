using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadProject.Helpers
{
    public class RetryHelper
    {
        private int retryWaitInMin;
        private int maxRetries;

        public RetryHelper()
        {
            this.retryWaitInMin = 1000;
            this.maxRetries = 3;
        }
      
        public RetryHelper(int retryWaitInSeconds, int maxRetry)
        {
            this.retryWaitInMin = retryWaitInSeconds;
            this.maxRetries = maxRetry;
        }       

        public void Execute<T,TExp>(Func<T> currentAction, Action<T> onSuccess,Action<TExp> onFailure) where TExp:Exception
        {
            int retryCounter = 0;

            if(currentAction == null || onSuccess==null)
            {
                throw new ArgumentNullException("actions supplied are incorrect");
            }

            while (retryCounter < maxRetries)
            {
                try
                {
                    onSuccess.Invoke(currentAction.Invoke());
                    retryCounter = maxRetries;
                }
                catch (TExp exceptionToExecuteAction)
                {
                    ++retryCounter;

                    if(onFailure!=null)
                    {
                        onFailure.Invoke(exceptionToExecuteAction);
                    }              
                }               
            }
        }
    }
}
