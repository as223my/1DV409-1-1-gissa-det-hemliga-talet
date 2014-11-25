using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GuessSecretNumber.Models
{
    public class SecretNumber
    {
        private int? _number; // Det slumpade talet. 
        public const int MaxNumberOfGuesses = 7; 

        private GuessedNumber _lastGuessedNumber; // Innehåller det senaste gissade numret. 
        private List<GuessedNumber> _guessedNumbers; // Lista med alla gissade nummer.

        [Range(1, 100, ErrorMessage = "* Talet måste vara mellan 1 till 100!")]
        [DisplayName("Gissa på ett tal mellan 1 och 100: "), Required(ErrorMessage = "* Du måste skicka in ett tal mellan 1 till 100!")]
        public int NewNumber { get; set; }
        public SecretNumber()
        {
            _lastGuessedNumber = new GuessedNumber();
            Initialize();
        }

        public void Initialize()
        {
            Random randomNumber = new Random();
            _number = randomNumber.Next(1, 100);
            _guessedNumbers = new List<GuessedNumber>();
        }

        public bool CanMakeGuess
        {
            get {
                    if (Count < MaxNumberOfGuesses)
                    {
                        return true;
                    }
                    else
                    {
                        return false; 
                    }
               }
        }

        // Returnerar antalet gissningar som gjorts. 
        public int? Count
        {
            get 
            { 
                return _guessedNumbers.Count; 
            }
        }

        // Lista (readOnly) som innehåller alla objekt gissade nummer samt Outcome. 
        public IList<GuessedNumber> GuessedNumbers
        {
            get
            {
                return _guessedNumbers.AsReadOnly(); 
            }
        }

        public GuessedNumber LastGuessedNumber
        {
            get { return _lastGuessedNumber; }
        }

        // Returnerar bara det slumpade talet om antal gissningar tagit slut. 
        public int? Number
        {
            get{   
                if(CanMakeGuess == true)
                {
                    return null;
                }
                else
                {
                    return _number; 
                }
            }
            private set { 
                _number = value; 
            }
        }

        // Räknar till headern i tabellen.
        public int Counting(int count)
        {
            count++;
            return count;
        }

        // Sätter och returnerar Outcome på det gissade talet. Om talet inte tillhör kategorierna NoMoreGuesses eller Oldguess så läggs de till i _guessedNumber listan. 
        public Outcome MakeGuess(int guess)
        {
            _lastGuessedNumber.Number = guess;

            if (CanMakeGuess != true)
            {
                _lastGuessedNumber.Outcome = Outcome.NoMoreGuesses;
                return _lastGuessedNumber.Outcome;
            }

            if (_guessedNumbers.Any<GuessedNumber>(number => number.Number == guess))
            {
                _lastGuessedNumber.Outcome = Outcome.OldGuess;
                return _lastGuessedNumber.Outcome;
            }
            else
            {
                if (guess < _number)
                {
                    _lastGuessedNumber.Outcome = Outcome.Low;
                }
                else if (guess > _number)
                {
                    _lastGuessedNumber.Outcome = Outcome.High;
                }
                else if (guess == _number)
                {
                    _lastGuessedNumber.Outcome = Outcome.Right;
                }

                _guessedNumbers.Add(_lastGuessedNumber); 
                return _lastGuessedNumber.Outcome;
            }
        }
    }
}
