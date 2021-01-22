# JIEba-netcore

基于[jieba.NETCore](https://github.com/linezero/jieba.NET) 

在.net core版的JIEba分词上，做了修改，使其支持net core2.0 和支持[Lucene.net](https://github.com/apache/lucenenet)接口
ps: 修改了JIEba分词，导致的高亮bug

# NuGet


 >[Lucene.Net.Jieba](https://www.nuget.org/packages/Lucene.Net.Jieba/)


# 集成到Lucene.Net示例

```c#
  Analyzer analyzer = new JiebaAnalyzer(TokenizerMode.Search);
  Analyzer analyzer = new JiebaAnalyzer(TokenizerMode.Default);
  TokenStream = analyzer.GetTokenStream(str,indexReader);

```
