using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IronTester.Common.Commands;
using IronTester.Common.Metadata;
using IronTester.Website.Models;

namespace IronTester.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AllRequests()
        {
            return View();
        }

        // GET: Default/Create
        public ActionResult CreateRequest()
        {
            ViewBag.ValidSourceCodePaths = string.Join(",<br/>", ValidationData.ValidInitializationPaths);
            ViewBag.ValidTests = string.Join(",", ValidationData.ValidTests);
            return View(new RequestCreationModel());
        }

        // POST: Default/Create
        [HttpPost]
        public ActionResult CreateRequest(RequestCreationModel model)
        {
            try
            {
                if (model.SourceCodeLocation != null && model.TestsRequested != null)
                {
                    MvcApplication.Bus.Send("IronTester.Server", 
                        MvcApplication.Bus.CreateInstance<PleaseDoTests>(y =>
                        {
                            y.RequestId = Guid.NewGuid();
                            y.SourceCodeLocation = model.SourceCodeLocation;
                            y.TestsRequested = model.TestsRequested.Split(',').ToList();
                        }));
                }
                else
                {
                    throw new NotImplementedException();
                }
                return RedirectToAction("AllRequests");
            }
            catch
            {
                return View();
            }
        }
    }
}