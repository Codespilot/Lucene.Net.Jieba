using Lucene.Net.Analysis.TokenAttributes;
using Lucene.Net.Analysis;
using Lucene.Net.Jieba.Segment;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace Lucene.Net.Jieba;

public class JiebaTokenizer : Tokenizer
{
    private string _inputText;

    private const string STOP_WORDS_PATH = "Resources/stopwords.txt";

    private readonly JiebaSegment _segment;
    private readonly TokenizerMode _mode;
    private ICharTermAttribute _termAtt;
    private IOffsetAttribute _offsetAtt;
    private IPositionIncrementAttribute _posIncrAtt;
    private ITypeAttribute _typeAtt;

    private readonly List<Segment.Token> _wordList = new List<Segment.Token>();

    private IEnumerator<Segment.Token> _iter;

    public JiebaTokenizer(TextReader input, TokenizerMode mode)
        : base(AttributeFactory.DEFAULT_ATTRIBUTE_FACTORY, input)
    {
        _segment = new JiebaSegment();
        _mode = mode;
        LoadStopWords();
        Init();
    }

    public Dictionary<string, int> StopWords { get; } = new Dictionary<string, int>();

    private void LoadStopWords()
    {
        var fileProvider = new EmbeddedFileProvider(GetType().GetTypeInfo().Assembly);
        var fileInfo = fileProvider.GetFileInfo(STOP_WORDS_PATH);

        using (var reader = new StreamReader(fileInfo.CreateReadStream()))
        {
            string value;
            while ((value = reader.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(value))
                    continue;
                if (StopWords.ContainsKey(value))
                    continue;
                StopWords.Add(value, 1);
            }
        }
    }

    private void Init()
    {
        _termAtt = AddAttribute<ICharTermAttribute>();
        _offsetAtt = AddAttribute<IOffsetAttribute>();
        _posIncrAtt = AddAttribute<IPositionIncrementAttribute>();
        _typeAtt = AddAttribute<ITypeAttribute>();
    }

    private string ReadToEnd(TextReader input)
    {
        return input.ReadToEnd();
    }

    public sealed override bool IncrementToken()
    {
        ClearAttributes();

        var word = Next();
        if (word != null)
        {
            var buffer = word.ToString();
            _termAtt.SetEmpty().Append(buffer);
            _offsetAtt.SetOffset(CorrectOffset(word.StartOffset), CorrectOffset(word.EndOffset));
            _typeAtt.Type = word.Type;
            return true;
        }

        End();
        Dispose();
        return false;
    }

    private Analysis.Token Next()
    {
        var res = _iter.MoveNext();
        if (!res)
        {
            return null;
        }

        var word = _iter.Current;
        if (word == null)
        {
            return null;
        }

        var token = new Analysis.Token(word.Word, word.StartIndex, word.EndIndex);
        return token;
    }

    public override void Reset()
    {
        base.Reset();

        _inputText = ReadToEnd(m_input);
        RemoveStopWords(_segment.Tokenize(_inputText, _mode));

        _iter = _wordList.GetEnumerator();
    }

    private void RemoveStopWords(IEnumerable<Segment.Token> words)
    {
        _wordList.Clear();

        foreach (var x in words)
        {
            if (!StopWords.ContainsKey(x.Word))
            {
                _wordList.Add(x);
            }
        }
    }
}