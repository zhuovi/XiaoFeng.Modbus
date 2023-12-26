using System;
using System.Collections.Generic;
using System.Text;
using XiaoFeng.IO;
using XiaoFeng.Modbus.Internal;
using XiaoFeng;
/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-23 14:57:18                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Protocols
{
    /// <summary>
    /// ModbusRtuPacket 类说明
    /// </summary>
    public class ModbusRtuRequestPacket : ModbusRequestPacket
    {
        #region 构造器
        /// <summary>
        /// 初始化一个实例
        /// </summary>
        public ModbusRtuRequestPacket() : base() { this.ProtocolType = ProtocolType.RTU; }
        /// <summary>
        /// 初始化一个实例
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="requestPacket">请求包</param>
        public ModbusRtuRequestPacket(byte[] data, ModbusRtuRequestPacket requestPacket) : base(data, requestPacket) { this.ProtocolType = ProtocolType.RTU; }
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