using System;
using System.Collections.Generic;
using System.Text;
using XiaoFeng.IO;
/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-21 15:31:44                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Model
{
    /// <summary>
    /// 寄存器模型
    /// </summary>
    /// <remarks>一个寄存器占两个字节</remarks>
    public class RegisterModel
    {
        #region 构造器
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        public RegisterModel()
        {

        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        public RegisterModel(ushort address, ushort value)
        {
            this.Address = address;
            this.Value = value;
        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        public RegisterModel(ushort address, byte[] value)
        {
            this.Address = address;
            this.Value = value.ToUInt16();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 地址
        /// </summary>
        public ushort Address { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        public ushort Value { get; set; }
        /// <summary>
        /// 十六进制数据
        /// </summary>
        public string HexValue => this.Value.GetBytes(false).ToHexString();
        /// <summary>
        /// 字节数据
        /// </summary>
        public byte[] Data => this.Value.GetBytes(false);
        #endregion

        #region 方法

        #endregion
    }
}