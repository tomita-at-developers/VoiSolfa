using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace VoiSolfa
{
    /// <summary>
    /// アセンブリ内で使用するロガー
    /// </summary>
    internal static class Logger
    {
        /// <summary>
        /// ロガー本体
        /// </summary>
        private static readonly ILogger _logger;
        /// <summary>
        /// アセンブル内に公開するロガー
        /// </summary>
        public static ILogger Writer => _logger;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static Logger()
        {
            try
            {
                //ログ設定ファイルに従いロガーを生成
                _logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(
                        new ConfigurationBuilder()
                                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddJsonFile("VoiSolfaLogSettings.json", true).Build()
                    ).CreateLogger();
            }
            catch (Exception ex)
            {
                //コンソール出力にフォールバック
                _logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .CreateLogger();
                //ログ出力
                _logger.Error(ex, "[Logger] Serilog configuration failed. Using default logger.");
            }
        }
    }
}
