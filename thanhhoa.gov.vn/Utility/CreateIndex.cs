using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace thanhhoa.gov.vn.Utility
{
    public class CreateIndex
    {
        private string[] patterns = { ".doc", ".xls", ".ppt", ".htm", ".txt", ".docx", ".xlsx", ".pptx", ".pdf", ".rtf" };
        private DirectoryInfo dir = null;
        private string schema = "";
        private bool first = false;
        public CreateIndex(string DIR, string SCHEMA)
        {
            first = false;

            string strPattern = ConfigurationSettings.AppSettings["DocFilePattern"];
            if (null != strPattern && strPattern.Length > 0)
            {
                string[] arrPattern = strPattern.Split(';');
                this.patterns = new string[arrPattern.Length];
                for (int i = 0; i < arrPattern.Length; i++)
                {
                    this.patterns[i] = "." + arrPattern[i].Trim();
                }
            }

            string rootPath = ConfigurationSettings.AppSettings["DocumentPath"];
            if (!rootPath.EndsWith("/") && !rootPath.EndsWith("\\"))
            {
                rootPath = rootPath + "/";
            }
            dir = new DirectoryInfo(JoinPath(rootPath, DIR));
            if (Directory.Exists(dir.FullName + "index")) first = false;
            else first = true;
            schema = SCHEMA;
        }

        public DirectoryInfo Dir
        {
            set
            {
                this.dir = value;
            }
        }
        public string Schema
        {
            set
            {
                this.schema = value;
            }
        }

        public static string JoinPath(string rootDir, params string[] arrFldName)
        {
            StringBuilder pathBuilder = new StringBuilder();

            pathBuilder.Append(rootDir);
            if (!rootDir.EndsWith("/") && !rootDir.EndsWith("\\"))
            {
                pathBuilder.Append("/");
            }

            foreach (string fldName in arrFldName)
            {
                pathBuilder.Append(fldName);
                pathBuilder.Append("/");
            }

            return pathBuilder.ToString();
        }
        public Document documentIn(System.IO.FileInfo f)
        {

            return null;
        }

        private bool GetFileTypeOK(string type)
        {
            foreach (string type1 in patterns)
            {
                if (type1 == type) return true;
            }
            return false;
        }

        public static String parseToFormat(String strString) {
            return strString.Replace("\n", "<br/>").Replace(" ", " &nbsp").Replace("\t", " &nbsp;&nbsp;&nbsp;&nbsp");
        }
    }
}