using System.Collections.Generic;

namespace Lucene.Net.Jieba.Segment;

public class DefaultDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public new TValue this[TKey key]
    {
        get
        {
            if (!ContainsKey(key))
            {
                Add(key, default);
            }
            return base[key];
        }
        set => base[key] = value;
    }
}
