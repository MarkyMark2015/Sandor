using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Uhr
{
    class Ziffernblatt
    {
        private bool[] _visibility = new bool[Ziffernblatt.MaxDigits];

        const string _digits =
            "ESKISTAFÜNF" +
            "ZEHNZWANZIG" +
            "DREIVIERTEL" +
            "VORFUNKNACH" +
            "HALBAELFÜNF" +
            "EINSXAMZWEI" +
            "DREIPMJVIER" +
            "SECHSNLACHT" +
            "SIEBENZWÖLF" +
            "ZEHNEUNKUHR";

        // Minuten
        int[] _fünf = new[] { 9, 10, 11, 12 };
        int[] _zehn = { 13, 14, 15, 16 };
        int[] _viertel = new[] { 28, 29, 30, 31, 32, 33, 34 };
        int[] _zwanzig = new[] { 17, 18, 19, 20, 21, 22, 23 };

        // Stunden
        List<List<int>> _stunden = new List<List<int>> {
             new List<int> { 96,97,98,99,100 }, // zwölf
             new List<int> { 57,58,59,60},      // eins
             new List<int> { 64,65,66,67 },     // zwei
             new List<int> { 68,69,70,71},      // drei
             new List<int> { 75,76,77,78},      // view
             new List<int> { 53,54,55,56 },     // fünf
             new List<int> { 79,80,81,82,83},   // sechs
             new List<int> { 90,91,92,93,94,95},    // sieben
             new List<int> { 86,87,88,89},      // acht
             new List<int> { 104,105,106,107},  // neun 
             new List<int> { 101,102,103,104},  // zehn
             new List<int> { 51,52,53},         // elf
             new List<int> { 96,97,98,99,100}   // zwölf
        };

        // Allgemein
        int[] _esIst = new[] { 2, 3, 5, 6, 7 };
        int[] _uhr = new[] { 109, 110, 111 };
        int[] _nach = new[] { 42, 43, 44, 45 };
        int[] _vor = new[] { 35, 36, 37 };
        int[] _halb = new[] { 46, 47, 48, 49 };

        // Liste mit den einzelnen Ziffern
        public static List<char> GetDigitList() => _digits.ToList();

        // Liste der erleuchteten Ziffern
        public List<bool> GetDigitsVisibility() => _visibility.ToList();

        // Zahl der Ziffern
        public static int MaxDigits => _digits.Count();

        // Zu erleuchtende Ziffern je nach Uhrzeit zurückgeben
        internal void Update(ObservableCollection<bool> selected, DateTime time)
        {
            for (int i = 0; i < selected.Count; i++) selected[i] = false;
            HighlightTime(selected, time);
        }
        private void HighlightHour(ObservableCollection<bool> selected, int hour)
        {
            SetFlag(selected, _stunden[hour % 12].ToArray(), null, null);
        }

        private void HighlightTime(ObservableCollection<bool> selected, DateTime time)
        {
            int min = (time.Minute + 4) / 5;
            switch (min)
            {
                case 0: // x: 00 - x Uhr
                    SetFlag(selected, _uhr, _esIst, null);
                    HighlightHour(selected, time.Hour);
                    break;
                case 1: // x: 05 - Fünf nach x
                    SetFlag(selected, _fünf, _nach, null);
                    HighlightHour(selected, time.Hour);
                    break;
                case 2: // x: 10 - Zehn nach x
                    SetFlag(selected, _zehn, _nach, null);
                    HighlightHour(selected, time.Hour);
                    break;
                case 3: // x: 15 - viertel nach x
                    SetFlag(selected, _viertel, _nach, null);
                    HighlightHour(selected, time.Hour);
                    break;
                case 4: // x: 20 - Zwanzig nach x
                    SetFlag(selected, _zwanzig, _nach, null);
                    HighlightHour(selected, time.Hour);
                    break;
                case 5: // x: 25 - fünf vor halb x + 1
                    SetFlag(selected, _fünf, _vor, _halb);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                case 6: // x: 30 - halb x + 1
                    SetFlag(selected, _halb, null, null);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                case 7: // x: 35 - fünf nach halb x + 1
                    SetFlag(selected, _fünf, _nach, _halb);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                case 8: // x: 40 - zwanzig vor x+1
                    SetFlag(selected, _zwanzig, _vor, null);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                case 9: // x: 45 - viertel vor x+1
                    SetFlag(selected, _viertel, _vor, null);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                case 10: // x: 50 - zehn vor x+1
                    SetFlag(selected, _zehn, _vor, null);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                case 11: // x: 55 - fünf vor x+1
                    SetFlag(selected, _fünf, _vor, null);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                case 12: // x: Uhr
                    SetFlag(selected, _uhr, _esIst, null);
                    HighlightHour(selected, time.Hour + 1);
                    break;
                default:
                    //;
                    break;
            }
        }

        private void SetFlag(ObservableCollection<bool> selected, int[] part1, int[] part2, int[] part3)
        {
            if (part1 != null) foreach (int i in part1) selected[i - 2] = true;
            if (part2 != null) foreach (int i in part2) selected[i - 2] = true;
            if (part3 != null) foreach (int i in part3) selected[i - 2] = true;
        }

    }
}

/*

    Ziffernblatt:

    ESKISTAFÜNF
    ZEHNZWANZIG
    DREIVIERTEL
    VORFUNKNACH
    HALBAELFÜNF
    EINSXAMZWEI
    DREIPMJVIER
    SECHSNLACHT
    SIEBENZWÖLF
    ZEHNEUNKUHR

    Kombinationen:

    x:00 - x Uhr
    x:05 - Fünf nach x 
    x:10 - Zehn nach x
    x:15 - viertel nach x
    x:20 - Zwanzig nach x
    x:25 - fünf vor halb x+1
    x:30 - halb x+1
    x:35 - fünf nach halb x+1
    x:40 - zwanzig vor x+1
    x:45 - viertel vor x+1
    x:50 - zehn vor x+1
    x:55 - fünf vor x+1


    */
