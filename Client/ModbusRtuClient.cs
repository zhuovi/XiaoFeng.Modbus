using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading.Tasks;
using XiaoFeng.Modbus.Internal;
using System.Threading;
using System.IO;
using XiaoFeng.IO;
using XiaoFeng;
using XiaoFeng.Modbus.Model;
using XiaoFeng.Modbus.Protocols;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-19 09:31:08                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Client
{
    /// <summary>
    /// ModbusRtu 客户端
    /// </summary>
    public class ModbusRtuClient : ModbusRtu
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusRtuClient() : base() { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        public ModbusRtuClient(string portName) : base(portName, 9600, Parity.None, 8, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        public ModbusRtuClient(string portName, int baudRate) : base(portName, baudRate, Parity.None, 8, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        public ModbusRtuClient(string portName, int baudRate, Parity parity) : base(portName, baudRate, parity, 8, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        public ModbusRtuClient(string portName, int baudRate, Parity parity, int dataBits) : base(portName, baudRate, parity, dataBits, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        public ModbusRtuClient(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            PortName = portName;
            BaudRate = baudRate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = stopBits;
        }

        #endregion

        #region 属性

        #endregion

        #region 方法
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serialPort">SerialPort</param>
        public override void Init(SerialPort serialPort) { }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="code">功能码</param>
        /// <param name="host">主机</param>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public virtual byte[] SendCommand(FunctionCodes code, byte host, ushort address, ushort value)
        {
            var msg = string.Empty;
            if (!this.OpenSerialPortStatus(ref msg)) return Array.Empty<byte>();

            var cmd = new byte[8];
            cmd[0] = host;
            cmd[1] = (byte)code;
            cmd[2] = (byte)(address >> 8);
            cmd[3] = (byte)(address & 0xFF);
            cmd[4] = (byte)(value >> 8);
            cmd[5] = (byte)(value & 0xFF);

            var crc = ModbusCRC.CRC(cmd, 0, cmd.Length - 2);
            cmd[6] = (byte)(crc & 0xFF);
            cmd[7] = (byte)(crc >> 8);

            Serial.Write(cmd, 0, cmd.Length);

            Thread.Sleep(10);

            WaitReceive();

            var ms = new MemoryStream();
            while (Serial.BytesToRead > 0)
            {
                var buffers = new byte[ReadBufferSize];
                int count = Serial.Read(buffers, 0, buffers.Length);
                ms.Write(buffers, 0, count);
            }
            if (ms.Length == 0) return Array.Empty<byte>();
            var bytes = ms.ToArray();
            if (bytes[1] == (byte)code + 0x80)
            {
                LogHelper.WriteLog(bytes[2].ToEnum<ExceptionCodes>().GetDescription());
                return Array.Empty<byte>();
            }
            if (bytes[0] == address)
            {
                var checkcrc = ModbusCRC.CRC(bytes, 0, bytes.Length - 2);
                var crc2 = bytes.ToUInt16(bytes.Length - 2);
                if (checkcrc != crc2) return Array.Empty<byte>();
                return bytes.ReadBytes(2, bytes.Length - 2 - 2);
            }
            return Array.Empty<byte>();
        }
        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="packet">数据包</param>
        /// <returns></returns>
        public ResultModel<byte[]> SendCommand(ModbusRtuRequestPacket packet)
        {
            if (packet == null) return ResultModel<byte[]>.Error("请求数据为空.");
            if (packet.Address < ModbusHelper.ADDRESS_MIN || packet.Address > ModbusHelper.ADDRESS_MAX) return ResultModel<byte[]>.Error("地址不合法.");
            var msg = string.Empty;
            if (!this.OpenSerialPortStatus(ref msg)) return ResultModel<byte[]>.Error(msg);
            var command = packet.ToArray();
            command = command.Encode(0, command.Length, this.VerificationType, this.EndianType, this.VerificationAlgorithm);
            var result = new ResultModel<byte[]>()
            {
                IsSuccessed = true,
                Request = command
            };
            this.Serial.Write(command, 0, command.Length);

            Thread.Sleep(10);

            this.WaitReceive();

            var ms = new MemoryStream();
            while (Serial.BytesToRead > 0)
            {
                var buffers = new byte[ReadBufferSize];
                int count = Serial.Read(buffers, 0, buffers.Length);
                ms.Write(buffers, 0, count);
            }
            if (ms.Length == 0)
            {
                result.IsSuccessed = false;
                result.ErrorMessage = "网络错误";
                return result;
            }
            var ResPacket = new ModbusRtuRequestPacket(ms.ToArray(), packet);
            if (ResPacket.Host != packet.Host)
            {
                result.IsSuccessed = false;
                result.ErrorMessage = "响应数据不正确";
                return result;
            }
            if (ResPacket.Code == 0 && ResPacket.ErroCode > 0)
            {
                result.IsSuccessed = false;
                result.ErrorCode = ResPacket.ErroCode;
                result.ErrorMessage = ResPacket.ErroCode.GetDescription();
                return result;
            }
            if (result.IsSuccessed)
            {
                result.Value = ResPacket.Data;
            }
            return result;
        }
       

        #region 读取

        #region 读线圈
        /// <summary>
        /// 读线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取长度</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] ReadCoil(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.COIL_MIN_COUNT || count > ModbusHelper.COIL_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.ReadCoils,
                Address = address,
                Count = count,
                Host = host
            };
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #region 读离散量输入
        /// <summary>
        /// 读离散量输入
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取长度</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] ReadInputDiscreteQuantity(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.COIL_MIN_COUNT || count > ModbusHelper.COIL_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.ReadInputDiscreteQuantity,
                Address = address,
                Count = count,
                Host = host
            };
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #region 读保持寄存器
        /// <summary>
        /// 读保持寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取长度</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] ReadHoldingRegisters(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.REGISTER_MIN_COUNT || count > ModbusHelper.REGISTER_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.ReadHoldingRegisters,
                Address = address,
                Count = count,
                Host = host
            };
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #region 读输入寄存器
        /// <summary>
        /// 读输入寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="count">读取长度</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] ReadInputRegister(ushort address, ushort count, byte host = 1)
        {
            if (count < ModbusHelper.REGISTER_MIN_COUNT || count > ModbusHelper.REGISTER_MAX_COUNT) return Array.Empty<byte>();
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.ReadInputRegister,
                Address = address,
                Count = count,
                Host = host
            };
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #endregion

        #region 写入

        #region 写单个线圈
        /// <summary>
        /// 写单个线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] WriteCoil(ushort address, bool value, byte host = 1)
        {
            var bytes = new byte[] { 0x00, 0x00 };
            bytes[0] = (byte)(value ? 0xFF : 0x00);
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.WriteCoil,
                Address = address,
                Data = bytes,
                Host = host
            };
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #region 写单个保持寄存器
        /// <summary>
        /// 写单个保持寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] WriteRegister(ushort address, ushort value, byte host = 1)
        {
            if (value < ModbusHelper.REGISTER_MIN_VALUE || value > ModbusHelper.REGISTER_MAX_VALUE) return Array.Empty<byte>();
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.WriteRegister,
                Address = address,
                Data = value.GetBytes(),
                Host = host
            };
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #region 写多个线圈
        /// <summary>
        /// 写多个线圈
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] WriteCoils(ushort address, ushort value, byte host = 1)
        {
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.WriteCoils,
                Address = address,
                Data = value.GetBytes(),
                Host = host
            };
            packet.Count = (ushort)(packet.Data.Length * 8);
            if (packet.Count < ModbusHelper.COILS_MIN_COUNT || packet.Count > ModbusHelper.COILS_MAX_COUNT) return Array.Empty<byte>();
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #region 写多个保持寄存器
        /// <summary>
        /// 写多个保持寄存器
        /// </summary>
        /// <param name="address">地址</param>
        /// <param name="value">值</param>
        /// <param name="host">主机</param>
        /// <returns></returns>
        public byte[] WriteRegisters(ushort address, ushort value, byte host = 1)
        {
            var packet = new ModbusRtuRequestPacket()
            {
                Code = FunctionCodes.WriteRegisters,
                Address = address,
                Data = value.GetBytes(),
                Host = host
            };
            packet.Count = (ushort)(Math.Ceiling(packet.Data.Length / 2f));
            if (packet.Count > ModbusHelper.REGISTER_MIN_COUNT || packet.Count < ModbusHelper.REGISTER_MAX_COUNT) return Array.Empty<byte>();
            var result = SendCommand(packet);
            if (!result.IsSuccessed) return Array.Empty<byte>();
            return result.Value;
        }
        #endregion

        #endregion

        #region 关闭
        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (Serial == null) return;
            if (Serial.IsOpen) Serial.Close();
            IsOpen = false;
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="disposing">标识</param>
        protected override void Dispose(bool disposing)
        {
            Close();
            Serial.Dispose();
            Serial = null;
            base.Dispose(disposing);
        }
        /// <summary>
        /// 析构器
        /// </summary>
        ~ModbusRtuClient()
        {
            Dispose(true);
        }
        #endregion

        #endregion
    }
}