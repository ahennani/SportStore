using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace SportStore.Models
{
    public class ApiError
    {
        public ApiError() { }

        public ApiError(ModelStateDictionary modelState)
        {
            Message = "Invalid parameters.";

            Errors = new();
            if (modelState.ErrorCount > 0)
            {
                foreach (var item in modelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Errors.Add(error.ErrorMessage);
                    }
                }
            }
        }

        public string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Details { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> Errors { get; set; }
    }
}
