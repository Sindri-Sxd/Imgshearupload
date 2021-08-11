using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ImageUpdate.Controllers
{
    public class ContractController : Controller
    {
        //
        // GET: /Contract/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UpdateImg()
        {
            return View();
        }

        public ActionResult PhoteEditor()
        {
            return View();
        }

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveFileInfor()
        {
            try
            {
                var files = HttpContext.Request.Files;
                DateTime SubmitDate = DateTime.Now;
                string path = "";
                string guidpath = "";
                var savePath = "~/UploadFiles/QCX/";
                for (int i = 0; i < files.Count; i++)
                {
                    HttpPostedFileBase file = files[i];
                    string File_Name = file == null ? "" : file.FileName;
                    if (!string.IsNullOrEmpty(File_Name))
                    {
                        if (file.ContentLength > 2097152)//限制2M大小
                        {
                            return Json(new { state = -1, msg = "限制2M以内！", Url = "" });
                        }
                        string FileType = File_Name.Substring(File_Name.LastIndexOf(".") + 1); //得到文件的后缀名  
                        if (FileType == "undefined")
                        {
                            FileType = "jpg";
                        }
                        guidpath = SubmitDate.ToString("yyyyMMddHHmmss") + "." + FileType; //得到重命名的文件名 

                        string filepath = (savePath.IndexOf("~") > -1) ? System.Web.HttpContext.Current.Server.MapPath(savePath) : savePath;
                        if (!Directory.Exists(filepath))//判断文件夹是否存在 
                        {
                            Directory.CreateDirectory(filepath);//不存在则创建文件夹 
                        }
                        path = filepath + guidpath;
                        file.SaveAs(path); //保存操作
                    }
                    else
                    {
                        return Json(new { state = 0, msg = "服务器故障！", Url = "" });
                    }
                }
                return Json(new { state = 1, msg = "成功！", Url = "/UploadFiles/QCX/" + guidpath });
            }
            catch (Exception ex)
            {
                return Json(new { state = 0, msg = ex.ToString(), Url = "" });
            }
        }
    }
}