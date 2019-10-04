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
    public class UtteranceController : Controller
    {
        private IntentService intentService;

        public UtteranceController()
        {
            intentService = new IntentService();
        }

        /// <summary>
        /// 모든 발화 메시지 조회
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult List()
        {
            var utterances = intentService.GetUtteranceSearch();
            return View(utterances);
        }


        /// <summary>
        /// 발화메시지 등록
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Create()
        {
            UtteranceManageVM vm = new UtteranceManageVM();
            vm.Intents = intentService.GetIntentsAll();
            vm.Intents.Insert(0, new Intents() { IntentIDX = 0, IntentName = "선택" });

            vm.Utterance = new Utterances();
            vm.SaveMode = SaveModes.Create;

            return View(vm);
        }

        /// <summary>
        /// 발화메시지 등록
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(UtteranceManageVM vm)
        {

            Intents intent = intentService.GetIntentInfo(Convert.ToInt32(vm.Utterance.IntentIDX));

            string authoringKey = ConfigurationManager.AppSettings["AuthoringKey"].ToString();
            string appID = ConfigurationManager.AppSettings["LuisAppID"].ToString();
            string appVersion = ConfigurationManager.AppSettings["AppVersion"].ToString();
            string appHost = ConfigurationManager.AppSettings["LuisHost"].ToString();


            vm.Utterance.IsUseYN = true;
            vm.Utterance.RegistUserID = "eddy";//HttpContext.User.Identity.Name;
            vm.Utterance.RegistDate = DateTime.Now;
            vm.Utterance.ModifyUserID = "eddy";//HttpContext.User.Identity.Name;
            vm.Utterance.ModifyDate = DateTime.Now;

            //LUIS 해당 인텐트에 발화메시지 등록처리
            Example example = new Example();
            example.IntentName = intent.IntentName;
            example.Text = vm.Utterance.Utterance;
            Utterance utterance = await LuisCreateUtterance(appID, appVersion, authoringKey, example);

            //발화메시지 DB저장
            vm.Utterance.ExampleID = utterance.ExampleId;
            intentService.AddUtterance(vm.Utterance);

            return RedirectToAction("List", "Utterance");
        }



        /// <summary>
        /// Luis Utterance 신규등록처리
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="appVersion"></param>
        /// <param name="authoringKey"></param>
        /// <param name="example"></param>
        /// <returns></returns>
        private async Task<Utterance> LuisCreateUtterance(string appID, string appVersion, string authoringKey, Example example)
        {
            using (var client = new LuisProgClient(authoringKey, Regions.WestUS))
            {
                return await client.Examples.AddAsync(appID, appVersion, example);
            }
        }

        /// <summary>
        /// Luis Utterance 삭제처리 
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="appVersion"></param>
        /// <param name="authoringKey"></param>
        /// <param name="exampleID"></param>
        /// <returns></returns>
        private async Task LuisDeleteUtterance(string appID, string appVersion, string authoringKey, int exampleID)
        {
            using (var client = new LuisProgClient(authoringKey, Regions.WestUS))
            {
                await client.Examples.DeleteAsync(appID, appVersion, exampleID.ToString());
            }
        }





    }
}