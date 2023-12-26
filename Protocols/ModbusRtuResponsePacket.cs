using System;
using System.Collections.Generic;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-25 11:15:19                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Protocols
{
    /// <summary>
    /// ModbusRtuResponse 类说明
    /// </summary>
    public class ModbusRtuResponsePacket : ModbusResponsePacket
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusRtuResponsePacket() : base() { }
        /// <summary>
        /// 初始化一个实例
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="requestPacket">请求包</param>
        public ModbusRtuResponsePacket(byte[] data, ModbusRtuResponsePacket requestPacket) : base(data, requestPacket) { }
        #endregion

        #region 属性

        #endregion

        #region 方法
        ///<inheritdoc/>
        public override void WriterStream()
        {

        }
        ///<inheritdoc/>
        public override Boolean ReaderStream()
        {
            return true;
        }
        #endregion
    }
}