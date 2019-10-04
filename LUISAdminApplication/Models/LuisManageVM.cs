using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LUISAdminApplication.Models
{
    public enum SaveModes
    {
        Create,
        Modify
    }

    public class IntentManageVM
    {
        public SaveModes SaveMode { get; set; }
        public Intents Intent { get; set; }
    }

    public class TrainPublishVM
    {
        public string BizTypeCode { get; set; }
        public string TaskType { get; set; }
        public string LuisResult { get; set; }
    }

    public class UtteranceManageVM
    {
        public List<Intents> Intents { get; set; }
        public SaveModes SaveMode { get; set; }
        public Utterances Utterance { get; set; }

    }

    public class VM
    {
        public string Status { get; set; }
        public DateTime PublishDate { get; set; }

    }


}