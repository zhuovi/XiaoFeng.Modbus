using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-20 08:53:22                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// 传输模式
    /// </summary>
    public enum ModbusMode
    {
        /// <summary>
        /// RTU
        /// </summary>
        RTU = 0,
        /// <summary>
        /// ASCII
        /// </summary>
        ASCII = 1
    }
}