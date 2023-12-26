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
*  Create Time : 2023-12-21 17:04:59                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Protocols
{
    /// <summary>
    /// ModbusTcpPacket 类说明
    /// </summary>
    public class ModbusTcpRequestPacket : ModbusRequestPacket
    {
        #region 构造器
        /// <summary>
        /// 初始化一个实例
        /// </summary>
        public ModbusTcpRequestPacket() : base() { this.ProtocolType = ProtocolType.TCP; }
        /// <summary>
        /// 初始化一个实例
        /// </summary>
        /// <param name="bytes">数字</param>
        /// <param name="requestPacket">请求包</param>
        public ModbusTcpRequestPacket(byte[] bytes, ModbusTcpRequestPacket requestPacket) : base(bytes, requestPacket) { this.ProtocolType = ProtocolType.TCP; }
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
            this.Writer.WriteBytes(this.TransactionFlags.GetBytes());
            this.Writer.WriteBytes(this.ProtocolFlags.GetBytes());
            this.Writer.WriteBytes((6u + (ushort)(this.Data != null && this.Data.Length > 0 ? (this.Data.Length + 1) : 0)).GetBytes());
        }
        ///<inheritdoc/>
        public override bool ReaderStream()
        {
            if (this.Reader.RemainingLength < 2) return false;
            this.TransactionFlags = this.Reader.ReadBytes(2).ToUInt16();

            if (this.Reader.RemainingLength < 2) return false;
            this.ProtocolFlags = this.Reader.ReadBytes(2).ToUInt16();

            if (this.Reader.RemainingLength < 2) return false;
            this.Length = this.Reader.ReadBytes(2).ToUInt16();
            return true;
        }
        /// <summary>
        /// 重写
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"RequestType:{this.RequestType.GetDescription()} ModbusType:{this.ModbusType.GetDescription()} Address:{this.Address} FunctionCode:{this.Code.GetDescription()}[{this.Code}] Host:{this.Host}";
        }
        #endregion
    }
}