﻿using Microsoft.AspNetCore.Builder;

namespace ESOA
{
  public static class ApplicationBuilderExtensions
  {
    public static IApplicationBuilder UseHttpException(this IApplicationBuilder application)
    {
      return application.UseMiddleware<HttpExceptionMiddleware>();
    }
  }
}
