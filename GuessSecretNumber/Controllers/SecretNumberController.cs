using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using GuessSecretNumber.Models;

namespace GuessSecretNumber.Controllers
{

    public class SecretNumberController : Controller
    {
        private SecretNumber SecretNumber
        {
            get { return Session["sn"] as SecretNumber ?? (SecretNumber)(Session["sn"] = new SecretNumber()); }
        }

        public ActionResult Index()
        {
            if (Session["sn"] != null)
            {
                Session.Clear(); 
            }
       
            return View(SecretNumber);
        }

        [HttpPost, ActionName("Index"), ValidateAntiForgeryToken]
        public ActionResult Index_POST(FormCollection collection)
        {
            
            if (Session.IsNewSession)
            {
                return View("error");
            }

            if (TryUpdateModel(SecretNumber, new[] { "NewNumber" }, collection))
            {
                Outcome outcome = SecretNumber.MakeGuess(SecretNumber.NewNumber);
                if (outcome == Outcome.Right)
                {
                    return View("RightGuess", SecretNumber);
                }

                return View("Guesses", SecretNumber);
            }

            return View("Index");

        }
    }
}