1、选择图片剪切



    <button id="replaceImg" class="l-btn">更换图片</button>
    <div style="width: 320px;height: 320px;border: solid 1px #555;padding: 5px;margin-top: 10px">
        <img id="finalImg" src="" width="100%">
    </div>


    <!--图片裁剪框 start-->
    <div style="display: none" class="tailoring-container">
        <div class="black-cloth" onclick="closeTailor(this)"></div>
        <div class="tailoring-content">
            <div class="tailoring-content-one">
                <label title="上传图片" for="chooseImg" class="l-btn choose-btn">
                    <input type="file" accept="image/jpg,image/jpeg,image/png" name="file" id="chooseImg" class="hidden" onchange="selectImg(this)">
                    选择图片
                </label>
                <div class="close-tailoring" onclick="closeTailor(this)">×</div>
            </div>
            <div class="tailoring-content-two">
                <div class="tailoring-box-parcel">
                    <img id="tailoringImg" src="~/images/snow.jpg" />
                </div>
                <div class="preview-box-parcel">
                    <p>图片预览：</p>
                    <div class="square previewImg"></div>
                    <div class="circular previewImg"></div>
                </div>
            </div>
            <div class="tailoring-content-three">
                <button class="l-btn cropper-reset-btn">复位</button>
                <button class="l-btn cropper-rotate-btn">旋转</button>
                <button class="l-btn cropper-scaleX-btn">换向</button>
                <button class="l-btn sureCut" id="sureCut">确定</button>
            </div>
        </div>
    </div>
  2、c#上传图片
          function SaveFile()
        {
            var formData = new FormData();
            for (var i = 0; i < Files.length; i++) {
                formData.append('file', Files[i]);
            }
            $.ajax({
                url: "@Url.Action("SaveFileInfor")",
                type: "post",
            dataType: "json",
            cache: false,
            data: formData,
            outtime: 0,
            async:false,
            processData: false,// 不处理数据
            contentType: false, // 不设置内容类型
            success: function (data) {
                if (data.state == 1) {
                    
                }
                else {
                    alert(data.msg);
                }
            },error:function(){
            }

        });
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
    
