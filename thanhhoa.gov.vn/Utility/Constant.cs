//using Microsoft.Office.Interop.Word;
//using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Utility
{
    public class Constant
    {
        public static int limit = 20;
        public static int maxPageLine = 5;

        public static String CONFIG_KEY_NAME = "office_name";
        public static String CONFIG_KEY_ADDRESS = "adress";
        public static String CONFIG_KEY_TEL = "tel";
        public static String CONFIG_KEY_FAX = "fax";
        public static String CONFIG_KEY_EMAIL = "email";
        public static String CONFIG_KEY_EMAIL_CONTACT = "email_contact";
        public static String CONFIG_KEY_PASS = "password";
        public static String CONFIG_KEY_TIME_WORK = "time_work";
        public static String CONFIG_KEY_MAPSX = "maps_x";
        public static String CONFIG_KEY_MAPSY = "maps_y";
        public static String CONFIG_KEY_LOGO_FILE_PATH = "logo_file_path";
        public static String CONFIG_KEY_BANNER_FILE_PATH = "banner_file_path";
        public static String CONFIG_KEY_SLIDE_BANNER_FILE_PATH = "slide_banner_file_path";
        public static String CONFIG_KEY_BEGIN_COURSE = "begin_course";
        public static String CONFIG_KEY_END_COURSE = "end_course";

        public static int FILTER_KIND_LEFT = 1;
        public static int FILTER_KIND_MIDDLE = 2;
        public static int FILTER_KIND_RIGHT = 3;
        public static int FILTER_KIND_FULL = 4;

        public static String ORDER_ASC = "ASC";
        public static String ORDER_DESC = "DESC";

        public static String USERNAME = "tài khoản";
        public static String ID = "ID";

        public static String DANG_NHAP = "Đăng nhập hệ thống quản trị";
        public static String DANG_XUAT = "Đăng xuất hệ thống quản trị";
        public static String ITEM_USER = "người dùng";
        public static String ITEM_GROUP = "nhóm người dùng";
        public static String ITEM_VIDEO = "video";
        public static String ITEM_ALBUM = "album";
        public static String ITEM_HINHANH = "hình ảnh";
        public static String ITEM_QUANGCAO = "quảng cáo";
        public static String ITEM_SLIDE = "slide trang chủ";
        public static String ITEM_TINTUC = "tin tức";
        public static String ITEM_VANBAN = "văn bản";
        public static String ITEM_BINHLUAN = "bình luận";
        public static String ITEM_BANDBINHLUAN = "từ khóa band bình luận";
        public static String ITEM_DANHMUC = "danh mục";
        public static String ITEM_CUUSINHVIEN = "cựu sinh viên";
        public static String ITEM_LUUBUTRATRUONG = "lưu bút ra trường";
        public static String ITEM_COCAUTOCHUC = "cơ cấu tổ chức";
        public static String ITEM_CAUHINHEHTHONG = "cấu hình hệ thống";
        public static String ITEM_CHUCVU = "chức vụ";
        public static String ITEM_DIEMTHI = "điểm thi";
        public static String ITEM_COURSE = "khoá học";

        public static String URL_NEW = "new";
        public static String URL_CHANEL = "chanel";

        public static String REGIST_SUCCESSFULL = "Thêm mới thông tin thành công!";
        public static String REGIST_ERR = "Đã có lỗi xảy ra. Thêm mới thông tin thất bại!";
        public static String EDIT_SUCCESSFULL = "Cập nhật thông tin thành công!";
        public static String EDIT_ERR = "Đã có lỗi xảy ra. Cập nhật thông tin thất bại!";
        public static String DELETE_SUCCESSFULL = "Xóa thông tin thành công!";
        public static String DELETE_ERR = "Đã có lỗi xảy ra. Xóa thông tin thất bại!";
        public static String CHANGE_STATE_SUCCESSFULL = "Chuyển trạng thái thành công!";
        public static String CHANGE_STATE_ERR = "Đã có lỗi xảy ra. Chuyển trạng thái thất bại!";
        public static String SYSTEM_CONFIG_SUCCESSFULL = "Cấu hình thông tin thành công!";
        public static String SYSTEM_CONFIG_ERR = "Cấu hình thông tin thất bại!";

        public static String BACKUP_FOLDER_PATH = "/App_Data/BackupDatabase/";

        public static int MESSAGE_TYPE_COMMENT = 1;
        public static int MESSAGE_TYPE_STUDENT = 2;
        public static int MESSAGE_TYPE_NOTEBOOK = 3;
        public static int MESSAGE_TYPE_OTHER = 99;

        public static String MESSAGE_COMMENT_CONTENT = " thêm mới bình luận!";
        public static String MESSAGE_COMMENT_STUDENT = " thêm mới cựu sinh viên!";
        public static String MESSAGE_EDIT_STUDENT = " thay đổi hồ sơ cựu sinh viên!";
        public static String MESSAGE_COMMENT_NOTEBOOK = " thêm mới lưu bút ra trường!";

        public static String AVATAR_DEFAULT = "/Images/avatar_default.jpg";

        //Format lich su truy cap
        public static String THEM(String agr0, String agr1, String agr2){
            return String.Format("Thêm mới {0} có {1}: {2}", agr0, agr1, agr2);
        }
        public static String THEM_IN(String agr0, String agr1, String agr2, String agr3, String agr4, String agr5)
        {
            return String.Format("Thêm mới {0} có {1}: {2} vào {3} có {4}: {5}", agr0, agr1, agr2, agr3, agr4, agr5);
        } 
        public static String CHINHSUA(String agr0, String agr1, String agr2){ 
            return String.Format("Chỉnh sửa {0} có {1}: {2}", agr0, agr1, agr2);
        }
        public static String XOA(String agr0, String agr1, String agr2){
            return String.Format("Xóa {0} có {1}: {2}", agr0, agr1, agr2);
        }
        public static String XOA_IN(String agr0, String agr1, String agr2, String agr3, String agr4, String agr5)
        {
            return String.Format("Xóa {0} có {1}: {2} trong {3} có {4}: {5}", agr0, agr1, agr2, agr3, agr4, agr5);
        }
        public static String CHUYEN_TRANG_THAI(String agr0, String agr1, String agr2, String agr4)
        {
            return String.Format("Chuyển trạng thái {0} có ID: {1}  từ: {2} &rarr; {3}", agr0, agr1, agr2, agr4);
        }

        
        //Admin
        /*private static void Convert(string docFilePath, string htmlFilePath, WdSaveFormat format)
        {

            DirectoryInfo dirInfo = new DirectoryInfo(docFilePath);
            FileInfo wordFile = new FileInfo(docFilePath);
            //
            object oMissing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.Application word = new Microsoft.Office.Interop.Word.Application();
            try
            {
                word.Visible = false;
                word.ScreenUpdating = false;
                //
                Object filename = (Object)wordFile.FullName;
                Document doc = word.Documents.Open(ref filename, ref oMissing,
                                                   ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                                   ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                                                   ref oMissing, ref oMissing, ref oMissing, ref oMissing);
                try
                {
                    doc.Activate();
                    object outputFileName = htmlFilePath;
                    object fileFormat = format;
                    doc.SaveAs(ref outputFileName,
                               ref fileFormat, ref oMissing, ref oMissing,
                               ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                               ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                               ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                }
                finally
                {
                    object saveChanges = WdSaveOptions.wdDoNotSaveChanges;
                    ((_Document)doc).Close(ref saveChanges, ref oMissing, ref oMissing);
                    doc = null;
                }
            }
            finally
            {
                ((_Application)word).Quit(ref oMissing, ref oMissing, ref oMissing);
                word = null;
            }
        }*/
    }
}