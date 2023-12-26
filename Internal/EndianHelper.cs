using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-23 10:57:48                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Internal
{
    /// <summary>
    /// EndianHelper 类说明
    /// </summary>
    public static class EndianHelper
    {
        #region 构造器

        #endregion

        #region 属性

        #endregion

        #region 方法
        /// <summary>
        /// 格式化字节
        /// </summary>
        /// <param name="value">数据</param>
        /// <param name="endian">字节序</param>
        /// <param name="isReverse">是否反转</param>
        /// <returns></returns>
        public static byte[] ByteFormatting(this byte[] value,EndianType endian,Boolean isReverse = false)
        {
            if (value == null || value.Length == 0) return Array.Empty<byte>();
            if (isReverse) Array.Reverse(value);
            if (value.Length == 2)
            {
                switch (endian)
                {
                    case EndianType.BIG:
                    case EndianType.BIG_BYTESWAP:
                        return value;
                    case EndianType.LITTLE:
                    case EndianType.LITTLE_BYTESWAP:
                        var _buffer = new byte[2];
                        _buffer[0] = value[1];
                        _buffer[1] = value[0];
                        return _buffer;
                }
            }
            if (value.Length % 4 > 0 || endian == EndianType.BIG) return value;
            var buffer = new byte[value.Length];
            for(var i = 0; i < value.Length; i += 4)
            {
                switch (endian)
                {
                    case EndianType.BIG_BYTESWAP://CDAB
                        buffer[0 + i] = value[2 + i];
                        buffer[1 + i] = value[3 + i];
                        buffer[2 + i] = value[0 + i];
                        buffer[3 + i] = value[1 + i];
                        return buffer;
                    case EndianType.LITTLE://DCBA
                        buffer[0 + i] = value[3 + i];
                        buffer[1 + i] = value[2 + i];
                        buffer[2 + i] = value[1 + i];
                        buffer[3 + i] = value[0 + i];
                        return buffer;
                    case EndianType.LITTLE_BYTESWAP://BADC
                        buffer[0 + i] = value[1 + i];
                        buffer[1 + i] = value[0 + i];
                        buffer[2 + i] = value[3 + i];
                        buffer[3 + i] = value[2 + i];
                        return buffer;
                }
            }
            return value;
        }
        #endregion
    }
}