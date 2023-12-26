using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XiaoFeng.Modbus.Algorithm;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-19 11:28:34                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Internal
{
    /// <summary>
    /// 校验类
    /// </summary>
    public static class Verification
    {
        #region 方法
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="bytes">字节</param>
        /// <param name="offset">开始位置</param>
        /// <param name="length">长度</param>
        /// <param name="verificationType">校验类型</param>
        /// <param name="endian">字节序类型</param>
        /// <param name="func">校验函数</param>
        /// <returns></returns>
        public static byte[] Encode(this byte[] bytes, int offset, int length, VerificationType verificationType, EndianType endian = EndianType.LITTLE, Func<byte[], byte[]> func = null)
        {
            if (bytes == null || bytes.Length == 0) return Array.Empty<byte>();
            if (func != null)
            {
                return bytes.Concat(func(bytes)).ToArray();
            }
            switch (verificationType)
            {
                case VerificationType.None:
                    return bytes;
                case VerificationType.ModbusCRC16:
                    var bs = CRC16.GetCRC16(bytes).ByteFormatting(endian);
                    return bytes.Concat(bs).ToArray();
                case VerificationType.ModbusCRC32:
                case VerificationType.CCITT_CRC16:
                case VerificationType.ADD8:
                case VerificationType.OADD8:
                case VerificationType.ADD16:
                case VerificationType.XOR8:

                    return bytes;
            }
            return bytes;
        }
        #endregion
    }
}