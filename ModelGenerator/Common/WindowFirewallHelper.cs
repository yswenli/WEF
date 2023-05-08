/****************************************************************************
*Copyright (c) 2023 RiverLand All Rights Reserved.
*CLR版本： 4.0.30319.42000
*机器名称：WALLE
*公司名称：RiverLand
*命名空间：WEF.ModelGenerator.Common
*文件名： WindowFirewallHelper
*版本号： V1.0.0.0
*唯一标识：1d3cbad4-3bce-41d5-a25e-1e31b318a88c
*当前的用户域：WALLE
*创建人： yswenli
*电子邮箱：walle.wen@tjingcai.com
*创建时间：2023/5/8 11:38:49
*描述：
*
*=================================================
*修改标记
*修改时间：2023/5/8 11:38:49
*修改人： yswenli
*版本号： V1.0.0.0
*描述：
*
*****************************************************************************/
using System;

using NetFwTypeLib;


namespace WEF.ModelGenerator.Common
{
    public static class WindowFirewallHelper
    {

        /// <summary>
        /// 添加防火墙白名单
        /// </summary>
        /// <param name="ipList"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SetWhiteList(string[] ipList, string name = "IP白名单")
        {
            try
            {
                AddRule(name, string.Empty, string.Empty, "0", string.Join(",", ipList), "0", false, true, true);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return false;
        }

        public static bool SetWhiteList(string ipList)
        {
            if (string.IsNullOrEmpty(ipList))
            {
                return false;
            }
            var arr = ipList.Split(new string[] { "\r\n", ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (arr == null || arr.Length < 1 || string.IsNullOrEmpty(arr[0]))
            {
                return false;
            }

            return SetWhiteList(arr);
        }

        /// <summary>
        /// 设置防火墙黑名单
        /// </summary>
        /// <param name="ipList"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SetBlackList(string[] ipList, string name = "IP黑名单")
        {
            try
            {
                AddRule(name, string.Empty, string.Empty, "0", string.Join(",", ipList), "0", false, false, true);

                return true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            return false;
        }

        /// <summary>
        /// 设置防火墙黑名单
        /// </summary>
        /// <param name="ipList"></param>
        /// <returns></returns>
        public static bool SetBlackList(string ipList)
        {
            if (string.IsNullOrEmpty(ipList))
            {
                return false;
            }
            var arr = ipList.Split(new string[] { "\r\n", ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

            if (arr == null || arr.Length < 1 || string.IsNullOrEmpty(arr[0]))
            {
                return false;
            }

            return SetBlackList(arr);
        }

        /// <summary>
        /// 防火墙管理
        /// </summary>
        private static NetFwTypeLib.INetFwMgr NetFwMgr
        {
            get
            {
                return (INetFwMgr)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwMgr"));
            }
        }
        /// <summary>
        /// 防火墙策略
        /// </summary>
        private static NetFwTypeLib.INetFwPolicy2 FirewallPolicy
        {
            get
            {
                return (INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
            }
        }

        /// <summary>
        /// 防火墙规则
        /// </summary>
        private static INetFwRule FirewallRule
        {
            get
            {
                return (INetFwRule)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwRule"));
            }
        }

        /// <summary>
        /// 防火墙启用状态
        /// </summary>
        public static bool FirewallEnabled
        {
            get
            {
                return NetFwMgr.LocalPolicy.CurrentProfile.FirewallEnabled;
            }
        }

        /// <summary>
        /// 开启防火墙
        /// </summary>
        /// <returns></returns>
        public static bool OpenFirewall()
        {
            try
            {
                //INetFwPolicy2 firewallPolicy =(INetFwPolicy2)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwPolicy2"));
                // 启用<高级安全Windows防火墙> - 专有配置文件的防火墙
                FirewallPolicy.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PRIVATE, true);
                // 启用<高级安全Windows防火墙> - 公用配置文件的防火墙
                FirewallPolicy.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_PUBLIC, true);
                // 启用<高级安全Windows防火墙> - 域配置文件的防火墙
                FirewallPolicy.set_FirewallEnabled(NET_FW_PROFILE_TYPE2_.NET_FW_PROFILE2_DOMAIN, true);
                return true;
            }
            catch (Exception e)
            {
                string error = $"防火墙修改出错：{e.Message}";
                throw new Exception(error);
            }
        }


        /// <summary>
        /// 通用规则命名 方便查询
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        private static string DesignAppRuleName(string appPath)
        {
            string ruleName = $"wef_{System.IO.Path.GetFileNameWithoutExtension(appPath)}";
            return ruleName;
        }

        /// <summary>
        /// 为WindowsDefender防火墙删除一条规则
        /// </summary>
        /// <param name="name"></param>
        public static void RemoveRule(string name)
        {
            var policy = FirewallPolicy;
            policy.Rules.Remove(name);
        }

        /// <summary>
        /// 检查规则是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool ExistRule(string name)
        {
            var policy = FirewallPolicy;
            for(INetFwRule rule in policy.Rules)
            {
                if (rule.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 为WindowsDefender防火墙添加一条规则
        /// </summary>
        public static void AddRule(string name,
            string exePath,
            string localAddresses,
            string localPort,
            string remoteAddresses,
            string remotePorts,
            bool isIn,
            bool allowed,
            bool enabled)
        {

            var policy = FirewallPolicy;
            //创建防火墙策略类的实例
            var rule = FirewallRule;
            //创建防火墙规则类的实例
            rule.Name = DesignAppRuleName(name);

            if (ExistRule(rule.Name))
            {
                RemoveRule(rule.Name);
            }

            //为规则添加名称
            rule.Description = DesignAppRuleName(name);
            //为规则添加描述
            rule.Direction = (isIn ? NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_IN : NET_FW_RULE_DIRECTION_.NET_FW_RULE_DIR_OUT);
            //选择入站规则还是出站规则，IN为入，OUT为出            
            rule.Protocol = (int)NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_ANY;
            //为规则添加协议类型
            if (!string.IsNullOrEmpty(exePath) && FileHelper.Exists(exePath))
            {
                rule.ApplicationName = exePath;
            }
            //为规则添加应用程序（注意这里是应用程序的绝对路径名）
            if (!string.IsNullOrEmpty(localAddresses))
            {
                rule.LocalAddresses = localAddresses;
            }
            //为规则添加本地IP地址
            if (!string.IsNullOrEmpty(localPort) && localPort != "0")
            {
                rule.LocalPorts = localPort;
            }
            //为规则添加本地端口
            if (!string.IsNullOrEmpty(remoteAddresses))
            {
                rule.RemoteAddresses = remoteAddresses;
            }
            //为规则添加远程IP地址
            if (!string.IsNullOrEmpty(remotePorts) && remotePorts != "0")
            {
                rule.RemotePorts = remotePorts;
            }
            //为规则添加远程端口
            rule.Action = (allowed ? NET_FW_ACTION_.NET_FW_ACTION_ALLOW : NET_FW_ACTION_.NET_FW_ACTION_BLOCK);
            //设置规则是阻止还是允许（ALLOW=允许，BLOCK=阻止）
            rule.Enabled = enabled;
            //是否启用规则
            policy.Rules.Add(rule);
            //添加规则到防火墙策略
        }


        /// <summary>
        /// 允许应用程序通过防火墙
        /// </summary>
        /// <param name="appPath">应用程序的绝对路径</param>
        /// <exception cref="FileNotFoundException">未找到程序文件</exception>
        public static void AllowAppUseFirewall(string appPath)
        {
            if (System.IO.File.Exists(appPath) == false)
            {
                throw new System.IO.FileNotFoundException("未找到程序文件");
            }
            //创建firewall管理类的实例： Type的GetTypeFromProgID是通过注册表信息项目创建实例类型

            //以程序名为规则名创建规则，以便查询
            string name = DesignAppRuleName(appPath);
            INetFwAuthorizedApplication appAuthorized = FindAppRule(name);
            if (appAuthorized != null)
            {
                RemoveAppUseFirewall(name);
            }
            //创建一个认证程序类的实例
            INetFwAuthorizedApplication app = (INetFwAuthorizedApplication)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwAuthorizedApplication"));
            //在例外列表里，程序显示的名称
            app.Name = name;
            //程序的绝对路径，这里使用程序本身
            app.ProcessImageFileName = appPath;
            //端口的范围，针对哪类或哪个IP地址
            //objPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            //此处可以指定IP地址版本信息
            //objPort.IpVersion = NET_FW_IP_VERSION_.NET_FW_IP_VERSION_V4;
            //是否启用该规则
            app.Enabled = true;
            //加入到防火墙的管理策略
            NetFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Add(app);
        }

        /// <summary>
        /// 查找特定程序防火墙对应规则
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static INetFwAuthorizedApplication FindAppRule(string name)
        {
            NET_FW_PROFILE_TYPE_ currentProfileType = NetFwMgr.CurrentProfileType;
            //查找防火墙规则中是否已有同名规则存在
            foreach (INetFwAuthorizedApplication item in NetFwMgr.LocalPolicy.GetProfileByType(currentProfileType).AuthorizedApplications)
            {
                if (item.Name == name)
                {
                    return item;
                }
            }
            return null;
        }

        /// <summary>
        /// 移除应用程序通过防火墙
        /// </summary>
        /// <param name="name">应用程序的绝对路径</param>
        public static void RemoveAppUseFirewall(string name)
        {
            //参数为程序的绝对路径
            NetFwMgr.LocalPolicy.CurrentProfile.AuthorizedApplications.Remove(name);
        }
        /// <summary>
        /// 通用规则命名 方便查询
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        private static string DesignPortRuleName(int Port)
        {
            string ruleName = $"wef_{Port} 端口";
            return ruleName;
        }
        /// <summary>
        /// 添加防火墙例外端口
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="port">端口</param>
        public static void AllowPortUseFirewall(int port)
        {
            string name = DesignPortRuleName(port);
            INetFwOpenPort objPort = (INetFwOpenPort)Activator.CreateInstance(Type.GetTypeFromProgID("HNetCfg.FwOpenPort"));
            objPort.Name = name;
            objPort.Port = port;
            objPort.Protocol = NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP;
            objPort.Scope = NET_FW_SCOPE_.NET_FW_SCOPE_ALL;
            objPort.Enabled = true;
            INetFwOpenPort openPort = FindFirewallRule(port);
            if (openPort != null)
            {
                RemovePortUseFirewall(port);
            }
            NetFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Add(objPort);
        }
        /// <summary>
        /// 删除防火墙例外端口
        /// </summary>
        /// <param name="port">端口</param>
        public static void RemovePortUseFirewall(int port)
        {
            NetFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts.Remove
            (port, NET_FW_IP_PROTOCOL_.NET_FW_IP_PROTOCOL_TCP);
        }
        /// <summary>
        /// 查找特定端口防火墙对应规则
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static INetFwOpenPort FindFirewallRule(int Port)
        {
            foreach (INetFwOpenPort mPort in NetFwMgr.LocalPolicy.CurrentProfile.GloballyOpenPorts)
            {
                if (mPort.Name == DesignPortRuleName(Port))
                {
                    return mPort;
                }
            }
            return null;
        }
    }
}
