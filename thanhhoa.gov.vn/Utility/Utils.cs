using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using thanhhoa.gov.vn.Models;

namespace thanhhoa.gov.vn.Utility
{
    public class Utils
    {
        public static string getLinkNew(int id)
        {
            CNTTDHXDEntities _cnttDB = new CNTTDHXDEntities();
            string rs = "";
            gov_news newInfo = _cnttDB.gov_news.Find(id);
            if (newInfo != null)
            {
                rs = "/new/" + ConvertToUnSign(newInfo.gov_menu.title) + "/" + ConvertToUnSign(newInfo.title) + "-" + id.ToString();
            }
            return rs;
        }

        public static string getLinkMenu(int id)
        {
            CNTTDHXDEntities _cnttDB = new CNTTDHXDEntities();
            string rs = "";
            gov_menu menuInfo = _cnttDB.gov_menu.Find(id);
            if (menuInfo != null)
            {
                rs = "/chanel/" + ConvertToUnSign(menuInfo.title) + "-" + id.ToString();
            }
            return rs;
        }

        public static string getLinkDefault(int id, TypeLink typeLink)
        {
            CNTTDHXDEntities _cnttDB = new CNTTDHXDEntities();
            string rs = "";
            switch((int) typeLink){
                case (int)TypeLink.tintuc :
                    gov_news newInfo = _cnttDB.gov_news.Find(id);
                    if (newInfo != null)
                    {
                        rs = "/new/" + ConvertToUnSign(newInfo.gov_menu.title) + "/" + ConvertToUnSign(newInfo.title) + "-" + id.ToString();
                    }
                    break;
                case (int)TypeLink.danhmuc:
                    gov_menu menuInfo = _cnttDB.gov_menu.Find(id);
                    if (menuInfo != null)
                    {
                        rs = "/chanel/" + ConvertToUnSign(menuInfo.title) + "-" + id.ToString();
                    }
                    break;
                case (int)TypeLink.album:
                    gov_album albumInfo = _cnttDB.gov_album.Find(id);
                    if (albumInfo != null)
                    {
                        rs = "/album/" + ConvertToUnSign(albumInfo.album_title) + "-" + id.ToString();
                    }
                    break;
                case (int)TypeLink.video:
                    gov_video videoInfo = _cnttDB.gov_video.Find(id);
                    if (videoInfo != null)
                    {
                        rs = "/video/" + ConvertToUnSign(videoInfo.title) + "-" + id.ToString();
                    }
                    break;
            }
            return rs;
        }

        public static string ConvertToUnSign(string text)
        {
            //Duyệt qua từng phần tử mảng, chuyển các ký tự thường và đặc biệt từ có dấu thành không dấu (tương ứng với bảng mã ASCII)
            for (int i = 33; i < 48; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 58; i < 65; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 91; i < 97; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            for (int i = 123; i < 127; i++)
            {
                text = text.Replace(((char)i).ToString(), "");
            }

            text = text.Replace(" ", "-"); //Chuyển khoảng trắng giữa các từ trong chuỗi thành "-"

            var regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");

            string strFormD = text.Normalize(NormalizationForm.FormD);

            return regex.Replace(strFormD, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').Replace('\\', '-').ToLower();
        }
        public static bool HassPermission(TypeAudit audit, int permission)
        {
            if (((int)audit & permission) == (int)audit)
                return true;
            return false;
        }

        public static string GetLanIPAddress()
        {
            /*IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            string IPAddress = string.Empty;
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAddress = ip.ToString();
                    break;
                }
            }*/
            string IPHost = Dns.GetHostName();
            string IP = Dns.GetHostByName(IPHost).AddressList[0].ToString();
            return IP;
        }

        public static string GetPublicIPAddress()
        {
            /*try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                return externalIP.Trim();
            }
            catch { return "Không có kết nối Internet"; }*/
            String IPAddress;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"])) 
                IPAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            else 
                IPAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            return IPAddress;
        }

        public static String[] editor = { 
                "&aacute;", "&agrave;", "&atilde;", "&acirc;", "&ocirc;", "&ecirc;", "&yacute;", "&uacute;", "&ugrave;", 
                "&iacute;", "&igrave;", "&oacute;", "&ograve;", "&otilde;", "&eacute;", "&egrave;"};
        public static String[] decode_editor = { 
                "á", "à", "ã", "â", "ô" ,"ê", "ý", "ú", "ù", 
                "í", "ì", "ó", "ò", "õ", "é", "è"};

        public static String ConvertContentEditor(String str)
        {
            if (!String.IsNullOrWhiteSpace(str)) { 
                for (int i = 0; i < editor.Count(); i++)
                {
                    str = str.Replace(editor[i], decode_editor[i]);
                }
            }
            return str;
        }

        public static String[] getFileInSplip(String str, Char delimiter)
        {
            return str.Split(delimiter);
        }
    }
}