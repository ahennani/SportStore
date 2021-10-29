using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// From Website

namespace SportStore.Helpers
{
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var apiDescription = context.ApiDescription;

            // operation.Deprecated = operation.Deprecated || apiDescription.IsDeprecated();
            operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var desc = apiDescription.ParameterDescriptions
                                         .First(p => p.Name == parameter.Name);

                if (parameter.Description is null)
                {
                    parameter.Description = desc.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default is null && desc.DefaultValue is not null)
                {
                    parameter.Schema.Default = new OpenApiString(desc.DefaultValue.ToString());
                }

                parameter.Required |= desc.IsRequired;
            }
        }

    }
}

//var t1 = false;
//var t2 = true;
//var t3 = t1 |= t2; // t1 = t1 || t2