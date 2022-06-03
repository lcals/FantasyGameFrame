using System;
using Cysharp.Text;
using Fantasy.Frame;
using Fantasy.Logic.Interface;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ZLogger;

namespace Fantasy.Logic.Achieve
{
    public static class FantasyLogModuleExpand {

        public static void ZLogDebug(this string log)
        {
            FantasyLogModule.Logger.ZLogDebug(log);
        }
    }

    public class FantasyLogModule : AModule, IFantasyLogModule
    {
        private static ILoggerFactory _loggerFactory;
        public static Microsoft.Extensions.Logging.ILogger Logger { get; private set; }
    
        public FantasyLogModule(PluginManager pluginManager, bool isUpdate) : base(pluginManager, isUpdate)
        {
            InitLog();
        }

        private static void InitLog()
        {
            _loggerFactory = UnityLoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
#if UNITY_EDITOR
                builder.AddZLoggerUnityDebug(options =>
                {
                    var prefixFormat = ZString.PrepareUtf8<string,LogLevel, DateTime>("[{0}][{1}][{2}]");
                    options.PrefixFormatter = (writer, info) =>
                        prefixFormat.FormatTo(ref writer, info.CategoryName,info.LogLevel, info.Timestamp.DateTime.ToLocalTime());
                });
#endif

                builder.AddZLoggerRollingFile(
                    fileNameSelector: (dt, x) => $"logs/{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log",
                    timestampPattern: x => x.ToLocalTime().Date,
                    rollSizeKB: 1024, options =>
                    {
                        var prefixFormat = ZString.PrepareUtf8<LogLevel, DateTime>("[{0}][{1}]");
                        options.PrefixFormatter = (writer, info) =>
                            prefixFormat.FormatTo(ref writer, info.LogLevel, info.Timestamp.DateTime.ToLocalTime());
                    });
            });
            Logger = _loggerFactory.CreateLogger("Global");
            Application.quitting += () => { _loggerFactory.Dispose(); };
        }
        public  ILogger<T> GetLogger<T>() where T : class
        {
            return _loggerFactory.CreateLogger<T>();
        }

        public  Microsoft.Extensions.Logging.ILogger GetLogger(string categoryName)
        {
            return _loggerFactory.CreateLogger(categoryName);
        }

        #region 

        public override void Awake()
        {
          
        }

        public override void Init()
        {
        }

        public override void AfterInit()
        {
        }

        public override void Execute()
        {
        }

        public override void BeforeShut()
        {
        }

        public override void Shut()
        {
        }
        
        

        #endregion
    }

    
    
}