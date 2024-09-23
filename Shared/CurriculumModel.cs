using System.Runtime.CompilerServices;

namespace AIV4.Shared
{

    //a bit of a mish-mass
    //the CurriculumModel
    //  Learning Areas
    //  Subjects
    //  Levels
    //  Strands
    //  Substrands
    //  Content Descriptions
    //  Elaborations

    //the DTCurriculum is limited to DT and has 
    //  Concepts
    //  YearLevels
    //  Content Descriptions
    //  Elaborations
    //note this is sligthly diff to the Curr in dotnetcore

    public class CurriculumModel
    {
        public string Name { get; set; } = ""; 
        public List<LearningArea> LearningAreas { get; set; } = new List<LearningArea>();
    }


    //the 
    public class DTCurriculum
    {
        public string CurrName { get; set; } = "";
        public List<Concept> Concepts { get; set; } = new List<Concept>();

        public List<ContentDescription> CDs = new List<ContentDescription>();
    }

    public class Concept
    {
        public string ConceptName { get; set; } = "";

        public int ID { get; set; } = 0;

        //ech yearlevel has a list of CDs
        public List<YearLevel> YearLevels { get; set; } = new List<YearLevel>();

        //this is a list of CDs for the whole concept with blanks inserted that can be shown as a series of cards
        public List<ContentDescription> FlattenedCDs { get; set; } = new List<ContentDescription>();
    }

    public class YearLevel
    {
        public string LevelName { get; set; } = "";

        public int Age { get; set; } = 5;//default Foundation
        public List<ContentDescription> CDs { get; set; } = new List<ContentDescription>();

        
    }


    public class LearningArea
    {
        public string LearningAreaName { get; set; } = "";
         public List<Subject> Subjects { get; set; } = new List<Subject>();
    }

    public class Subject
    {
        public string? SubjectName { get; set; }
        public List<Level>? Levels { get; set; }

    }


    public class Level
    {
        public string LevelName { get; set; } = "";
        public int Age { get; set; }
        public string Description { get; set; } = "";
        public string DTAchievementStandard { get; set; } = "";
        public string LAAchievementStandard { get; set; } = "";

        public List<Strand> Strands { get; set; }=new List<Strand>();
    }

    public class Strand
    {
        public string StrandName { get; set; } = "";
        public List<SubStrand> SubStrands { get; set; } = new List<SubStrand>();

    }

    public class SubStrand
    {
        public string SubStrandName { get; set; } = "";
        public List<ContentDescription> ContentDescriptions { get; set; } = new List<ContentDescription>();

    }

    public class ContentDescription
    {
        public string Description { get; set; } = "";

        public int Index { get; set; } = 0;//used in the cards

        public string Code { get; set; } = "";
        public string ConceptName { get; set; } = "";
        public string Age { get; set; } = "";

        public string LevelName { get; set; } = "";
        public List<string>? Elaborations { get; set; }
    }



}

