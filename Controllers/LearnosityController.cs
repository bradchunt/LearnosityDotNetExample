
using LearnositySDK.Request;
using LearnositySDK.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;


namespace LearnosityDotNetExample.Controllers
{
public class LearnosityController: Controller
{
        //public consumer key and secret that work for our public demos itembank found at https://demos.learnosity.com
        private readonly string consumerKey = "yis0TYCu7U9V4o7M";
        private readonly string secret = "74c5fd430cf1242a527f6223aebd42d30464be22";
        private readonly string domain;  

        public LearnosityController()
        {
            domain = "localhost";
        }

        public ActionResult Items()
        {
            JsonObject security = new JsonObject();
            security.set("consumer_key", consumerKey);
            security.set("domain", domain);

            JsonObject request = new JsonObject();
            request.set("mode", "item_list"); //https://reference.learnosity.com/author-api/initialization/mode

            JsonObject user = new JsonObject(); //Info for the user that is authoring items, saved as meta data
            user.set("id","Brad-Hunt-User-ID");
            user.set("firstname","Brad");
            user.set("lastname","Hunt");
            user.set("email","Brad.Hunt@learnosity.com");
            
            request.set("user",user);


            Init init = new Init("author", security, secret, request);
            ViewBag.SignedRequest = init.generate();
            return View();
        }
         public ActionResult Activities()
        {
            JsonObject security = new JsonObject();
            security.set("consumer_key", consumerKey);
            security.set("domain", domain);

            JsonObject request = new JsonObject();
            request.set("mode", "activity_list"); //https://reference.learnosity.com/author-api/initialization/mode

            JsonObject user = new JsonObject();
            user.set("id","Brad-Hunt-User-ID");
            user.set("firstname","Brad");
            user.set("lastname","Hunt");
            user.set("email","Brad.Hunt@learnosity.com");
            
            request.set("user",user);


            Init init = new Init("author", security, secret, request);
            ViewBag.SignedRequest = init.generate();
            return View();
        }

        public ActionResult Assess()
        {
            string sessionId=Uuid.generate();
            string userId= "Brad-Hunt-Demo-Student-User";

            JsonObject security = new JsonObject();
            security.set("consumer_key", consumerKey);
            security.set("domain", domain);

            JsonObject request = new JsonObject();
            request.set("name", "Exploring Planets");               //https://reference.learnosity.com/items-api/initialization/name
            request.set("activity_template_id", "demo-activity-1"); //https://reference.learnosity.com/items-api/initialization/activity_template_id
            request.set("user_id", userId);                         //https://reference.learnosity.com/items-api/initialization/user_id
            request.set("rendering_type", "assess");                //https://reference.learnosity.com/items-api/initialization/rendering_type
            request.set("type", "submit_practice");                 //https://reference.learnosity.com/items-api/initialization/type
            request.set("activity_id", "1stPeriodScience"); ;       //https://reference.learnosity.com/items-api/initialization/activity_id
            request.set("session_id",sessionId);                    //https://reference.learnosity.com/items-api/initialization/session_id


            //https://reference.learnosity.com/items-api/initialization/config.configuration.onsubmit_redirect_url
            JsonObject configuration = new JsonObject();
            configuration.set("onsubmit_redirect_url", String.Format("/Learnosity/Reports?sessionid={0}&userid={1}", sessionId,userId)); 

            JsonObject config = new JsonObject();
            config.set("configuration", configuration);

            request.set("config", config);
            
            Init init = new Init("items", security, secret, request);
            ViewBag.SignedRequest = init.generate();
            return View();
        }

        public ActionResult Reports()
        {
            // Get sessionid from the query string
            string sessionId = HttpContext.Request.Query["sessionid"].ToString();

            JsonObject security = new JsonObject();
            security.set("consumer_key", consumerKey);
            security.set("domain", domain);

            JsonObject request = new JsonObject();

            JsonObject report1= new JsonObject();
            report1.set("type", "session-detail-by-item");          //https://reference.learnosity.com/reports-api/reporttypes#sessionsDetailByItemReport
            report1.set("id", "session-detail");                    //https://reference.learnosity.com/reports-api/reporttypes#sessiondetailbyitem-id
            report1.set("user_id","Brad-Hunt-Demo-Student-User");   //https://reference.learnosity.com/reports-api/reporttypes#sessiondetailbyitem-user_id
            report1.set("session_id", sessionId);                   //https://reference.learnosity.com/reports-api/reporttypes#sessiondetailbyitem-session_id
          
            JsonObject reports = new JsonObject(true);              //https://reference.learnosity.com/reports-api/initialization/reports  (true here indicates array of reports)
            reports.set(report1);
      
            request.set("reports", reports);

            Init init = new Init("reports", security, secret, request);
            ViewBag.SignedRequest = init.generate();
            return View();
        }
    }
}


