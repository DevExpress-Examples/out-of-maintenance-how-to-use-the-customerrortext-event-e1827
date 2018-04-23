using System;
using System.Web;
using DevExpress.Web.ASPxScheduler;
using DevExpress.XtraScheduler;

public class ResourceFiller {
    public static string[] Users = new string[] { "Sarah Brighton", "Ryan Fischer", "Andrew Miller" };
    public static string[] Usernames = new string[] { "sbrighton", "rfischer", "amiller" };

    public static void FillResources(ASPxSchedulerStorage storage, int count) {
        ResourceCollection resources = storage.Resources.Items;
        storage.BeginUpdate();
        try {
            int cnt = Math.Min(count, Users.Length);
            for(int i = 1; i <= cnt; i++) {
                resources.Add(storage.CreateResource(Usernames[i - 1], Users[i - 1]));
            }
        }
        finally {
            storage.EndUpdate();
        }
    }
}

#region #errortexthelper
public class ErrorTextHelper {
    static string NewLinesToBr(string text) {
        text = text.Replace("\r", string.Empty);
        return text.Replace("\n", "<br/>");
    }
    public static string PrepareMessage(string subjectText, string detailInfoText, bool showDetailedErrorInfo) {
        string subject = String.Format("{0}\n", subjectText);
        string detailInfo = String.Format("Detailed information is included below.\n\n- {0}", detailInfoText);
        if(!showDetailedErrorInfo)
            detailInfo = String.Empty;
        subject = NewLinesToBr(HttpUtility.HtmlEncode(subject));
        detailInfo = NewLinesToBr(HttpUtility.HtmlEncode(detailInfo));
        return String.Format("{0},{1}|{2}{3}", subject.Length, detailInfo.Length, subject, detailInfo);
    }
}
#endregion #errortexthelper