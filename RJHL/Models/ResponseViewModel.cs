using System;
using Newtonsoft.Json;

namespace RJHL.Models
{
    public class ResponseViewModel
    {
        public enum Type
        {
            SessionEnded = -99,
            Ok = 1,
            Warning = 2,
            Error = 3,
            Information = 4,
            Undefined = 99,
        }

        public enum HttpCode
        {
            Ok = 200,
            Created = 201,
            NoContent = 204,
            BadRequest = 400,
            Forbidden = 403,
            NotFound = 404,
            InternalServerError = 500
        }

        public class Paging
        {
            private int _offset = 0;
            private int _limit = 20;
            private int _result = 20;
            private int _total = 0;

            [JsonProperty("offset")]
            public int Offset
            {
                get { return _offset; }
                set { _offset = value; }
            }

            [JsonProperty("limit")]
            public int Limit
            {
                get { return _limit; }
                set { _limit = value; }
            }

            [JsonProperty("result")]
            public int Result
            {
                get { return _result; }
                set { _result = value; }
            }

            [JsonProperty("total")]
            public int Total
            {
                get { return _total; }
                set { _total = value; }
            }
        }

        public class Message
        {
            [JsonProperty("type")]
            public Type Type { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }
            
            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; } 
            
            [JsonProperty("statusText")]
            public string StatusText { get; set; }

            [JsonProperty("stackTrace")]
            public string StackTrace { get; set; }
        }

        [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
        public class ResponseView
        {
            private bool _success = true;
            private DateTime _datetime = DateTime.UtcNow;


            [JsonProperty("success")]
            public bool Success
            {
                get { return _success; }
                set { _success = value; }
            }

            [JsonProperty("data")]
            public dynamic Data { get; set; }

            [JsonProperty("optionalData")]
            public dynamic OptionalData { get; set; }

            [JsonProperty("paging")]
            public Paging Paging { get; set; }

            [JsonProperty("message")]
            public Message Message { get; set; }
            

            [JsonProperty("datetime")]
            public DateTime Datetime
            {
                get { return _datetime; }
                set { _datetime = value; }
            }




            
        }


        public static Type GetReponseTypeByCode(string typeCode)
        {
            Type oResponseType;
            switch (typeCode)
            {
                case "1":
                    oResponseType = Type.Ok;
                    break;
                case "2":
                    oResponseType = Type.Warning;
                    break;
                case "3":
                    oResponseType = Type.Error;
                    break;
                case "4":
                    oResponseType = Type.Information;
                    break;
                case "-99":
                    oResponseType = Type.SessionEnded;
                    break;
                default:
                    oResponseType = Type.Undefined;
                    break;
            }
            return oResponseType;
        }

        public static int GetResponseTypeCodeByType(Type oType)
        {
            int responseType;
            switch (oType)
            {
                case Type.Ok:
                    responseType = 1;
                    break;
                case Type.Warning:
                    responseType = 2;
                    break;
                case Type.Error:
                    responseType = 3;
                    break;
                case Type.Information:
                    responseType = 4;
                    break;
                default:
                    responseType = 99;
                    break;
            }

            return responseType;
        }

        public static bool IsSuccess(Type oType)
        {
            return oType == Type.Ok || oType == Type.Information;
        }

    }
}
