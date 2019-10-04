using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;

using LUISAdminApplication.Models;

using Cognitive.LUIS.Programmatic;
using Cognitive.LUIS.Programmatic.Models;

using LUISAdminApplication.Services;

namespace LUISAdminApplication.Controllers
{
    public class PublishController : Controller
    {

        /// <summary>
        /// LUIS 트레이닝 실행
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Train()
        {
            string authoringKey = ConfigurationManager.AppSettings["AuthoringKey"].ToString();
            string appID = ConfigurationManager.AppSettings["LuisAppID"].ToString();
            string appVersion = ConfigurationManager.AppSettings["AppVersion"].ToString();
            string appHost = ConfigurationManager.AppSettings["LuisHost"].ToString();


            TrainingDetails train = await LuisTrain(appID, appVersion, authoringKey);
            string result = train.Status;
            return RedirectToAction("List", "Utterance",new { result= result });
        }


        /// <summary>
        /// LUIS 서비스 게시
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> Publish()
        {
            string authoringKey = ConfigurationManager.AppSettings["AuthoringKey"].ToString();
            string appID = ConfigurationManager.AppSettings["LuisAppID"].ToString();
            string appVersion = ConfigurationManager.AppSettings["AppVersion"].ToString();
            string appHost = ConfigurationManager.AppSettings["LuisHost"].ToString();

            Publish publish = await LuisPublish(appID, appVersion, authoringKey);
            string result = publish.PublishedDateTime.ToString();

            return RedirectToAction("List", "Utterance", new { result = result });
        }



        /// <summary>
        /// LUIS 트레이닝 
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="appVersion"></param>
        /// <param name="authoringKey"></param>
        /// <returns></returns>
        private async Task<TrainingDetails> LuisTrain(string appID, string appVersion, string authoringKey)
        {
            using (var client = new LuisProgClient(authoringKey, Regions.WestUS))
            {
                return await client.Training.TrainAsync(appID, appVersion);
            }
        }

        /// <summary>
        /// LUIS 게시하기
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="appVersion"></param>
        /// <param name="authoringKey"></param>
        /// <returns></returns>
        private async Task<Publish> LuisPublish(string appID, string appVersion, string authoringKey)
        {
            using (var client = new LuisProgClient(authoringKey, Regions.WestUS))
            {
                return await client.Publishing.PublishAsync(appID, appVersion, false, false);
            }
        }
    }
}