using System;
using System.Collections.Generic;
using System.Text;
using XiaoFeng.IO;
using XiaoFeng.Modbus.Packets;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-25 11:10:44                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Protocols
{
    /// <summary>
    /// ModbusResponsePacket 类说明
    /// </summary>
    public abstract class ModbusResponsePacket : ModbusBasePacket
    {
        #region 构造器
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        public ModbusResponsePacket()
        {
            this.Writer = new MemoryBufferWriter();
            this.ModbusType = ModbusType.Response;
        }
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="requestPacket">请求包</param>
        public ModbusResponsePacket(byte[] data, ModbusResponsePacket requestPacket) : base(requestPacket)
        {
            this.Reader = new MemoryBufferReader(data);
            this.ModbusType = ModbusType.Request;
            this.UnPacket();
        }
        #endregion

        #region 属性

        #endregion

        #region 方法
        ///<inheritdoc/>
        public override byte[] ToArray()
        {
            return Array.Empty<byte>();
        }
        ///<inheritdoc/>
        public override void UnPacket()
        {
            if (this.Reader == null || this.Reader.Length == 0) return;
            if (!this.ReaderStream()) return;
            if (this.Reader.RemainingLength < 1) return;
            this.Host = (byte)this.Reader.ReadByte();

            if (this.Reader.RemainingLength < 1) return;
            var code = this.Reader.ReadByte();
            if (code > 0x80)
            {
                this.Code = (FunctionCodes)(code - 0x80);
                this.ErroCode = (ExceptionCodes)this.Reader.ReadByte();
            }
            else
            {
                this.Code = (FunctionCodes)code;
                this.ErroCode = 0;
                var key = $"{this.Host}-key";
                switch (this.Code)
                {
                    case FunctionCodes.ReadCoils://1

                        break;
                    case FunctionCodes.ReadInputDiscreteQuantity://2

                        break;
                    case FunctionCodes.ReadHoldingRegisters://3

                        break;
                    case FunctionCodes.ReadInputRegister://4

                        break;
                    case FunctionCodes.WriteCoil://5

                        break;
                    case FunctionCodes.WriteRegister://6

                        break;
                    case FunctionCodes.WriteCoils://15

                        break;
                    case FunctionCodes.WriteRegisters://16

                        break;
                }
                if (this.RequestType == RequestType.READ)
                {
                    if (this.Reader.RemainingLength < 1) return;
                    //数据长度
                    var length = this.Reader.ReadByte();
                    if (this.Code == FunctionCodes.ReadCoils || this.Code == FunctionCodes.ReadInputDiscreteQuantity)
                    {
                        length += length % 8 == 0 ? 0 : 1;
                    }
                    else if (this.Code == FunctionCodes.ReadHoldingRegisters || this.Code == FunctionCodes.ReadInputRegister)
                    {
                        length = 2 * length;
                    }
                    if (this.Reader.RemainingLength >= length)
                        this.Data = this.Reader.ReadBytes(length);
                }
                else
                {
                    if (this.Reader.RemainingLength < 2) return;
                    this.Address = this.Reader.ReadBytes(2).ToUInt16();

                    if (this.Reader.RemainingLength < 2) return;
                    this.Count = this.Reader.ReadBytes(2).ToUInt16();
                }
                if (!this.Reader.EndOfStream)
                    this.VerificationCode = this.Reader.ReadBytes();
            }
        }
        #endregion
    }
}