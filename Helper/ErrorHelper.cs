using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Newtonsoft.Json.Linq;


namespace TokaAPI.Helper
{

    public class ErrorHelper
    {
        public static ResponseObject Response(int StatusCode, string Message)
        {
            return new ResponseObject()
            {
                Type = "Custom",
                StatusCode = StatusCode,
                Message = Message
            };
        }

        public static List<ModelErrors> GetModelStateErrors(ModelStateDictionary Model)
        {
            return Model.Select(value => new ModelErrors()
            {
                Type = "Model",
                Key = value.Key,
                Messages = value.Value.Errors.Select(error => error.ErrorMessage).ToList()
            }).ToList();
        }

        public class ResponseObject
        {
            public string Type { get; set; }
            public int StatusCode { get; set; }
            public string Message { get; set; }
        }

        public class ModelErrors
        {
            public string Type { get; set; }
            public string Key { get; set; }
            public List<string> Messages { get; set; }

        }
    }


}