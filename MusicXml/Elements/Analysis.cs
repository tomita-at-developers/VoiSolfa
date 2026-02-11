namespace Developers.MusicXml.Elements
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

        public PitchContext Context { get; set; } = new PitchContext(ChromaticIndex, EnharmonicIndex);
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
            Dump += "Context.ChromaticIndex=" + this.Context.ChromaticIndex.ToString() + ",";
            Dump += "Context.EnharmonicIndex=" + this.Context.EnharmonicIndex.ToString() + ",";
            Dump += "Description=" + this.Description;
            return Dump;
        }

        #endregion
    }
}
