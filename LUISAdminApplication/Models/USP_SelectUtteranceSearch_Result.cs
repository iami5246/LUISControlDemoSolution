//------------------------------------------------------------------------------
// <auto-generated>
//     이 코드는 템플릿에서 생성되었습니다.
//
//     이 파일을 수동으로 변경하면 응용 프로그램에서 예기치 않은 동작이 발생할 수 있습니다.
//     이 파일을 수동으로 변경하면 코드가 다시 생성될 때 변경 내용을 덮어씁니다.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LUISAdminApplication.Models
{
    using System;
    
    public partial class USP_SelectUtteranceSearch_Result
    {
        public int UtteranceIDX { get; set; }
        public Nullable<int> IntentIDX { get; set; }
        public Nullable<int> ExampleID { get; set; }
        public string Utterance { get; set; }
        public bool IsUseYN { get; set; }
        public Nullable<System.DateTime> RegistDate { get; set; }
        public string RegistUserID { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public string ModifyUserID { get; set; }
        public string IntentName { get; set; }
    }
}