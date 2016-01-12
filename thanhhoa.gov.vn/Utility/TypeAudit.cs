using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace thanhhoa.gov.vn.Utility
{

    public enum TypeAudit
    {
        Addnew = 1,
        Edit = 2,
        Delete = 4,
        ManagerComment = 8,
        ManagerStudent = 16,
        ManagerNotebook = 32,
        Video = 64,
        Album = 128,
        Gallery = 256,
        QuestionCategory = 512,
        AnswerQuestion = 1024,
        GroupUser = 2048,
        TruyCapHeThong = 4096,
        CoCauToChuc = 8192,
        QuangCao = 16384,
        CauHinhHeThong = 32768,
        SlideTrangChu = 65536,
        ChucVu = 131072,
        BackupDatabase = 262144,
        Course=524288
    }

    public enum TypeModule
    {
        DANHMUC = 1,
        MODULE_TINTUC = 2,
        MODULE_VANBAN = 3,
        MODULE_MEDIA = 4,
        MODULE_HOIDAP = 5,
        MODULE_NGUOIDUNG = 6,
        MODULE_HETHONG = 7,
        MODULE_CUUSINHVIEN = 8,
        MODULE_DIEMTHI = 9
    }

    public enum AccessModule { 
        truyCap = 1,
        danhMuc = 2,
        tinTuc = 3,
        binhLuan = 4,
        video = 5,
        album = 6,
        hinhAnh = 7,
        nguoiDung = 8,
        nhomNguoiDung = 9,
        vanBan = 10,
        slideTrangChu = 11,
        cuuSinhVien = 12,
        luuButRaTruong = 13,
        heThong = 14,
        diemThi = 15
    }

    public enum AccessType
    {
        //1.Truy cập
        dangNhap = 1,
        dangXuat = 2,

        //2. Danh mục
        themMoiDanhMuc = 3,
        chinhSuaDanhMuc = 4,
        xoaDanhMuc = 5,
        chuyenTrangThaiDanhMuc = 6,

        //3.Tin tức
        themMoiTinTuc = 7,
        chinhSuaTinTuc = 8,
        xoaTinTuc = 9,
        chuyenTrangThaiTinTuc = 10,

        //4.Bình luận
        xoaBinhLuan = 11,
        chuyenTrangThaiBinhLuan = 12,
        xoaKeyBinhLuan = 53,
        themKeyBinhLuan = 54,

        //5.Video
        themMoiVideo = 13,
        chinhSuaVideo = 14,
        xoaVideo = 15,

        //6.Album
        themMoiAlbum = 16,
        chinhSuaAlbum = 17,
        xoaAlbum = 18,
        themAnhVaoAlbum = 19,
        XoaAnhKhoiAlbum = 20,

        //7.Hình ảnh
        themMoiAnh = 21,
        chinhSuaAnh = 22,
        xoaAnh = 23,

        //8.Người dùng
        themMoiUser = 24,
        chinhSuaUser = 25,
        xoaUser = 26,
        chuyenTrangThai = 27,

        //9. Nhóm người dùng
        themMoiGroup = 28,
        chinhSuaGroup = 29,
        xoaGroup = 30,
        themNguoiDungVaoGroup = 31,
        xoaNguoiDungKhoiGroup = 32,

        //10. Văn bản
        themMoiVanBan = 33,
        chinhSuaVanBan = 34,
        xoaVanBan = 35,

        //11. Slide Trang chủ
        themMoiSlide = 36,
        chinhSuaSlide = 37,
        xoaSlide = 38,

        //12. Cựu sinh viên
        themMoiCuuSinhVien = 39,
        chinhSuaCuuSinhVien = 40,
        xoaCuuSinhVien = 41,
        chuyenTrangThaiCuuSinhVien = 42,

        //13. Lưu bút ra trường
        chinhSuaLuuBut = 43,
        xoaLuuBut = 44,
        chuyenTrangThaiLuuBut = 45,

        //14. Hệ thống
        themMoiCoCauToChuc= 46,
        chinhSuaCoCauToChuc= 47,
        xoaCoCauToChuc = 48,
        themMoiQuangCao = 49,
        chinhSuaQuangCao = 50,
        xoaQuangCao = 51,
        cauHinhHeThong = 52,
        themMoiChucVu = 55,
        chinhSuaChuVu = 56,
        xoaChucVu = 57,

        //15.Diem Thi
        themMoiDiemThi = 58,
        chinhSuaDiemThi = 59,
        xoaDiemThi = 60,

        //16. Khoa hoc
        themMoiKhoaHoc = 61,
        chinhSuaKhoaHoc = 62,
        xoaKhoaHoc = 63
    }

    public enum TypeLink
    { 
        tintuc = 1,
        danhmuc = 2,
        album = 3,
        video = 4
    }
}