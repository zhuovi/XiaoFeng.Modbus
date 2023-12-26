using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-21 17:07:09                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// Modbus 类型
    /// </summary>
    public enum ModbusType
    {
        /// <summary>
        /// 请求
        /// </summary>
        [Description("请求")]
        Request = 0,
        /// <summary>
        /// 响应
        /// </summary>
        [Description("响应")] 
        Response = 1
    }
}