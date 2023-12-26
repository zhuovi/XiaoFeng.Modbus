using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using XiaoFeng.IO;
using XiaoFeng.Modbus.Server;
using XiaoFeng.Zip;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-12-25 09:54:23                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus.Internal
{
    /// <summary>
    /// ModbusRtu 类说明
    /// </summary>
    public abstract class ModbusRtu: Disposable
    {
        #region 构造器
        /// <summary>
        /// 无参构造器
        /// </summary>
        public ModbusRtu()
        {

        }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        public ModbusRtu(string portName) : this(portName, 9600, Parity.None, 8, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        public ModbusRtu(string portName, int baudRate) : this(portName, baudRate, Parity.None, 8, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        public ModbusRtu(string portName, int baudRate, Parity parity) : this(portName, baudRate, parity, 8, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        public ModbusRtu(string portName, int baudRate, Parity parity, int dataBits) : this(portName, baudRate, parity, dataBits, StopBits.One) { }
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="portName">串口名称</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        public ModbusRtu(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            PortName = portName;
            BaudRate = baudRate;
            Parity = parity;
            DataBits = dataBits;
            StopBits = stopBits;
        }
        #endregion

        #region 属性
        /// <summary>
        /// 串口对象
        /// </summary>
        protected SerialPort Serial { get; set; }
        /// <summary>
        /// 读取串口数据超时时间 单位为毫秒
        /// </summary>
        public int ReadTimeout { get; set; } = -1;
        /// <summary>
        /// 写入串口数据超时时间 单位为毫秒
        /// </summary>
        public int WriteTimeout { get; set; } = -1;
        /// <summary>
        /// 是否打开通讯
        /// </summary>
        public bool? IsOpen { get; set; }
        /// <summary>
        /// 串口名称
        /// </summary>
        public string PortName { get; set; } = "COM1";
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; } = 9600;
        /// <summary>
        /// 校验位
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;
        /// <summary>
        /// 数据位
        /// </summary>
        public int DataBits { get; set; } = 8;
        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;
        /// <summary>
        /// 编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.ASCII;
        /// <summary>
        /// 读取缓存大小
        /// </summary>
        public int ReadBufferSize { get; set; } = 4096;
        /// <summary>
        /// 写入缓存大小
        /// </summary>
        public int WriteBufferSize { get; set; } = 2048;
        /// <summary>
        /// 换行符
        /// </summary>
        public string NewLine { get; set; } = "\n";
        /// <summary>
        /// 是否启用DTR
        /// </summary>
        public bool DtrEnable { get; set; } = false;
        /// <summary>
        /// 是否启用RTS
        /// </summary>
        public bool RtsEnable { get; set; } = false;
        /// <summary>
        /// 中断状态
        /// </summary>
        public bool BreakState { get; set; } = true;
        /// <summary>
        /// 是否丢弃空
        /// </summary>
        public bool DiscardNull { get; set; } = false;
        /// <summary>
        /// 是否有握手
        /// </summary>
        public Handshake Handshake { get; set; } = Handshake.None;
        /// <summary>
        /// 奇偶校验替换
        /// </summary>
        public byte ParityReplace { get; set; } = 63;
        /// <summary>
        /// 接收字节阈值
        /// </summary>
        public int ReceivedBytesThreshold { get; set; } = 1;
        /// <summary>
        /// 校验类型
        /// </summary>
        public VerificationType VerificationType { get; set; }
        /// <summary>
        /// 字节超时时长 单位为毫秒
        /// </summary>
        public int ByteTimeout { get; set; } = 20;
        /// <summary>
        /// 传输模式
        /// </summary>
        public ModbusMode Mode { get; set; } = ModbusMode.ASCII;
        /// <summary>
        /// 字节序类型
        /// </summary>
        public EndianType EndianType { get; set; } = EndianType.LITTLE;
        /// <summary>
        /// 校验算法
        /// </summary>
        public Func<byte[], byte[]> VerificationAlgorithm { get; set; }
        /// <summary>
        /// 获取所有的串口
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames() => SerialPort.GetPortNames();
        /// <summary>
        /// 接收数据事件
        /// </summary>
        public event SerialDataReceivedEventHandler SerialDataReceived;
        /// <summary>
        /// 出错事件
        /// </summary>
        public event SerialErrorReceivedEventHandler SerialErrorReceived;
        /// <summary>
        /// 
        /// </summary>
        public event SerialPinChangedEventHandler SerialPinChanged;
        /// <summary>
        /// 服务器数据
        /// </summary>
        public SerialPortServerData ServerData { get; set; } = new SerialPortServerData();
        #endregion

        #region 方法

        #region 创建SerialPort
        /// <summary>
        /// 创建SerialPort
        /// </summary>
        /// <returns></returns>
        protected SerialPort CreateSerialPort()
        {
            this.Serial = new SerialPort(PortName, BaudRate, Parity, DataBits, StopBits)
            {
                DtrEnable = DtrEnable,
                RtsEnable = RtsEnable,
                WriteBufferSize = WriteBufferSize,
                ReadBufferSize = ReadBufferSize,
                ReadTimeout = ReadTimeout,
                WriteTimeout = WriteTimeout,
                NewLine = NewLine,
                BreakState = BreakState,
                DiscardNull = DiscardNull,
                Handshake = Handshake,
                ParityReplace = ParityReplace,
                ReceivedBytesThreshold = ReceivedBytesThreshold
            };
            this.Init(this.Serial);
            if (SerialDataReceived != null) this.Serial.DataReceived += SerialDataReceived;
            if (SerialErrorReceived != null) this.Serial.ErrorReceived += SerialErrorReceived;
            if (SerialPinChanged != null) this.Serial.PinChanged += SerialPinChanged;
            return this.Serial;
        }
        #endregion

        #region 初始化
        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Init(SerialPort serialPort);
        #endregion

        #region 是否打开
        /// <summary>
        /// 打开状态
        /// </summary>
        /// <param name="message">返回消息</param>
        /// <returns></returns>
        public virtual Boolean OpenSerialPortStatus(ref string message)
        {
            message = string.Empty;
            if (this.Serial == null)
                this.Serial = this.CreateSerialPort();
            if (this.Serial.IsOpen) return true;
            try
            {
                this.Serial.Open();
                this.IsOpen = false;
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }
        #endregion

        #region 等待
        /// <summary>
        /// 等待
        /// </summary>
        public void WaitReceive()
        {
            var end = DateTime.Now.AddMilliseconds(ByteTimeout);
            var count = Serial.BytesToRead;
            while (Serial.IsOpen && end > DateTime.Now)
            {
                Thread.Sleep(ReadTimeout);
                if (count != Serial.BytesToRead)
                {
                    end = DateTime.Now.AddMilliseconds(ByteTimeout);
                    count = Serial.BytesToRead;
                }
                else
                    break;
            }
        }
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
        ~ModbusRtu()
        {
            Dispose(true);
        }
        #endregion

        #region 保存离线数据
        /// <summary>
        /// 保存离线数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public Boolean SaveSerialPortData(string path)
        {
            var _path = path.GetBasePath();
            if (this.ServerData == null)
            {
                FileHelper.Create(_path, FileAttribute.File);
                return true;
            }
            var NewLine = new byte[] { 13, 10 };
            var Writer = new MemoryBufferWriter();

            if (this.ServerData.Coils != null && this.ServerData.Coils.Any())
            {
                this.ServerData.Coils.Each(c =>
                {
                    Writer.WriteByte((byte)(c.Value == 0 ? 0 : 1));
                });
            }
            Writer.WriteBytes(NewLine);
            if (this.ServerData.Registers != null && this.ServerData.Registers.Any())
            {
                this.ServerData.Registers.Each(r =>
                {
                    Writer.WriteBytes(r.Data);
                });
            }
            Writer.WriteBytes(NewLine);
            var line = new string('-', 90).GetBytes();
            Writer.WriteBytes(line);
            Writer.WriteBytes(NewLine);

            Writer.WriteBytes("当前文件为 XiaoFeng.Modbus 服务端数据存储数据库文件".GetBytes());
            Writer.WriteBytes(NewLine);

            Writer.WriteBytes("请不要手动修改当前文件内容，以防数据丢失".GetBytes());
            Writer.WriteBytes(NewLine);

            Writer.WriteBytes(line);
            Writer.WriteBytes(NewLine);

            FileHelper.DeleteFile(_path);
            FileHelper.WriteBytes(_path, Writer.ToArray());

            return true;
        }
        /// <summary>
        /// 加载离线数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public Boolean LoadSerialPortData(string path)
        {
            var _path = path.GetBasePath();
            if (!FileHelper.Exists(_path)) return false;
            this.ServerData = new SerialPortServerData();
           
            var Reader = new MemoryBufferReader(FileHelper.OpenBytes(_path));
            var coils = Reader.ReadLine();
            var regs = Reader.ReadLine();
            
            if (coils!=null && coils.Any())
            {
                for(ushort i = 0; i < coils.Length; i++)
                {
                    var c = coils[i];
                    this.ServerData.Coils.Add(new Model.CoilModel(i, (byte)(c == 0 ? 0 : 255)));
                }
            }
            if(regs!=null && regs.Any())
            {
                var length = regs.Length - (regs.Length % 2 == 0 ? 0 : 1);
                for(ushort i = 0; i < length; i += 2)
                {
                    this.ServerData.Registers.Add(new Model.RegisterModel(i, regs.ReadBytes(i,2)));
                }
            }
            return true;
        }
        #endregion
        #endregion
    }
}