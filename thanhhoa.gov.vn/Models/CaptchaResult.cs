using System.Drawing.Imaging;
using System.Web;
using System.Web.Mvc;

namespace thanhhoa.gov.vn.Models
{
public class CaptchaResult : ActionResult
{
    public string _captchaText;
    public CaptchaResult(string captchaText)
    {
        _captchaText = captchaText;
    }
    public override void ExecuteResult(ControllerContext context)
    {
        Captcha c = new Captcha();
        c.Text = _captchaText;
        c.Width = 200;
        c.Height = 50;
        c.FamilyName = "Century Schoobook";

        HttpContextBase cb = context.HttpContext;

        cb.Response.Clear();
        cb.Response.ContentType = "image/jpeg";
        c.Image.Save(cb.Response.OutputStream, ImageFormat.Jpeg);
        c.Dispose();
    }
}
}