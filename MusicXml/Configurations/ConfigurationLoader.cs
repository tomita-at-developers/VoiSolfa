using Developers.MusicXml.Configurations.Models;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Developers.MusicXml.Configurations
{
    public static class ConfigurationLoader
    {
        #region "public methods"

        /// <summary>
        /// 設定ローダー
        /// </summary>
        public static MusicConfigurations Load()
        {
            MusicConfigurations RetVal = new MusicConfigurations();
            try
            {
                //設定ファイルの読み取り
                var config = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddXmlFile("MusicXml.config", true, true)
                   .Build();
                var TempConfigs = config.Get<MusicConfigurations>();
                //設定ファイルが読み取れた場合は適用
                if (TempConfigs != null)
                {
                    RetVal = TempConfigs;
                }
            }
            catch (Exception ex)
            {
                //ConfigurationBuilderは設定ファイルの内容がXmlとして正しくない場合に例外となるようだ
                Logger.Writer.Error(ex, "Configurations.ConfigurationManager failed to read MusicXml.config.");
            }
            return RetVal;
        }

        #endregion
    }
}
