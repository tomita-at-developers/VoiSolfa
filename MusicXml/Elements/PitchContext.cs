namespace Developers.MusicXml.Elements
{
    /// <summary>
    /// 音の位置づけ情報
    /// </summary>
    /// <param name="ChromaticIndex"></param>
    /// <param name="EnharmonicIndex"></param>
    /// <param name="HasEnharmonic"></param>
    public class PitchContext
    {
        public int DiatonicIndex { get; init; }
        public int DiatonicAlter { get; init; }
        /// <summary>
        /// 半音階のインデックス(ルート音からの変位(0-based)
        /// </summary>
        public int ChromaticIndex { get; init; }
        /// <summary>
        /// 同音異名のインデックス(HasEnharmonic=tureの場合に意味を持つ。0はシャープ音、1はフラット音。)
        /// </summary>
        public int EnharmonicIndex { get; init; }

        public PitchContext(int ChromaticIndex, int EnharmonicIndex)
        {
            const int _SHARP = +1;
            const int _NATURAL = 0;
            const int _FLAT = -1;

            this.ChromaticIndex = ChromaticIndex;
            this.EnharmonicIndex = EnharmonicIndex;
            switch (ChromaticIndex)
            {
                //ド
                case 0:
                    this.DiatonicIndex = 0;
                    this.DiatonicAlter = _NATURAL;
                    break;
                //ド#, レb
                case 1:
                    if (EnharmonicIndex == 0)
                    {
                        this.DiatonicIndex = 0;
                        this.DiatonicAlter = _SHARP;
                    }
                    else
                    {
                        this.DiatonicIndex = 1;
                        this.DiatonicAlter = _FLAT;
                    }
                    break;
                //レ
                case 2:
                    this.DiatonicIndex = 1;
                    this.DiatonicAlter = _NATURAL;
                    break;
                //レ#, ミb
                case 3:
                    if (EnharmonicIndex == 0)
                    {
                        this.DiatonicIndex = 1;
                        this.DiatonicAlter = _SHARP;
                    }
                    else
                    {
                        this.DiatonicIndex = 2;
                        this.DiatonicAlter = _FLAT;
                    }
                    break;
                //ミ
                case 4:
                    this.DiatonicIndex = 2;
                    this.DiatonicAlter = _NATURAL;
                    break;
                //ファ
                case 5:
                    this.DiatonicIndex = 3;
                    this.DiatonicAlter = _NATURAL;
                    break;
                //ファ#, ソb
                case 6:
                    if (EnharmonicIndex == 0)
                    {
                        this.DiatonicIndex = 3;
                        this.DiatonicAlter = _SHARP;
                    }
                    else
                    {
                        this.DiatonicIndex = 4;
                        this.DiatonicAlter = _FLAT;
                    }
                    break;
                //ソ
                case 7:
                    this.DiatonicIndex = 4;
                    this.DiatonicAlter = _NATURAL;
                    break;
                //ソ#, ラb
                case 8:
                    if (EnharmonicIndex == 0)
                    {
                        this.DiatonicIndex = 4;
                        this.DiatonicAlter = _SHARP;
                    }
                    else
                    {
                        this.DiatonicIndex = 5;
                        this.DiatonicAlter = _FLAT;
                    }
                    break;
                //ラ
                case 9:
                    this.DiatonicIndex = 5;
                    this.DiatonicAlter = _NATURAL;
                    break;
                //ラ#, シb
                case 10:
                    if (EnharmonicIndex == 0)
                    {
                        this.DiatonicIndex = 5;
                        this.DiatonicAlter = _SHARP;
                    }
                    else
                    {
                        this.DiatonicIndex = 6;
                        this.DiatonicAlter = _FLAT;
                    }
                    break;
                //シ
                case 11:
                    this.DiatonicIndex = 6;
                    this.DiatonicAlter = _NATURAL;
                    break;
            }
        }
    }
}
