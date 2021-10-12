using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang
{
    public enum PartOfSpeech
    {
        NOUN,
        PRONOUN,
        ADJECTIVE,
        ADVERB,
        VERB,
        PRESPOSITION,
        CONJUNCTION,
        INTERJECTION,
        PARTICIPLE,
        PARTICLE,
        DETERMINER,
        UNKNOWN,
    }

    public class Word
    {
        public string spelling;
        public int frequencyRank;
        //public string basicTranslation;
        //public HashSet<string> etymologies = new HashSet<string>();
        //public string meanDescription;
        //public List<Meaning> meanings = new List<Meaning>();
        public List<string> englishForms = new List<string>();

        public bool nativeWord = true;

        //public string basicInfo = "";

        public List<HTMLExtractor.Chunk> chunks = new List<HTMLExtractor.Chunk>();
    }
}
