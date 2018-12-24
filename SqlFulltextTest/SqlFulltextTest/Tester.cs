using Dapper;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SqlFulltextTest
{
    class Tester
    {
        string _connStr = "Server=.;Database=Test;Integrated Security=SSPI;";
        Random _rnd = new Random();
        string _part = "Geso";

        #region Public

        public void FillDb()
        {
            Console.Write(">");
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                for (int i = 0; i < 1000; i++)
                {
                    GenerateChunk(conn);
                    Console.Write($"{i} ");
                }
            }
            Console.Write("<");
        }
        
        public void TestQueryLikeWildcard()
        {
            var sql = $"SELECT Name FROM Names WHERE Name LIKE '{_part}%'";
            RunQuery(sql);
        }

        public void TestQueryWildcardLike()
        {
            var sql = $"SELECT Name FROM Names WHERE Name LIKE '%{_part}%'";
            RunQuery(sql);
        }

        public void TestQueryFulltext()
        {
            var sql = $"SELECT Name FROM Names WHERE CONTAINS(Name,'\"{_part}*\"')";
            RunQuery(sql);
        }

        public void TestQueryFulltextInteractive(string part)
        {
            var sql = $"SELECT Name FROM Names WHERE CONTAINS(Name,'\"{part}*\"')";
            RunQuery(sql);
        }

        #endregion

        private void RunQuery(string sql)
        {
            Console.WriteLine($">{sql}");
            using (var conn = new SqlConnection(_connStr))
            {
                conn.Open();
                var result = conn.Query<string>(sql);
                List<string> res = result.ToList();
                for (int i=0; i<Math.Min(res.Count,10); i++)
                    Console.WriteLine($"      {res[i]}");
                Console.Write($"   => {res.Count} results ");
            }
        }

        private void GenerateChunk(SqlConnection conn)
        {
            var trans = conn.BeginTransaction();
            foreach (var s in GenerateNames(1000))
            {
                var sql = $"INSERT INTO Names (Name) VALUES ('{s}')";
                var cmd = new SqlCommand(sql, conn, trans);
                cmd.ExecuteNonQuery();
            }
            trans.Commit();
        }

        private List<string> GenerateNames(int count)
        {
            var li = new List<string>();
            for (int i = 0; i < count; i++)
            {
                var s1 = GenerateName(2, 4);
                var s2 = GenerateName(1, 1);
                var s3 = GenerateName(2, 4);
                li.Add($"{s1} {s2} {s3}");
            }
            return li;
        }

        private string GenerateName(int LenMin, int LenMax)
        {
            const string CharSetUpper = "ABCDEFGHIJKLMNOPQRSTUVWXYZØÅÖÄÜ";
            const string CharSetLowerKons = "bcdfghjklmnpqrstvwxyzÆæ";
            const string CharSetLowerVow = "aaaeeeeiiiioooouuuuøåöäüóòôúùûîíìáàâéèê";
            var l = _rnd.Next(LenMin, LenMax);
            var s = CharSetUpper.Substring(_rnd.Next(0, 25), 1);
            s += CharSetLowerVow.Substring(_rnd.Next(0, CharSetLowerVow.Length - 1), 1);
            for (int i = 0; i < l; i++)
                s += CharSetLowerKons.Substring(_rnd.Next(0, CharSetLowerKons.Length - 1), 1) +
                     CharSetLowerVow.Substring(_rnd.Next(0, CharSetLowerVow.Length - 1), 1);
            return s;
        }

    }

}
