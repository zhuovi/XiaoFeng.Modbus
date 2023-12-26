using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-13 15:01:55                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// 协议类型
    /// </summary>
    public enum ProtocolType
    {
        /// <summary>
        /// TCP
        /// </summary>
        TCP = 0,
        /// <summary>
        /// UDP
        /// </summary>
        UDP = 1,
        /// <summary>
        /// RTU
        /// </summary>
        RTU = 2
    }
}