using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Services;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Viewhelper;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    public class TestscoreController : BaseController
    {
        //
        // GET: /Admin/Testscore/
        private String folderDiemThi = "";

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            TestscoreViewhelper testscoreViewhelper = new TestscoreViewhelper();
            saveData(testscoreViewhelper);
            return View();
        }

        [HttpPost]
        public ActionResult Index(TestscoreViewhelper testscoreViewhelper)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(testscoreViewhelper);
            return View();
        }

        public TestscoreViewhelper saveData(TestscoreViewhelper testscoreViewhelper)
        {
            List<gov_testscore> lstTestscore = _cnttDB.gov_testscore.ToList();
            lstTestscore = setSearchFilter(lstTestscore, testscoreViewhelper);

            int totalCount = lstTestscore.Count;
            testscoreViewhelper.TotalCount = totalCount;

            if (testscoreViewhelper.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                testscoreViewhelper.TotalPage = totalPage;
                testscoreViewhelper.Page = pageTransition(testscoreViewhelper.Direction, testscoreViewhelper.Page, totalPage);
                testscoreViewhelper.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, testscoreViewhelper.Page);
                testscoreViewhelper.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, testscoreViewhelper.Page, testscoreViewhelper.FirstPage);
                int take = Constant.limit;
                int skip = (testscoreViewhelper.Page - 1) * take;
                testscoreViewhelper.LstTestscore = lstTestscore.OrderByDescending(m => m.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["testscoreViewhelper"] = testscoreViewhelper;
            return testscoreViewhelper;
        }

        public List<gov_testscore> setSearchFilter(List<gov_testscore> lstTestscore, TestscoreViewhelper TestscoreViewhelper)
        {
            Expression<Func<gov_testscore, bool>> predicate = PredicateBuilder.False<gov_testscore>();
            if (!String.IsNullOrWhiteSpace(TestscoreViewhelper.KeySearch))
            {
                predicate = predicate.Or(d => d.test_class != null && d.test_class.ToUpper().Contains(TestscoreViewhelper.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.test_name != null && d.test_name.ToUpper().Contains(TestscoreViewhelper.KeySearch.ToUpper()));
            }
            return lstTestscore;
        }

        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["data"] = _cnttDB.gov_system_config.ToList();
            return View();
        }

        public ActionResult SaveRegist(gov_testscore item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            if (Request.Files.Count > 0)
            {
                var size = Request.Files.Count;
                var lstFileName = "";
                for (int i = 0; i < size; i++)
                {
                    // Add file in App_data
                    var fileName = string.Empty;
                    var file = Request.Files[i];
                    var bytes = new byte[file.ContentLength];
                    if (bytes.Length > 0)
                    {
                        file.InputStream.Read(bytes, 0, file.ContentLength);
                        fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);
                        fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileName;

                        var fileDir = FileRepository.RootStorage;
                        if (!System.IO.Directory.Exists(fileDir))
                            System.IO.Directory.CreateDirectory(fileDir);
                        var filePath = fileDir + "\\" + fileName;
                        System.IO.File.WriteAllBytes(filePath, bytes);

                        //Add file in Upload
                        var fileDirChirld = FileRepository.ChirldStorage;
                        if (!System.IO.Directory.Exists(fileDirChirld))
                            System.IO.Directory.CreateDirectory(fileDirChirld);
                        var filePathChirld = fileDirChirld + "\\" + fileName;
                        System.IO.File.WriteAllBytes(filePathChirld, bytes);
                        lstFileName += fileName;
                        if((i + 1) != size)
                            lstFileName += Constant.COLON;
                    }
                }
                item.attach_file_name = lstFileName;
            }
            item.entry_datetime = DateTime.Now;
            item.entry_username = Session.getCurrentUser().username;
            item.update_datetime = DateTime.Now;
            item.update_username = Session.getCurrentUser().username;
            try
            {
                _cnttDB.gov_testscore.Add(item);
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.themMoiDiemThi, Constant.THEM(Constant.ITEM_DIEMTHI, Constant.ID, item.id.ToString()));
                    TempData["message"] = "Thêm mới thông tin thành công!";
                }
                else
                {
                    TempData["err"] = "Đã có lỗi xảy ra. Thêm mới thông tin thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = "Đã có lỗi xảy ra. Thêm mới thông tin thất bại!";
            }
            return Redirect("Index");
        }

        public ActionResult Edit(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["testscoreInfo"] = _cnttDB.gov_testscore.Find(id);
            ViewData["data"] = _cnttDB.gov_system_config.ToList();
            return View();
        }

        public ActionResult SaveEdit(gov_testscore item)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            gov_testscore testscoreInfo = _cnttDB.gov_testscore.Find(item.id);
            if (Request.Files.Count > 0)
            {
                //Delete file
                foreach (var file_delete in Utils.getFileInSplip(testscoreInfo.attach_file_name, ':'))
                {
                    //Delete file App_data
                    var filePathDelete = FileRepository.RootStorage + "\\" + file_delete;
                    if (System.IO.File.Exists(filePathDelete))
                    {
                        System.IO.File.Delete(filePathDelete);
                    }

                    //Delete file Upaload
                    filePathDelete = FileRepository.ChirldStorage + "\\" + file_delete;
                    if (System.IO.File.Exists(filePathDelete))
                    {
                        System.IO.File.Delete(filePathDelete);
                    }
                }

                var size = Request.Files.Count;
                var lstFileName = "";
                for (int i = 0; i < size; i++)
                {
                    // Add file in App_data
                    var fileName = string.Empty;
                    var file = Request.Files[i];
                    var bytes = new byte[file.ContentLength];
                    if (bytes.Length > 0)
                    {
                        file.InputStream.Read(bytes, 0, file.ContentLength);
                        fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);
                        fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileName;

                        var fileDir = FileRepository.RootStorage;
                        if (!System.IO.Directory.Exists(fileDir))
                            System.IO.Directory.CreateDirectory(fileDir);
                        var filePath = fileDir + "\\" + fileName;
                        System.IO.File.WriteAllBytes(filePath, bytes);

                        //Add file in Upload
                        var fileDirChirld = FileRepository.ChirldStorage;
                        if (!System.IO.Directory.Exists(fileDirChirld))
                            System.IO.Directory.CreateDirectory(fileDirChirld);
                        var filePathChirld = fileDirChirld + "\\" + fileName;
                        System.IO.File.WriteAllBytes(filePathChirld, bytes);
                        lstFileName += fileName;
                        if ((i + 1) != size)
                            lstFileName += Constant.COLON;
                    }
                }
                /*var fileName = string.Empty;
                var file = Request.Files[0];
                var bytes = new byte[file.ContentLength];
                if (bytes.Length > 0)
                {
                    //Delete file App_data
                    var filePathDelete = FileRepository.RootStorage + "\\" + testscoreInfo.attach_file_name;
                    if (System.IO.File.Exists(filePathDelete))
                    {
                        System.IO.File.Delete(filePathDelete);
                    }

                    //Delete file Upaload
                    filePathDelete = FileRepository.ChirldStorage + "\\" + testscoreInfo.attach_file_name;
                    if (System.IO.File.Exists(filePathDelete))
                    {
                        System.IO.File.Delete(filePathDelete);
                    }

                    // App_data
                    file.InputStream.Read(bytes, 0, file.ContentLength);
                    fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);
                    fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileName;

                    var fileDir = FileRepository.RootStorage;
                    if (!System.IO.Directory.Exists(fileDir))
                        System.IO.Directory.CreateDirectory(fileDir);
                    var filePath = fileDir + "\\" + fileName;
                    System.IO.File.WriteAllBytes(filePath, bytes);

                    //Add file in Upload
                    var fileDirChirld = FileRepository.ChirldStorage;
                    if (!System.IO.Directory.Exists(fileDirChirld))
                        System.IO.Directory.CreateDirectory(fileDirChirld);
                    var filePathChirld = fileDirChirld + "\\" + fileName;
                    System.IO.File.WriteAllBytes(filePathChirld, bytes);

                    String fileFormat = fileName.Substring(fileName.LastIndexOf("."));
                    testscoreInfo.file_format = fileFormat;
                    testscoreInfo.attach_file_name = fileName;
                }*/
                testscoreInfo.attach_file_name = lstFileName;
            }

            testscoreInfo.test_class = item.test_class;
            testscoreInfo.namhoc = item.namhoc;
            testscoreInfo.test_name = item.test_name;
            testscoreInfo.kythi = item.kythi;
            testscoreInfo.update_datetime = DateTime.Now;
            testscoreInfo.update_username = Session.getCurrentUser().username;
            try
            {
                int rs = _cnttDB.SaveChanges();
                if (rs > 0)
                {
                    insertHistory(AccessType.chinhSuaDiemThi, Constant.CHINHSUA(Constant.ITEM_DIEMTHI, Constant.ID, item.id.ToString()));
                    TempData["message"] = "Cập nhật thông tin thành công!";
                }
                else
                {
                    TempData["err"] = "Đã có lỗi xảy ra. Cập nhật thông tin thất bại!";
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = "Đã có lỗi xảy ra. Cập nhật thông tin thất bại!";
            }
            return Redirect("Index");
        }

        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_DIEMTHI, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            gov_testscore testScoreInfo = _cnttDB.gov_testscore.Find(id);
            if (testScoreInfo != null)
            {
                try
                {
                    _cnttDB.gov_testscore.Remove(testScoreInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        foreach (var file_delete in Utils.getFileInSplip(testScoreInfo.attach_file_name, ':'))
                        {
                            //Delete file App_data
                            var filePathDelete = FileRepository.RootStorage + "\\" + file_delete;
                            if (System.IO.File.Exists(filePathDelete))
                            {
                                System.IO.File.Delete(filePathDelete);
                            }

                            //Delete file Upaload
                            filePathDelete = FileRepository.ChirldStorage + "\\" + file_delete;
                            if (System.IO.File.Exists(filePathDelete))
                            {
                                System.IO.File.Delete(filePathDelete);
                            }
                        }
                        /*
                        //insertHistory(AccessType.xoaDanhMuc, Constant.XOA(Constant.ITEM_DANHMUC, Constant.ID, id.ToString()));
                        var filePath = FileRepository.RootStorage + "\\" + testScoreInfo.attach_file_name;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }

                        //Delete file Upload
                        filePath = FileRepository.ChirldStorage + "\\" + testScoreInfo.attach_file_name;
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }*/
                        insertHistory(AccessType.xoaDiemThi, Constant.XOA(Constant.ITEM_DIEMTHI, Constant.ID, id.ToString()));
                        TempData["message"] = "Xóa thông tin thành công!";
                    }
                    else
                    {
                        TempData["err"] = "Đã có lỗi xảy ra. Xóa thông tin thất bại!";
                    }
                }
                catch (Exception ex)
                {
                    return Redirect("/admin/error/error404");
                }
            }
            else
            {
                return Redirect("/admin/error/error405");
            }
            return Redirect("Index");
        }
    }
}
