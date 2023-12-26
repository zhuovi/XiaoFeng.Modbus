using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-21 15:30:30                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Model
{
    /// <summary>
    /// 线圈模型
    /// </summary>
    /// <remarks>一个线圈占一位，也就是一个字节有8个线圈</remarks>
    public class CoilModel
    {
        #region 构造器
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        public CoilModel()
        {

        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        public CoilModel(ushort address, byte value)
        {
            this.Address = address;
            this.Value = value;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 线圈地址
        /// </summary>
        public ushort Address { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public byte Value { get; set; }
        #endregion

        #region 方法

        #endregion
    }
}