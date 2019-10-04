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
    public class IntentController : Controller
    {
        private IntentService intentService;

        public IntentController()
        {
            intentService = new IntentService();
        }

 

        /// <summary>
        /// 인텐트 목록 조회
        /// </summary>
        /// <returns></returns>
        public ActionResult List()
        {
            var intents = intentService.GetIntentsAll();
            return View(intents);
        }

        /// <summary>
        /// 인텐트 정보 등록
        /// </summary>
        /// <param name="idx"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Manage(int? idx)
        {
            IntentManageVM vm = new IntentManageVM();
            vm.Intent = new Intents();
            vm.Intent.IsUseYN = true;
            vm.SaveMode = SaveModes.Create;

            if (idx != null)
            {
                vm.Intent = intentService.GetIntentInfo((int)idx);
                vm.Intent.IsUseYN = true;
                vm.SaveMode = SaveModes.Modify;
            }

            return View(vm);
        }

        [HttpPost]
        public async Task<ActionResult> Manage(IntentManageVM vm)
        {
            string authoringKey = ConfigurationManager.AppSettings["AuthoringKey"].ToString();
            string appID = ConfigurationManager.AppSettings["LuisAppID"].ToString();
            string appVersion = ConfigurationManager.AppSettings["AppVersion"].ToString();
            string appHost = ConfigurationManager.AppSettings["LuisHost"].ToString();


            if (vm.SaveMode == SaveModes.Create)
            {
                vm.Intent.ModifyUserID = "eddy";//HttpContext.User.Identity.Name;
                vm.Intent.ModifyDate = DateTime.Now;

                //LUIS 인텐트 추가
                string intentID = await LuisCreateIntent(appID, appVersion, authoringKey, vm.Intent);
                vm.Intent.LuisAppID = appID;
                vm.Intent.IntentID = intentID;

                //인텐트 DB 등록
                intentService.AddIntent(vm.Intent);
            }
            else
            {
                //인텐트 정보 DB 조회
                var dbIntent = intentService.GetIntentInfo(vm.Intent.IntentIDX);

                //인텐트 아이디가 없으면 신규 LUIS 인텐트 등록처리
                if (string.IsNullOrEmpty(vm.Intent.IntentID))
                {
                    string intentID = await LuisCreateIntent(appID, appVersion, authoringKey, vm.Intent);
                    vm.Intent.LuisAppID = appID;
                    vm.Intent.IntentID = intentID;
                }

                //인텐트 명 변경된 경우 수정처리
                if (vm.Intent.IntentName != dbIntent.IntentName)
                {
                    await LuisModifyIntent(appID, appVersion, authoringKey, vm.Intent);
                }

                vm.Intent.ModifyUserID = HttpContext.User.Identity.Name;
                vm.Intent.ModifyDate = DateTime.Now;

                intentService.UpdateIntent(vm.Intent);
            }
            return RedirectToAction("List", "Intent");
        }


        #region LUIS 통신모듈


        /// <summary>
        /// LUIS 인텐트 신규등록 처리
        /// </summary>
        /// <param name="bizTypeCode"></param>
        /// <param name="intent"></param>
        /// <returns></returns>
        private async Task<string> LuisCreateIntent(string appID, string appVersion, string authoringKey, Intents intent)
        {
            using (var client = new LuisProgClient(authoringKey, Regions.WestUS))
            {
                return await client.Intents.AddAsync(intent.IntentName, appID, appVersion);
            }
        }

        /// <summary>
        /// LUIS 인텐트명 수정처리
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="appVersion"></param>
        /// <param name="authoringKey"></param>
        /// <param name="intent"></param>
        private async Task LuisModifyIntent(string appID, string appVersion, string authoringKey, Intents intent)
        {
            using (var client = new LuisProgClient(authoringKey, Regions.WestUS))
            {
                await client.Intents.RenameAsync(intent.IntentID, intent.IntentName, appID, appVersion);
            }
        }

        /// <summary>
        /// 인텐트 및 발화메시지 삭제
        /// </summary>
        /// <param name="appID"></param>
        /// <param name="appVersion"></param>
        /// <param name="authoringKey"></param>
        /// <param name="intent"></param>
        /// <returns></returns>
        private async Task LuisDeleteIntent(string appID, string appVersion, string authoringKey, Intents intent)
        {
            using (var client = new LuisProgClient(authoringKey, Regions.WestUS))
            {
                await client.Intents.DeleteAsync(intent.IntentID, appID, appVersion, true);
            }
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

        #endregion


    }
}