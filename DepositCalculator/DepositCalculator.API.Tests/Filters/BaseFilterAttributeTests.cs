using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;

namespace DepositCalculator.API.Filters
{
    internal abstract class BaseFilterAttributeTests
    {
        protected ActionContext _fakeActionContext;

        [SetUp]
        public void BaseSetup()
        {
            var fakeHttpContext = new DefaultHttpContext();

            _fakeActionContext = new ActionContext(
                fakeHttpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());
        }
    }
}