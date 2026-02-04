using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// <transpose>情報
    /// </summary>
    public class Transpose : MidiElement
    {
        #region "properties"

        /// <summary>
        /// このインスタンスに対応するXElement
        /// </summary>
        public XElement? Source { get; init; } = null;
        /// <summary>
        /// 転調のダイアトニック表現(Stepのみ)
        /// </summary>
        public int? Diatonic { get; private set; } = null;
        /// <summary>
        /// 転調のクロマチック表現
        /// </summary>
        public int Chromatic { get; private set; } = 0;

        #endregion

        #region "constructors"

        /// <summary>
        /// コンストラクタ(XDocument以外)
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Diatonic"></param>
        /// <param name="Chromatic"></param>
        public Transpose(XElement? Source, int? Diatonic, int Chromatic)
        {
            this.Source = Source;
            this.Diatonic = Diatonic;
            this.Chromatic = Chromatic;
        }

        /// <summary>
        /// コンストラクタ(XDoxument版)
        /// </summary>
        /// <param name="SourceElm"></param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatException"></exception>
        public Transpose(XElement SourceElm)
        {
            //ソース読み取り
            XElement? ElmDiatonic = SourceElm.Element("diatonic");
            XElement? ElmChromatic = SourceElm.Element("chromatic");

            //ノード保存
            this.Source = SourceElm;
            //<transpose><diatonic>
            if (ElmDiatonic != null)
            {
                if (!int.TryParse(ElmDiatonic.Value, out int RawTansDiatonicInt))
                {
                    throw new ArgumentException("<attributes><transpose>: <diatonic>: Invalid value.");
                }
                this.Diatonic = RawTansDiatonicInt;
            }
            //<transpose><chromatic>
            if (ElmChromatic != null)
            {
                if (!int.TryParse(ElmChromatic.Value, out int RawTansChromaticInt))
                {
                    throw new ArgumentException("<attributes><transpose>: <chromatic>: Invalid value.");
                }
                this.Chromatic = RawTansChromaticInt;
            }
        }

        #endregion

        #region "public methods"

        /// <summary>
        /// クローンの作成
        /// </summary>
        /// <returns></returns>
        public Transpose Clone()
        {
            return (Transpose)this.MemberwiseClone();
        }

        /// <summary>
        /// コンサートキーへ移調(削除ではなく0に更新
        /// </summary>
        public void TransposeToConcertKey()
        {
            //移調なしに更新
            if (this.Diatonic != null)
            {
                this.Diatonic = 0;
            }
            this.Chromatic = 0;
            //XDocumentに反映
            UpdateXml();
        }

        #endregion

        #region "private methods"

        /// <summary>
        /// XDocumentへの反映
        /// </summary>
        private void UpdateXml()
        {
            if (this.Source != null)
            {
                if (this.Diatonic != null)
                {
                    this.Source.SetElementValue("diatonic", this.Diatonic.ToString());
                }
                this.Source.SetElementValue("chromatic", this.Chromatic.ToString());
            }
            else
            {
                throw new NullReferenceException("Instance has no source XElement.");
            }
        }

        #endregion

        #region "debug methods"

        /// <summary>
        /// デバック用ダンプ
        /// </summary>
        /// <returns></returns>
        public override string DebugDump()
        {
            string Dump = string.Empty;

            Dump += "<transpose>";
            Dump += "<diatonic>" + Diatonic.ToString();
            Dump += "<crhomatic>" + Chromatic.ToString();
            return Dump;
        }

        #endregion
    }
}
