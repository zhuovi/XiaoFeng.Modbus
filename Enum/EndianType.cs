using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-22 14:53:14                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// 字节序类型
    /// </summary>
    public enum EndianType
    {
        /// <summary>
        /// 大端序 ABCD
        /// </summary>
        BIG = 0,
        /// <summary>
        /// 小端序 DCBA
        /// </summary>
        LITTLE = 1,
        /// <summary>
        /// 中端序 CDAB, Honeywell 316 风格
        /// </summary>
        BIG_BYTESWAP = 2,
        /// <summary>
        /// 中端序 BADC, PDP-11 风格 
        /// </summary>
        LITTLE_BYTESWAP = 3
    }
}