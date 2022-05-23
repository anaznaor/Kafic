using Kafic.Models;

namespace Kafic.Services
{
    public class RacunService
    {
        /// <summary>
        /// Metoda koja vraća račun s većim ukupnim iznosom
        /// </summary>

        public string BoljiRacunCorrect(Racun r1, Racun r2)
        {
            var ukIznos1 = r1.UkupanIznos;
            var ukIznos2 = r2.UkupanIznos;
            var korisnik1 = r1.IdKorisnik;
            var korisnik2 = r2.IdKorisnik;

            if (korisnik1 == korisnik2)
            {
                if (ukIznos1 > ukIznos2)
                {
                    return r1.IdRacun.ToString();
                }
                else
                {
                    return r2.IdRacun.ToString();
                }
            }
            else return "Nisu isti korisnici.";
        }

        /// <summary>
        /// funkcija ista kao i prethodna samo ce vratiti netocnu evaluaciju rezultata.
        /// Predstavlja neispravan dio koda koji se testira.
        /// </summary>

        public string BoljiRacunIncorrect(Racun r1, Racun r2)
        {
            var ukIznos1 = r1.UkupanIznos;
            var ukIznos2 = r2.UkupanIznos;
            var korisnik1 = r1.IdKorisnik;
            var korisnik2 = r2.IdKorisnik;

            if (korisnik1 == korisnik2)
            {
                if (ukIznos1 < ukIznos2)
                {
                    return r1.IdRacun.ToString();
                }
                else
                {
                    return r2.IdRacun.ToString();
                }
            }
            else return "Nisu isti korisnici.";
        }
    }
}
