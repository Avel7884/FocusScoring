using FocusAccess.ResponseClasses;

namespace FocusAccess.Methods
{
    public class Req //: ApiMethod<ReqValue,InnUrlArg>
    {
        internal Req(JsonAccess access) //: base(access,"/req")
        {/*
            Fio = new ParamQueryTransfer<FioValue,InnUrlArg>(new ApiParameter<FioValue>("FIO", "FIO", this,
                "/ArrayOfreq/req/UL/legalName"),access);
            Name = new ParamQueryTransfer<NameValue, InnUrlArg>(new ApiParameter<NameValue>("Name","name",this,"/ArrayOfreq/req/UL/legalName/legalNames/"),access );
        */
        }
/*

        public override string Url => "/req";
        public ParamQueryTransfer<FioValue,InnUrlArg> Fio { get; }
        public ParamQueryTransfer<NameValue,InnUrlArg> Name { get; }*/
        public string Url { get; }
    }
}