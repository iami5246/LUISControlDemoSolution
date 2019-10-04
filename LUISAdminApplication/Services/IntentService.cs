using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using LUISAdminApplication.Models;

namespace LUISAdminApplication.Services
{
    public class IntentService
    {
        //EF DB First DB관리객체 생성
        private LUISControlDBEntities db = new LUISControlDBEntities();

        public IntentService()
        {


        }

        public void AddIntent(Intents intent)
        {
            intent.RegistDate = DateTime.Now;
            intent.ModifyDate = DateTime.Now;
            db.Intents.Add(intent);
            db.SaveChanges();
        }

        public void UpdateIntent(Intents intents)
        {
            Intents data = GetIntentInfo(intents.IntentIDX);

            data.BizTypeCode = intents.BizTypeCode;
            data.LuisAppID = intents.LuisAppID;
            data.IntentID = intents.IntentID;
            data.IntentName = intents.IntentName;
            data.Reply1 = intents.Reply1;
            data.ReplyLink = intents.ReplyLink;
            data.SystemName = intents.SystemName;
            data.IsUseYN = intents.IsUseYN;
            data.ModifyUserID = intents.ModifyUserID;
            data.ModifyDate = intents.ModifyDate;

            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
        }


        public Intents GetIntentInfo(int intentIDX)
        {
            return db.Intents.Where(c => c.IntentIDX == intentIDX).FirstOrDefault();
        }


        public List<Intents> GetIntentsAll()
        {
            return db.Intents.ToList();
        }

        public void DeleteIntent(int intentIDX)
        {
            var intent = db.Intents.Where(c => c.IntentIDX == intentIDX).FirstOrDefault();
            db.Intents.Remove(intent);
            db.SaveChanges();
        }


        public void DeleteUtterance(int uidx)
        {
            var utt = db.Utterances.Where(c => c.UtteranceIDX == uidx).FirstOrDefault();
            db.Utterances.Remove(utt);
            db.SaveChanges();
        }



        public void AddUtterance(Utterances utterance)
        {
            utterance.RegistDate = DateTime.Now;
            utterance.ModifyDate = DateTime.Now;
            db.Utterances.Add(utterance);
            db.SaveChanges();
        }

        public void Updateutterance(Utterances utterance)
        {
            Utterances data = GetUtteranceInfo(utterance.UtteranceIDX);

            data.IntentIDX = utterance.IntentIDX;
            data.ExampleID = utterance.ExampleID;
            data.Utterance = utterance.Utterance;
            data.IsUseYN = utterance.IsUseYN;
            data.ModifyUserID = utterance.ModifyUserID;
            data.ModifyDate = utterance.ModifyDate;

            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
        }


        public Utterances GetUtteranceInfo(int utteranceIDX)
        {
            return db.Utterances.Where(c => c.UtteranceIDX == utteranceIDX).FirstOrDefault();
        }


        public List<Utterances> GetUtteranceAll()
        {
            return db.Utterances.ToList();
        }


        public List<USP_SelectUtteranceSearch_Result> GetUtteranceSearch()
        {
            return db.USP_SelectUtteranceSearch().ToList<USP_SelectUtteranceSearch_Result>();
        }

    }
}