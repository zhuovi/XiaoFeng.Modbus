using System;
using System.Collections.Generic;
using System.Text;
using XiaoFeng.IO;

/****************************************************************
*  Copyright © (2024) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2024-01-02 11:18:22                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Protocols
{
    /// <summary>
    /// ModbusTcpResponsePacket 类说明
    /// </summary>
    public class ModbusTcpResponsePacket:ModbusResponsePacket
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusTcpResponsePacket()
        {

        }
        #endregion

        #region 属性
        /// <summary>
        /// 请求/响应事务 相当于校验，服务端原文返回
        /// </summary>
        public ushort TransactionFlags { get; set; } = 0;
        /// <summary>
        /// Modbus 协议
        /// </summary>
        public ushort ProtocolFlags { get; set; } = 0;
        #endregion

        #region 方法
        ///<inheritdoc/>
        public override void WriterStream()
        {
            this.Writer.WriteBytes(this.TransactionFlags.GetBytes(false));
            this.Writer.WriteBytes(this.ProtocolFlags.GetBytes(false));
            var length = (ushort)((ushort)6 + (ushort)(this.Data != null && this.Data.Length > 0 ? (this.Data.Length + 1) : 0));
            this.Writer.WriteBytes(length.GetBytes(false));
        }
        ///<inheritdoc/>
        public override bool ReaderStream()
        {
            if (this.Reader.RemainingLength < 2) return false;
            this.TransactionFlags = this.Reader.ReadBytes(2).ToUInt16(false);

            if (this.Reader.RemainingLength < 2) return false;
            this.ProtocolFlags = this.Reader.ReadBytes(2).ToUInt16(false);

            if (this.Reader.RemainingLength < 2) return false;
            this.Length = this.Reader.ReadBytes(2).ToUInt16(false);
            return true;
        }
        #endregion
    }
}