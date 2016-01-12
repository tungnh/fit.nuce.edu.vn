using thanhhoa.gov.vn.Models;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Lucene.Net.Search.Highlight;
using Lucene.Net.Analysis;

namespace thanhhoa.gov.vn.Utility
{
    public class LuceneSerives
    {
        public abstract class Base<T> where T : class
        {
            protected int hits_limit = Int32.MaxValue;
            protected string _luceneDir = "";
            protected FSDirectory _directoryTemp;
            protected FSDirectory _directory
            {
                get
                {
                    if (_directoryTemp == null) _directoryTemp = FSDirectory.Open(new DirectoryInfo(_luceneDir));
                    if (IndexWriter.IsLocked(_directoryTemp)) IndexWriter.Unlock(_directoryTemp);
                    var lockFilePath = Path.Combine(_luceneDir, "write.lock");
                    if (File.Exists(lockFilePath)) File.Delete(lockFilePath);
                    return _directoryTemp;
                }
            }

            abstract public void _addToLuceneIndex(T sampleData, IndexWriter writer);

            public void AddUpdateLuceneIndex(List<T> sampleDatas)
            {
                // init lucene
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // add data to lucene search index (replaces older entry if any)
                    foreach (var sampleData in sampleDatas) _addToLuceneIndex(sampleData, writer);

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }

            public void AddUpdateLuceneIndex(T sampleData)
            {
                AddUpdateLuceneIndex(new List<T> { sampleData });
            }

            public virtual void ClearLuceneIndexRecord(int record_id)
            {
                // init lucene
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entry
                    var searchQuery = new TermQuery(new Term("Id", record_id.ToString()));
                    writer.DeleteDocuments(searchQuery);

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }

            protected bool ClearLuceneIndex()
            {
                try
                {
                    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                    using (var writer = new IndexWriter(_directory, analyzer, true, IndexWriter.MaxFieldLength.UNLIMITED))
                    {
                        // remove older index entries
                        writer.DeleteAll();

                        // close handles
                        analyzer.Close();
                        writer.Dispose();
                    }
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }

            public void Optimize()
            {
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    analyzer.Close();
                    writer.Optimize();
                    writer.Dispose();
                }
            }

            abstract public T _mapLuceneDocumentToData(Document doc);

            protected IEnumerable<T> _mapLuceneToDataList(IEnumerable<Document> hits)
            {
                return hits.Select(_mapLuceneDocumentToData).ToList();
            }

            protected IEnumerable<T> _mapLuceneToDataList(IEnumerable<ScoreDoc> hits, IndexSearcher searcher)
            {
                return hits.Select(hit => _mapLuceneDocumentToData(searcher.Doc(hit.Doc))).ToList();
            }

            protected Query parseQuery(string searchQuery, QueryParser parser)
            {
                Query query;
                try
                {
                    query = parser.Parse(searchQuery.Trim());
                }
                catch (ParseException)
                {
                    query = parser.Parse(QueryParser.Escape(searchQuery.Trim()));
                }
                return query;
            }

            abstract public IEnumerable<T> _search(string searchQuery, string searchField = "");

            abstract public IEnumerable<T> _searchAdvanced(string searchQuery,  string doc_type, string searchField = "");

            public IEnumerable<T>   Search(string input, string fieldName = "")
            {
                if (string.IsNullOrWhiteSpace(input)) return new List<T>();

                //var terms = input.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim() + "*");
                //input = string.Join(" ", terms);

                return _search(input, fieldName);
            }

            public IEnumerable<T> SearchAdvanced(string searchQuery, string doc_type, string searchField = "")
            {
                if (string.IsNullOrWhiteSpace(searchQuery)) return new List<T>();

                //var terms = input.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim() + "*");
                //input = string.Join(" ", terms);

                return _searchAdvanced(searchQuery, doc_type, searchField);
            }

            public IEnumerable<T> SearchDefault(string input, string fieldName = "")
            {
                return string.IsNullOrWhiteSpace(input) ? new List<T>() : _search(input, fieldName);
            }

            public IEnumerable<T> GetAllIndexRecords()
            {
                // validate search index
                if (!System.IO.Directory.EnumerateFiles(_luceneDir).Any()) return new List<T>();

                // set up lucene searcher
                var searcher = new IndexSearcher(_directory, false);
                var reader = IndexReader.Open(_directory, false);
                var docs = new List<Document>();
                var term = reader.TermDocs();
                while (term.Next()) docs.Add(searcher.Doc(term.Doc));
                reader.Dispose();
                searcher.Dispose();
                return _mapLuceneToDataList(docs);
            }

            protected string convertToUnSign3(string input)
            {
                if (string.IsNullOrEmpty(input)) return string.Empty;
                var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
                string temp = input.Normalize(NormalizationForm.FormD);
                return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace('\u00D0', 'D').Replace('\u0089', 'D');
            }
        }

        public class LuceneDocuments : Base<gov_doc_draft>
        {
            public LuceneDocuments()
            {
                _luceneDir = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("/Lucene/"), "Document");
            }

            public override void _addToLuceneIndex(gov_doc_draft item, IndexWriter writer)
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("id", item.id.ToString()));
                writer.DeleteDocuments(searchQuery);

                // add new index entry
                var doc = new Document();

                // add lucene fields mapped to db fields
                doc.Add(new Field("id", item.id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("doc_name", !string.IsNullOrWhiteSpace(item.doc_name) ? item.doc_name: string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("doc_code", item.doc_code.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("doc_summany", !string.IsNullOrWhiteSpace(item.doc_summany) ? item.doc_summany: string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("doc_content", !string.IsNullOrWhiteSpace(item.doc_content) ? item.doc_content : string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("date_start_promulgate", item.date_start_promulgate.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("user_sign", item.user_sign.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("active_flg", item.active_flg.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("doc_type", item.doc_type.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                String docFullText = item.doc_name + " " + item.doc_code + " " + item.doc_content+ " " + item.doc_summany;
                doc.Add(new Field("doc_full_text", docFullText, Field.Store.YES, Field.Index.ANALYZED));
                String docCodeAndSummany = item.doc_code + " " + item.doc_summany + " " + item.doc_name;
                doc.Add(new Field("doc_code_and_summany", docCodeAndSummany, Field.Store.YES, Field.Index.ANALYZED));
                // add entry to index
                writer.AddDocument(doc);
            }
            public override void ClearLuceneIndexRecord(int record_id)
            {
                // init lucene
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entry
                    var searchQuery = new TermQuery(new Term("id", record_id.ToString()));
                    writer.DeleteDocuments(searchQuery);

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }

            public override gov_doc_draft _mapLuceneDocumentToData(Document doc)
            {
                return new gov_doc_draft
                {
                    id = Convert.ToInt32(doc.Get("id")),
                    doc_name = doc.Get("doc_name"),
                    doc_summany = doc.Get("doc_summany"),
                    doc_code = doc.Get("doc_code")
                    //attach_file_name = doc.Get = doc.Get("doc_code")
                };
            }

            public override IEnumerable<gov_doc_draft> _searchAdvanced(string searchQuery, string doc_type, string searchField = "")
            {
                // validation
                if (string.IsNullOrWhiteSpace(searchQuery.Replace("*", "").Replace("?", ""))) return new List<gov_doc_draft>();

                // set up lucene searcher
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

                    // search by single field
                    if (!string.IsNullOrWhiteSpace(searchField))
                    {
                        List<gov_doc_draft> results = new List<gov_doc_draft>();
                        BooleanQuery booleanQuery = new BooleanQuery();
                        booleanQuery.Add(getQuery(searchQuery, searchField), Occur.MUST);
                        booleanQuery.Add(getQuery(doc_type, "doc_type"), Occur.MUST);
                        var hits = searcher.Search(booleanQuery, null, hits_limit, Sort.RELEVANCE);

                        IFormatter formatter = new SimpleHTMLFormatter("<b style='background-color: yellow;'>", "</b>");
                        SimpleFragmenter fragmenter = new SimpleFragmenter();
                        QueryScorer scorer = new QueryScorer(booleanQuery);
                        Highlighter highlighter = new Highlighter(formatter, scorer);
                        highlighter.TextFragmenter = fragmenter;

                        for (int i = 0; i < hits.TotalHits; i++)
                        {
                            // get the document from index
                            Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);

                            TokenStream streamDocCode = analyzer.TokenStream("", new StringReader(doc.Get("doc_code")));
                            String doc_code = highlighter.GetBestFragments(streamDocCode, doc.Get("doc_code"), 10, "...");
                            if (String.IsNullOrWhiteSpace(doc_code))
                                doc_code = doc.Get("doc_code");

                            TokenStream streamDocSummany = analyzer.TokenStream("", new StringReader(doc.Get("doc_summany")));
                            String doc_summany = highlighter.GetBestFragments(streamDocSummany, doc.Get("doc_summany"), 10, "...");
                            if (String.IsNullOrWhiteSpace(doc_summany))
                                doc_summany = doc.Get("doc_summany");

                            TokenStream streamDocName = analyzer.TokenStream("", new StringReader(doc.Get("doc_name")));
                            String doc_name = highlighter.GetBestFragments(streamDocName, doc.Get("doc_name"), 10, "...");
                            if (String.IsNullOrWhiteSpace(doc_name))
                                doc_name = doc.Get("doc_name");

                            results.Add(new gov_doc_draft
                            {
                                id = Convert.ToInt32(doc.Get("id")),
                                doc_name = doc_name,
                                doc_summany = doc_summany,
                                doc_code = doc_code
                            });
                        } 

                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                    // search by multiple fields (ordered by RELEVANCE)
                    else
                    {
                        var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] { "doc_content" }, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                        var results = _mapLuceneToDataList(hits, searcher);
                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                }
            }

            public override IEnumerable<gov_doc_draft> _search(string searchQuery, string searchField = "")
            {
                // validation
                if (string.IsNullOrWhiteSpace(searchQuery.Replace("*", "").Replace("?", ""))) return new List<gov_doc_draft>();

                // set up lucene searcher
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

                    // search by single field
                    if (!string.IsNullOrWhiteSpace(searchField))
                    {
                        var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, hits_limit).ScoreDocs;
                        var results = _mapLuceneToDataList(hits, searcher);
                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                    // search by multiple fields (ordered by RELEVANCE)
                    else
                    {
                        var parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, new[] { "doc_content" }, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                        var results = _mapLuceneToDataList(hits, searcher);
                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                }
            }
            /*public override IEnumerable<gov_doc_draft> _searchAdvanced(string[] subkeyList, string[] subkeyFiled, int size)
            {
                BooleanQuery booleanQuery = new BooleanQuery();
                for (int i = 0; i < size; i++)
                {
                    booleanQuery.Add(getQuery(subkeyList[i], subkeyFiled[i]), Occur.MUST);
                }
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var hits = searcher.Search(booleanQuery, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    searcher.Dispose();
                    return results;
                }
            }*/

            private Query getQuery(String queryString, String searchField)
            {
                QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30,
                searchField, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));

                Query query = null;
                queryParser.AllowLeadingWildcard = true;
                query = queryParser.Parse(queryString);
                return query;
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------
        //LuceneNew
        public class LuceneNews : Base<gov_news>
        {
            public LuceneNews()
            {
                _luceneDir = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("/Lucene/"), "News");
            }

            public override void _addToLuceneIndex(gov_news item, IndexWriter writer)
            {
                // remove older index entry
                var searchQuery = new TermQuery(new Term("id", item.id.ToString()));
                writer.DeleteDocuments(searchQuery);

                // add new index entry
                var doc = new Document();

                // add lucene fields mapped to db fields
                doc.Add(new Field("id", item.id.ToString(), Field.Store.YES, Field.Index.NOT_ANALYZED));
                doc.Add(new Field("menu_id", item.menu_id.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("title", item.title.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("description", !string.IsNullOrWhiteSpace(item.description) ? Utils.ConvertContentEditor(item.description) : string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("new_content", !string.IsNullOrWhiteSpace(item.new_content) ? Utils.ConvertContentEditor(item.new_content) : string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("total_view", item.total_view.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("avatar", !string.IsNullOrWhiteSpace(item.avatar) ? item.avatar: string.Empty, Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("is_tinlq", item.is_tinlq.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("is_shared", item.is_shared.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("is_home", item.is_home.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("is_comment", item.is_comment.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("active_flg", item.active_flg.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("entry_datetime", item.entry_datetime.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("entry_username", item.entry_username.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("update_datetime", item.update_datetime.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                doc.Add(new Field("update_username", item.update_username.ToString(), Field.Store.YES, Field.Index.ANALYZED));
                
                String fullText = item.title + " " + Utils.ConvertContentEditor(item.new_content);
                if (!String.IsNullOrWhiteSpace(item.description))
                    fullText += " " + Utils.ConvertContentEditor(item.description);
                doc.Add(new Field("full_text", fullText, Field.Store.NO, Field.Index.ANALYZED));
                // add entry to index
                writer.AddDocument(doc);
            }
            public override void ClearLuceneIndexRecord(int record_id)
            {
                // init lucene
                var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                using (var writer = new IndexWriter(_directory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED))
                {
                    // remove older index entry
                    var searchQuery = new TermQuery(new Term("id", record_id.ToString()));
                    writer.DeleteDocuments(searchQuery);

                    // close handles
                    analyzer.Close();
                    writer.Dispose();
                }
            }

            public override gov_news _mapLuceneDocumentToData(Document doc)
            {
                return new gov_news
                {
                    id = Convert.ToInt32(doc.Get("id")),
                    title = doc.Get("title"),
                    description = doc.Get("description"),
                    new_content = doc.Get("new_content"),
                    total_view = Convert.ToInt32(doc.Get("total_view")),
                    avatar = doc.Get("avatar"),
                    active_flg = Convert.ToBoolean(doc.Get("active_flg")),
                    entry_username = doc.Get("entry_username"),
                    update_username = doc.Get("update_username")
                };
            }

            public override IEnumerable<gov_news> _search(string searchQuery, string searchField = "")
            {
                // validation
                if (string.IsNullOrWhiteSpace(searchQuery.Replace("*", "").Replace("?", ""))) return new List<gov_news>();

                // set up lucene searcher
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);

                    // search by single field
                    if (!string.IsNullOrWhiteSpace(searchField))
                    {
                        var parser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30, searchField, analyzer);
                        var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(query, hits_limit).ScoreDocs;
                        var results = _mapLuceneToDataList(hits, searcher);
                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                    // search by multiple fields (ordered by RELEVANCE)
                    else
                    {
                        List<gov_news> results = new List<gov_news>();
                        BooleanQuery booleanQuery = new BooleanQuery();
                        booleanQuery.Add(getQuery(searchQuery, "full_text"), Occur.MUST);
                        booleanQuery.Add(getQuery(Boolean.TrueString, "active_flg"), Occur.MUST);
                        //var query = parseQuery(searchQuery, parser);
                        var hits = searcher.Search(booleanQuery, null, hits_limit, Sort.RELEVANCE);

                        //Hi
                        IFormatter formatter = new SimpleHTMLFormatter("<b style='background-color: yellow;'>", "</b>");
                        SimpleFragmenter fragmenter = new SimpleFragmenter();
                        QueryScorer scorer = new QueryScorer(booleanQuery);
                        Highlighter highlighter = new Highlighter(formatter, scorer);
                        highlighter.TextFragmenter = fragmenter;

                        for (int i = 0; i < hits.TotalHits; i++)
                        {
                            // get the document from index
                            Document doc = searcher.Doc(hits.ScoreDocs[i].Doc);

                            TokenStream streamTitle = analyzer.TokenStream("", new StringReader(doc.Get("title")));
                            String title = highlighter.GetBestFragments(streamTitle, doc.Get("title"), 10, "...");
                            if (String.IsNullOrWhiteSpace(title))
                                title = doc.Get("title");

                            TokenStream streamDescription = analyzer.TokenStream("", new StringReader(doc.Get("description")));
                            String description = highlighter.GetBestFragments(streamDescription, doc.Get("description"), 10, "...");
                            if (String.IsNullOrWhiteSpace(description))
                                description = doc.Get("description");
                            results.Add(new gov_news
                            {
                                id = Convert.ToInt32(doc.Get("id")),
                                title = title,
                                description = description,
                                //title = doc.Get("title"),
                                //description = doc.Get("description"),
                                new_content = doc.Get("new_content"),
                                total_view = Convert.ToInt32(doc.Get("total_view")),
                                avatar = doc.Get("avatar"),
                                active_flg = Convert.ToBoolean(doc.Get("active_flg")),
                                entry_username = doc.Get("entry_username"),
                                update_username = doc.Get("update_username"),
                                score = Convert.ToDouble(hits.ScoreDocs[i].Score)
                            });
                        } 

                        analyzer.Close();
                        searcher.Dispose();
                        return results;
                    }
                }
            }
            public override IEnumerable<gov_news> _searchAdvanced(string subkeyList, string subkeyFiled, string size)
            {
                /*BooleanQuery booleanQuery = new BooleanQuery();
                for (int i = 0; i < size; i++)
                {
                    booleanQuery.Add(getQuery(subkeyList[i], subkeyFiled[i]), Occur.MUST);
                }
                using (var searcher = new IndexSearcher(_directory, false))
                {
                    var hits = searcher.Search(booleanQuery, null, hits_limit, Sort.RELEVANCE).ScoreDocs;
                    var results = _mapLuceneToDataList(hits, searcher);
                    searcher.Dispose();
                    return results;
                }*/
                return null;
            }

            private Query getQuery(String queryString, String searchField)
            {
                QueryParser queryParser = new QueryParser(Lucene.Net.Util.Version.LUCENE_30,
                searchField, new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30));

                Query query = null;
                queryParser.AllowLeadingWildcard = true;
                query = queryParser.Parse(queryString);
                return query;
            }
        }
    }
}