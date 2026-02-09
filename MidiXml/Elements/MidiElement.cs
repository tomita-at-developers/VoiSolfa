using System.ComponentModel;
using System.Reflection;

namespace Developers.MidiXml.Elements
{
    /// <summary>
    /// MusicXmlのElementが持つ基本情報
    /// </summary>
    abstract public class MidiElement
    {
        abstract public string DebugDump();
    }
}
