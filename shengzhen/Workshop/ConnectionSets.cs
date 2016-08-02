using System;
using System.Collections.Generic;
using System.Text;

namespace Workshop
{
    public class ConnectionSets
    {
        /// <summary>
        /// 数据源类型
        /// </summary>
        public int Type;

        /// <summary>
        /// 库文件路径
        /// </summary>
        public string FilePath;

        /// <summary>
        /// 服务器
        /// </summary>
        public string Server;

        /// <summary>
        /// 数据库
        /// </summary>
        public string DataBase;

        /// <summary>
        /// 用户
        /// </summary>
        public string User;

        /// <summary>
        /// 密码
        /// </summary>
        public string Password;

        /// <summary>
        /// 是否使用域名
        /// </summary>
        public bool IsDomainName;
    }
}
