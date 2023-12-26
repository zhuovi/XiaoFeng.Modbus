using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-20 09:17:51                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Internal
{
    /// <summary>
    /// Modbus 帮助类
    /// </summary>
    public static class ModbusHelper
    {
        #region 属性
        /// <summary>
        /// 地址最小值
        /// </summary>
        public const ushort ADDRESS_MIN = 0x0000;
        /// <summary>
        /// 地址最大值
        /// </summary>
        public const ushort ADDRESS_MAX = 0xFFFF;
        /// <summary>
        /// 线圈最小数量
        /// </summary>
        public const ushort COIL_MIN_COUNT = 0x0001;
        /// <summary>
        /// 线圈最大数量
        /// </summary>
        public const ushort COIL_MAX_COUNT = 0x07D0;
        /// <summary>
        /// 线圈输出量最小值
        /// </summary>
        public const ushort COILS_MIN_COUNT = 0x0001;
        /// <summary>
        /// 线圈输出量最大值
        /// </summary>
        public const ushort COILS_MAX_COUNT = 0x07B0;
        /// <summary>
        /// 寄存器最小数量
        /// </summary>
        public const ushort REGISTER_MIN_COUNT = 0x0001;
        /// <summary>
        /// 寄存器最大数量
        /// </summary>
        public const ushort REGISTER_MAX_COUNT = 0x007B;
        /// <summary>
        /// 寄存器最小值
        /// </summary>
        public const ushort REGISTER_MIN_VALUE = 0x0000;
        /// <summary>
        /// 寄存器最大值
        /// </summary>
        public const ushort REGISTER_MAX_VALUE = 0xFFFF;
        /// <summary>
        /// 文件最小字节数
        /// </summary>
        public const ushort FILE_SIZE_MIN = 0x0007;
        /// <summary>
        /// 文件最大字节数
        /// </summary>
        public const ushort FILE_SIZE_MAX = 0x00F5;
        /// <summary>
        /// 文件号最小值
        /// </summary>
        public const ushort FILE_NUMBER_MIN = 0x0000;
        /// <summary>
        /// 文件号最大值
        /// </summary>
        public const ushort FILE_NUMBER_MAX = 0xFFFF;
        /// <summary>
        /// 文件记录号最小值
        /// </summary>
        public const ushort FILE_RECORD_NUMBER_MIN = 0x0000;
        /// <summary>
        /// 文件记录号最大值
        /// </summary>
        public const ushort FILE_RECORD_NUMBER_MAX = 0x270F;
        /// <summary>
        /// 值
        /// </summary>
        private static readonly ushort[] CRC_TA = new ushort[] { 0x0000, 0xCC01, 0xD801, 0x1400, 0xF001, 0x3C00, 0x2800, 0xE401, 0xA001, 0x6C00, 0x7800, 0xB401, 0x5000, 0x9C01, 0x8801, 0x4400 };
        #endregion

        #region 方法
        /// <summary>Crc校验</summary>
        /// <param name="data">数据</param>
        /// <param name="offset">偏移</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static ushort CRC(byte[] data, int offset, int length = -1)
        {
            if (data == null || data.Length < 1) return 0;

            if (length <= 0) length = data.Length - offset;

            ushort u = 0xFFFF;
            for (var i = offset; i < length; i++)
            {
                var b = data[i];
                u = (ushort)(CRC_TA[(b ^ u) & 15] ^ (u >> 4));
                u = (ushort)(CRC_TA[((b >> 4) ^ u) & 15] ^ (u >> 4));
            }

            return u;
        }

        /// <summary>Crc校验</summary>
        /// <param name="data">数据流</param>
        /// <param name="length">数量</param>
        /// <returns></returns>
        public static ushort CRC(Stream data, int length = -1)
        {
            if (data == null || data.Length < 1) return 0;

            ushort u = 0xFFFF;

            for (var i = 0; length < 0 || i < length; i++)
            {
                var b = data.ReadByte();
                if (b < 0) break;

                u = (ushort)(CRC_TA[(b ^ u) & 15] ^ (u >> 4));
                u = (ushort)(CRC_TA[((b >> 4) ^ u) & 15] ^ (u >> 4));
            }

            return u;
        }
        #endregion

        #region LRC
        /// <summary>LRC校验</summary>
        /// <param name="data">数据</param>
        /// <param name="offset">偏移</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        public static byte LRC(byte[] data, int offset, int length = -1)
        {
            if (data == null || data.Length < 1) return 0;
            if (length <= 0) length = data.Length - offset;
            byte rs = 0;
            for (var i = 0; i < length; i++)
            {
                rs += data[offset + i];
            }
            return (byte)((rs ^ 0xFF) + 1);
        }

        /// <summary>LRC校验</summary>
        /// <param name="stream">流</param>
        /// <returns></returns>
        public static byte LRC(Stream stream)
        {
            byte rs = 0;
            while (true)
            {
                var b = stream.ReadByte();
                if (b < 0) break;

                rs += (byte)b;
            }

            return (byte)((rs ^ 0xFF) + 1);
        }

        #endregion

        #region 获取随机校验头
        /// <summary>
        /// 获取随机校验头
        /// </summary>
        /// <returns></returns>
        public static byte[] GetCheckHeader()
        {
            return RandomHelper.GetRandomBytes(2);
        }
        #endregion
    }
}