using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GuessSecretNumber.Models
{
   /*Instanser av strukturen GuessedNumber används för att lagra information om genomförda gissningar.*/
    public struct GuessedNumber
    {
        public int? Number; //Publikt fält av typen int? som innehåller en gissnings värde.
        public Outcome Outcome; //Publikt fält av typen Outcome som innehåller utfallet av en gissning.
    }
}