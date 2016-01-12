using thanhhoa.gov.vn.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using thanhhoa.gov.vn.Models;
using thanhhoa.gov.vn.Utility;
using thanhhoa.gov.vn.Controllers;
using thanhhoa.gov.vn.Viewhelper;
using System.Linq.Expressions;

namespace thanhhoa.gov.vn.Areas.Admin.Controllers
{
    [Authorize]
    public class DocumentController : BaseController
    {
        //
        // GET: /Admin/Document/
        private string fileSaveFolder = "/Upload/Document";
        public ActionResult Regist()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            return View();
        }

        [HttpPost]
        public ActionResult SaveRegist(gov_doc_draft document)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Addnew))
            {
                return Redirect("/admin/error/error403");
            }
            if (Request.Files.Count > 0)
            {
                var fileName = string.Empty;
                var file = Request.Files[0];
                var bytes = new byte[file.ContentLength];
                if (bytes.Length > 0)
                {
                    try
                    {
                        file.InputStream.Read(bytes, 0, file.ContentLength);
                        document.attach_file_name = fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);

                        var fileFolder = fileSaveFolder;
                        var fileDir = Server.MapPath("/") + fileFolder;
                        if (!System.IO.Directory.Exists(fileDir))
                            System.IO.Directory.CreateDirectory(fileDir);
                        var filePath = fileFolder + "\\" + fileName.Substring(0, fileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + fileName.Substring(fileName.LastIndexOf("."));
                        System.IO.File.WriteAllBytes(Server.MapPath("/") + filePath, bytes);
                        document.attach_file_path = filePath.Replace("\\", "/");

                    }
                    catch (Exception ex)
                    {
                        TempData["err"] = "Thêm mới thông tin thất bại. Tên File văn bản không được vượt quá 240 ký tự!";
                        return Redirect("Index");
                    }
                }
            }
            try {
                System.IO.TextReader textreader = new IFilter.FilterReader(Server.MapPath("/") + document.attach_file_path);
                string fileContent = textreader.ReadToEnd();
                document.doc_content = fileContent;
            }catch( Exception ex){

            }
            document.entry_username = Session.getCurrentUser().username; ;
            document.entry_datetime = DateTime.Now;
            document.update_username = Session.getCurrentUser().username; ;
            document.update_datetime = DateTime.Now;
            document.doc_type = true;
            try
            {
                document = _cnttDB.gov_doc_draft.Add(document);
                int rs = _cnttDB.SaveChanges();
                var lucene = new LuceneSerives.LuceneDocuments();
                if (rs > 0)
                {
                    lucene.AddUpdateLuceneIndex(document);
                    insertHistory(AccessType.themMoiVanBan, Constant.THEM(Constant.ITEM_VANBAN, Constant.ID, document.id.ToString()));
                    TempData["message"] = Constant.REGIST_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.REGIST_ERR;
                }
            }
            catch (Exception ex)
            {
                TempData["err"] = Constant.REGIST_ERR;
            }
            return Redirect("Index");
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            DocumentViewhepler documentViewhepler = new DocumentViewhepler();
            saveData(documentViewhepler);
            return View();
        }

        [HttpPost]
        public ActionResult Index(DocumentViewhepler DocumentViewhepler)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Addnew)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Edit)
                && !SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            saveData(DocumentViewhepler);
            return View();
        }

        public ActionResult Delete(int id)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Delete))
            {
                return Redirect("/admin/error/error403");
            }
            gov_doc_draft documentInfo = _cnttDB.gov_doc_draft.Find(id);
            if (documentInfo != null)
            {
                try
                {
                    _cnttDB.gov_doc_draft.Remove(documentInfo);
                    int rs = _cnttDB.SaveChanges();
                    if (rs > 0)
                    {
                        var lucene = new LuceneSerives.LuceneDocuments();
                        lucene.ClearLuceneIndexRecord(id);
                        insertHistory(AccessType.xoaVanBan, Constant.XOA(Constant.ITEM_VANBAN, Constant.ID, id.ToString()));
                        TempData["message"] = Constant.DELETE_SUCCESSFULL;
                    }
                    else
                    {
                        return Redirect("/admin/error/error404");
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

        [HttpGet]
        public ActionResult Edit(int id) {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            ViewData["documentInfo"] = _cnttDB.gov_doc_draft.Find(id);
            return View();
        }

        [HttpPost]
        public ActionResult SaveEdit(gov_doc_draft document)
        {
            if (Session.getCurrentUser() == null)
                return Redirect("/admin/account/logon");
            if (!SercurityServices.HasPermission((int)TypeModule.MODULE_VANBAN, Session.getCurrentUser().username, TypeAudit.Edit))
            {
                return Redirect("/admin/error/error403");
            }
            if (Request.Files.Count > 0)
            {
                var fileName = string.Empty;
                var file = Request.Files[0];
                var bytes = new byte[file.ContentLength];
                if (bytes.Length > 0)
                {
                    try
                    {
                        file.InputStream.Read(bytes, 0, file.ContentLength);
                        document.attach_file_name = fileName = (file.FileName.IndexOf('\\') != -1 ? file.FileName.Substring(file.FileName.LastIndexOf('\\') + 1) : file.FileName);

                        var fileFolder = fileSaveFolder;
                        var fileDir = Server.MapPath("/") + fileFolder;
                        if (!System.IO.Directory.Exists(fileDir))
                            System.IO.Directory.CreateDirectory(fileDir);
                        var filePath = fileFolder + "\\" + fileName.Substring(0, fileName.LastIndexOf(".")) + "_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + fileName.Substring(fileName.LastIndexOf("."));
                        System.IO.File.WriteAllBytes(Server.MapPath("/") + filePath, bytes);
                        document.attach_file_path = filePath.Replace("\\", "/");
                    }
                    catch (Exception ex)
                    {
                        TempData["err"] = "Cập nhật thông tin thất bại. Tên File văn bản không được vượt quá 240 ký tự!";
                        return Redirect("Index");
                    }
                }
            }
            try
            {
                System.IO.TextReader textreader = new IFilter.FilterReader(Server.MapPath("/") + document.attach_file_path);
                string fileContent = textreader.ReadToEnd();
                document.doc_content = fileContent;
            } catch(Exception){
                TempData["err"] = "Cập nhật thông tin thất bại. File văn bản không tồn tại hoặc đã bị xóa!";
                return Redirect("Edit?id=" + document.id);
            }
            
            document.update_username = Session.getCurrentUser().username;
            document.update_datetime = DateTime.Now;

            gov_doc_draft documentInfo = _cnttDB.gov_doc_draft.Find(document.id);
            documentInfo.attach_file_name = document.attach_file_name;
            documentInfo.attach_file_path = document.attach_file_path;
            documentInfo.date_start_promulgate = document.date_start_promulgate;
            documentInfo.doc_code = document.doc_code;
            documentInfo.doc_name = document.doc_name;
            documentInfo.doc_summany = document.doc_summany;
            documentInfo.user_sign = document.user_sign;
            documentInfo.update_username = document.update_username;
            documentInfo.update_datetime = document.update_datetime;
            try
            {
                int rs = _cnttDB.SaveChanges();
                var lucene = new LuceneSerives.LuceneDocuments();
                if (rs > 0)
                {
                    lucene.AddUpdateLuceneIndex(document);
                    insertHistory(AccessType.chinhSuaVanBan, Constant.CHINHSUA(Constant.ITEM_VANBAN, Constant.ID, document.id.ToString()));
                    TempData["message"] = Constant.EDIT_SUCCESSFULL;
                }
                else
                {
                    TempData["err"] = Constant.EDIT_SUCCESSFULL;
                }
            } catch(Exception ex){
                TempData["err"] = Constant.EDIT_SUCCESSFULL;
            }
            return Redirect("Index");
        }

        public DocumentViewhepler saveData(DocumentViewhepler documentViewhepler)
        {
            List<gov_doc_draft> lstDocument = _cnttDB.gov_doc_draft.ToList();
            lstDocument = setSearchFilter(lstDocument, documentViewhepler);

            int totalCount = lstDocument.Count;
            documentViewhepler.TotalCount = totalCount;

            if (documentViewhepler.TotalCount > 0)
            {
                int totalPage = pageCalculation(totalCount, Constant.limit);
                documentViewhepler.TotalPage = totalPage;
                documentViewhepler.Page = pageTransition(documentViewhepler.Direction, documentViewhepler.Page, totalPage);
                documentViewhepler.FirstPage = fistPageCalculation(Constant.maxPageLine, totalPage, documentViewhepler.Page);
                documentViewhepler.LastPage = lastPageCalculation(Constant.maxPageLine, totalPage, documentViewhepler.Page, documentViewhepler.FirstPage);
                int take = Constant.limit;
                int skip = (documentViewhepler.Page - 1) * take;
                documentViewhepler.LstDocument = lstDocument.OrderByDescending(d => d.entry_datetime).Skip(skip).Take(take).ToList();
            }
            ViewData["documentViewhepler"] = documentViewhepler;
            return documentViewhepler;
        }

        public List<gov_doc_draft> setSearchFilter(List<gov_doc_draft> lstDocument, DocumentViewhepler documentViewhepler)
        {
            Expression<Func<gov_doc_draft, bool>> predicate = PredicateBuilder.False<gov_doc_draft>();
            if (!String.IsNullOrWhiteSpace(documentViewhepler.KeySearch))
            {
                predicate = predicate.Or(d => d.doc_code != null && d.doc_code.ToUpper().Contains(documentViewhepler.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.doc_name != null && d.doc_name.ToUpper().Contains(documentViewhepler.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.doc_summany!= null && d.doc_summany.ToUpper().Contains(documentViewhepler.KeySearch.ToUpper()));
                predicate = predicate.Or(d => d.user_sign!= null && d.user_sign.ToUpper().Contains(documentViewhepler.KeySearch.ToUpper()));
                lstDocument = lstDocument.Where(predicate.Compile()).ToList();
            }
            return lstDocument;
        }
    }
}
