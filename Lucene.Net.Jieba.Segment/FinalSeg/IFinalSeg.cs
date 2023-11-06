using System.Collections.Generic;

namespace Lucene.Net.Jieba.Segment.FinalSeg;

public interface IFinalSeg
{
    IEnumerable<string> Cut(string sentence);
}