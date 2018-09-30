using System;
using Common;

namespace Root
{
    public static class AssertHelper
    {
        public static void NoPass(bool condition, string errorMsg)
        {
            if (condition)
            {
                throw new InputException(errorMsg);
            }
        }

        public static void NoPass(bool condition, string errorMsg,int errorNo)
        {
            if (condition)
            {
                throw new InputException(errorMsg,errorNo);
            }
        }

        public static void NoPass(bool condition, ICustomerErrorType error)
        {
            if (condition)
            {
                throw new InputException(error);
            }
        }
    }
}