using System.Collections;
using System.Collections.Generic;

namespace DIY.Math
{
    public class Math
    {
        //求以A为底，2的对数(结果向下取整)
        public static int Log2Int(int _a)
        {
            int logResult = 0;
            while (_a > 1)
            {
                _a >>= 1;
                logResult++;
            }
            return logResult;
        }
    }

}
