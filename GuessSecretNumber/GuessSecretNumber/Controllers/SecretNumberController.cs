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
        public ActionResult Index()
        {
            // Objekten i session tas bort så nytt spel kan börja. 
            if (Session["objekt"] != null)
            {
                Session.Clear();
            }

            // Sätter en session, vars uppgift är att kolla om tiden gått ut eller ej. 
            Session["Game"] = "Game"; 
            return View(); 
        }

        [HttpPost, ActionName("Index"), ValidateAntiForgeryToken]
        public ActionResult Index_POST(SecretNumber model, FormCollection collection)
        {
            if (Session["Game"] == "Game")
            {
                // Testar om input talet i formuläret är godkänt enlig villkor, försökte ha båda i samma men fick ingen rätsida på det. 
                if (TryUpdateModel(model, new[] { "NewNumber" }, collection) && model.NewNumber > 100)
                {
                    ModelState.AddModelError("NewNumber", "* Du kan inte gissa på ett tal större än 100!");
                }
                if (TryUpdateModel(model, new[] { "NewNumber" }, collection) && model.NewNumber < 1)
                {
                    ModelState.AddModelError("newnumber", "* Du kan inte gissa på ett tal mindre än 1!");
                }

                List<SecretNumber> list = new List<SecretNumber>();

                // En ny spelomgång börjar om Session["objekt"] är null.
                if (Session["objekt"] == null)
                {
                    // Om inputen i formuläret var giltligt, kan en gissning ske. Annars "stannar" man kvar i Index vyn och får ett felmeddelande. 
                    if (ModelState.IsValid)
                    {
                        Outcome outcome = model.MakeGuess(model.NewNumber);
                        if (outcome == Outcome.Right)
                        {
                            return View("RightGuess", model);
                        }

                        // Lägger till vårat första objekt till Session["objekt"], så att vi kan bygga på listan GuessedNumber (_guessedNumbers) i modellen, och hålla koll på det hemliga talet.
                        list.Add(model);
                        Session["objekt"] = list;

                        return View("Guesses", model);
                    }

                    return View("Index"); 
                }

                // Om en gissning redan har skett sedan innan, dvs ett spel har påbörjats. 

                list = (List<SecretNumber>)Session["objekt"];

                // Om ett fel sker i spelomgången returneras det första objektet till vyn, annars byggs listan på med nästa gissning. 
                SecretNumber _firstmodel = model; // tilldelades model tillfälligt då jag fick klagomål från return annars. 

                foreach (SecretNumber firstModel in (List<SecretNumber>)Session["objekt"])
                {
                    _firstmodel = firstModel;

                    if (ModelState.IsValid)
                    {
                        // Lägger till det nya objektets tal (model) i det första sparade objektets lista med GuessedNumbers. 
                        Outcome outcome = firstModel.MakeGuess(model.NewNumber);
                        if (outcome == Outcome.Right)
                        {
                            return View("RightGuess", firstModel);
                        }

                        return View("Guesses", firstModel);
                    }
                }

                return View("Guesses", _firstmodel);

            }else{
                Session.Clear();
                return View("error");
            }
        }
    }
}