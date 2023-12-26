using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-19 11:23:30                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// 校验类型
    /// </summary>
    public enum VerificationType
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,
        /// <summary>
        /// MODBUS CRC16
        /// </summary>
        ModbusCRC16 = 1,
        /// <summary>
        /// MODBUS CRC32
        /// </summary>
        ModbusCRC32 = 2,
        /// <summary>
        /// CCITT CRC16
        /// </summary>
        CCITT_CRC16 = 3,
        /// <summary>
        /// 0-ADD8
        /// </summary>
        OADD8 = 4,
        /// <summary>
        /// ADD8
        /// </summary>
        ADD8 = 5,
        /// <summary>
        /// ADD16
        /// </summary>
        ADD16 = 6,
        /// <summary>
        /// XOR8
        /// </summary>
        XOR8 = 7
    }
}