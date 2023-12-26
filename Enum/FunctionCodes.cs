using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

/****************************************************************
*  Copyright © (2023) www.eelf.cn All Rights Reserved.          *
*  Author : jacky                                               *
*  QQ : 7092734                                                 *
*  Email : jacky@eelf.cn                                        *
*  Site : www.eelf.cn                                           *
*  Create Time : 2023-10-30 11:04:32                            *
*  Version : v 1.0.0                                            *
*  CLR Version : 4.0.30319.42000                                *
*****************************************************************/
namespace XiaoFeng.Modbus
{
    /// <summary>
    /// MODBUS 公共功能码
    /// </summary>
    public enum FunctionCodes : byte
    {
        /// <summary>
        /// 读线圈
        /// </summary>
        [Description("读线圈")]
        [Read]
        ReadCoils = 1,
        /// <summary>
        /// 读离散量输入
        /// </summary>
        [Description("读离散量输入")]
        [Read] 
        ReadInputDiscreteQuantity = 2,
        /// <summary>
        /// 读保持寄存器
        /// </summary>
        [Description("读保持寄存器")]
        [Read] 
        ReadHoldingRegisters = 3,
        /// <summary>
        /// 读输入寄存器
        /// </summary>
        [Description("读输入寄存器")]
        [Read] 
        ReadInputRegister = 4,
        /// <summary>
        /// 写单个线圈
        /// </summary>
        [Description("写单个线圈")]
        [Write]
        WriteCoil = 5,
        /// <summary>
        /// 写单个保持寄存器
        /// </summary>
        [Description("写单个保持寄存器")]
        [Write]
        WriteRegister = 6,
        /// <summary>
        /// 诊断
        /// </summary>
        [Description("诊断")]
        Diagnosis = 8,
        /// <summary>
        /// 写多个线圈
        /// </summary>
        [Description("写多个线圈")]
        [Write]
        WriteCoils = 15,
        /// <summary>
        /// 写多个保持寄存器
        /// </summary>
        [Description("写多个保持寄存器")]
        [Write]
        WriteRegisters = 16,
        /// <summary>
        /// 读文件记录
        /// </summary>
        [Description("读文件记录")]
        [Read]
        ReadFileRecord = 20,
        /// <summary>
        /// 写文件记录
        /// </summary>
        [Description("写文件记录")]
        [Write]
        WriteFileRecord = 21,
        /// <summary>
        /// 屏蔽写寄存器
        /// </summary>
        [Description("屏蔽写寄存器")]
        [Write]
        MaskWriteRegister = 22,
        /// <summary>
        /// 读/写多个保持寄存器
        /// </summary>
        [Description("读/写多个保持寄存器")]
        [Write]
        ReadWriteMultipleRegisters = 23,
        /// <summary>
        /// 读设备识别码
        /// </summary>
        [Description("读设备识别码")]
        [Read]
        ReadDeviceIdentificationCode = 43
    }
}