using NUnit.Framework.Internal;
using UnityEngine;

public class StatuePuzzleExplanation : MonoBehaviour
{
    //Krótki, czysty schemat logiczny gameplayu:

    //1.Warunek aktywacji
    //Wszytkie posągi obracają się i patrzą w jeden punkt na ścianie
    //Na ścianie pojawia się główny przycisk

    //2. Aktywacja etapu 2
    //Kliknięcie przycisku na ścianie
    //Na każdym posągu pojawia się przycisk + numer 1–7

    //3. Interakcja z posągami
    //Kliknięcie przycisku na posągu
    //Odtwarzany jest sygnał Morse’a
    //Sygnał odpowiada jednej literze

    //4. Odczyt
    //Gracz zapisuje litery wg numerów 1–7
    //Powstaje hasło / słowo

    //5. Finał
    //Poprawne słowo → odblokowanie kolejnego obszaru / mechanizmu

}
