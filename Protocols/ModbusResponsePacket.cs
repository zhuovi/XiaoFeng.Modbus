using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using XiaoFeng.IO;
using XiaoFeng.Modbus.Internal;
using XiaoFeng.Modbus.Packets;
using XiaoFeng.Modbus.Server;

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
        /// <summary>
        /// 初始化一个新实例
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="serialPortServerData">服务端数据</param>
        public ModbusResponsePacket(byte[] data,SerialPortServerData serialPortServerData)
        {
            this.Reader = new MemoryBufferReader(data);
            this.ModbusType= ModbusType.Request;
            this.ServerData = serialPortServerData;
            this.UnPacket();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 服务端数据
        /// </summary>
        public SerialPortServerData ServerData { get; set; }
        #endregion

        #region 方法
        ///<inheritdoc/>
        public override byte[] ToArray()
        {
            if (this.Writer == null) this.Writer = new MemoryBufferWriter();
            if (this.ServerData == null) return this.WriteError();
            this.Writer.WriteByte(this.Host);
            this.Writer.WriteByte((byte)this.Code);

            if(this.RequestType== RequestType.READ)
            {
                if (this.Code == FunctionCodes.ReadCoils)
                {
                    if (this.ServerData.Coils == null) return this.WriteError();
                    this.Data = this.ServerData.ReadCoil(this.Address, this.Count);
                }
                else if (this.Code == FunctionCodes.ReadInputDiscreteQuantity)
                {
                    if (this.ServerData.Discretes == null) return this.WriteError();
                    this.Data = this.ServerData.ReadDiscrete(this.Address, this.Count);
                }
                else if (this.Code == FunctionCodes.ReadHoldingRegisters || this.Code == FunctionCodes.ReadInputRegister)
                {
                    if (this.ServerData.Registers == null) return this.WriteError();
                    this.Data = this.ServerData.ReadRegister(this.Address, this.Count);
                }

                this.Writer.WriteByte((byte)this.Data.Length);
                this.Writer.WriteBytes(this.Data);
            }
            else
            {
                this.Writer.WriteBytes(this.Address.GetBytes(false));
                if (this.Code == FunctionCodes.WriteCoil || this.Code == FunctionCodes.WriteRegister)
                {
                    this.Writer.WriteBytes(this.Data);
                }
                else
                {
                    this.Writer.WriteBytes(this.Count.GetBytes(false));
                }
            }
            var bytes = this.Writer.ToArray();
            var crc = bytes.Encode(0, bytes.Length, VerificationType.ModbusCRC16, EndianType.BIG, null);
            this.Writer.Clear();
            this.Writer.WriteBytes(crc);
            return this.Writer.ToArray();
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
                this.ErrorCode = (ExceptionCodes)this.Reader.ReadByte();
            }
            else
            {
                this.Code = (FunctionCodes)code;
                this.ErrorCode = 0;

                if (this.Reader.RemainingLength < 2) return;
                this.Address = this.Reader.ReadBytes(2).ToUInt16();

                if(this.RequestType== RequestType.WRITE)
                {
                    if(this.Code== FunctionCodes.WriteCoil)
                    {

                    }
                    else
                    {
                        if (this.Reader.RemainingLength < 2) return;
                        this.Count = this.Reader.ReadBytes(2).ToUInt16();

                        if (this.Reader.RemainingLength < 1) return;
                        this.Length = this.Reader.ReadByte();
                    }
                    if (this.Reader.RemainingLength < this.Length) return;
                    this.Data = this.Reader.ReadBytes(this.Length);
                }
                else
                {
                    if (this.Reader.RemainingLength < 2) return;
                    this.Count = this.Reader.ReadBytes(2).ToUInt16();
                }
                if (!this.Reader.EndOfStream)
                    this.VerificationCode = this.Reader.ReadBytes();
                return;

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
                    this.Count = this.Reader.ReadBytes(2).ToUInt16();
                }
                if (!this.Reader.EndOfStream)
                    this.VerificationCode = this.Reader.ReadBytes();
            }
        }
        /// <summary>
        /// 输出错误响应数据
        /// </summary>
        /// <returns></returns>
        public byte[] WriteError()
        {
            this.Writer.Clear();
            this.Writer.WriteByte((byte)(this.Count + 0x80));
            this.Writer.WriteByte((byte)ExceptionCodes.SlaveDeviceFailure);
            return this.Writer.ToArray();
        }
        #endregion
    }
}