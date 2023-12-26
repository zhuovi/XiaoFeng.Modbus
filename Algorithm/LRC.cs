using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-23 10:55:12                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Algorithm
{
    /// <summary>
    /// LRC 类说明
    /// </summary>
    public class LRC
    {
        #region 构造器
        #endregion

        #region 属性

        #endregion

        #region 方法
        /// <summary>
        /// 获取LRC校验码
        /// </summary>
        /// <param name="value">数据</param>
        /// <returns></returns>
        public static byte[] GetLRC(byte[] value)
        {
            if (value == null) return null;

            int sum = 0;
            for (int i = 0; i < value.Length; i++)
            {
                sum += value[i];
            }

            sum = sum % 256;
            sum = 256 - sum;

            byte[] LRC = new byte[] { (byte)sum };
            return value.Concat(LRC).ToArray();
        }
        #endregion
    }
}