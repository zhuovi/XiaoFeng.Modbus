﻿using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-23 16:15:25                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// 写入
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class WriteAttribute : Attribute
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public WriteAttribute()
        {

        }
        #endregion

        #region 属性

        #endregion

        #region 方法

        #endregion
    }
}