using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;

namespace MMSServices.Models
{
    [DataContract]
    public class ApiResponse
    {
        [DataMember]
        public int StatusCode { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public string Message { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public object Data { get; set; }

        [DataMember]
        public int IsError { get; set; }

        public ApiResponse(HttpStatusCode statusCode, object result = null, string errorMessage = null, int isError=0)
        {
            StatusCode = (int)statusCode;
            Data = result;
            Message = errorMessage;
            IsError = isError;
        }
    }
}