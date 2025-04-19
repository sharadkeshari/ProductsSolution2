using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.Filters
{
    public class AddHeaderParameterOperationFilter : IOperationFilter
    {

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "X-Custom-Header", // Name of the header
                In = ParameterLocation.Header,
                Description = "Custom header added by the user",
                Required = false // Set true if the header is required
            });
            
        }
    }
}


