using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using static Developers.MidiXml.Elements.MidiDefs;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// 音程の分析情報
    /// </summary>
    /// <remarks>
    /// コンストラクタ
    /// </remarks>
    /// <param name="chromaticIndex"></param>
    /// <param name="enharmonicIndex"></param>
    public class Analysis(int ChromaticIndex, int EnharmonicIndex, string Description)
    {
        #region "public properties"

        /// <summary>
        /// 半音階インデックス
        /// </summary>
        public int ChromaticIndex { get; set; } = ChromaticIndex;
        /// <summary>
        /// 同音異名インデックス
        /// </summary>
        public int EnharmonicIndex { get; set; } = EnharmonicIndex;
        /// <summary>
        /// アナリーゼのメモ
        /// </summary>
        public string Description { get; set; } = Description;

        #endregion

        #region "constructors"

        //primary constructor only.

        #endregion

        #region "debug methods"

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "[Analysis]";
            Dump += "ChromaticIndex=" + this.ChromaticIndex.ToString() +",";
            Dump += "EnharmonicIndex=" + this.EnharmonicIndex.ToString() + ",";
            Dump += "Description=" + this.Description;
            return Dump;
        }

        #endregion
    }
}
