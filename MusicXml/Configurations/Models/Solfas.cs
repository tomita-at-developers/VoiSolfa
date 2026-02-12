using Microsoft.Extensions.Configuration;

namespace Developers.MusicXml.Configurations.Models
{
    /// <summary>
    /// Solfaの設定
    /// </summary>
    public class Solfa
    {
        [ConfigurationKeyName("name")] public string Name { get; set; } = string.Empty;
        [ConfigurationKeyName("do")] public string Do { get; set; } = "do";
        [ConfigurationKeyName("do-sharp")] public string DoSharp { get; set; } = "di";
        [ConfigurationKeyName("re-flat")] public string ReFlat { get; set; } = "ra";
        [ConfigurationKeyName("re")] public string Re { get; set; } = "re";
        [ConfigurationKeyName("re-sharp")] public string ReSharp { get; set; } = "ri";
        [ConfigurationKeyName("mi-flat")] public string MiFlat { get; set; } = "me";
        [ConfigurationKeyName("mi")] public string Mi { get; set; } = "mi";
        [ConfigurationKeyName("fa")] public string Fa { get; set; } = "fa";
        [ConfigurationKeyName("fa-sharp")] public string FaSharp { get; set; } = "fi";
        [ConfigurationKeyName("sol-flat")] public string SolFlat { get; set; } = "se";
        [ConfigurationKeyName("sol")] public string Sol { get; set; } = "sol";
        [ConfigurationKeyName("sol-sharp")] public string SolSharp { get; set; } = "si";
        [ConfigurationKeyName("la-flat")] public string LaFlat { get; set; } = "le";
        [ConfigurationKeyName("la")] public string La { get; set; } = "la";
        [ConfigurationKeyName("la-sharp")] public string LaSharp { get; set; } = "li";
        [ConfigurationKeyName("ti-flat")] public string TiFlat { get; set; } = "te";
        [ConfigurationKeyName("ti")] public string Ti { get; set; } = "ti";

        public List<List<string>> ToList()
        {
            List<List<string>> RetVal =
            [
                [Do],
                [DoSharp, ReFlat],
                [Re],
                [ReSharp, MiFlat],
                [Mi],
                [Fa],
                [FaSharp, SolFlat],
                [Sol],
                [SolSharp, LaFlat],
                [La],
                [LaSharp, TiFlat],
                [Ti],
            ];
            return RetVal;
        }
    }
}
