using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Core;
using Lucene.Net.Analysis.TokenAttributes;
using System.IO;
using Lucene.Net.Jieba.Segment;

namespace Lucene.Net.Jieba
{
    public class JiebaAnalyzer : Analysis.Analyzer
    {
        private readonly TokenizerMode _mode;

        public JiebaAnalyzer(TokenizerMode mode)
        {
            _mode = mode;
        }

        protected override TokenStreamComponents CreateComponents(string filedName, TextReader reader)
        {
            var tokenizer = new JiebaTokenizer(reader, _mode);

            var tokenStream = (TokenStream) new LowerCaseFilter(Lucene.Net.Util.LuceneVersion.LUCENE_48, tokenizer);

            tokenStream.AddAttribute<ICharTermAttribute>();
            tokenStream.AddAttribute<IOffsetAttribute>();

            return new TokenStreamComponents(tokenizer, tokenStream);
        }
    }
}