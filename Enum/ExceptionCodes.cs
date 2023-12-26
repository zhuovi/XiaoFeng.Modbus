using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-10-30 11:29:25                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// MODBUS 异常响应码
    /// </summary>
    public enum ExceptionCodes : byte
    {
        /// <summary>
        /// 非法功能
        /// </summary>
        /// <remarks>
        /// 对于服务器(或从站)来说，询问中接收到的功能码是不可允许的操作。这也许是因为功能码仅仅适用于新设备而在被选单元中是不可实现的。同时，还指出服务器(或从站)在错误状态中处理这种请求，例如：因为它是未配置的，并且要求返回寄存器值。
        /// </remarks>
        [Description("非法功能")]
        IllegalFunction = 0x01,
        /// <summary>
        /// 非法数据地址
        /// </summary>
        /// <remarks>
        /// 对于服务器(或从站)来说，询问中接收到的数据地址是不可允许的地址。特别是，参考号和传输长度的组合是无效的。对于带有 100 个寄存器的控制器来说，带有偏移量 96 和长度 4 的请求会成功，带有偏移量 96 和长度 5 的请求将产生异常码 02。
        /// </remarks>
        [Description("非法数据地址")]
        IllegalDataAddress = 0x02,
        /// <summary>
        /// 非法数据值
        /// </summary>
        /// <remarks>
        /// 对于服务器(或从站)来说，询问中包括的值是不可允许的值。这个值指示了组合请求剩余结构中的故障，例如：隐含长度是不正确的。并不意味着，因为MODBUS 协议不知道任何特殊寄存器的任何特殊值的重要意义，寄存器中被提交存储的数据项有一个应用程序期望之外的值。
        /// </remarks>
        [Description("非法数据值")]
        IllegalDataValue = 0x03,
        /// <summary>
        /// 从站设备故障
        /// </summary>
        /// <remarks>
        /// 当服务器(或从站)正在设法执行请求的操作时，产生不可重新获得的差错。
        /// </remarks>
        [Description("从站设备故障")]
        SlaveDeviceFailure = 0x04,
        /// <summary>
        /// 确认
        /// </summary>
        /// <remarks>
        /// 与编程命令一起使用。服务器(或从站)已经接受请求，并切正在处理这个请求，但是需要长的持续时间进行这些操作。返回这个响应防止在客户机(或主站)中发生超时错误。客户机(或主站)可以继续发送轮询程序完成报文来确定是否完成处理。
        /// </remarks>
        [Description("确认")]
        Acknowledge = 0x05,
        /// <summary>
        /// 从属设备忙
        /// </summary>
        /// <remarks>
        /// 与编程命令一起使用。服务器(或从站)正在处理长持续时间的程序命令。服务器(或从站)空闲时，用户(或主站)应该稍后重新传输报文。
        /// </remarks>
        [Description("从属设备忙")]
        SlaveDeviceBusy = 0x06,
        /// <summary>
        /// 存储奇偶性差错
        /// </summary>
        /// <remarks>
        /// 与功能码 20 和 21 以及参考类型 6 一起使用，指示扩展文件区不能通过一致性校验。服务器(或从站)设法读取记录文件，但是在存储器中发现一个奇偶校验错误。客户机(或主方)可以重新发送请求，但可以在服务器(或从站)设备上要求服务。
        /// </remarks>
        [Description("存储奇偶性差错")]
        StorageParityError = 0x08,
        /// <summary>
        /// 网关路径不可用
        /// </summary>
        /// <remarks>
        /// 与网关一起使用，指示网关不能为处理请求分配输入端口至输出端口的内部通信路径。通常意味着网关是错误配置的或过载的。
        /// </remarks>
        [Description("网关路径不可用")]
        GatewayPathUnavailable = 0x0A,
        /// <summary>
        /// 网关目标设备响应失败
        /// </summary>
        /// <remarks>
        /// 与网关一起使用，指示没有从目标设备中获得响应。通常意味着设备未在网络中。
        /// </remarks>
        [Description("网关目标设备响应失败")]
        GatewayTargetDeviceResponseFailed = 0x0B,
    }
}