using AIV4.Shared;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AIV4.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CurriculumController: ControllerBase
    {
        [HttpGet]
        public DTCurriculum Get()
        {
            CurriculumModel? curriculum;
            DTCurriculum? dtcurriculum = new DTCurriculum();
            List<ContentDescription> CDs = new List<ContentDescription>();
            //open the Cuuriculum json from the server
            try
            {
                //var json = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"data\\AUCurrV9a.json"}");
                var json = System.IO.File.ReadAllText("Data\\AUCurrV9a.json");
                curriculum = JsonConvert.DeserializeObject<CurriculumModel>(json);
                //dynamic myObject = JsonConvert.DeserializeObject<dynamic>(json);
                dtcurriculum = Repack(curriculum);//repack as concepts not strands with substrands
            }
            catch (Exception ex) 
            {
                var a = ex;
                curriculum = new CurriculumModel();
                curriculum.Name = "File not found, or could not be opened";
            }
            var concepts = dtcurriculum.Concepts;
            foreach (var concept in concepts)
            {
                concept.FlattenedCDs = InterleaveCDs(concept.ConceptName, concept.YearLevels);
                //foreach (var cd in concept.FlattenedCDs) //preferred and works here but doesnt pass it through 
                    //dtcurriculum.CDs.Add(cd);
            }
            return dtcurriculum;
        }

        protected DTCurriculum Repack(CurriculumModel curriculum)
        {
            //repckage the curr into concepts (subStrands)
            var dt = curriculum.LearningAreas[0].Subjects[0]; //dt curriulum
            HashSet<string> conceptNames = new HashSet<string>();
            foreach (var level in dt.Levels)
            {
                foreach (var strand in level.Strands)
                {
                    foreach (var substrand in strand.SubStrands)
                    {
                        //conceptnames.Add(substrand.SubStrandName + " (" + strand.StrandName + ")");
                        conceptNames.Add(substrand.SubStrandName);
                    }
                }
            }

            DTCurriculum dtCurr = new DTCurriculum();
            dtCurr.CurrName = dt.SubjectName;
            int i = 0;
            foreach (var conceptName in conceptNames)
            {
                Concept concept = new Concept();
                concept.ConceptName = conceptName;
                concept.ID = ++i;
                dtCurr.Concepts.Add(concept); //add it to the list of concepts in the dtCurr
                foreach (var level in dt.Levels)
                {
                    var currLevel = new YearLevel();
                    currLevel.LevelName = level.LevelName;
                    currLevel.Age = level.Age;
                    concept.YearLevels.Add(currLevel);
                    foreach (var strand in level.Strands)
                    {
                        foreach (var substrand in strand.SubStrands)
                        {
                            if (substrand.SubStrandName == conceptName)
                            {
                                foreach (var contdesc in substrand.ContentDescriptions)
                                {
                                    var cd = new ContentDescription();
                                    cd.Code = contdesc.Code;
                                    cd.Description = contdesc.Description;
                                    cd.LevelName = level.LevelName;
                                    cd.Age = level.Age.ToString();
                                    currLevel.CDs.Add(cd);
                                    foreach (var elab in contdesc.Elaborations)
                                    {
                                        if (cd.Elaborations == null)
                                            cd.Elaborations = new List<string>();
                                        cd.Elaborations.Add(elab);
                                    }

                                }

                            }
                        }
                    }
                }
            }

            return dtCurr;
        }


        
        protected List<ContentDescription> InterleaveCDs(string conceptName, List<YearLevel> yearLevels)
        {
            List<ContentDescription> flattenedList = new List<ContentDescription>();
            int maxLength = 0;

            foreach (var yearlevel in yearLevels) //find the largest number of CDs in any year level  
            {
                if (yearlevel.CDs.Count > maxLength)
                    maxLength = yearlevel.CDs.Count;
            }
            int index = 0;
            for (int i = 0; i < maxLength; i++)
            {
                foreach (var yearlevel in yearLevels)
                {
                    if (i < yearlevel.CDs.Count)
                    {
                        yearlevel.CDs[i].Index = index;
                        yearlevel.CDs[i].ConceptName = conceptName; 
                        flattenedList.Add(yearlevel.CDs[i]);
                    }
                    else
                    {
                        flattenedList.Add(new ContentDescription());
                    }
                    index++;
                }
            }

            return flattenedList;
        }



    }
}
