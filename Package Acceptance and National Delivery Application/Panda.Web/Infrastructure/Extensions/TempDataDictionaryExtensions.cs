﻿namespace Panda.Web.Infrastructure.Extensions
{
    using Common;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;

    public static class TempDataDictionaryExtensions
    {
        public static void AddSuccessMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[WebConstants.TempDataSuccessMessageKey] = message;
        }

        public static void AddErrorMessage(this ITempDataDictionary tempData, string message)
        {
            tempData[WebConstants.TempDataErrorMessageKey] = message;
        }
    }
}